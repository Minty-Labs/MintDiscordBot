namespace Discord.Common.Helpers;

public static class Try {
    public static void Catch(Action action, bool emptyOnError = false) {
        try {
            action();
        }
        catch (Exception ex) {
            if (!emptyOnError) 
                Console.WriteLine($"Exception: {ex.Message}\n{ex.StackTrace}");
        }
    }
    
    public static void Catch(Action action, Action onError) {
        try {
            action();
        }
        catch (Exception ex) {
            Console.WriteLine($"Exception: {ex.Message} \n{ex.StackTrace}");
            onError();
        }
    }

    public static T Catch<T>(Func<T> func, bool emptyOnError = false) {
        try {
            return func();
        }
        catch (Exception ex) {
            if (!emptyOnError) 
                Console.WriteLine($"Exception: {ex.Message}\n{ex.StackTrace}");
            return default!;
        }
    }
    
    public static async Task Catch(Func<Task> func, bool emptyOnError = false) {
        try {
            await func();
        }
        catch (Exception ex) {
            if (!emptyOnError) 
                Console.WriteLine($"Exception: {ex.Message}\n{ex.StackTrace}");
        }
    }
    
    public static async Task<T> Catch<T>(Func<Task<T>> func, bool emptyOnError = false) {
        try {
            return await func();
        }
        catch (Exception ex) {
            if (!emptyOnError) 
                Console.WriteLine($"Exception: {ex.Message}\n{ex.StackTrace}");
            return default!;
        }
    }
    
    public static async Task Catch(Func<Task> func, Action onError) {
        try {
            await func();
        }
        catch (Exception ex) {
            Console.WriteLine($"Exception: {ex.Message} \n{ex.StackTrace}");
            onError();
        }
    }
    
    public static async Task<T> Catch<T>(Func<Task<T>> func, Action onError) {
        try {
            return await func();
        }
        catch (Exception ex) {
            Console.WriteLine($"Exception: {ex.Message} \n{ex.StackTrace}");
            onError();
            return default!;
        }
    }
    
    public static async Task Catch(Func<Task> func, Action<Exception> onError) {
        try {
            await func();
        }
        catch (Exception ex) {
            Console.WriteLine($"Exception: {ex.Message} \n{ex.StackTrace}");
            onError(ex);
        }
    }
    
    public static async Task<T> Catch<T>(Func<Task<T>> func, Action<Exception> onError) {
        try {
            return await func();
        }
        catch (Exception ex) {
            Console.WriteLine($"Exception: {ex.Message} \n{ex.StackTrace}");
            onError(ex);
            return default!;
        }
    }
    
    public static async Task Catch(Func<Task> func, Action<Exception> onError, Action finallyAction) {
        try {
            await func();
        }
        catch (Exception ex) {
            Console.WriteLine($"Exception: {ex.Message} \n{ex.StackTrace}");
            onError(ex);
        }
        finally {
            finallyAction();
        }
    }
    
    public static async Task<T> Catch<T>(Func<Task<T>> func, Action<Exception> onError, Action finallyAction) {
        try {
            return await func();
        }
        catch (Exception ex) {
            Console.WriteLine($"Exception: {ex.Message} \n{ex.StackTrace}");
            onError(ex);
            return default!;
        }
        finally {
            finallyAction();
        }
    }
    
    public static async Task Catch(Func<Task> func, Action<Exception> onError, Action finallyAction, bool emptyOnError = false) {
        try {
            await func();
        }
        catch (Exception ex) {
            if (!emptyOnError) 
                Console.WriteLine($"Exception: {ex.Message} \n{ex.StackTrace}");
            onError(ex);
        }
        finally {
            finallyAction();
        }
    }
    
    public static async Task<T> Catch<T>(Func<Task<T>> func, Action<Exception> onError, Action finallyAction, bool emptyOnError = false) {
        try {
            return await func();
        }
        catch (Exception ex) {
            if (!emptyOnError) 
                Console.WriteLine($"Exception: {ex.Message} \n{ex.StackTrace}");
            onError(ex);
            return default!;
        }
        finally {
            finallyAction();
        }
    }
}