using System;
using System.Threading.Tasks;
using System.Windows.Input;
using InterTwitter.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterTwitter.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisteringEntry : Grid
    {
        private enum ActualEntryType
        {
            NotSet,
            Password,
            NotPassword
        }

        private ActualEntryType _actualEntryType = ActualEntryType.NotSet;

        public RegisteringEntry()
        {
            InitializeComponent();

            BorderlessEntry.Focused += OnBorderlessEntryFocused;
            BorderlessEntry.Unfocused += OnBorderlessEntryUnfocused;
            LabelPlaceholder.IsVisible = false;
            EyeButton.IsVisible = false;
            ClearButton.IsVisible = false;
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;

        #region -- Public properties --

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: nameof(Text),
            returnType: typeof(string),
            declaringType: typeof(RegisteringEntry),
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                var oldValue = Text;
                SetValue(TextProperty, value);
                TextChanged?.Invoke(this, new TextChangedEventArgs(oldValue, Text));
            }
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
            propertyName: nameof(Placeholder),
            returnType: typeof(string),
            declaringType: typeof(RegisteringEntry),
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
            propertyName: nameof(ErrorText),
            returnType: typeof(string),
            declaringType: typeof(RegisteringEntry),
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay);

        public string ErrorText
        {
            get => (string) GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
            propertyName: nameof(IsPassword),
            returnType: typeof(bool),
            declaringType: typeof(RegisteringEntry),
            defaultValue: default(bool),
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsPassword
        {
            get => (bool) GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        public static readonly BindableProperty UnderlineColorProperty = BindableProperty.Create(
            propertyName: nameof(UnderlineColor),
            returnType: typeof(Color),
            declaringType: typeof(RegisteringEntry),
            defaultValue: default(Color),
            defaultBindingMode: BindingMode.TwoWay);

        public Color UnderlineColor
        {
            get => (Color) GetValue(UnderlineColorProperty);
            set
            {
                SetValue(UnderlineColorProperty, value); 
                OnPropertyChanged(nameof(UnderlineColor));
            }
        }

        public static readonly BindableProperty IsErrorTextVisibleProperty = BindableProperty.Create(
            propertyName: nameof(IsErrorTextVisible),
            returnType: typeof(bool),
            declaringType: typeof(RegisteringEntry),
            defaultValue: default(bool),
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsErrorTextVisible
        {
            get => (bool)GetValue(IsErrorTextVisibleProperty);
            set => SetValue(IsErrorTextVisibleProperty, value);
        }

        private ICommand _clearClickedCommand;
        public ICommand ClearClickedCommand =>
            _clearClickedCommand ??= SingleExecutionCommand.FromFunc(OnClearClicked);

        private ICommand _changeTextTypeCommand;
        public ICommand ChangeTextTypeCommand =>
            _changeTextTypeCommand ??= SingleExecutionCommand.FromFunc(OnChangeTextType);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Text))
            {
                if (string.IsNullOrEmpty(Text))
                {
                    ClearButton.IsVisible = false;
                    EyeButton.IsVisible = false;
                }
                else
                {
                    ClearButton.IsVisible = true;

                    if (_actualEntryType == ActualEntryType.Password)
                    {
                        EyeButton.IsVisible = true;
                    }
                }

                CorrectEntryWidth();
            }

            if (propertyName == nameof(IsPassword))
            {
                EyeButton.Source = IsPassword ? "ic_eye_blue.png" : "ic_eye_gray.png";

                EyeButton.IsVisible = !string.IsNullOrEmpty(Text);

                CorrectEntryWidth();
            }
        }

        #endregion

        #region -- Private helpers --

        private Task OnClearClicked()
        {
            Text = string.Empty;
            OnBorderlessEntryUnfocused(BorderlessEntry, EventArgs.Empty);

            return Task.CompletedTask;
        }

        private Task OnChangeTextType()
        {
            IsPassword = !IsPassword;

            return Task.CompletedTask;
        }

        private void OnBorderlessEntryFocused(object sender, FocusEventArgs e)
        {
            LabelPlaceholder.IsVisible = true;
            BorderlessEntry.Placeholder = string.Empty;

            if (_actualEntryType == ActualEntryType.NotSet)
            {
                _actualEntryType = IsPassword ? ActualEntryType.Password : ActualEntryType.NotPassword;
            }
        }

        private void OnBorderlessEntryUnfocused(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                LabelPlaceholder.IsVisible = false;
                BorderlessEntry.Placeholder = Placeholder;
            }
        }

        private void CorrectEntryWidth()
        {
            if (EyeButton.IsVisible && ClearButton.IsVisible)
            {
                SetColumnSpan(BorderlessEntry, 1);
            }
            else if (ClearButton.IsVisible)
            {
                SetColumnSpan(BorderlessEntry, 2);
            }
            else
            {
                SetColumnSpan(BorderlessEntry, 3);
            }
        }

        #endregion
    }
}