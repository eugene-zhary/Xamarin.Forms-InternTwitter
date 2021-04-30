using InterTwitter.Extensions;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.Views.Navigation;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Posts
{
    public class BasePostViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public BasePostViewModel(User userModel, Post postModel)
        {
            _eventAggregator = App.Resolve<IEventAggregator>();

            PostService = App.Resolve<IPostService>();
            NavigationService = App.Resolve<INavigationService>();

            _userModel = userModel;
            _postModel = postModel;
            FormattedString = new FormattedString();

            FormattedString.Spans.Add(new Span() { Text = postModel.Text});
            FormattedString.Spans.ToList().ForEach(u => u.Text.Split(' ').Where(u => u.Contains("#")).ToList().ForEach(v => IncludeSelect(u, v).ToList().ForEach(u => FormattedString.Spans.Add(u))));
            if (FormattedString.Spans.Count != 1)
            {
                FormattedString.Spans.RemoveAt(0);
            }

        }

        #region -- Public properties --

        protected INavigationService NavigationService { get; private set; }
        protected IPostService PostService { get; private set; }

        private User _userModel;
        public User UserModel
        {
            get => _userModel;
            set => SetProperty(ref _userModel, value, nameof(UserModel));
        }

        private Post _postModel;
        public Post PostModel
        {
            get => _postModel;
            set => SetProperty(ref _postModel, value, nameof(PostModel));
        }

        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set => SetProperty(ref _isLiked, value, nameof(IsLiked));
        }

        private bool _isBookmarked;
        public bool IsBookmarked
        {
            get => _isBookmarked;
            set => SetProperty(ref _isBookmarked, value, nameof(IsBookmarked));
        }

        private FormattedString _formattedString;
        public FormattedString FormattedString
        {
            get => _formattedString;
            set => SetProperty(ref _formattedString, value);
        }

        public int LikesCount
        {
            get => PostModel.LikedUserIds.Count();
        }

        private ICommand _likesCommand;
        public ICommand LikesCommand => _likesCommand ??= SingleExecutionCommand.FromFunc(OnLikesAsync);

        private ICommand _bookmarksCommand;
        public ICommand BookmarksCommand => _bookmarksCommand ??= SingleExecutionCommand.FromFunc(OnBookmarksAsync);

        private ICommand _openPostCommand;
        public ICommand OpenPostCommand => _openPostCommand ??= SingleExecutionCommand.FromFunc(OnOpenPostAsync);

        private ICommand _navigateToProfileCommand;
        public ICommand NavigateToProfileCommand => _navigateToProfileCommand ??= SingleExecutionCommand.FromFunc(OnNavigationToProfileAsync);

        private ICommand _tagTapCommad;
        public ICommand TagTapCommand => _tagTapCommad ??= SingleExecutionCommand.FromFunc<string>(OnTagTapAsync);

        private async Task OnTagTapAsync(string Tag)
        {
            NavigationParameters pairs = new NavigationParameters();
            pairs.Add(nameof(Tag), Tag);

            await NavigationService.SelectTabFromFlyoutAsync(nameof(SearchView), pairs);
        }

        #endregion

        #region -- Private helpers --

        private IEnumerable<Span> IncludeSelect(Span span, string Select)
        {
            if (!span.Text.Contains(Select) || string.IsNullOrWhiteSpace(Select))
            {
                return new Span[] { span };
            }

            Span BeforeTag = new Span() { Text = span.Text[..span.Text.IndexOf(Select)]};
            Span AfterTag = new Span() { Text = span.Text[(span.Text.IndexOf(Select) + Select.Length)..] };
            Span Tag; 
            if (Select.Contains("#"))
            {
                Tag = new Span() { Text = Select, TextColor = Color.FromHex("#2356C5") };
                Tag.GestureRecognizers.Add(new TapGestureRecognizer() { Command = TagTapCommand, CommandParameter = Tag.Text });
            }
            else
            {
                Tag = new Span() { Text = Select, BackgroundColor = Color.FromHex("#C7D6F7") };
            }

            var newspan = new List<Span>(IncludeSelect(BeforeTag, Select).Append(Tag));
            newspan.AddRange(IncludeSelect(AfterTag, Select));

            return newspan;
        }

        private async Task OnNavigationToProfileAsync()
        {
            var pairs = new NavigationParameters
            {
                { nameof(UserModel), UserModel }
            };

            await NavigationService.NavigateAsync($"{nameof(ProfileView)}", pairs, true, true);
        }

        private async Task OnLikesAsync()
        {
            IsLiked = !IsLiked;

            if (IsLiked)
            {
                await PostService.LikePostAsync(PostModel.Id);
            }
            else
            {
                await PostService.UnlikePostAsync(PostModel.Id);
            }

            RaisePropertyChanged(nameof(LikesCount));
        }

        private async Task OnBookmarksAsync()
        {
            IsBookmarked = !IsBookmarked;

            if (IsBookmarked)
            {
                await PostService.BookmarkPostAsync(PostModel.Id);
            }
            else
            {
                await PostService.UnbookmarkPostAsync(PostModel.Id);
            }
        }

        private Task OnOpenPostAsync()
        {
            _eventAggregator.GetEvent<NavigationEvent>().Publish(this);

            return Task.CompletedTask;
        }

        #endregion
    }

}
