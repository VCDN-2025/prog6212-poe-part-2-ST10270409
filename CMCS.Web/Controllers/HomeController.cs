using Microsoft.AspNetCore.Mvc;
namespace CMCS.Web.Controllers;
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
