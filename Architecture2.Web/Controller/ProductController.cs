﻿using System.Web.Http;
using Architecture2.Common.SharedStruct;
using Architecture2.Logic.Product;
using MediatR;

namespace Architecture2.Web.Controller
{
    public class ProductController : ApiController
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Result<Find.ProductItem> FindPaged(int? pageSize, int? skip, string code = null, string name = null, string sort = null)
        {
            return _mediator.Send(new Find.Query {PageSize = pageSize, Skip = skip, Sort = sort, Param = new Find.Param { Code = code, Name = name } });
        }

        [HttpDelete]
        public IHttpActionResult Delete(Delete.Command command)
        {
            _mediator.Publish(command);

            return Ok();
        }


    }
}
