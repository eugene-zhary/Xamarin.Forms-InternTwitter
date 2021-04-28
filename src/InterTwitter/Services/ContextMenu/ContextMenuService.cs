using InterTwitter.Helpers;
using InterTwitter.Services.Permission;
using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
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

        #region -- IContextMenuService implementation --

        public async Task<AOResult> SaveImg(string url)
        {
            var result = new AOResult();

            try
            {
                byte[] imgData = await DownloadImgAsync(url);

                string filePath = $"InterTwitter.{DateTime.Now:dd.MM.yyyy_hh.mm.ss}.jpg";
                await MediaGallery.SaveAsync(MediaFileType.Image, imgData, filePath);

                result.SetSuccess();
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(SaveImg)} : exception", "Something went wrong", ex);
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

        #endregion


        #region -- Private helpers --

        private async Task<byte[]> DownloadImgAsync(string url)
        {
            using var webClient = new WebClient();

            return await Task.Run(() => webClient.DownloadData(url));
        }

        #endregion
    }
}
