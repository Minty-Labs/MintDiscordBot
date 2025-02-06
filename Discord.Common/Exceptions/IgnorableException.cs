namespace Discord.Common.Exceptions
{
    [Serializable]
    public class IgnorableException(string message, Exception innerException = null, bool printException = true) : Exception(message, innerException)
    {
        public bool PrintException { get; } = printException;
    }
}
