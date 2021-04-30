using InterTwitter.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using InterTwitter.Views.Navigation;
using System.Threading.Tasks;
using InterTwitter.Helpers;
using InterTwitter.ViewModels.Posts;
using InterTwitter.Services.UserService;
using InterTwitter.Services.Authorization;

namespace InterTwitter.ViewModels.Navigation
{
    public class SearchViewModel : BaseTabViewModel
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public SearchViewModel(INavigationService navigation, IPostService postService,
                               IUserService userService, IAuthorizationService authorizationService) : base(navigation)
        {
            IconPath = "ic_search_gray.png";
            _postService = postService;
            _userService = userService;
            _authorizationService = authorizationService;
            ShowNoResult = false;
            ShowPostList = false;
            ShowTagList = true;
        }

        #region -- Public properties --

        private ObservableCollection<PopularThemes> _items;
        public ObservableCollection<PopularThemes> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _noResultText;
        public string NoResultText
        {
            get => _noResultText;
            set => SetProperty(ref _noResultText, value);
        }

        private bool _ShowPostList = false;
        public bool ShowPostList
        {
            get => _ShowPostList;
            set => SetProperty(ref _ShowPostList, value);
        }

        private bool _ShowNoRsult = false;
        public bool ShowNoResult
        {
            get => _ShowNoRsult;
            set => SetProperty(ref _ShowNoRsult, value);
        }

        private bool _ShowTagList = true;
        public bool ShowTagList
        {
            get => _ShowTagList;
            set => SetProperty(ref _ShowTagList, value);
        }

        private string _profileImage;
        public string ProfileImage
        {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }

        private ObservableCollection<BasePostViewModel> _searcedPost = new ObservableCollection<BasePostViewModel>();
        public ObservableCollection<BasePostViewModel> SearchedPosts
        {
            get => _searcedPost;
            set => SetProperty(ref _searcedPost, value);
        }

        private ICommand _textChangedCommand;
        public ICommand TextChangedCommand => _textChangedCommand ??= SingleExecutionCommand.FromFunc(OnTextChangedAsync, delayMillisec:0);
        
        private ICommand _hidePostsCollectionCommand;
        public ICommand HidePostsCollectionCommand => _hidePostsCollectionCommand ??= new Command(OnHidePostsCollectionCommand);
      
        private ICommand _navigationToSearchCommand;
        public ICommand NavigationToSearchCommand => _navigationToSearchCommand ??= new Command<PopularThemes>(OnNavigationToSearchAsync);

        #endregion

        #region -- Private helpers -- 

        private async Task OnTextChangedAsync()
        {
            await UpdateCollecitonAsync(Text);

            NoResultText = Text.Length > 10 ? Text[..10] + "..." : Text;
        }

        private void OnHidePostsCollectionCommand(object obj)
        {
            ShowPostList = false;
            ShowTagList = true;

            Text = "";
        }

        private async void OnNavigationToSearchAsync(PopularThemes popularThemes)
        {
            Text = popularThemes.Tag;

            await UpdateCollecitonAsync(popularThemes.Tag);
        }
        private IEnumerable<Span> IncludeSelect(Span span, string Select)
        {
            if (!span.Text.Contains(Select) || string.IsNullOrWhiteSpace(Select))
            {
                return new Span[] { span };
            }

            Span BeforeTag = new Span() { Text = span.Text[..span.Text.IndexOf(Select)] };
            Span AfterTag = new Span() { Text = span.Text[(span.Text.IndexOf(Select) + Select.Length)..] };
            Span Tag;
            if (Select.Contains("#"))
            {
                Tag = new Span() { Text = Select, TextColor = Color.FromHex("#2356C5")};
            }
            else
            {
                Tag = new Span() { Text = Select, BackgroundColor = Color.FromHex("#C7D6F7") };
            }

            var newspan = new List<Span>(IncludeSelect(BeforeTag, Select));
            newspan.Add(Tag);
            newspan.AddRange(IncludeSelect(AfterTag, Select));

            return newspan;
        }

        private async Task UpdateCollecitonAsync(string Tag)
        {
            SearchedPosts.Clear();

            var posts = await _postService.GetPostsAsync();

            posts.Result.Where(u => u.PostModel.Text.Contains(Tag)).ToList().ForEach(SearchedPosts.Add);
            var startCount = SearchedPosts.Select(u => u.FormattedString.Spans.Count).ToList();
            SearchedPosts.ToList().ForEach(u => u.FormattedString.Spans.ToList().ForEach(span => IncludeSelect(span, Tag).ToList().ForEach(v => u.FormattedString.Spans.Add(v))));


            for (int i = 0; i < SearchedPosts.Count; i++)
            {
                for (int j = 0; j < SearchedPosts[i].FormattedString.Spans.Count - startCount[i]; j++)
                {
                    SearchedPosts[i].FormattedString.Spans.RemoveAt(0);
                }
            }

            ShowPostList = true;
            ShowTagList = false;


            ShowNoResult = SearchedPosts.Count == 0;
        }

        #endregion

        #region -- Overrides --

        public override void OnAppearing()
        {
            IconPath = "ic_search_blue.png";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_search_gray.png";
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Items = new ObservableCollection<PopularThemes>();

            var Posts = await _postService.GetPostsAsync();

            Posts.Result.ToList()
                .ForEach(u => u.PostModel.Text.Split(' ')
                .Where(u => (u.Contains('#') && !Items.Where(v => v.Tag == u).Any())).ToList()
                .ForEach(u => Items.Add(new PopularThemes { Tag = u.Replace(",", ""), Count = Posts.Result.Count(v => v.PostModel.Text.Contains(u)) })));

            Items = new ObservableCollection<PopularThemes>(Items.OrderByDescending(u => u.Count));

            ProfileImage = (await _userService.GetUserAsync(_authorizationService.GetCurrentUserId)).Result.ProfileImagePath;

            if (parameters.TryGetValue<string>("Tag", out var tag))
            {
                Text = tag;
            }
        }

        #endregion
    }

    #region -- Class Helpers --

    public class PopularThemes
    {
        public string Tag { get; set; }
        public int Count { get; set; } = 0;
    }

    #endregion
}
