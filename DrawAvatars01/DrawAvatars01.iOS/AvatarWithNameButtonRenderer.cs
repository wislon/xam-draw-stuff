using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

using CoreGraphics;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Enums;

using Splat;
using DrawAvatars01;
using DrawAvatars01.iOS;

[assembly: ExportRenderer(typeof(AvatarWithNameButton), typeof(AvatarWithNameButtonRenderer))]
namespace DrawAvatars01.iOS
{
    /// <summary>
    /// Draws a button on the iOS platform with the image shown in the correct
    /// position with the correct size.
    /// </summary>
    public class AvatarWithNameButtonRenderer : ButtonRenderer, IEnableLogger
    {
        /// <summary>
        /// Identifies the iPad.
        /// 
        /// </summary>
        private const string IPAD = "iPad";

        /// <summary>
        /// Gets the underlying element typed as an <see cref="P:XLabs.Forms.Controls.AvatarWithNameButtonRenderer.AvatarWithNameButton"/>.
        /// 
        /// </summary>
        private AvatarWithNameButton AvatarWithNameButton
        {
            get { return (AvatarWithNameButton)Element; }
        }

        public static void Initialise()
        {
            LogHost.Default.Debug("AvatarWithNameButtonRenderer Initialised");
        }

        /// <summary>
        /// Returns the proper <see cref="T:Xamarin.Forms.Platform.iOS.IImageSourceHandler"/> based on the type of <see cref="T:Xamarin.Forms.ImageSource"/> provided.
        /// 
        /// </summary>
        /// <param name="source">The <see cref="T:Xamarin.Forms.ImageSource"/> to get the handler for.</param>
        /// <returns>
        /// The needed handler.
        /// </returns>
        private static IImageSourceHandler GetHandler(ImageSource source)
        {
            var imageSourceHandler = (IImageSourceHandler)null;
            if(source is UriImageSource)
                imageSourceHandler = (IImageSourceHandler)new ImageLoaderSourceHandler();
            else if(source is FileImageSource)
                imageSourceHandler = (IImageSourceHandler)new FileImageSourceHandler();
            else if(source is StreamImageSource)
                imageSourceHandler = (IImageSourceHandler)new StreamImagesourceHandler();
            return imageSourceHandler;
        }

        /// <summary>
        /// Gets the width based on the requested width, if request less than 0, returns 50.
        /// 
        /// </summary>
        /// <param name="requestedWidth">The requested width.</param>
        /// <returns>
        /// The width to use.
        /// </returns>
        private int GetWidth(int requestedWidth)
        {
            return requestedWidth > 0 ? requestedWidth : 50;
        }

        /// <summary>
        /// Gets the height based on the requested height, if request less than 0, returns 50.
        /// 
        /// </summary>
        /// <param name="requestedHeight">The requested height.</param>
        /// <returns>
        /// The height to use.
        /// </returns>
        private int GetHeight(int requestedHeight)
        {
            return requestedHeight > 0 ? requestedHeight : 50;
        }

