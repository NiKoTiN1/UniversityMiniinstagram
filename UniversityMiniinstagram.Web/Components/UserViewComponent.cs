using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;

namespace UniversityMiniinstagram.Web.Components
{
    public class UserViewComponent : ViewComponent
    {
        public UserViewComponent(DatabaseContext context)
        {
            _context = context;
        }
        DatabaseContext _context;
        public string Invoke()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            if(userIdClaim != null)
            {
                var user = _context.Users.FirstOrDefault(user => user.Id == userIdClaim.Value);
                return "Nick is: " + user.UserName;
            }
            return "Ты норм ваще?"; 
        }
    }
}
