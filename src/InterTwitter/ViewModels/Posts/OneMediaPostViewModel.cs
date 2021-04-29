using InterTwitter.Models;
using System.Linq;

namespace InterTwitter.ViewModels.Posts
{
    public class OneMediaPostViewModel : BasePostViewModel
    {
        public OneMediaPostViewModel(User userModel, Post postModel) : base(userModel, postModel)
        {
            MediaPath = PostModel.MediaPaths?.FirstOrDefault();
        }

        #region -- Public properties --

        private string _mediaPath;
        public string MediaPath
        {
            get => _mediaPath;
            set => SetProperty(ref _mediaPath, value, nameof(MediaPath));
        }

        #endregion
    }
}
