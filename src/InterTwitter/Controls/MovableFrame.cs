using Xamarin.Forms;

namespace InterTwitter.Controls
{
    public class MovableFrame : Frame
    {
        public MovableFrame()
        {
            base.IsVisible = IsVisible;
        }

        #region -- Public properties --

        public new static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
            propertyName: nameof(IsVisible),
            returnType: typeof(bool),
            declaringType: typeof(MovableFrame),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        public new bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(IsVisible))
            {
                base.IsVisible = IsVisible;
            }
        }

        #endregion
    }
}
