using System.Threading.Tasks;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
