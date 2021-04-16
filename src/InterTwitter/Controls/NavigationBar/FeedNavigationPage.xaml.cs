using InterTwitter.Helpers;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        public static readonly BindableProperty ImagePathProperty = BindableProperty.Create(
            propertyName: nameof(ImagePath),
            returnType: typeof(string),
            declaringType: typeof(FeedNavigationBar));
       

        public ICommand AddPostTapGestureRecognizer => new Command<object>(OnAddPostTapTapGestureRecognizer);
        private static void OnAddPostTapTapGestureRecognizer(object obj)
        {
            //todo : OpenPageAddPost
        }

        public string ImagePath
        {
            get => (string)GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }
    }
}