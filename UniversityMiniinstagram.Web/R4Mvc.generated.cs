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

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
public static partial class MVC
{
    public static readonly UniversityMiniinstagram.Web.Controllers.AccountController Account = new UniversityMiniinstagram.Web.Controllers.R4MVC_AccountController();
    public static readonly UniversityMiniinstagram.Web.Controllers.AdminController Admin = new UniversityMiniinstagram.Web.Controllers.R4MVC_AdminController();
    public static readonly UniversityMiniinstagram.Web.Controllers.NewsController News = new UniversityMiniinstagram.Web.Controllers.R4MVC_NewsController();
    public static readonly UniversityMiniinstagram.Web.Controllers.SettingsController Settings = new UniversityMiniinstagram.Web.Controllers.R4MVC_SettingsController();
    public static readonly R4Mvc.SharedController Shared = new R4Mvc.SharedController();
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
public static partial class MVCPages
{
}

namespace R4Mvc
{
    [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
    public class Dummy
    {
        private Dummy()
        {
        }

        public static Dummy Instance = new Dummy();
    }

    [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
    public partial class SharedController
    {
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames => s_ViewNames;
            public class _ViewNamesClass
            {
                public readonly string _CommentBlock = "_CommentBlock";
                public readonly string _Layout = "_Layout";
                public readonly string _ProfilePost = "_ProfilePost";
            }

            public readonly string _CommentBlock = "~/Views/Shared/_CommentBlock.cshtml";
            public readonly string _Layout = "~/Views/Shared/_Layout.cshtml";
            public readonly string _ProfilePost = "~/Views/Shared/_ProfilePost.cshtml";
        }

        static readonly ViewsClass s_Views = new ViewsClass();
        public ViewsClass Views => s_Views;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
public static partial class Links
{
    public const string UrlPath = "~";
    public static string Url() => R4MvcHelpers.ProcessVirtualPath(UrlPath);
    public static string Url(string fileName) => R4MvcHelpers.ProcessVirtualPath(UrlPath + "/" + fileName);
    public static partial class css
    {
        public const string UrlPath = "~/css";
        public static string Url() => R4MvcHelpers.ProcessVirtualPath(UrlPath);
        public static string Url(string fileName) => R4MvcHelpers.ProcessVirtualPath(UrlPath + "/" + fileName);
        public static readonly string chat_css = Url("chat.css");
    }

    public static partial class Images
    {
        public const string UrlPath = "~/Images";
        public static string Url() => R4MvcHelpers.ProcessVirtualPath(UrlPath);
        public static string Url(string fileName) => R4MvcHelpers.ProcessVirtualPath(UrlPath + "/" + fileName);
        public static readonly string _8623d1d6_af8d_4755_bab2_7edfa0f30786_jpg = Url("8623d1d6-af8d-4755-bab2-7edfa0f30786.jpg");
        public static readonly string add_png = Url("add.png");
        public static readonly string b9b47a6d_8b7c_4b18_a4bf_b33a2d77befe_jpg = Url("b9b47a6d-8b7c-4b18-a4bf-b33a2d77befe.jpg");
        public static readonly string btn_google_signin_light_normal_web_png = Url("btn_google_signin_light_normal_web.png");
        public static readonly string d2baf609_d7cb_4c66_b594_1bff56c3794f_jpg = Url("d2baf609-d7cb-4c66-b594-1bff56c3794f.jpg");
        public static readonly string noPhoto_png = Url("noPhoto.png");
    }

    public static partial class js
    {
        public const string UrlPath = "~/js";
        public static string Url() => R4MvcHelpers.ProcessVirtualPath(UrlPath);
        public static string Url(string fileName) => R4MvcHelpers.ProcessVirtualPath(UrlPath + "/" + fileName);
        public static partial class signalr
        {
            public const string UrlPath = "~/js/signalr";
            public static string Url() => R4MvcHelpers.ProcessVirtualPath(UrlPath);
            public static string Url(string fileName) => R4MvcHelpers.ProcessVirtualPath(UrlPath + "/" + fileName);
            public static partial class dist
            {
                public const string UrlPath = "~/js/signalr/dist";
                public static string Url() => R4MvcHelpers.ProcessVirtualPath(UrlPath);
                public static string Url(string fileName) => R4MvcHelpers.ProcessVirtualPath(UrlPath + "/" + fileName);
                public static partial class browser
                {
                    public const string UrlPath = "~/js/signalr/dist/browser";
                    public static string Url() => R4MvcHelpers.ProcessVirtualPath(UrlPath);
                    public static string Url(string fileName) => R4MvcHelpers.ProcessVirtualPath(UrlPath + "/" + fileName);
                    public static readonly string signalr_js = Url("signalr.js");
                    public static readonly string signalr_min_js = Url("signalr.min.js");
                }
            }
        }

        public static readonly string addRemoveModerRoots_js = Url("addRemoveModerRoots.js");
        public static readonly string banUnban_js = Url("banUnban.js");
        public static readonly string commentArea_js = Url("commentArea.js");
        public static readonly string commentary_js = Url("commentary.js");
        public static readonly string commentaryDateSetup_js = Url("commentaryDateSetup.js");
        public static readonly string commentaryReports_js = Url("commentaryReports.js");
        public static readonly string dropdownDeleteCommentary_js = Url("dropdownDeleteCommentary.js");
        public static readonly string dropdownReportCommentary_js = Url("dropdownReportCommentary.js");
        public static readonly string imageLoad_js = Url("imageLoad.js");
        public static readonly string like_js = Url("like.js");
        public static readonly string pardonCommentReport_js = Url("pardonCommentReport.js");
        public static readonly string pardonPostReport_js = Url("pardonPostReport.js");
        public static readonly string postFunctions_js = Url("postFunctions.js");
        public static readonly string postReports_js = Url("postReports.js");
        public static readonly string profilePost_js = Url("profilePost.js");
        public static readonly string setLanguage_js = Url("setLanguage.js");
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal static class R4MvcHelpers
{
    private static string ProcessVirtualPathDefault(string virtualPath) => virtualPath;
    public static Func<string, string> ProcessVirtualPath = ProcessVirtualPathDefault;
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult : ActionResult, IR4MvcActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(string area, string controller, string action, string protocol = null)
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }

    public string Controller
    {
        get;
        set;
    }

    public string Action
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_JsonResult : JsonResult, IR4MvcActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_JsonResult(string area, string controller, string action, string protocol = null): base(null)
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }

    public string Controller
    {
        get;
        set;
    }

    public string Action
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_ContentResult : ContentResult, IR4MvcActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_ContentResult(string area, string controller, string action, string protocol = null)
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }

    public string Controller
    {
        get;
        set;
    }

    public string Action
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_FileResult : FileResult, IR4MvcActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_FileResult(string area, string controller, string action, string protocol = null): base(null)
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }

    public string Controller
    {
        get;
        set;
    }

    public string Action
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_RedirectResult : RedirectResult, IR4MvcActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_RedirectResult(string area, string controller, string action, string protocol = null): base(" ")
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }

