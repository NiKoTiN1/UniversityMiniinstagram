using Microsoft.AspNetCore.Mvc;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Web.ViewComponents
{
    public class CommentViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CommentViewModel vm)
        {
            return View(vm);
        }
    }
}
