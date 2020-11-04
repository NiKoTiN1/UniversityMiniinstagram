﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Web.Components
{
    public class CommentViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CommentViewModel vm)
        {
            return View(vm);
        }
    }
}
