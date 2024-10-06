using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace ASI.Basecode.WebApp.Controllers
{
    public class ExpenseController : ControllerBase<ExpenseController>
    {
        public ExpenseController(IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, IConfiguration configuration, IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
        }

        public IActionResult AddExpense()
        {
            return View();
        }
    }
}
