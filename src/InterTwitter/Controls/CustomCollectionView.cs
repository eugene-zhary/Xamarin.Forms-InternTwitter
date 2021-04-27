using InterTwitter.Enums;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InterTwitter.Controls
{
    public class CustomCollectionView : CollectionView
    {
        private double _scrolledOffset;

        public CustomCollectionView()
        {

        }

        #region -- Public properties --

        public static readonly BindableProperty CurrentIndexProperty
            = BindableProperty.Create(nameof(CurrentIndex), typeof(int), typeof(CustomCollectionView), defaultBindingMode: BindingMode.TwoWay);

        public int CurrentIndex
        {
            get => (int)GetValue(CurrentIndexProperty);
            set => SetValue(CurrentIndexProperty, value);
        }

        public static readonly BindableProperty ScrollStateProperty
            = BindableProperty.Create(nameof(ScrollState), typeof(EScrollState), typeof(CustomCollectionView), defaultBindingMode: BindingMode.TwoWay);

        public EScrollState ScrollState
        {
            get => (EScrollState)GetValue(ScrollStateProperty);
            set => SetValue(ScrollStateProperty, value);
        }


        #endregion

        #region -- Overrides --

        protected override void OnScrolled(ItemsViewScrolledEventArgs e)
        {
            base.OnScrolled(e);

            ScrollState = (e.VerticalOffset > _scrolledOffset) ? EScrollState.ScrollDown : EScrollState.ScrollUp;

            _scrolledOffset = e.VerticalOffset;

            if(e.CenterItemIndex > 1)
            {
                Preferences.Set(nameof(CurrentIndex), e.CenterItemIndex);
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if(propertyName.Equals(nameof(CurrentIndex)))
            {
                this.ScrollTo(CurrentIndex , animate:false, position: ScrollToPosition.End);
            }
        }

        #endregion

    }
}
