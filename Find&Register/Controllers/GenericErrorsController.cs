using Find_Register.Filters;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Find_Register.Controllers;

[JourneyLayoutFilter(Journey.Other)]

public class GenericErrorsController : Controller
{
    public GenericErrorsController()
    {
        ViewBag.layoutModel = new LayoutDataModel(Journey.Other);
    }

    //500 errors
    public IActionResult InternalServerError()
    {
        ViewBag.ContactUsLink = "https://www.gov.uk/government/organisations/homes-england/about/access-and-opening";
        return View();
    }

    //404 errors 
    public IActionResult PageNotFound()
    {
        ViewBag.ContactUsLink = "https://www.gov.uk/government/organisations/homes-england/about/access-and-opening";
        return View();
    }

    //503 errors 
    public IActionResult ServiceUnavailable()
    {
        ViewBag.ContactUsLink = "https://www.gov.uk/government/organisations/homes-england/about/access-and-opening";
        return View();
    }
}

