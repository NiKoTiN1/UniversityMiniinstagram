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
            this.PostService = postService;
            this.RenderService = renderService;
        }

        private readonly IViewRenderService RenderService;
        private readonly IPostService PostService;

        public async Task SendMessage(string postId, string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            var vm = new SendCommentViewModel()
            {
                PostId = postId,
                Text = text
            };
            Claim userIdClaim = Context.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            Comment commresult = await this.PostService.AddComment(vm, userIdClaim.Value);
            if (commresult == null)
            {
                return;
            }
            var commentViewModel = new CommentViewModel()
            {
                Comment = commresult,
                IsDeleteAllowed = await this.PostService.IsDeleteAllowed(commresult.User, userIdClaim.Value),
                IsReportAllowed = await this.PostService.IsReportAllowed(commresult.UserId, userIdClaim.Value, commentId: commresult.Id),
                ShowReportColor = false
            };
            var res = await this.RenderService.RenderToStringAsync(R4MvcExtensions._CommentBlock, commentViewModel);
            await Clients.All.SendAsync(R4MvcExtensions.SendCommentHub, res, postId, commentViewModel.Comment.Id.ToString());
        }
    }
}
