using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Exceptions
{
    [Serializable]
    public sealed class AppCustomException : Exception
    {
        public HttpStatusCode HttpCode { get; }
        public string ApplicationCode { get; }
        public List<Tuple<string, string>> Errors { get; }
        public Dictionary<int, string[]> ErrorPlaceholders { get; }

        private AppCustomException(SerializationInfo info, StreamingContext context)
       : base(info, context)
        {
        }

        public AppCustomException(HttpStatusCode httpCode,
            List<Tuple<string, string>> errors = null,
            Dictionary<int, string[]> errorPlaceholders = null,
            string applicationCode = null)
        {
            Errors = errors;
            HttpCode = httpCode;
            ApplicationCode = applicationCode;
            ErrorPlaceholders = errorPlaceholders;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
