using DLToolkit.Forms.Controls;
using InterTwitter.Services;
using InterTwitter.Services.Authorization;
using InterTwitter.Services.ContextMenu;
using InterTwitter.Services.Permission;
using InterTwitter.Services.Settings;
using InterTwitter.Services.UserService;
using InterTwitter.ViewModels;
using InterTwitter.ViewModels.Flyout;
using InterTwitter.ViewModels.Navigation;
using InterTwitter.ViewModels.Posts;
using InterTwitter.Views;
using InterTwitter.Views.Flyout;
using InterTwitter.Views.Navigation;
using InterTwitter.Views.PostPage;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;

namespace InterTwitter
{
    public partial class App : PrismApplication
    {
        public static T Resolve<T>() => (Application.Current as App).Container.Resolve<T>();

        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        #region -- Overrides --

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IMockService>(Container.Resolve<MockService>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IPermissionManager>(Container.Resolve<PermissionManager>());

            containerRegistry.RegisterInstance<IPostService>(Container.Resolve<PostService>());
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IContextMenuService>(Container.Resolve<ContextMenuService>());

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignUpStartPage, SignUpStartPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpEndPage, SignUpEndPageViewModel>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInPageViewModel>();
            containerRegistry.RegisterForNavigation<FlyoutMenuView, FlyoutMenuViewModel>();
            containerRegistry.RegisterForNavigation<FlyoutTabbedView, FlyoutTabbedViewMode>();
            containerRegistry.RegisterForNavigation<FlyoutNavigationView, FlyoutNavigationViewModel>();
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<SearchView, SearchViewModel>();
            containerRegistry.RegisterForNavigation<NotifycationView, NotifycationViewModel>();
            containerRegistry.RegisterForNavigation<BookmarksView, BookmarksViewModel>();
            containerRegistry.RegisterForNavigation<PhotoPostPage, PhotoPostPageViewModel>();
            containerRegistry.RegisterForNavigation<GalleryPostPage, GalleryPostPageViewModel>();
            containerRegistry.RegisterForNavigation<GifPostPage, GifPostPageViewModel>();
            containerRegistry.RegisterForNavigation<VideoPostPage, VideoPostPageViewModel>();
            containerRegistry.RegisterForNavigation<PhotoPreviewPage, PhotoPreviewPageViewModel>();
            containerRegistry.RegisterForNavigation<GalleryPreviewPage, GalleryPreviewPageViewModel>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            FlowListView.Init();

            await NavigationService.NavigateAsync(nameof(FlyoutNavigationView));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #endregion
    }
}
