using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace UniversityMiniinstagram.Web.Constraints
{
    public class CustomRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return values["numb"]?.ToString() == "1";
        }
    }
}
