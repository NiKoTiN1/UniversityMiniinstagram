﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Services.Services;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NewsController : Controller
    {
        public NewsController(DatabaseContext context, PostServices postServices, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _postServices = postServices;
            _appEnvironment = appEnvironment;
        }

        DatabaseContext _context;
        PostServices _postServices;
        IWebHostEnvironment _appEnvironment;

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllPosts()
        {
            ICollection<Post> posts = _context.Posts.ToList();
            foreach(var post in posts)
            {
                var imageId = post.ImageId;
                ICollection<Like> Likes = _context.Likes.Where(like => like.PostId == post.Id).ToList();
                var image = _context.Images.FirstOrDefault(a => a.Id == imageId);
                post.Likes = Likes;
                post.Image = image;
            }
            return View(posts);
        }
        [HttpGet]
        public IActionResult GetPost(string postId)
        {
            var post = _context.Posts.FirstOrDefault(a => a.Id == new Guid(postId));
            if (post == null)
                return BadRequest();
            return Ok(post);
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
                        return Ok(result);
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("addComment")]
        public ActionResult CommentPost([FromForm] CommentViewModel vm)
        {
            if (ModelState.IsValid && vm != null)
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    var result = _postServices.AddComment(vm, userIdClaim.Value);
                    if (result != null)
                        return Ok(result);
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
