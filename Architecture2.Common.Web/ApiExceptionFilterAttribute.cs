using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Architecture2.Common.Exception.Logic;
using Architecture2.Common.Tool;
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
            if (Extension.IsType(exception, new List<Type> { typeof(ValidationException), typeof(ForeignKeyException<>)}))
                return new Tuple<HttpStatusCode, string>(HttpStatusCode.BadRequest, exception.Message);

            if (Extension.IsType(exception, typeof(NotFoundException<>)))
                return new Tuple<HttpStatusCode, string>(HttpStatusCode.NotFound, exception.Message);

            if (Extension.IsType(exception, typeof(OptimisticConcurrencyException<>)))
                return new Tuple<HttpStatusCode, string>((HttpStatusCode)428, exception.Message);

            return null;
        }

    }
}