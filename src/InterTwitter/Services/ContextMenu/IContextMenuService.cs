using InterTwitter.Helpers;
using System.Threading.Tasks;

namespace InterTwitter.Services.ContextMenu
{
    public interface IContextMenuService
    {
        Task<AOResult> SaveImgFromWeb(string url);
        Task<AOResult> ShareImg(string url);
        Task<AOResult> ShareProfile(string ProfileName ,string ImagePath);
    }
}
