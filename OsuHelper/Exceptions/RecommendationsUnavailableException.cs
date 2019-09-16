using System;

namespace OsuHelper.Exceptions
{
    public class RecommendationsUnavailableException : Exception
    {
        public RecommendationsUnavailableException(string message)
            : base(message)
        {
        }
    }
}