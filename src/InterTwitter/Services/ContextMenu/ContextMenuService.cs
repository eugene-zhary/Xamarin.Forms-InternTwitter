using InterTwitter.Helpers;
using InterTwitter.Services.Permission;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.MediaGallery;

namespace InterTwitter.Services.ContextMenu
{
    public class ContextMenuService : IContextMenuService
    {
        private readonly IPermissionManager _permissionManager;

        public ContextMenuService(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        public async Task<AOResult> SaveImgFromWeb(string url)
        {
            var result = new AOResult();

            try
            {
                bool isGranted = false;

                if (Device.RuntimePlatform == Device.iOS)
                {
                    isGranted = await _permissionManager.RequestStoragePermissionAsync();
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    var status = await Permissions.CheckStatusAsync<SaveMediaPermission>();
                    isGranted = status == PermissionStatus.Granted;
                }


                if (isGranted)
                {
                    using var webClient = new WebClient();

                    webClient.DownloadDataAsync(new Uri(url));
                    webClient.DownloadDataCompleted += WebClient_DownloadDataCompleted;

                    result.SetSuccess();
                }
                else
                {
                    result.SetFailure();
                }
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(SaveImgFromWeb)} : exception", "Something went wrong", ex);
            }

            return result;
        }

        private async void WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                string filePath = $"InterTwitter.{DateTime.Now:dd.MM.yyyy_hh.mm.ss}.jpg";
                await MediaGallery.SaveAsync(MediaFileType.Image, e.Result, filePath);
            }
            catch(Exception ex)
            {

            }
        }
        public async Task<AOResult> ShareProfile(string ProfileName, string ImagePath)
        {
            var result = new AOResult();
            try
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Title = ProfileName,
                    Uri = ImagePath
                });

                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(ShareImg)} : exception", "Something went wrong", ex);
            }

            return result;
        }
        public async Task<AOResult> ShareImg(string url)
        {
            var result = new AOResult();

            try
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Title = "InterTwitter",
                    Uri = url
                });

                result.SetSuccess();
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(ShareImg)} : exception", "Something went wrong", ex);
            }

            return result;
        }
    }
}
