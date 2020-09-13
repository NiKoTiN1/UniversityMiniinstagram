using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Services;
using UniversityMiniinstagram.View;
using System.Security.Claims;
using System;

namespace UniversityMiniinstagram.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadFileController : Controller
    {
        DatabaseContext _context;
        IWebHostEnvironment _appEnvironment;
        ImageServices _imageService;

        public UploadFileController(DatabaseContext context, IWebHostEnvironment appEnvironment, ImageServices imageService)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            return Ok(_context.Images.ToList());
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddFile([FromForm] ImageViewModel vm)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            //var imageAddingResult = await _imageService.Add(vm, _appEnvironment.WebRootPath, new Guid(userIdClaim.Value));
            //if (imageAddingResult)
            //    return Ok();
            return BadRequest();
        }
    }
}
