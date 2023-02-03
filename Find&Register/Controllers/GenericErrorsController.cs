using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Find_Register.Controllers
{
    public class GenericErrorsController : Controller
    {
        //500 errors
        public IActionResult InternalServerError()
        {
            return View();
        }

        //404 errors
        public IActionResult PageNotFound()
        {
            return View();
        }

        //503 errors
        public IActionResult ServiceUnavailable()
        {
            return View();
        }
    }
}

