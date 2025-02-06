using Discord.Common.Helpers;
using Microsoft.Extensions.Logging;

namespace Discord.Common.Other
{
    public class RoundRobinTaskQueue
    {
        // Lock that protects the queue
        private readonly SemaphoreSlim enqueueSemaphore = new(1, 1);

        // Lock that makes sure one task is done at a time.
        private readonly SemaphoreSlim executeSemaphore = new(1, 1);

        // Queue of tasks
        private readonly Dictionary<ulong, (Queue<Func<Task>> queue, long time)> userQueues = [];

        // Last user a task was done for.
        private ulong lastUser = 0;

        public int Count() => userQueues.Count;
        public DateTime LastNonZeroID = DateTime.MinValue;

        public async Task Enqueue(ulong id, Func<Task> task, long time)
        {
            {
                await enqueueSemaphore.WaitAsync();
                if (id != 0)
                    LastNonZeroID = DateTime.UtcNow;

                try
                {
                    // Add the task to the queue.
                    if (!userQueues.TryGetValue(id, out var queue))
                    {
                        queue = (new Queue<Func<Task>>(), time);
                        userQueues.Add(id, queue);
                    }
                    userQueues[id].queue.Enqueue(task);
                }
                catch (Exception e)
                {
                    LoggerHelper.GlobalLogger.LogInformation("RRTaskQueue Queue Exception: \n" + e.ToString());
                    return;
                }
                finally
                {
                    enqueueSemaphore.Release();
                }
            }
            {
                bool release = false;
                await executeSemaphore.WaitAsync();
                await enqueueSemaphore.WaitAsync();
                try
                {
                    // Rotate through the users until we find one with a task.
                    List<ulong> users = userQueues.ToList().OrderBy(x => x.Value.time).Select(x => x.Key).ToList();

                    var index = users.IndexOf(lastUser);

                    void rotate(bool isBackwards)
                    {
                        if (index == -1 || (index == users.Count - 1 && !isBackwards) || (index == 0 && isBackwards))
                            index = isBackwards ? users.Count - 1 : 0;
                        else
                            index += isBackwards ? -1 : 1;
                        lastUser = users[index];
                    }

                    rotate(false);

                    // Dequeue the task.
                    var taskToDo = userQueues[lastUser].queue.Dequeue();

                    // If the queue is empty, remove it.
                    if (userQueues[lastUser].queue.Count == 0)
                    {
                        var user = lastUser;
                        rotate(true);
                        userQueues.Remove(user);
                    }

                    // Release the enqueue lock.
                    release = true;
                    enqueueSemaphore.Release();

                    // Execute the task.
                    await taskToDo();
                }
                catch (Exception e)
                {
                    LoggerHelper.GlobalLogger.LogInformation("RRTaskQueue Execute Exception: \n" + e.ToString());
                    Console.WriteLine(e);
                    return;
                }
                finally
                {
                    executeSemaphore.Release();
                    if (!release)
                        enqueueSemaphore.Release();
                }
            }
        }
    }
}
