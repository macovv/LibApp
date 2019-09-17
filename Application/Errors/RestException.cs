using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Errors
{
    class RestException : Exception
    {
        public HttpStatusCode Code { get; set; }
        public object Errors { get; set; }
        public RestException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            this.Errors = errors;
        }
    }
}
