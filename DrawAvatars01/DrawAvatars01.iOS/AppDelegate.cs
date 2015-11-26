using System;
using System.IO;
using DrawAvatars01.iOS.Platform;
using Foundation;
using NControl.iOS;
using Splat;
using UIKit;
using XLabs.Forms.Controls;

namespace DrawAvatars01.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            string dataPath = GetAppDataFolder();

            Locator.CurrentMutable.RegisterConstant(new IOSLogger(), typeof(ILogger));

            NControlViewRenderer.Init();
            AvatarWithNameButtonRenderer.Initialise();


            LoadApplication(new App(dataPath));

            return base.FinishedLaunching(app, options);
        }

        private static string GetAppDataFolder()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var library = Path.Combine(documents, "..", "Library");
            var directoryname = Path.Combine(library, "AppData");
            if (!Directory.Exists(directoryname))
            {
                Directory.CreateDirectory(directoryname);
            }
            return directoryname;
        }

    }
}
