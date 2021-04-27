using InterTwitter.Enums;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InterTwitter.Controls
{
    public class CustomCollectionView : CollectionView
    {
        private double _scrolledOffset;

        #region -- Public properties --

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
        }

        #endregion

    }
}
