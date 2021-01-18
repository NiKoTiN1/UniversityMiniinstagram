using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Web.Controllers
{
    [Authorize(Roles = "User")]
    [Route("admin")]
    public class AdminController : Controller
    {
        public AdminController(IAdminService adminService, IPostService postService)
        {
            this.AdminService = adminService;
            this.PostService = postService;
        }

        private readonly IAdminService AdminService;
        private readonly IPostService PostService;

        [HttpPost]
        [Route("report/send")]
        public async Task<ActionResult> SendReport([FromForm] SendReportViewModel vm)
        {
            if (ModelState.IsValid && vm != null)
            {
                Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    vm.UserId = userIdClaim.Value;
                }
                if (vm.CommentId != null)
                {
                    await this.AdminService.ReportComment(vm);
                }
                else
                {
                    await this.AdminService.ReportPost(vm);
                }
            }
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("reports/posts")]
        public async Task<ActionResult> GetPostReports()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ViewBag.UserId = userIdClaim.Value;
            ICollection<AdminPostReportsVeiwModel> vmList = await this.AdminService.GetPostReports(userIdClaim.Value);
            return View(vmList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("reports/comments")]
        public async Task<ActionResult> GetCommentReports()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ViewBag.UserId = userIdClaim.Value;
            return View(await this.AdminService.GetCommentReports(userIdClaim.Value));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("roles")]
        public async Task<ActionResult> SetDeleteRoles()
        {
            List<UserRolesViewModel> vm = await this.AdminService.GetUsersAndRoles();
            return View(vm);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("appeals")]
        public ActionResult Appeals()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("pardon/post")]
        public async Task<IActionResult> PardonPost(string reportId)
        {
            var result = await this.AdminService.RemovePostReport(reportId);
            return result ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("pardon/comment")]
        public async Task<IActionResult> PardonComment(string reportId)
        {
            var result = await this.AdminService.RemoveCommentReport(reportId);
            return result ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("post/decision")]
        public async Task<IActionResult> PostReportDecision(AdminPostReportDecisionViewModel vm)
        {
            var result = await this.AdminService.PostReportDecision(vm);
            return result ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("comment/decision")]
        public async Task<IActionResult> CommentReportDecision(AdminCommentReportDecisionViewModel vm)
        {
            var result = await this.AdminService.CommentReportDecision(vm);
            return result ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("set-moder-roots")]
        public async Task<IActionResult> AddModerator(string userId)
        {
            var result = await this.AdminService.AddModeratorRoots(userId);
            return result ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("set-user-roots")]
        public async Task<IActionResult> RemoveModerator(string userId)
        {
            var result = await this.AdminService.RemoveModeratorRoots(userId);
            return result ? Ok() : (IActionResult)BadRequest();
        }
    }
}
