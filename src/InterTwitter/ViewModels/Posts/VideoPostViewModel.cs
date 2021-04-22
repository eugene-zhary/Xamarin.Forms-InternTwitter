using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Posts
{
    public class VideoPostViewModel : BasePostViewModel
    {
        public VideoPostViewModel(User userModel, Post postModel, IPostService postManager) : base(userModel, postModel, postManager)
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

        private ICommand _openVideoCommand;
        public ICommand OpenVideoCommand => _openVideoCommand ??= SingleExecutionCommand.FromFunc(OnOpenVideo);

        #endregion

        #region -- Private helpers --

        private async Task OnOpenVideo()
        {
            await PostManager.NavigateToVideoAsync(VideoPath);
        }

        #endregion
    }
}
