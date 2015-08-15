using System.Web.Http;
using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct;
using Architecture2.Logic.Product;

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
        public PagedCollectionResult<Find.ProductItem> FindPaged(int? pageSize, int? skip, string code = null, string name = null, string sort = null)
        {
            return _mediator.Send(new Find.Query {PageSize = pageSize, Skip = skip, SortExp = sort, Code = code, Name = name });
        }

        [HttpDelete]
        public IHttpActionResult Delete(Delete.Command command)
        {
            _mediator.Send(command);

            return Ok();
        }


    }
}
