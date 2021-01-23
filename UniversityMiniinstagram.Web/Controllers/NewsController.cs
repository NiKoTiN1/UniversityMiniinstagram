using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Web.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("news")]
    public class NewsController : Controller
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
        public async Task<IActionResult> GetAllPosts()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var postsViewModels = new List<PostsViewModel>();
            ViewBag.UserId = userIdClaim.Value;
            return View(await this.PostServices.GetAllPosts(userIdClaim.Value));
        }

        [HttpGet]
        [Route("addPost")]
        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        [Route("addPost")]
        public async Task<IActionResult> AddPost([FromForm] CreatePostViewModel vm)
        {
            if (!ModelState.IsValid && vm == null)
            {
                return Unauthorized();
            }
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            if (await this.PostServices.AddPost(vm, this.AppEnvironment.WebRootPath, userIdClaim.Value) == null)
            {
                return Unauthorized();
            }
            return RedirectToAction("GetAllPosts");
        }

        [HttpPost]
        [Route("getPost")]
        public async Task<IActionResult> GetPost([FromForm] string postId)
        {
            if (!ModelState.IsValid && postId == null)
            {
                return Unauthorized();
            }
            PostsViewModel postVm = await this.PostServices.GetProfilePost(postId);
            ViewBag.UserId = postVm.Post.User.Id;
            return PartialView("_ProfilePost", postVm);
        }

        [HttpDelete]
        [Route("removedComment")]
        public async Task<ActionResult> RemoveCommentPost([FromForm] string commentId)
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
        public async Task<IActionResult> LikePost([FromForm] string postId)
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
        public async Task<IActionResult> RemoveLike([FromForm] string postId)
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
        public async Task<IActionResult> RemovePost([FromForm] string postId)
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
