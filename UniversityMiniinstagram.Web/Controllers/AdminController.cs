using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Web.Controllers
{
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
        public async Task<ActionResult> GetPostReports()
        {
            List<AdminPostReportsVeiwModel> vmList = new List<AdminPostReportsVeiwModel>();
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            ViewBag.UserId = userIdClaim.Value;
            var reports = _adminService.GetPostReports();
            foreach(var report in reports)
            {
                AdminPostReportsVeiwModel vm = new AdminPostReportsVeiwModel();
                vm.Report = report;
                vm.CommentViewModel = new List<CommentViewModel>();
                report.Post = await _postService.GetPost(report.PostId.Value);
                foreach(var comment in report.Post.Comments)
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
            return View(vmList);
        }

        [HttpGet]
        public ActionResult GetCommentReports()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SetDeleteRoles()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Appeals()
        {
            return View();
        }

        [HttpPost]
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
        public async Task<IActionResult> PostReportDecision(AdminPostReportDecisionViewModel vm)
        {
            var result = await _adminService.PostReportDecision(vm);
            if(result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
