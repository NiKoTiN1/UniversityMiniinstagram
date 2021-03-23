// <auto-generated />
// This file was generated by R4Mvc.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the r4mvc.json file (i.e. the settings file), save it and run the generator tool again.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo.Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
#pragma warning disable 1591, 3008, 3009, 0108
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using R4Mvc;

namespace UniversityMiniinstagram.Web.Controllers
{
    public partial class AccountController
    {
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected AccountController(Dummy d)
        {
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(IActionResult result)
        {
            var callInfo = result.GetR4ActionResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<IActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(IActionResult result)
        {
            var callInfo = result.GetR4ActionResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<IActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToPage(IActionResult result)
        {
            var callInfo = result.GetR4ActionResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToPage(Task<IActionResult> taskResult)
        {
            return RedirectToPage(taskResult.Result);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToPagePermanent(IActionResult result)
        {
            var callInfo = result.GetR4ActionResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToPagePermanent(Task<IActionResult> taskResult)
        {
            return RedirectToPagePermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public virtual IActionResult Login()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Login);
        }

        [NonAction]
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public virtual IActionResult SetLanguage()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.SetLanguage);
        }

        [NonAction]
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public virtual IActionResult BanUser()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.BanUser);
        }

        [NonAction]
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public virtual IActionResult UnBanUser()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.UnBanUser);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public AccountController Actions => MVC.Account;
        [GeneratedCode("R4Mvc", "1.0")]
        public readonly string Area = "";
        [GeneratedCode("R4Mvc", "1.0")]
        public readonly string Name = "Account";
        [GeneratedCode("R4Mvc", "1.0")]
        public const string NameConst = "Account";
        [GeneratedCode("R4Mvc", "1.0")]
        static readonly ActionNamesClass s_ActionNames = new ActionNamesClass();
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames => s_ActionNames;
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Profile = "Profile";
            public readonly string EditProfile = "EditProfile";
            public readonly string Login = "Login";
            public readonly string GoogleLogin = "GoogleLogin";
            public readonly string GoogleResponse = "GoogleResponse";
            public readonly string Registration = "Registration";
            public readonly string Logout = "Logout";
            public readonly string SetLanguage = "SetLanguage";
            public readonly string AccessDenied = "AccessDenied";
            public readonly string BanPage = "BanPage";
            public readonly string BanUser = "BanUser";
            public readonly string UnBanUser = "UnBanUser";
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Profile = "Profile";
            public const string EditProfile = "EditProfile";
            public const string Login = "Login";
            public const string GoogleLogin = "GoogleLogin";
            public const string GoogleResponse = "GoogleResponse";
            public const string Registration = "Registration";
            public const string Logout = "Logout";
            public const string SetLanguage = "SetLanguage";
            public const string AccessDenied = "AccessDenied";
            public const string BanPage = "BanPage";
            public const string BanUser = "BanUser";
            public const string UnBanUser = "UnBanUser";
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames => s_ViewNames;
            public class _ViewNamesClass
            {
                public readonly string AccessDenied = "AccessDenied";
                public readonly string BanPage = "BanPage";
                public readonly string EditProfile = "EditProfile";
                public readonly string Login = "Login";
                public readonly string Profile = "Profile";
                public readonly string Registration = "Registration";
            }

            public readonly string AccessDenied = "~/Views/Account/AccessDenied.cshtml";
            public readonly string BanPage = "~/Views/Account/BanPage.cshtml";
            public readonly string EditProfile = "~/Views/Account/EditProfile.cshtml";
            public readonly string Login = "~/Views/Account/Login.cshtml";
            public readonly string Profile = "~/Views/Account/Profile.cshtml";
            public readonly string Registration = "~/Views/Account/Registration.cshtml";
        }

        [GeneratedCode("R4Mvc", "1.0")]
        static readonly ViewsClass s_Views = new ViewsClass();
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public ViewsClass Views => s_Views;
    }

    [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
    public partial class R4MVC_AccountController : UniversityMiniinstagram.Web.Controllers.AccountController
    {
        public R4MVC_AccountController(): base(Dummy.Instance)
        {
        }

        [NonAction]
        partial void ProfileOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> Profile()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Profile);
            ProfileOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void EditProfileOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> EditProfile()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.EditProfile);
            EditProfileOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void LoginOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, string ReturnUrl);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Login(string ReturnUrl)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Login);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ReturnUrl", ReturnUrl);
            LoginOverride(callInfo, ReturnUrl);
            return callInfo;
        }

        [NonAction]
        partial void GoogleLoginOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult GoogleLogin()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.GoogleLogin);
            GoogleLoginOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void GoogleResponseOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GoogleResponse()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.GoogleResponse);
            GoogleResponseOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void RegistrationOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Registration()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Registration);
            RegistrationOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void LogoutOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> Logout()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Logout);
            LogoutOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void SetLanguageOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, string culture);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult SetLanguage(string culture)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.SetLanguage);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "culture", culture);
            SetLanguageOverride(callInfo, culture);
            return callInfo;
        }

        [NonAction]
        partial void RegistrationOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, UniversityMiniinstagram.Views.RegistrationViewModel model);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> Registration(UniversityMiniinstagram.Views.RegistrationViewModel model)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Registration);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            RegistrationOverride(callInfo, model);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void LoginOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, UniversityMiniinstagram.Views.LoginViewModel model);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> Login(UniversityMiniinstagram.Views.LoginViewModel model)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Login);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            LoginOverride(callInfo, model);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void EditProfileOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, UniversityMiniinstagram.Views.EditProfileViewModel model);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> EditProfile(UniversityMiniinstagram.Views.EditProfileViewModel model)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.EditProfile);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            EditProfileOverride(callInfo, model);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void AccessDeniedOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> AccessDenied()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.AccessDenied);
            AccessDeniedOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void BanPageOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult BanPage()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.BanPage);
            BanPageOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void BanUserOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, string userId);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> BanUser(string userId)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.BanUser);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "userId", userId);
            BanUserOverride(callInfo, userId);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void UnBanUserOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, string userId);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UnBanUser(string userId)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.UnBanUser);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "userId", userId);
            UnBanUserOverride(callInfo, userId);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }
    }
}
#pragma warning restore 1591, 3008, 3009, 0108