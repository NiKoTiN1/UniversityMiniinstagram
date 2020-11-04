using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Web.ViewComponents
{
    public class RoleLineViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(UserRolesViewModel vm)
        {
            return View(vm);
        }
    }
}
