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
            if (text != "" && text != "" && text != " ")
            {
                var vm = new SendCommentViewModel()
                {
                    PostId = postId,
                    Text = text
                };
                Claim userIdClaim = Context.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                Comment commresult = await this.PostService.AddComment(vm, userIdClaim.Value);

                var commentViewModel = new CommentViewModel()
                {
                    Comment = commresult,
                    IsDeleteRelated = await this.PostService.IsDeleteRelated(commresult.User, userIdClaim.Value),
                    IsReportRelated = await this.PostService.IsReportRelated(commresult.UserId, userIdClaim.Value, commentId: commresult.Id),
                    ShowReportColor = false
                };
                var res = await this.RenderService.RenderToStringAsync("_CommentBlock", commentViewModel);
                await Clients.All.SendAsync("SendCommentHub", res, postId, commentViewModel.Comment.Id.ToString());
            }
        }
    }
}
