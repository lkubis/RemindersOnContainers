using System.Threading.Tasks;
using Audit.API.Infrastructure;
using Audit.API.Infrastructure.Repositories;
using Audit.API.Models;
using Audit.API.ViewModels;
using Framework.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Audit.API.Controllers
{
    [JwtAuthorize]
    [Route("api/v1/[controller]")]
    public class AuditController : Controller
    {
        #region | Fields

        private readonly IMessageRepository _repoository;

        #endregion

        #region | Constructors

        public AuditController(IMessageRepository repository)
        {
            _repoository = repository;
        }

        #endregion

        #region | Public Methods

        #region | Items - GET: /items[?pageSize={pageSize}&pageIndex={pageIndex}]

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _repoository.FindAll().CountAsync(BsonDocumentFilterDefinition<Message>.Empty);

            var itemsOnPage = await _repoository.FindAll()
                .Find(BsonDocumentFilterDefinition<Message>.Empty)
                .Skip(pageSize * pageIndex)
                .Limit(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<Message>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        #endregion

        #endregion
    }
}
