using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class FrontendController : Controller
{
    public IActionResult Index()
    {
        return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(),
            "wwwroot", "index.html"), "text/HTML");
    }
}
