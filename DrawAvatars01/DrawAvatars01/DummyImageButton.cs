using Splat;
using XLabs.Forms.Controls;

namespace DrawAvatars01
{
    public class DummyImageButton : ImageButton, IEnableLogger
    {
        public DummyImageButton()
        {
            this.Log().Debug("DummyImageButton ctor");

        }
    }
}