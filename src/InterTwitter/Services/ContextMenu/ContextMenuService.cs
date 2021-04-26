using InterTwitter.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.MediaGallery;

namespace InterTwitter.Services.ContextMenu
{
    public class ContextMenuService : IContextMenuService
    {

        public async Task<AOResult> SaveImgFromWeb(string url)
        {
            var result = new AOResult();

            try
            {
                var status = await Permissions.CheckStatusAsync<SaveMediaPermission>();

                if(status == PermissionStatus.Granted)
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
