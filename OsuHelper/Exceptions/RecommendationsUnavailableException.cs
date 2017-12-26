using System;

namespace OsuHelper.Exceptions
{
    public class RecommendationsUnavailableException : Exception
    {
        public string Reason { get; }

        public override string Message => $"Recommendations are unavailable. Reason: {Reason}";

        public RecommendationsUnavailableException(string reason)
        {
            Reason = reason;
        }
    }
}