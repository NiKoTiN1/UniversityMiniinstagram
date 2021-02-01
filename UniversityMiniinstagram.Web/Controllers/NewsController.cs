using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Constants;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Attributes;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Web.Controllers
{
    [ApiController]
    [AuthorizeEnum(Roles.User, Roles.Admin, Roles.Moderator)]
    [Route("news")]
    public partial class NewsController : Controller
    {
        public NewsController(IPostService postServices, IWebHostEnvironment appEnvironment, IMapper mapper)
        {
            this.postService = postServices;
            this.AppEnvironment = appEnvironment;
            this.mapper = mapper;
        }

        private readonly IPostService postService;
        private readonly IWebHostEnvironment AppEnvironment;
        private readonly IMapper mapper;
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
            List<PostsViewModel> postsVM = this.mapper.Map<List<PostsViewModel>>(await this.postService.GetAllPosts(userIdClaim.Value));
            foreach(PostsViewModel postVM in postsVM)
            {
                postVM.IsDeleteAllowed = await this.postService.IsDeleteAllowed(postVM.Post.User, userIdClaim.Value);
                postVM.IsReportAllowed = await this.postService.IsReportAllowed(postVM.Post.UserId, userIdClaim.Value, postVM.Post.Id);
                foreach (CommentViewModel commentVM in postVM.CommentVM)
                {
                    commentVM.IsDeleteAllowed = await this.postService.IsDeleteAllowed(commentVM.Comment.User, userIdClaim.Value);
                    commentVM.IsReportAllowed = await this.postService.IsReportAllowed(commentVM.Comment.UserId, userIdClaim.Value, commentVM.Comment.Id);
                    commentVM.ShowReportColor = false;
                }
            }
            return View(postsVM);
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
                return BadRequest();
            }
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            if (await this.postService.AddPost(file, this.AppEnvironment.WebRootPath, userIdClaim.Value, description, categoryPost) == null)
            {
                return BadRequest();
            }
            return RedirectToAction(MVC.News.GetAllPosts());
        }

        [HttpPost]
        [Route("getPost")]
        public virtual async Task<IActionResult> GetPost([FromForm] string postId)
        {
            if (!ModelState.IsValid || postId == null)
            {
                return BadRequest();
            }
            PostsViewModel postVM = this.mapper.Map<PostsViewModel>(await this.postService.GetProfilePost(postId));
            foreach(CommentViewModel commentVM in postVM.CommentVM)
            {
                commentVM.IsDeleteAllowed = await this.postService.IsDeleteAllowed(commentVM.Comment.User, postVM.Post.UserId);
                commentVM.IsReportAllowed = await this.postService.IsReportAllowed(commentVM.Comment.UserId, postVM.Post.UserId, commentVM.Comment.Id);
                commentVM.ShowReportColor = false;
            }
            ViewBag.UserId = postVM.Post.User.Id;
            return PartialView(MVC.Shared.Views._ProfilePost, postVM);
        }

        [HttpDelete]
        [Route("removedComment")]
        public virtual async Task<ActionResult> RemoveCommentPost([FromForm] string commentId)
        {
            if (!ModelState.IsValid && commentId == null)
            {
                return BadRequest();
            }
            var postId = await this.postService.RemoveComment(commentId);
            if (postId == null)
            {
                return BadRequest();
            }
            return Ok(postId);
        }

        [HttpPost]
        [Route("addLike")]
        public virtual async Task<IActionResult> LikePost([FromForm] string postId)
        {
            if (!ModelState.IsValid && postId == null)
            {
                return BadRequest();
            }
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var isLiked = await this.postService.IsLiked(postId, userIdClaim.Value);
            if (isLiked)
            {
                return BadRequest();
            }
            Like result = await this.postService.AddLike(postId, userIdClaim.Value);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("removeLike")]
        public virtual async Task<IActionResult> RemoveLike([FromForm] string postId)
        {
            if (!ModelState.IsValid && postId == null)
            {
                return BadRequest();
            }
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            if (!await this.postService.IsLiked(postId, userIdClaim.Value))
            {
                return BadRequest();
            }
            await this.postService.RemoveLike(postId, userIdClaim.Value);
            return Ok();
        }

        [HttpDelete]
        [Route("removedPost")]
        public virtual async Task<IActionResult> RemovePost([FromForm] string postId)
        {
            if (!ModelState.IsValid && postId == null)
            {
                return BadRequest();
            }
            await this.postService.DeletePost(postId);
            return Ok();
        }
    }
}
