﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Xam.Plugin.AutoUpdate.Droid;
using Xam.Plugin.AutoUpdate.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileOpener))]
namespace Xam.Plugin.AutoUpdate.Droid
{
    public class FileOpener : IFileOpener
    {
        private static Context mainActivity;

        public static void Init(Context activity)
        {
            mainActivity = activity;
        }
        
        public void OpenFile(byte[] data, string name)
        {
            string directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(directory, name);
            foreach (string file in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(file) == ".apk")
                    File.Delete(file);
            }
            
            File.WriteAllBytes(path, data);

            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(FileProvider.GetUriForFile(mainActivity, "com.companyname.Samples.fileProvider", new Java.IO.File(path)), "application/vnd.android.package-archive");
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            mainActivity.StartActivity(intent);
        }
    }
}