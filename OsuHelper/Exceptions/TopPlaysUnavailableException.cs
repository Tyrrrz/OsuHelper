using System;

namespace OsuHelper.Exceptions
{
    public class TopPlaysUnavailableException : Exception
    {
        public override string Message => "User has no top plays set in given game mode.";
    }
}