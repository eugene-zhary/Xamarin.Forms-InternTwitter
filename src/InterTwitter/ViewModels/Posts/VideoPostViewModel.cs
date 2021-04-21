using InterTwitter.Models;
using InterTwitter.Services;
using System.Linq;

namespace InterTwitter.ViewModels.Posts
{
    public class VideoPostViewModel : BasePostViewModel
    {
        public VideoPostViewModel(User userModel, Post postModel, IPostService postManager) : base(userModel, postModel, postManager)
        {
            VideoPath = postModel.MediaPaths.FirstOrDefault();
        }

        #region -- Public properties --

        private string _videoPath;
        public string VideoPath
        {
            get => _videoPath;
            set => SetProperty(ref _videoPath, value, nameof(VideoPath));
        }

        #endregion
    }
}
