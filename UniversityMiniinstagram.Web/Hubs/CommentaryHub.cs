using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Services.Services;
using UniversityMiniinstagram.View;
using UniversityMiniinstagram.Web.Controllers;

    
namespace UniversityMiniinstagram.Web.Hubs
{
    [Authorize]
    public class CommentaryHub: Hub
    {
        public CommentaryHub(DatabaseContext context, PostServices postServices)
        {
            _context = context;
            _postServices = postServices;
        }

        DatabaseContext _context;
        PostServices _postServices;
        public async Task AddComment(string postId, string text)
        {
            var userIdClaim = this.Context.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            CommentViewModel vm = new CommentViewModel() { PostId = new Guid(postId), Text = text };

            if (userIdClaim != null)
            {
                var result = _postServices.AddComment(vm, userIdClaim.Value);
                var username = _context.Users.FirstOrDefault(user => user.Id == userIdClaim.Value);
                if (result != null)
                {
                    string block = "<div class=\"media chat-item\">" +
                    "<div class=\"media-body\">" +
                        "<div class=\"chat-item-title\">" +
                            "<span class=\"font-weight-bold\" data-filter-by=\"text\">" + username + "</span>" +
                        "</div>" +
                        "<div class=\"chat-item-body DIV-filter-by-text\" data-filter-by=\"text\">" +
                            "<p>" + result.Text + "</p>" +
                        "</div>" +
                    "</div>" +
                    "</div>";

                    await Clients.All.SendAsync("Commented", block, postId);
                }
            }
        }
    }
}
