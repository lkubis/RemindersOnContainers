using System.Threading.Tasks;
using Framework.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    [Authorize]
    public class RemindersController : Controller
    {
        #region | Fields

        private readonly IReminderService _reminderService;
        
        #endregion

        #region | Constructors

        public RemindersController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        #endregion

        #region | Public Methods

        #region | Items - GET: /Index[?pageSize={pageSize}&pageIndex={pageIndex}]

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var model = await _reminderService.Items(pageSize: pageSize, pageIndex: pageIndex);
            return View(model);
        }

        #endregion

        #endregion
    }
}