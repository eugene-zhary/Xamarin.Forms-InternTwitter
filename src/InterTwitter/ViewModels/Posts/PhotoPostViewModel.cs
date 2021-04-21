using InterTwitter.Models;
using InterTwitter.Services;
using System.Linq;

namespace InterTwitter.ViewModels.Posts
{
    public class PhotoPostViewModel : BasePostViewModel
    {
        public PhotoPostViewModel(User userModel, Post postModel, IPostService postManager) : base(userModel, postModel, postManager)
        {
            PhotoSource = PostModel.MediaPaths.FirstOrDefault();
        }

        #region -- Public properties -- 

        public string PhotoSource { get; private set; }

        #endregion
    }
}
