using System.Net;

namespace Helpers.Constants
{
    public static class ErrorStatusCodes
    {
        public const HttpStatusCode NotFound = HttpStatusCode.NotFound;
        public const HttpStatusCode InvalidAttribute = HttpStatusCode.UnprocessableEntity;
        public const HttpStatusCode InvalidHeader = HttpStatusCode.BadRequest;
    }
}
