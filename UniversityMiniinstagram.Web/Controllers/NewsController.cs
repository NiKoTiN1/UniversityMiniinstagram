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
                List<PostsViewModel> postsViewModels = new List<PostsViewModel>();
                ViewBag.UserId = userIdClaim.Value;
                var posts = await _postServices.GetAllPosts();
                foreach(var post in posts)
                {
                    if(post.IsShow)
                    {
                        PostsViewModel postVm = new PostsViewModel()
                        {
                            Post = post,
                            IsReportRelated = await _postServices.isReportRelated(post.UserId, userIdClaim.Value, postId: post.Id),
                            IsDeleteRelated = await _postServices.isDeleteRelated(post.User, userIdClaim.Value),
                            vm = new List<CommentViewModel>()
                        };
                        foreach (var comment in post.Comments)
                        {
                            if (comment.IsShow)
                            {
                                CommentViewModel commVm = new CommentViewModel();
                                commVm.Comment = comment;
                                commVm.IsDeleteRelated = await _postServices.isDeleteRelated(comment.User, userIdClaim.Value);
                                commVm.IsReportRelated = await _postServices.isReportRelated(comment.UserId, userIdClaim.Value, commentId: comment.Id);
                                commVm.ShowReportColor = false;
                                postVm.vm.Add(commVm);
                            }
                        }
                        postsViewModels.Add(postVm);
                    }
                }
                return View(postsViewModels);
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
        public async Task<IActionResult> AddPost([FromForm] CreatePostViewModel vm)
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
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    var post = await _postServices.GetPost(postId);
                    ViewBag.UserId = post.User.Id;
                    PostsViewModel postVm = new PostsViewModel()
                    {
                        Post = post,
                        IsReportRelated = await _postServices.isReportRelated(post.UserId, userIdClaim.Value, postId:postId),
                        vm = new List<CommentViewModel>()
                    };
                    foreach (var comment in post.Comments)
                    {
                        if (comment.IsShow)
                        {
                            CommentViewModel commVm = new CommentViewModel()
                            {
                                Comment = comment,
                                IsDeleteRelated = await _postServices.isDeleteRelated(comment.User, userIdClaim.Value),
                                IsReportRelated = await _postServices.isReportRelated(comment.UserId, userIdClaim.Value, commentId: comment.Id),
                                ShowReportColor = false
                            };
                            postVm.vm.Add(commVm);
                        }
                    }
                    return PartialView("_ProfilePost", postVm);
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("addComment")]
        public async Task<ActionResult> CommentPost([FromForm] SendCommentViewModel vm)
        {
            if (ModelState.IsValid && vm.Text != null && vm.PostId != null)
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    var result = await _postServices.AddComment(vm, userIdClaim.Value);
                    CommentViewModel commentViewModel = new CommentViewModel()
                    {
                        Comment = result,
                        IsDeleteRelated = await _postServices.isDeleteRelated(result.User, userIdClaim.Value),
                        IsReportRelated = await _postServices.isReportRelated(result.UserId, userIdClaim.Value, commentId:result.Id),
                        ShowReportColor = false
                    };
                    return PartialView("_CommentBlock", commentViewModel);
                }
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("removedComment")]
        public ActionResult RemoveCommentPost([FromForm] Guid commentId)
        {
            if (ModelState.IsValid && commentId != null)
            {
                var postId = _postServices.RemoveComment(commentId);
                if(postId != new Guid())
                {
                    return Ok(postId);
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
        [HttpDelete]
        [Route("removedPost")]

        public async Task<IActionResult> RemovePost([FromForm] Guid postId)
        {
            if (ModelState.IsValid && postId != null)
            {
                var post = await _postServices.GetPost(postId);
                _postServices.DeletePost(post);
                return Ok();
            }
            return Unauthorized();
        }
    }
}
