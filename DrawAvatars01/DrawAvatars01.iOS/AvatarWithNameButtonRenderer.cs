using DrawAvatars01;
using DrawAvatars01.iOS;
using Splat;
using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(AvatarWithNameButton), typeof(AvatarWithNameButtonRenderer))]
namespace DrawAvatars01.iOS
{
    public class AvatarWithNameButtonRenderer : ImageButtonRenderer, IEnableLogger
    {
        public static void Initialise()
        {
            LogHost.Default.Debug("AvatarWithNameButtonRenderer Initialised");
        }         
    }
}