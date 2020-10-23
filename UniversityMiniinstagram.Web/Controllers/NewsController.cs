using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Services.Services;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("news")]
    public class NewsController : Controller
    {
        public NewsController(IPostService postServices, IWebHostEnvironment appEnvironment)
        {
            _postServices = postServices;
            _appEnvironment = appEnvironment;
        }

        IPostService _postServices;
        IWebHostEnvironment _appEnvironment;

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllPosts()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if(userIdClaim != null)
            {
                ViewBag.UserId = userIdClaim.Value;
                var posts = await _postServices.GetAllPosts();
                return View(posts);
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("addPost")]
        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        [Route("addPost")]
        public async Task<IActionResult> AddPost([FromForm] PostViewModel vm)
        {
            if(ModelState.IsValid && vm != null)
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    var result = await _postServices.AddPost(vm, _appEnvironment.WebRootPath, userIdClaim.Value);
                    if (result != null)
                        return RedirectToAction("GetAllPosts");
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("getPost")]
        public async Task<IActionResult> GetPost([FromForm] Guid postId)
        {
            if (ModelState.IsValid && postId != null)
            {
                var post = await _postServices.GetPost(postId);
                ViewBag.UserId = post.User.Id;
                return PartialView("_PostViewCompanent", post);
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("addComment")]
        public async Task<ActionResult> CommentPost([FromForm] CommentViewModel vm)
        {
            if (ModelState.IsValid && vm.Text != null && vm.PostId != null)
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    var result = await _postServices.AddComment(vm, userIdClaim.Value);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("addLike")]
        public IActionResult LikePost([FromForm] Guid postId)
        {
            if (ModelState.IsValid && postId != null)
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    var result = _postServices.AddLike(postId, userIdClaim.Value);
                    if (result != null)
                        return Ok(result);
                }
            }
            return Unauthorized();
        }
        [HttpDelete]
        [Route("removeLike")]

        public IActionResult RemoveLike([FromForm] Guid postId)
        {
            if (ModelState.IsValid && postId != null)
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    _postServices.RemoveLike(postId, userIdClaim.Value);
                    return Ok();
                }
            }
            return Unauthorized();
        }
    }
}
