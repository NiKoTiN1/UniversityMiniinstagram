using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Web.Hubs
{
    [Authorize]
    public class CommentaryHub : Hub
    {
        public CommentaryHub(IPostService postService, IViewRenderService renderService)
        {
            this.postService = postService;
            this.renderService = renderService;
        }

        private readonly IViewRenderService renderService;
        private readonly IPostService postService;

        public async Task SendMessage(string postId, string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            Claim userIdClaim = Context.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            Comment commresult = await this.postService.AddComment(postId, text, userIdClaim.Value);
            if (commresult == null)
            {
                return;
            }
            var commentViewModel = new CommentViewModel()
            {
                Comment = commresult,
                IsDeleteAllowed = await this.postService.IsDeleteAllowed(commresult.User, userIdClaim.Value),
                IsReportAllowed = await this.postService.IsReportAllowed(commresult.UserId, userIdClaim.Value, commentId: commresult.Id),
                ShowReportColor = false
            };
            string res = await this.renderService.RenderToStringAsync(R4MvcExtensions._CommentBlock, commentViewModel);
            await Clients.All.SendAsync(R4MvcExtensions.SendCommentHub, res, postId, commentViewModel.Comment.Id.ToString());
        }
    }
}
