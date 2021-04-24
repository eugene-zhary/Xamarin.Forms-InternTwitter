﻿using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services.Authorization;
using InterTwitter.Services.UserService;
using InterTwitter.Validators;
using InterTwitter.Views.Flyout;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Navigation
{
    public class ChangeProfileViewModel : BaseViewModel, INavigatedAware
    {
        public ChangeProfileViewModel(
            INavigationService navigationService, IAuthorizationService AuthorizationService, 
            IUserService userService, IPageDialogService dialogService) : base(navigationService)
        {
            _authorizationService = AuthorizationService;
            _userService = userService;
            _dialogService = dialogService;
        }
        User CurrentUser;

        #region -- Public properties --

        private string _password;
        public string OldPassword
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _NewPassword;
        public string NewPassword
        {
            get => _NewPassword;
            set => SetProperty(ref _NewPassword, value);
        }
        private string _userBackGround;
        public string UserBackGround
        {
            get => _userBackGround;
            set => SetProperty(ref _userBackGround, value);
        }
        private string _userImagePath;
        public string UserImagePath
        {
            get => _userImagePath;
            set => SetProperty(ref _userImagePath, value);
        }
        private string _userMail;
        public string UserMail
        {
            get => _userMail;
            set => SetProperty(ref _userMail, value);
        }
        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        private bool _IsEmpty;

        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;
        private readonly IPageDialogService _dialogService;
        private ICommand _navigationToBackCommand;
        public ICommand NavigationToBackCommand => _navigationToBackCommand ??= SingleExecutionCommand.FromFunc(OnNavigationToBackCommand);

        private ICommand _confirmCommand;
        public ICommand ConfirmCommand => _confirmCommand ??= SingleExecutionCommand.FromFunc(OnConfirmCommand);

        #endregion

        private async Task OnConfirmCommand()
        {
            if (CurrentUser.Email != UserMail && StringValidator.Validate(UserMail, StringValidator.Email))
            {
                CurrentUser.Email = UserMail;
            }
            if (CurrentUser.Name != UserName && StringValidator.Validate(UserName, StringValidator.Name))
            {
                CurrentUser.Name = UserName;
            }
            if (NewPassword != OldPassword && StringValidator.Validate(NewPassword, StringValidator.Password))
            {
                CurrentUser.Password = NewPassword;
            }
            if (UserImagePath != CurrentUser.ProfileImagePath)
            {
                CurrentUser.ProfileImagePath = UserImagePath;
            }
            if (UserBackGround != CurrentUser.ProfileBackgroundImagePath)
            {
                CurrentUser.ProfileBackgroundImagePath = UserBackGround;
            }
            var Result = await _dialogService.DisplayAlertAsync("Save Changes?", "You have been alerted", "Confirm", "Cancel");

            if (Result)
            {
                await _userService.UpdateUserAsync(CurrentUser);

                NavigationToBackCommand.Execute("");
            }
        }

        private async Task OnNavigationToBackCommand()
        {
            if (NavigationService.GetNavigationUriPath().Count(u => u == '/') >= 2)
            {
                await NavigationService.GoBackAsync();
            }
            else
            {
                await NavigationService.NavigateAsync($"{nameof(FlyoutNavigationView)}");
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var user = await _userService.GetUserAsync(_authorizationService.GetCurrentUserId);

            if (user.TrackingResult)
            { 
                CurrentUser = user.Result;
                UserName = CurrentUser.Name;
                UserMail = CurrentUser.Email;
                UserBackGround = CurrentUser.ProfileBackgroundImagePath;
                UserImagePath = CurrentUser.ProfileImagePath;
            }
        }
    }
}