        /// <summary>
        /// Handles the initial drawing of the button.
        /// 
        /// </summary>
        /// <param name="e">Information on the <see cref="P:XLabs.Forms.Controls.AvatarWithNameButtonRenderer.AvatarWithNameButton"/>.</param>
        protected override async void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            var imageButton = AvatarWithNameButton;
            var targetButton = Control;
            if(imageButton != null && targetButton != null && imageButton.Source != null)
            {
                targetButton.LineBreakMode = UILineBreakMode.WordWrap;
                var width = GetWidth(imageButton.ImageWidthRequest);
                var height = GetHeight(imageButton.ImageHeightRequest);
                await SetupImages(imageButton, targetButton, width, height);
                switch(imageButton.Orientation)
                {
                    case ImageOrientation.ImageToLeft:
                    {
                        AlignToLeft(targetButton);
                        break;
                    }
                    case ImageOrientation.ImageOnTop:
                    {
                        AlignToTop(imageButton.ImageHeightRequest, imageButton.ImageWidthRequest, targetButton);
                        break;
                    }
                    case ImageOrientation.ImageToRight:
                    {
                        AlignToRight(imageButton.ImageWidthRequest, targetButton);
                        break;
                    }
                    case ImageOrientation.ImageOnBottom:
                    {
                        AlignToBottom(imageButton.ImageHeightRequest, imageButton.ImageWidthRequest, targetButton);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Called when the underlying model's properties are changed.
        /// </summary>
        /// <param name="sender">Model sending the change event.</param>
        /// <param name="e">Event arguments.</param>
        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if(e.PropertyName == AvatarWithNameButton.SourceProperty.PropertyName ||
               e.PropertyName == AvatarWithNameButton.DisabledSourceProperty.PropertyName ||
               e.PropertyName == AvatarWithNameButton.ImageTintColorProperty.PropertyName ||
               e.PropertyName == AvatarWithNameButton.DisabledImageTintColorProperty.PropertyName)
            {
                var sourceButton = Element as AvatarWithNameButton;
                if(sourceButton != null && sourceButton.Source != null)
                {
                    var imageButton = AvatarWithNameButton;
                    var targetButton = Control;
                    if(imageButton != null && targetButton != null && imageButton.Source != null)
                    {
                        await SetupImages(imageButton, targetButton, 
                                               imageButton.ImageWidthRequest, 
                                               imageButton.ImageHeightRequest);
                    }
                }
            }
        }

        private async Task SetupImages(AvatarWithNameButton imageButton, UIButton targetButton, int width, int height)
        {
            var tintColor = imageButton.ImageTintColor == Color.Transparent
                ? (UIColor)null
                : imageButton.ImageTintColor.ToUIColor();

            var disabledTintColor = imageButton.DisabledImageTintColor == Color.Transparent
                ? (UIColor)null
                : imageButton.DisabledImageTintColor.ToUIColor();

            await SetImageAsync(imageButton.Source, width, height, targetButton,
                                                           UIControlState.Normal, tintColor);

            if(imageButton.DisabledSource != null || disabledTintColor != null)
            {
                await SetImageAsync(imageButton.DisabledSource ?? imageButton.Source, width,
                                    height, targetButton, UIControlState.Disabled,
                                    disabledTintColor);
            }
        }

        /// <summary>
        /// Properly aligns the title and image on a button to the left.
        /// </summary>
        /// <param name="targetButton">The button to align.</param>
        private static void AlignToLeft(UIButton targetButton)
        {
            targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
            targetButton.TitleLabel.TextAlignment = UITextAlignment.Left;
            var uiEdgeInsets = new UIEdgeInsets(0, 2, 0, -2f);
            targetButton.TitleEdgeInsets = uiEdgeInsets;
        }

        /// <summary>
        /// Properly aligns the title and image on a button to the right.
        /// 
        /// </summary>
        /// <param name="widthRequest">The requested image width.</param><param name="targetButton">The button to align.</param>
        private static void AlignToRight(int widthRequest, UIButton targetButton)
        {
            targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
            targetButton.TitleLabel.TextAlignment = UITextAlignment.Right;
            var uiEdgeInsets1 = new UIEdgeInsets(0, 0, 0, widthRequest + 2);
            targetButton.TitleEdgeInsets = uiEdgeInsets1;
            var uiEdgeInsets2 = new UIEdgeInsets(0, widthRequest, 0, -1 * widthRequest);
            targetButton.ImageEdgeInsets = uiEdgeInsets2;
        }

        /// <summary>
        /// Properly aligns the title and image on a button when the image is over the title.
        /// 
        /// </summary>
        /// <param name="heightRequest">The requested image height.</param>
        /// <param name="widthRequest">The requested image width.</param>
        /// <param name="targetButton">The button to align.</param>
        private static void AlignToTop(int heightRequest, int widthRequest, UIButton targetButton)
        {
            targetButton.VerticalAlignment = UIControlContentVerticalAlignment.Top;
            targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            targetButton.TitleLabel.TextAlignment = UITextAlignment.Center;
            // ISSUE: reference to a compiler-generated method
            targetButton.SizeToFit();
            var width = targetButton.TitleLabel.IntrinsicContentSize.Width;
            UIEdgeInsets uiEdgeInsets1;
            UIEdgeInsets uiEdgeInsets2;
            if(UIDevice.CurrentDevice.Model.Contains(IPAD))
            {
                uiEdgeInsets1 = new UIEdgeInsets(heightRequest, Convert.ToInt32(-1 * widthRequest / 2),
                                                 (-1 * heightRequest), Convert.ToInt32(widthRequest / 2));
                uiEdgeInsets2 = new UIEdgeInsets(0, Convert.ToInt32(width / 2), 0, -1 * Convert.ToInt32(width / 2));
            }
            else
            {
                uiEdgeInsets1 = new UIEdgeInsets(heightRequest, Convert.ToInt32(-1 * widthRequest / 2),
                                                 (-1 * heightRequest), Convert.ToInt32(widthRequest / 2));
                uiEdgeInsets2 = new UIEdgeInsets(0, width / 2, 0, -1f * width / 2);
            }
            targetButton.TitleEdgeInsets = uiEdgeInsets1;
            targetButton.ImageEdgeInsets = uiEdgeInsets2;
        }

        /// <summary>
        /// Properly aligns the title and image on a button when the title is over the image.
        /// </summary>
        /// <param name="heightRequest">The requested image height.</param>
        /// <param name="widthRequest">The requested image width.</param>
        /// <param name="targetButton">The button to align.</param>
        private static void AlignToBottom(int heightRequest, int widthRequest, UIButton targetButton)
        {
            targetButton.VerticalAlignment = UIControlContentVerticalAlignment.Bottom;
            targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            targetButton.TitleLabel.TextAlignment = UITextAlignment.Center;
            // ISSUE: reference to a compiler-generated method
            targetButton.SizeToFit();
            var width = targetButton.TitleLabel.IntrinsicContentSize.Width;
            UIEdgeInsets uiEdgeInsets1;
            UIEdgeInsets uiEdgeInsets2;
            if(UIDevice.CurrentDevice.Model.Contains(IPAD))
            {
                uiEdgeInsets1 = new UIEdgeInsets((-1 * heightRequest),
                                                 Convert.ToInt32(-1 * widthRequest / 2), heightRequest,
                                                 Convert.ToInt32(widthRequest / 2));
                uiEdgeInsets2 = new UIEdgeInsets(0, width / 2, 0, -1f * width / 2);
            }
            else
            {
                uiEdgeInsets1 = new UIEdgeInsets((-1 * heightRequest), -1 * widthRequest, heightRequest, widthRequest);
                uiEdgeInsets2 = new UIEdgeInsets(0, 0, 0, 0);
            }
            targetButton.TitleEdgeInsets = uiEdgeInsets1;
            targetButton.ImageEdgeInsets = uiEdgeInsets2;
        }

        /// <summary>
        /// Loads an image from a bundle given the supplied image name, resizes it to the
        /// height and width request and sets it into a <see cref="T:UIKit.UIButton"/>.
        /// </summary>
        /// <param name="source">The <see cref="T:Xamarin.Forms.ImageSource"/> to load the image from.</param>
        /// <param name="widthRequest">The requested image width.</param>
        /// <param name="heightRequest">The requested image height.</param>
        /// <param name="targetButton">A <see cref="T:UIKit.UIButton"/> to set the image into.</param>
        /// <param name="state">The state.</param>
        /// <param name="tintColor">Color of the tint.</param>
        private static async Task SetImageAsync(ImageSource source,
                                                int widthRequest,
                                                int heightRequest,
                                                UIButton targetButton,
                                                UIControlState state = UIControlState.Normal,
                                                UIColor tintColor = null)
        {
            var handler = GetHandler(source);
            using(var uiImage1 = await handler.LoadImageAsync(source, new CancellationToken(), 1f))
            {
                var uiImage2 = uiImage1;
                if(heightRequest > 0 && widthRequest > 0 && (uiImage1.Size.Height != heightRequest || uiImage1.Size.Width != widthRequest))
                {
                    // squish it so it fits within the size you requested.
                    uiImage2 = uiImage2.Scale(new CGSize(widthRequest, heightRequest));
                }

                var width = uiImage2.CGImage.Width;
                var uiImage3 = ClipToCircle(uiImage2, width, (float)width / 2);

                if(tintColor != null)
                {
                    targetButton.TintColor = tintColor;
                    targetButton.SetImage(uiImage3.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate), state);
                }
                else
                {
                    targetButton.SetImage(uiImage3.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), state);
                }
            }
        }

