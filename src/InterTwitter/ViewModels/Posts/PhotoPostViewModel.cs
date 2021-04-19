using InterTwitter.Models;
using System.Linq;

namespace InterTwitter.ViewModels.Posts
{
    public class PhotoPostViewModel : BasePostViewModel
    {
        public PhotoPostViewModel(Post postModel)
        {
            PostModel = postModel;
            PhotoSource = PostModel.MediaPaths.FirstOrDefault();
        }

        #region -- Public properties -- 

        public string PhotoSource { get; private set; }

        #endregion
    }
}