    public string Controller
    {
        get;
        set;
    }

    public string Action
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_RedirectToActionResult : RedirectToActionResult, IR4MvcActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_RedirectToActionResult(string area, string controller, string action, string protocol = null): base(" ", " ", " ")
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }

    public string Controller
    {
        get;
        set;
    }

    public string Action
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_RedirectToRouteResult : RedirectToRouteResult, IR4MvcActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_RedirectToRouteResult(string area, string controller, string action, string protocol = null): base(null)
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }

    public string Controller
    {
        get;
        set;
    }

    public string Action
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_ActionResult : ActionResult, IR4PageActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_ActionResult(string pageName, string pageHandler, string protocol = null)
    {
        this.InitMVCT4Result(pageName, pageHandler, protocol);
    }

    public string PageName
    {
        get;
        set;
    }

    public string PageHandler
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_JsonResult : JsonResult, IR4PageActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_JsonResult(string pageName, string pageHandler, string protocol = null): base(null)
    {
        this.InitMVCT4Result(pageName, pageHandler, protocol);
    }

    public string PageName
    {
        get;
        set;
    }

    public string PageHandler
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_ContentResult : ContentResult, IR4MvcActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_ContentResult(string area, string controller, string action, string protocol = null)
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }

    public string Controller
    {
        get;
        set;
    }

    public string Action
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_FileResult : FileResult, IR4PageActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_FileResult(string pageName, string pageHandler, string protocol = null): base(null)
    {
        this.InitMVCT4Result(pageName, pageHandler, protocol);
    }

    public string PageName
    {
        get;
        set;
    }

    public string PageHandler
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_RedirectResult : RedirectResult, IR4PageActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_RedirectResult(string pageName, string pageHandler, string protocol = null): base(" ")
    {
        this.InitMVCT4Result(pageName, pageHandler, protocol);
    }

    public string PageName
    {
        get;
        set;
    }

    public string PageHandler
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_RedirectToActionResult : RedirectToActionResult, IR4PageActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_RedirectToActionResult(string pageName, string pageHandler, string protocol = null): base(" ", " ", " ")
    {
        this.InitMVCT4Result(pageName, pageHandler, protocol);
    }

    public string PageName
    {
        get;
        set;
    }

    public string PageHandler
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}

[GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
internal partial class R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_RedirectToRouteResult : RedirectToRouteResult, IR4PageActionResult
{
    public R4Mvc_Microsoft_AspNetCore_Mvc_RazorPages_RedirectToRouteResult(string pageName, string pageHandler, string protocol = null): base(null)
    {
        this.InitMVCT4Result(pageName, pageHandler, protocol);
    }

    public string PageName
    {
        get;
        set;
    }

    public string PageHandler
    {
        get;
        set;
    }

    public string Protocol
    {
        get;
        set;
    }

    public RouteValueDictionary RouteValueDictionary
    {
        get;
        set;
    }
}
#pragma warning restore 1591, 3008, 3009, 0108
