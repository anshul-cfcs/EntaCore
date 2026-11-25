using Microsoft.AspNetCore.Mvc;

namespace EntaCoreAPI.Controllers
{
    public class SuperAdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
