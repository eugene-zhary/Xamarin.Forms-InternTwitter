using InterTwitter.Enums;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterTwitter.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedNavigationBar : Grid
    {
        public FeedNavigationBar()
        {
            InitializeComponent();
        }

        #region -- Public properties --

        public static readonly BindableProperty ScrollStateProperty
            = BindableProperty.Create(nameof(ScrollState), typeof(EScrollState), typeof(CustomCollectionView), propertyChanged: OnScrollStateChanged, defaultBindingMode: BindingMode.TwoWay);

        public EScrollState ScrollState
        {
            get => (EScrollState)GetValue(ScrollStateProperty);
            set => SetValue(ScrollStateProperty, value);
        }

        public static readonly BindableProperty ImagePathProperty = BindableProperty.Create(
            propertyName: nameof(ImagePath),
            returnType: typeof(string),
            declaringType: typeof(FeedNavigationBar));

        public string ImagePath
        {
            get => (string)GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }

        public ICommand AddPostTapGestureRecognizer => new Command<object>(OnAddPostTapTapGestureRecognizer);

        #endregion


        #region -- Private helpers --

        private static void OnAddPostTapTapGestureRecognizer(object obj)
        {
            //todo : OpenPageAddPost
        }

        private static void OnScrollStateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var navBar = bindable as FeedNavigationBar;
            var oldState = (EScrollState)oldValue;
            var newState = (EScrollState)newValue;

            if(navBar != null && oldState != newState)
            {
                if(newState == EScrollState.ScrollUp)
                {
                    navBar.TranslateTo(0, 0);
                }
                else
                {
                    navBar.TranslateTo(0, -navBar.Height);
                }
            }
        }

        #endregion
    }
}