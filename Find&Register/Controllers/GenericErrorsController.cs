using Find_Register.Filters;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Find_Register.Controllers;

[JourneyLayoutFilter(Journey.Other)]
[Route("GenericErrors")]
public class GenericErrorsController : Controller
{
    public GenericErrorsController()
    {
        ViewBag.layoutModel = new LayoutDataModel(Journey.Other);
    }

    //500 errors
    [Route("500")]
    public IActionResult InternalServerError()
    {
        return View();
    }

    //404 errors
    [Route("404")]
    public IActionResult PageNotFound()
    {
        return View();
    }

    //503 errors
    [Route("503")]
    public IActionResult ServiceUnavailable()
    {
        return View();
    }
}

