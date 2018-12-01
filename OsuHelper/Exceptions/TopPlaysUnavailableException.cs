using System;

namespace OsuHelper.Exceptions
{
    public class TopPlaysUnavailableException : Exception
    {
        public TopPlaysUnavailableException()
            : base("User has no top plays set in given game mode.")
        {
        }
    }
}