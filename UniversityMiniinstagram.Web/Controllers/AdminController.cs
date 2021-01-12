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
            var vmList = new List<AdminPostReportsVeiwModel>();
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ViewBag.UserId = userIdClaim.Value;
            ICollection<Database.Models.Report> reports = await this.AdminService.GetPostReports();
            foreach (Database.Models.Report report in reports)
            {
                report.Post = await this.PostService.GetPost(report.PostId);
                if (report.Post.UserId != userIdClaim.Value)
                {
                    var vm = new AdminPostReportsVeiwModel
                    {
                        Report = report,
                        CommentViewModel = new List<CommentViewModel>()
                    };
                    foreach (Database.Models.Comment comment in report.Post.Comments)
                    {
                        var commVm = new CommentViewModel()
                        {
                            Comment = comment,
                            IsDeleteRelated = false,
                            IsReportRelated = false,
                            ShowReportColor = false
                        };
                        vm.CommentViewModel.Add(commVm);
                    }
                    vmList.Add(vm);
                }
            }
            return View(vmList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        [Route("reports/comments")]
        public async Task<ActionResult> GetCommentReports()
        {
            var vmList = new List<AdminPostReportsVeiwModel>();
            Claim userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ViewBag.UserId = userIdClaim.Value;
            ICollection<Database.Models.Report> reports = await this.AdminService.GetCommentReports();
            foreach (Database.Models.Report report in reports)
            {
                report.Post = await this.PostService.GetPost(report.Comment.PostId);
                var isModerateAllowed = await this.AdminService.IsModerateAllowed(userIdClaim.Value, report.Comment.UserId);
                if ((report.Comment.UserId != userIdClaim.Value) && isModerateAllowed)
                {
                    var vm = new AdminPostReportsVeiwModel
                    {
                        Report = report,
                        CommentViewModel = new List<CommentViewModel>()
                    };
                    foreach (Database.Models.Comment comment in report.Post.Comments)
                    {
                        var commVm = new CommentViewModel()
                        {
                            Comment = comment,
                            IsDeleteRelated = false,
                            IsReportRelated = false,
                        };
                        if (report.CommentId == comment.Id)
                        {
                            commVm.ShowReportColor = true;
                        }
                        vm.CommentViewModel.Add(commVm);
                    }
                    vmList.Add(vm);
                }
            }
            return View(vmList);
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
        [Route("pardon")]
        public async Task<IActionResult> PardonPost(string reportId)
        {
            var result = await this.AdminService.RemoveReport(reportId);
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
