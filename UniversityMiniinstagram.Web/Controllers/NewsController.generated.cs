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
    public partial class NewsController
    {
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected NewsController(Dummy d)
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
        public virtual IActionResult GetPost()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.GetPost);
        }

        [NonAction]
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public virtual IActionResult RemoveCommentPost()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.RemoveCommentPost);
        }

        [NonAction]
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public virtual IActionResult LikePost()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.LikePost);
        }

        [NonAction]
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public virtual IActionResult RemoveLike()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.RemoveLike);
        }

        [NonAction]
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public virtual IActionResult RemovePost()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.RemovePost);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public NewsController Actions => MVC.News;
        [GeneratedCode("R4Mvc", "1.0")]
        public readonly string Area = "";
        [GeneratedCode("R4Mvc", "1.0")]
        public readonly string Name = "News";
        [GeneratedCode("R4Mvc", "1.0")]
        public const string NameConst = "News";
        [GeneratedCode("R4Mvc", "1.0")]
        static readonly ActionNamesClass s_ActionNames = new ActionNamesClass();
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames => s_ActionNames;
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string GetAllPosts = "GetAllPosts";
            public readonly string AddPost = "AddPost";
            public readonly string GetPost = "GetPost";
            public readonly string RemoveCommentPost = "RemoveCommentPost";
            public readonly string LikePost = "LikePost";
            public readonly string RemoveLike = "RemoveLike";
            public readonly string RemovePost = "RemovePost";
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string GetAllPosts = "GetAllPosts";
            public const string AddPost = "AddPost";
            public const string GetPost = "GetPost";
            public const string RemoveCommentPost = "RemoveCommentPost";
            public const string LikePost = "LikePost";
            public const string RemoveLike = "RemoveLike";
            public const string RemovePost = "RemovePost";
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames => s_ViewNames;
            public class _ViewNamesClass
            {
                public readonly string AddPost = "AddPost";
                public readonly string GetAllPosts = "GetAllPosts";
            }

            public readonly string AddPost = "~/Views/News/AddPost.cshtml";
            public readonly string GetAllPosts = "~/Views/News/GetAllPosts.cshtml";
        }

        [GeneratedCode("R4Mvc", "1.0")]
        static readonly ViewsClass s_Views = new ViewsClass();
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public ViewsClass Views => s_Views;
    }

    [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
    public partial class R4MVC_NewsController : UniversityMiniinstagram.Web.Controllers.NewsController
    {
        public R4MVC_NewsController(): base(Dummy.Instance)
        {
        }

        [NonAction]
        partial void GetAllPostsOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAllPosts()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.GetAllPosts);
            GetAllPostsOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void AddPostOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult AddPost()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.AddPost);
            AddPostOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void AddPostOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, Microsoft.AspNetCore.Http.IFormFile file, UniversityMiniinstagram.Database.Models.Category categoryPost, string description);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> AddPost(Microsoft.AspNetCore.Http.IFormFile file, UniversityMiniinstagram.Database.Models.Category categoryPost, string description)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.AddPost);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "file", file);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "categoryPost", categoryPost);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "description", description);
            AddPostOverride(callInfo, file, categoryPost, description);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void GetPostOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, string postId);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetPost(string postId)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.GetPost);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "postId", postId);
            GetPostOverride(callInfo, postId);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void RemoveCommentPostOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, string commentId);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult> RemoveCommentPost(string commentId)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.RemoveCommentPost);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "commentId", commentId);
            RemoveCommentPostOverride(callInfo, commentId);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.ActionResult>(callInfo);
        }

        [NonAction]
        partial void LikePostOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, string postId);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> LikePost(string postId)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.LikePost);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "postId", postId);
            LikePostOverride(callInfo, postId);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void RemoveLikeOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, string postId);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> RemoveLike(string postId)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.RemoveLike);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "postId", postId);
            RemoveLikeOverride(callInfo, postId);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void RemovePostOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, string postId);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> RemovePost(string postId)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.RemovePost);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "postId", postId);
            RemovePostOverride(callInfo, postId);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }
    }
}
#pragma warning restore 1591, 3008, 3009, 0108
