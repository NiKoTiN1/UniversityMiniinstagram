using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class AdminController : Controller
    {
        public AdminController(IAdminService adminService, IPostService postService)
        {
            _adminService = adminService;
            _postService = postService;
        }

        IAdminService _adminService;
        IPostService _postService;

        [HttpPost]
        public ActionResult SendReport([FromForm] SendReportViewModel vm)
        {
            if (ModelState.IsValid && vm != null)
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    vm.UserId = userIdClaim.Value;
                }
                if(vm.CommentId != new Guid())
                {
                    _adminService.ReportComment(vm);
                }
                else
                {
                    _adminService.ReportPost(vm);
                }
            }
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Modarator")]
        public async Task<ActionResult> GetPostReports()
        {
            List<AdminPostReportsVeiwModel> vmList = new List<AdminPostReportsVeiwModel>();
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ViewBag.UserId = userIdClaim.Value;
            var reports = _adminService.GetPostReports();
            foreach(var report in reports)
            {
                report.Post = await _postService.GetPost(report.PostId.Value);
                if (report.Post.UserId != userIdClaim.Value)
                {
                    AdminPostReportsVeiwModel vm = new AdminPostReportsVeiwModel();
                    vm.Report = report;
                    vm.CommentViewModel = new List<CommentViewModel>();
                    foreach (var comment in report.Post.Comments)
                    {
                        CommentViewModel commVm = new CommentViewModel()
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
        [Authorize(Roles = "Admin,Modarator")]
        public async Task<ActionResult> GetCommentReports()
        {
            List<AdminPostReportsVeiwModel> vmList = new List<AdminPostReportsVeiwModel>();
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ViewBag.UserId = userIdClaim.Value;
            var reports = _adminService.GetCommentReports();
            foreach (var report in reports)
            {
                report.Post = await _postService.GetPost(report.Comment.PostId);
                var isModerateAllowed = await _adminService.isModerateAllowed(userIdClaim.Value, report.Comment.UserId);
                if ((report.Comment.UserId != userIdClaim.Value) && isModerateAllowed)
                {
                    AdminPostReportsVeiwModel vm = new AdminPostReportsVeiwModel();
                    vm.Report = report;
                    vm.CommentViewModel = new List<CommentViewModel>();
                    foreach (var comment in report.Post.Comments)
                    {
                        CommentViewModel commVm = new CommentViewModel()
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
        public async Task<ActionResult> SetDeleteRoles()
        {
            var vm = await _adminService.GetUsersAndRoles();
            return View(vm);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Appeals()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Modarator")]
        public IActionResult PardonPost(Guid reportId)
        {
            var result = _adminService.RemoveReport(reportId);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Modarator")]
        public async Task<IActionResult> PostReportDecision(AdminPostReportDecisionViewModel vm)
        {
            var result = await _adminService.PostReportDecision(vm);
            if(result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Modarator")]
        public async Task<IActionResult> CommentReportDecision(AdminCommentReportDecisionViewModel vm)
        {
            var result = await _adminService.CommentReportDecision(vm);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddModerator(string userId)
        {
            var result = await _adminService.AddModeratorRoots(userId);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveModerator(string userId)
        {
            var result = await _adminService.RemoveModeratorRoots(userId);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
