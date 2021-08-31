using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Constants
{
    public static class ErrorStatusCodes
    {
        public const HttpStatusCode NotFound = HttpStatusCode.NotFound;
        public const HttpStatusCode InvalidAttribute = HttpStatusCode.UnprocessableEntity;
        public const HttpStatusCode InvalidHeader = HttpStatusCode.BadRequest;
    }
}
