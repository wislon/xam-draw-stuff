using Splat;
using XLabs.Forms.Controls;

namespace DrawAvatars01
{
    public class ForceXLabsLinkerButton : ImageButton, IEnableLogger
    {
        public ForceXLabsLinkerButton()
        {
            LogHost.Default.Debug("ForceXLabsLinkerButton ctor");
        } 
    }
}