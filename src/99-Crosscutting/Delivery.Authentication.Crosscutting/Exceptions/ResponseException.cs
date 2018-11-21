using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Delivery.Authentication.Crosscutting.Exceptions
{
    public class ResponseException: Exception
    {
        public  int StatusCode { get; private set; }

        public ResponseException(HttpStatusCode statusCode, string message): base(message)
        {
            StatusCode = (int)statusCode;
        }

        public ResponseException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
