//Referencing list//
//https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-8.0//

using Microsoft.AspNetCore.Mvc;
namespace CMCS.Web.Controllers;
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
