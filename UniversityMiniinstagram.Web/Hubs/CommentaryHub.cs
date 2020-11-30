using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
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
        public CommentaryHub(IPostService postService, IViewRenderService renderService)
        {
            _postService = postService;
            _renderService = renderService;
        }
        IViewRenderService _renderService;
        IPostService _postService;

        public async Task SendMessage(Guid postId, string text)
        {
            if(text != "" && text != "" && text != " ")
            {
                SendCommentViewModel vm = new SendCommentViewModel()
                {
                    PostId = postId,
                    Text = text
                };
                var userIdClaim = this.Context.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                var commresult = await _postService.AddComment(vm, userIdClaim.Value);

                var offset = Convert.ToInt32(Context.Features.Get<IHttpContextFeature>().HttpContext.Request.Cookies["offset"]);
                commresult.Date.AddHours(offset);

                CommentViewModel commentViewModel = new CommentViewModel()
                {
                    Comment = commresult,
                    IsDeleteRelated = await _postService.isDeleteRelated(commresult.User, userIdClaim.Value),
                    IsReportRelated = await _postService.isReportRelated(commresult.UserId, userIdClaim.Value, commentId: commresult.Id),
                    ShowReportColor = false
                };
                var res = await _renderService.RenderToStringAsync("_CommentBlock", commentViewModel);
                await Clients.All.SendAsync("SendCommentHub", res, postId);
            }
        }      
    }
}
