using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace UniversityMiniinstagram.Web.Controllers
{
    [Authorize]
    [Route("settings")]
    public partial class SettingsController : Controller
    {
        [HttpGet]
        [Route("")]
        public virtual IActionResult GetSettings()
        {
            return View();
        }

        [HttpPost]
        [Route("language")]
        public virtual IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMinutes(5) }
            );

            return Ok();
        }
    }
}
