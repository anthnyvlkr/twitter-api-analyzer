namespace TwitterAPI.Analyzer.Common.Exceptions;

public class TweetCalculationServiceException : Exception
{
    public TweetCalculationServiceException()
    { }

    public TweetCalculationServiceException(string message)
        : base(message)
    { }

    public TweetCalculationServiceException(string message, Exception inner)
        : base(message, inner)
    { }
}