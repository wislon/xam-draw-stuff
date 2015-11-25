using DrawAvatars01;
using DrawAvatars01.iOS;
using Splat;
using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(DummyImageButton), typeof(DummyImageButtonRenderer))]
namespace DrawAvatars01.iOS
{
    public class DummyImageButtonRenderer : ImageButtonRenderer, IEnableLogger
    {
        public static void Initialise()
        {
            LogHost.Default.Debug("DummyImageButtonRenderer Initialised");
        }
         
    }
}