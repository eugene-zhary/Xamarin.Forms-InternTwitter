using InterTwitter.Models;
using InterTwitter.Services;
using Prism.Navigation;
using System.Linq;

namespace InterTwitter.ViewModels.Posts
{
    public class PhotoPostViewModel : BasePostViewModel
    {
        public PhotoPostViewModel(User userModel, Post postModel, IPostService postManager, INavigationService navigationService) : base(userModel, postModel, postManager, navigationService)
        {
            PhotoSource = PostModel.MediaPaths.FirstOrDefault();
        }

        #region -- Public properties -- 

        public string PhotoSource { get; private set; }

        #endregion
    }
}