        /// <summary>
        /// See http://stackoverflow.com/a/8975222/1135847 (and the one above it
        /// for rectangular images)
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="radius">Note that if you make the radius bigger than half the width of the image, you just end up with rounded corners</param>
        /// <returns></returns>
        private static UIImage ClipToCircle(UIImage image, float width, float radius)
        {
            UIGraphics.BeginImageContext(new CGSize(width, width));
            var c = UIGraphics.GetCurrentContext();

            //Note: You need to write the Device.IsRetina code yourself 
            // radius = Device.IsRetina ? radius * 2 : radius;

            c.BeginPath();
            c.MoveTo(width, width / 2);
            c.AddArcToPoint(width, width, width / 2, width, radius);
            c.AddArcToPoint(0, width, 0, width / 2, radius);
            c.AddArcToPoint(0, 0, width / 2, 0, radius);
            c.AddArcToPoint(width, 0, width, width / 2, radius);
            c.ClosePath();
            c.Clip();

            image.Draw(new CGPoint(0, 0));
            var converted = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return converted;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if(AvatarWithNameButton.Orientation != ImageOrientation.ImageToRight) { return; }
            Control.ImageEdgeInsets = new UIEdgeInsets(0, Control.Frame.Size.Width - 2 - AvatarWithNameButton.ImageWidthRequest, 0, 0);
        }
    }
}