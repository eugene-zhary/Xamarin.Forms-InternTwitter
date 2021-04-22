using Prism.Navigation;

namespace InterTwitter.ViewModels.Posts
{
    public class WatchVideoViewModel : BaseViewModel
    {
        public WatchVideoViewModel(INavigationService navigation) : base(navigation)
        {

        }

        #region -- Public properties --

        private string _videoPath;
        public string VideoPath
        {
            get => _videoPath;
            set => SetProperty(ref _videoPath, value, nameof(VideoPath));
        }

        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters.ContainsKey(nameof(VideoPath)))
            {
                VideoPath = parameters.GetValue<string>(nameof(VideoPath));
            }
        }

        #endregion
    }
}
