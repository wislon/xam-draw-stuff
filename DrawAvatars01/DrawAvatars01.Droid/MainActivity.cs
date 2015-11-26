using System;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using DrawAvatars01.Droid.Platform;
using NControl.Droid;
using Splat;
using XLabs.Forms.Controls;

namespace DrawAvatars01.Droid
{
    [Activity(Label = "DrawAvatars01", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            string dataPath = GetAppDataFolder();

            NControlViewRenderer.Init();
            ForceXLabsLinkerButtonRenderer.Initialise();

            Locator.CurrentMutable.RegisterConstant(new AndroidLogger(), typeof(ILogger));

            LoadApplication(new App(dataPath));
        }

        /// <summary>
        /// Just use internal sandbox user directory for now, otherwise we'll need to
        /// start fighting with Android permissions across different versions.
        /// </summary>
        /// <returns></returns>
        private static string GetAppDataFolder()
        {
            return Application.Context.GetExternalFilesDir(null).AbsolutePath;
        }

    }

}

