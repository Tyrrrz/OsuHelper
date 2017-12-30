using System;
using System.Net;

namespace OsuHelper.Exceptions
{
    public class HttpErrorStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public override string Message =>
            $"Response status code does not indicate success: {(int) StatusCode} ({StatusCode}).";

        public HttpErrorStatusCodeException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}