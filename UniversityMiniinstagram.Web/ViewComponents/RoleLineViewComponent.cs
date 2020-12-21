using Microsoft.AspNetCore.Mvc;
using UniversityMiniinstagram.Views;

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
