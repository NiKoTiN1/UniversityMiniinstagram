using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Constants;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Attrebutes;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Web.Controllers
{
    [ApiController]
    [AuthorizeEnum(Roles.User)]
    [Route("news")]
    public partial class NewsController : Controller
    {
        public NewsController(IPostService postServices, IWebHostEnvironment appEnvironment)
        {
            this.PostServices = postServices;
            this.AppEnvironment = appEnvironment;
        }

        private readonly IPostService PostServices;
        private readonly IWebHostEnvironment AppEnvironment;

        [HttpGet]
        [Route("all")]
        public virtual async Task<IActionResult> GetAllPosts()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            ViewBag.UserId = userIdClaim.Value;
            return View(await this.PostServices.GetAllPosts(userIdClaim.Value));
        }

        [HttpGet]
        [Route("addPost")]
        public virtual IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        [Route("addPost")]
        public virtual async Task<IActionResult> AddPost([FromForm] IFormFile file, Category categoryPost, string description)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized();
            }
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            if (await this.PostServices.AddPost(file, this.AppEnvironment.WebRootPath, userIdClaim.Value, description, categoryPost) == null)
            {
                return Unauthorized();
            }
            return RedirectToAction(MVC.News.GetAllPosts());
        }

        [HttpPost]
        [Route("getPost")]
        public virtual async Task<IActionResult> GetPost([FromForm] string postId)
        {
            if (!ModelState.IsValid && postId == null)
            {
                return Unauthorized();
            }
            PostsViewModel postVm = await this.PostServices.GetProfilePost(postId);
            ViewBag.UserId = postVm.Post.User.Id;
            return PartialView(MVC.Shared.Views._ProfilePost, postVm);
        }

        [HttpDelete]
        [Route("removedComment")]
        public virtual async Task<ActionResult> RemoveCommentPost([FromForm] string commentId)
        {
            if (!ModelState.IsValid && commentId == null)
            {
                return Unauthorized();
            }
            var postId = await this.PostServices.RemoveComment(commentId);
            if (postId == null)
            {
                return Unauthorized();
            }
            return Ok(postId);
        }

        [HttpPost]
        [Route("addLike")]
        public virtual async Task<IActionResult> LikePost([FromForm] string postId)
        {
            if (!ModelState.IsValid && postId == null)
            {
                return Unauthorized();
            }
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var isLiked = await this.PostServices.IsLiked(postId, userIdClaim.Value);
            if (isLiked)
            {
                return Unauthorized();
            }
            Like result = await this.PostServices.AddLike(postId, userIdClaim.Value);
            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("removeLike")]
        public virtual async Task<IActionResult> RemoveLike([FromForm] string postId)
        {
            if (!ModelState.IsValid && postId == null)
            {
                return Unauthorized();
            }
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            if (!await this.PostServices.IsLiked(postId, userIdClaim.Value))
            {
                return Unauthorized();
            }
            await this.PostServices.RemoveLike(postId, userIdClaim.Value);
            return Ok();
        }

        [HttpDelete]
        [Route("removedPost")]
        public virtual async Task<IActionResult> RemovePost([FromForm] string postId)
        {
            if (!ModelState.IsValid && postId == null)
            {
                return Unauthorized();
            }
            await this.PostServices.DeletePost(postId);
            return Ok();
        }
    }
}
