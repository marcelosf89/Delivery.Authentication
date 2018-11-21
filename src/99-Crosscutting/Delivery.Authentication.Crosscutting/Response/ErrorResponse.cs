using System;
using System.Collections.Generic;
using System.Text;

namespace Delivery.Authentication.Crosscutting.Response
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
