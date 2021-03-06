using InterTwitter.Models;
using System.Linq;

namespace InterTwitter.ViewModels.Posts
{
    public class PhotoPostViewModel : BasePostViewModel
    {
        public PhotoPostViewModel(User userModel, Post postModel) : base(userModel, postModel)
        {
            PhotoSource = PostModel.MediaPaths.FirstOrDefault();
        }

        #region -- Public properties -- 

        public string PhotoSource { get; private set; }

        #endregion
    }
}
