using Microsoft.AspNetCore.Mvc;

namespace SpaccioDescans.Web.Controllers;

[Route("[controller]")]
public class ReportsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
