using InterTwitter.Helpers;
using InterTwitter.Services.Permission;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InterTwitter.Services.ContextMenu
{
    public class ContextMenuService : IContextMenuService
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IMediaService _mediaService;

        public ContextMenuService(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
            _mediaService = DependencyService.Get<IMediaService>();
        }

        public async Task<AOResult> SaveImgFromWeb(string url)
        {
            var result = new AOResult();

            try
            {
                if(await _permissionManager.RequestStoragePermissionAsync())
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

        private void WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            string fileName = $"InterTwitter.{DateTime.Now:dd.MM.yyyy_hh.mm.ss}.jpg";
            _mediaService.SaveImageFromByte(e.Result, fileName);
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
