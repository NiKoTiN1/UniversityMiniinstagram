using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Constants;
using UniversityMiniinstagram.Services.Attrebutes;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Web.Controllers
{
    [AuthorizeEnum(Roles.User)]
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
            if (!ModelState.IsValid && vm == null)
            {
                return BadRequest();
            }
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return BadRequest();
            }
            vm.UserId = userIdClaim.Value;
            if (vm.CommentId != null)
            {
                await this.AdminService.ReportComment(vm);
                return Ok();
            }
            await this.AdminService.ReportPost(vm);
            return Ok();
        }

        [HttpGet]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("reports/posts")]
        public async Task<ActionResult> GetPostReports()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ViewBag.UserId = userIdClaim.Value;
            ICollection<AdminPostReportsVeiwModel> vmList = await this.AdminService.GetPostReports(userIdClaim.Value);
            return View(vmList);
        }

        [HttpGet]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("reports/comments")]
        public async Task<ActionResult> GetCommentReports()
        {
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ViewBag.UserId = userIdClaim.Value;
            return View(await this.AdminService.GetCommentReports(userIdClaim.Value));
        }

        [HttpGet]
        [AuthorizeEnum(Roles.Admin)]
        [Route("roles")]
        public async Task<ActionResult> SetDeleteRoles()
        {
            List<UserRolesViewModel> vm = await this.AdminService.GetUsersAndRoles();
            return View(vm);
        }

        [HttpGet]
        [AuthorizeEnum(Roles.Admin)]
        [Route("appeals")]
        public ActionResult Appeals()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("pardon/post")]
        public async Task<IActionResult> PardonPost(string reportId)
        {
            return await this.AdminService.RemovePostReport(reportId) ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("pardon/comment")]
        public async Task<IActionResult> PardonComment(string reportId)
        {
            return await this.AdminService.RemoveCommentReport(reportId) ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("post/decision")]
        public async Task<IActionResult> PostReportDecision(AdminPostReportDecisionViewModel vm)
        {
            return await this.AdminService.PostReportDecision(vm) ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin, Roles.Moderator)]
        [Route("comment/decision")]
        public async Task<IActionResult> CommentReportDecision(AdminCommentReportDecisionViewModel vm)
        {
            return await this.AdminService.CommentReportDecision(vm) ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin)]
        [Route("set-moder-roots")]
        public async Task<IActionResult> AddModerator(string userId)
        {
            return await this.AdminService.AddModeratorRoots(userId) ? Ok() : (IActionResult)BadRequest();
        }

        [HttpPost]
        [AuthorizeEnum(Roles.Admin)]
        [Route("set-user-roots")]
        public async Task<IActionResult> RemoveModerator(string userId)
        {
            return await this.AdminService.RemoveModeratorRoots(userId) ? Ok() : (IActionResult)BadRequest();
        }
    }
}
