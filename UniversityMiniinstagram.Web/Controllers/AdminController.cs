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
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        IAdminService _adminService;

        [HttpPost]
        public ActionResult SendReport([FromForm] SendReportViewModel vm)
        {
            if (ModelState.IsValid && vm != null)
            {
                //check if report was creted
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
    }
}
