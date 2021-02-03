using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Constants;
using UniversityMiniinstagram.Services.Attributes;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Web.Controllers
{
    [AuthorizeEnum(Roles.User)]
    [Route("admin")]
    public partial class AdminController : Controller
    {
        public AdminController(IAdminService adminService, IPostService postService, IMapper mapper)
        {
            this.adminService = adminService;
            this.postService = postService;
            this.mapper = mapper;
        }

        private readonly IAdminService adminService;
        private readonly IPostService postService;
        private readonly IMapper mapper;

        [HttpPost]
        [Route("report/send")]
        public virtual async Task<ActionResult> SendReport([FromForm] string userId, string postId = null, string commentId = null)
        {
            if (!ModelState.IsValid && userId == null)
            {
                return BadRequest();
            }
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return BadRequest();
            }
            userId = userIdClaim.Value;
            if (commentId != null)
            {
                await this.adminService.ReportComment(userId, commentId);
                return Ok();
            }
            await this.adminService.ReportPost(postId, userId);
            return Ok();
        }

        [HttpGet]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("reports/posts")]
        public virtual async Task<ActionResult> GetPostReports()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ViewBag.UserId = userIdClaim.Value;
            return View(new List<AdminPostReportsVeiwModel>(this.mapper.Map<ICollection<AdminPostReportsVeiwModel>>(await this.adminService.GetPostReports(userIdClaim.Value))));
        }

        [HttpGet]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("reports/comments")]
        public virtual async Task<ActionResult> GetCommentReports()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ViewBag.UserId = userIdClaim.Value;
            ICollection<AdminCommentReportsVeiwModel> reportsVM = this.mapper.Map<ICollection<AdminCommentReportsVeiwModel>>(await this.adminService.GetCommentReports(userIdClaim.Value));
            foreach(AdminCommentReportsVeiwModel reportVM in reportsVM)
            {
                reportVM.CommentViewModel.First(commentVM => reportVM.Report.CommentId == reportVM.Report.CommentId).ShowReportColor = true;
            }    
            return View(reportsVM);
        }

        [HttpGet]
        [AuthorizeEnum(Roles.Admin)]
        [Route("roles")]
        public virtual async Task<ActionResult> SetDeleteRoles()
        {
            ICollection<UserRolesViewModel> usersVM = this.mapper.Map<ICollection<UserRolesViewModel>>(await this.adminService.GetUsersAndRoles());
            foreach(UserRolesViewModel userVM in usersVM)
            {
                userVM.UserRoles = await this.adminService.GetUserRoles(userVM.User);
            }
            return View(usersVM);
        }

        [HttpGet]
        [AuthorizeEnum(Roles.Admin)]
        [Route("appeals")]
        public virtual ActionResult Appeals()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("pardon/post")]
        public virtual async Task<IActionResult> PardonPost(string reportId)
        {
            return await this.adminService.RemovePostReport(reportId) ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("pardon/comment")]
        public virtual async Task<IActionResult> PardonComment(string reportId)
        {
            return await this.adminService.RemoveCommentReport(reportId) ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("post/decision")]
        public virtual async Task<IActionResult> PostReportDecision(string reportId, bool isBanUser, bool isDeletePost, bool isHidePost)
        {
            return await this.adminService.PostReportDecision(reportId, isBanUser, isDeletePost, isHidePost) ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("comment/decision")]
        public virtual async Task<IActionResult> CommentReportDecision(string reportId, bool isHideComment, bool isDeleteComment, bool isBanUser, bool isDeletePost, bool isHidePost)
        {
            return await this.adminService.CommentReportDecision(reportId, isHideComment, isDeleteComment, isBanUser, isDeletePost, isHidePost) ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin)]
        [Route("set-moder-roots")]
        public virtual async Task<IActionResult> AddModerator(string userId)
        {
            return await this.adminService.AddModeratorRoots(userId) ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin)]
        [Route("set-user-roots")]
        public virtual async Task<IActionResult> RemoveModerator(string userId)
        {
            return await this.adminService.RemoveModeratorRoots(userId) ? Ok() : (IActionResult)BadRequest();
        }
    }
}
