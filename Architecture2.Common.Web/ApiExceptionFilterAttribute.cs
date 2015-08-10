using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using FluentValidation;

namespace Architecture2.Common.Web
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var responseParams = GetResponseParams(context.Exception);

            if (responseParams != null)
                context.Response = context.Request.CreateResponse(responseParams.Item1, responseParams.Item2);
        }

        private static Tuple<HttpStatusCode, string> GetResponseParams(System.Exception exception)
        {
            var validationException = exception as ValidationException;

            return validationException != null ? new Tuple<HttpStatusCode, string>(HttpStatusCode.BadRequest, validationException.Message) : null;
        }

    }
}