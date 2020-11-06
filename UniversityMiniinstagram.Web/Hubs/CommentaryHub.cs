using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Services.Services;
using UniversityMiniinstagram.View;
using UniversityMiniinstagram.Web.Controllers;

namespace UniversityMiniinstagram.Web.Hubs
{
    [Authorize]
    public class CommentaryHub : Hub
    {
        public CommentaryHub(IPostService postService, NewsController newsController, IViewRenderService renderService)
        {
            _postService = postService;
            _newsController = newsController;
            _renderService = renderService;
        }
        NewsController _newsController;
        IViewRenderService _renderService;
        IPostService _postService;

        public async Task SendMessage(Guid postId, string text)
        {
            //var userIdClaim = Context.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            //var data = await _newsController.CommentPost(new SendCommentViewModel() 
            //{
            //    PostId = postId,
            //    Text = text,
            //    UserId = userIdClaim.Value 
            //});
            SendCommentViewModel vm = new SendCommentViewModel()
            {
                PostId = postId,
                Text = text
            };
            var userIdClaim = this.Context.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            var commresult = await _postService.AddComment(vm, userIdClaim.Value);
            CommentViewModel commentViewModel = new CommentViewModel()
            {
                Comment = commresult,
                IsDeleteRelated = await _postService.isDeleteRelated(commresult.User, userIdClaim.Value),
                IsReportRelated = await _postService.isReportRelated(commresult.UserId, userIdClaim.Value, commentId: commresult.Id),
                ShowReportColor = false
            };


            //var template = File.ReadAllText(@"Views\Shared\_CommentBlock.cshtml");
            //Engine.Razor.AddTemplate("azaza", template);
            //var result = Engine.Razor.RunCompile("azaza", typeof(CommentViewModel), commentViewModel.GetType());
            ////Engine.Razor.Run("azaza", typeof(CommentViewModel), commentViewModel);
            //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Views\Shared\_CommentBlock.cshtml");
            //var res1 = await _renderService.RenderToStringAsync($"~/Views/Shared/_CommentBlock.cshtml", commentViewModel);
            var res = await _renderService.RenderToStringAsync("_CommentBlock", commentViewModel);
            await Clients.All.SendAsync("SendCommentHub", res, postId);
        }      
    }
}
