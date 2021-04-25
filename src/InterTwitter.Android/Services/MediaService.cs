using System;
using Android.Content;
using InterTwitter.Droid.Services;
using InterTwitter.Services;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(MediaService))]
namespace InterTwitter.Droid.Services
{
    public class MediaService : IMediaService
    {
        Context CurrentContext => CrossCurrentActivity.Current.Activity;
        public void SaveImageFromByte(byte[] imageByte, string fileName)
        {
            try
            {
                Java.IO.File storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
                string path = System.IO.Path.Combine(storagePath.ToString(), fileName);
                System.IO.File.WriteAllBytes(path, imageByte);
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
                CurrentContext.SendBroadcast(mediaScanIntent);
            }
            catch(Exception ex)
            {

            }
        }
    }
}