using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("Error")]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        ViewBag.Path = feature?.Path;
        ViewBag.ErrorMessage = feature?.Error.Message;
        return View("Error");
    }

    [Route("Error/404")]
    public IActionResult NotFoundError()
    {
        return View("NotFound");
    }
}
