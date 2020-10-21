using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;

namespace UniversityMiniinstagram.Web.Components
{
    public class PostViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Guid postId = new Guid())
        {
            return View(new Post() { Id = postId });
        }
    }
}
