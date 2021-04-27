using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Views;
using InterTwitter.Views.Flyout;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Posts
{
    public class VideoPostViewModel : BasePostViewModel
    {
        public VideoPostViewModel(User userModel, Post postModel) : base(userModel, postModel)
        {
            VideoPath = postModel.MediaPaths?.FirstOrDefault();
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
