﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
            if (userIdClaim != null)
            {
                var postsViewModels = new List<PostsViewModel>();
                ViewBag.UserId = userIdClaim.Value;
                ICollection<Database.Models.Post> posts = await this.PostServices.GetAllPosts();
                foreach (Database.Models.Post post in posts)
                {
                    if (post.IsShow)
                    {
                        var postVm = new PostsViewModel()
                        {
                            Post = post,
                            IsReportRelated = await this.PostServices.IsReportRelated(post.UserId, userIdClaim.Value, postId: post.Id),
                            IsDeleteRelated = await this.PostServices.IsDeleteRelated(post.User, userIdClaim.Value),
                            CommentVM = new List<CommentViewModel>()
                        };
                        foreach (Database.Models.Comment comment in post.Comments)
                        {
                            if (comment.IsShow)
                            {
                                var commVm = new CommentViewModel
                                {
                                    Comment = comment,
                                    IsDeleteRelated = await this.PostServices.IsDeleteRelated(comment.User, userIdClaim.Value),
                                    IsReportRelated = await this.PostServices.IsReportRelated(comment.UserId, userIdClaim.Value, commentId: comment.Id),
                                    ShowReportColor = false
                                };
                                postVm.CommentVM.Add(commVm);
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
            if (ModelState.IsValid && vm != null)
            {
                Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    Database.Models.Post result = await this.PostServices.AddPost(vm, this.AppEnvironment.WebRootPath, userIdClaim.Value);
                    if (result != null)
                    {
                        return RedirectToAction("GetAllPosts");
                    }
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
                Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    Database.Models.Post post = await this.PostServices.GetPost(postId);
                    ViewBag.UserId = post.User.Id;
                    var postVm = new PostsViewModel()
                    {
                        Post = post,
                        IsReportRelated = await this.PostServices.IsReportRelated(post.UserId, userIdClaim.Value, postId: postId),
                        CommentVM = new List<CommentViewModel>()
                    };
                    foreach (Database.Models.Comment comment in post.Comments)
                    {
                        if (comment.IsShow)
                        {
                            var commVm = new CommentViewModel()
                            {
                                Comment = comment,
                                IsDeleteRelated = await this.PostServices.IsDeleteRelated(comment.User, userIdClaim.Value),
                                IsReportRelated = await this.PostServices.IsReportRelated(comment.UserId, userIdClaim.Value, commentId: comment.Id),
                                ShowReportColor = false
                            };
                            postVm.CommentVM.Add(commVm);
                        }
                    }
                    return PartialView("_ProfilePost", postVm);
                }
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("removedComment")]
        public async Task<ActionResult> RemoveCommentPost([FromForm] Guid commentId)
        {
            if (ModelState.IsValid && commentId != null)
            {
                Guid postId = await this.PostServices.RemoveComment(commentId);
                if (postId != new Guid())
                {
                    return Ok(postId);
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("addLike")]
        public async Task<IActionResult> LikePost([FromForm] Guid postId)
        {
            if (ModelState.IsValid && postId != null)
            {
                Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    var isLiked = this.PostServices.IsLiked(postId, userIdClaim.Value);
                    if (!isLiked)
                    {
                        Database.Models.Like result = await this.PostServices.AddLike(postId, userIdClaim.Value);
                        if (result != null)
                        {
                            return Ok(result);
                        }
                    }
                }
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("removeLike")]
        public async Task<IActionResult> RemoveLike([FromForm] Guid postId)
        {
            if (ModelState.IsValid && postId != null)
            {
                Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    var isLiked = this.PostServices.IsLiked(postId, userIdClaim.Value);
                    if (isLiked)
                    {
                        await this.PostServices.RemoveLike(postId, userIdClaim.Value);
                        return Ok();
                    }
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
                Database.Models.Post post = await this.PostServices.GetPost(postId);
                await this.PostServices.DeletePost(post);
                return Ok();
            }
            return Unauthorized();
        }
    }
}
