namespace TwitterAPI.Analyzer.Storage.Exceptions;

public class TwitterRepositoryException : Exception
{
    public TwitterRepositoryException()
    { }

    public TwitterRepositoryException(string message)
        : base(message)
    { }

    public TwitterRepositoryException(string message, Exception inner)
        : base(message, inner)
    { }
}