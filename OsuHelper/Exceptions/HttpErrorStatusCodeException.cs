using System;
using System.Net;

namespace OsuHelper.Exceptions
{
    public class HttpErrorStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpErrorStatusCodeException(HttpStatusCode statusCode)
            : base($"Response status code does not indicate success: {(int) statusCode} ({statusCode}).")
        {
            StatusCode = statusCode;
        }
    }
}