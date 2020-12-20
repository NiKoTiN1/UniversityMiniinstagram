using Microsoft.AspNetCore.Mvc;
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
