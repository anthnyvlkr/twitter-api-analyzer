namespace TwitterAPI.Analyzer.Common.Exceptions;

public class TwitterClientFactoryException : Exception
{
    public TwitterClientFactoryException()
    { }

    public TwitterClientFactoryException(string message)
        : base(message)
    { }

    public TwitterClientFactoryException(string message, Exception inner)
        : base(message, inner)
    { }
}