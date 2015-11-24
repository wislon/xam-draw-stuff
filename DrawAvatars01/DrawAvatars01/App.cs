using System;
using System.Collections.Generic;
using System.Windows.Input;
using NControl.Abstractions;
using NGraphics;
using Splat;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;
using Font = NGraphics.Font;
using Point = NGraphics.Point;
using Size = NGraphics.Size;
using TextAlignment = NGraphics.TextAlignment;

namespace DrawAvatars01
{
    public class App : Application, IEnableLogger
    {
        private readonly string _dataPath;

        public App(string dataPath)
        {
            _dataPath = dataPath;

            // The root page of your application
            MainPage = new NavigationPage(new Page1ContentPage(_dataPath));

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    public class Page1ContentPage : ContentPage
    {
        private readonly string _dataPath;
        private readonly Image _drawnImage;
        private AvatarButton _avatar;

        public Page1ContentPage(string dataPath)
        {
            _dataPath = dataPath;
            var btnImage = new Button
                               {
                                   Text = "Click me for picture",
                               };
            btnImage.Clicked += GenerateImage;

            _avatar = new AvatarButton() {WidthRequest = 48, HeightRequest = 48, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center}; //{AvatarInitial= "B"};

            _drawnImage = new Image()
                          {
                              WidthRequest = 128,
                              HeightRequest = 128,
                              VerticalOptions = LayoutOptions.CenterAndExpand,
                              HorizontalOptions = LayoutOptions.CenterAndExpand,
                              Aspect = Aspect.AspectFill 
                          };

            var btnPage2 = new Button()
                               {
                                   BorderColor = Color.Blue,
                                   BorderWidth = 2,
                                   BorderRadius = 4,
                                   Text = "Go to Page 2",
                               };
            btnPage2.Clicked += GoToPage2Clicked;

            Content = new StackLayout
                      {
                          Padding = 4,
                          Spacing = 16,
                          Orientation = StackOrientation.Vertical,
                          VerticalOptions = LayoutOptions.Center,
                          HorizontalOptions = LayoutOptions.FillAndExpand,
                          Children =
                          {
                              btnImage,
                              _avatar,
                              _drawnImage,
                              btnPage2,
                          }
                      };
        }

        private async void GoToPage2Clicked(object sender, EventArgs eventArgs)
        {
            await this.Navigation.PushAsync(new ImageButtonPage(_dataPath));
        }

        private async void GenerateImage(object sender, EventArgs e)
        {
            _avatar.AvatarInitial = "C";
            _avatar.FillColor = Color.Blue;

            /*
             * 
                    01 - #71cfbf
                    02 - #e3a685
                    03 - #7eaae5
                    04 - #ca7c7a
                    05 - #bc789b
                    06 - #dd8785
             */
            var colours = new List<string>()
                          {
                              "#71cfbf","#e3a685", "#7eaae5", "#ca7c7a", "#bc789b", "#dd8785"
                          };

            // (NGraphics.IImageCanvas)
            double canvasSize = 96;
            double textSize = canvasSize / 2;

            var canvasx = new ApplePlatform().CreateImageCanvas(new Size(canvasSize), 2);
            //canvasx.FillEllipse(rect, new RadialGradientBrush(new NGraphics.Color("#0000ff"), new NGraphics.Color("#ff0000")));

            //canvasx.DrawRectangle(rect, new Pen("#9966CC", 5D), new SolidBrush(NGraphics.Color.FromRGB(0)));
            var rect = new Rect(new Size(canvasSize));

            var initials = new string[]
                            {
                                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R",
                                "S", "T", "U", "V", "W", "X", "Y", "Z"
                            };

            foreach(var colourString in colours)
            {
                var backColour = new NGraphics.Color(colourString);
                canvasx.DrawRectangle(rect, new Pen(backColour), new SolidBrush(backColour));

                double penWidth = 1D;

                var innerBox = new Rect(new Point(rect.Center.X - textSize / 2, rect.Center.Y - textSize / 2), new Size(textSize));
                // DEBUG only to see where the text appears within the parent container for centering purposes
                //canvasx.DrawRectangle(innerBox, new Pen(Colors.LightGray));

                // bug in NGraphics on ios draws text ABOVE the rectangle you specify
                var textRec = new Rect(new Point(innerBox.Left + penWidth, innerBox.Top + innerBox.Height - penWidth - (textSize * 0.1)), new Size(textSize));
                var font = new Font("Metric-Regular", textSize);

                canvasx.DrawText("W", textRec, font, TextAlignment.Center, Pens.White, Brushes.White);
                //canvasx.DrawText("W", rect.Center, new Font(), new NGraphics.Color("#9966cc") );

                string fileName = System.IO.Path.Combine(_dataPath, string.Format("{0:dd-MM-yyyy-HH-mm-ss}.png", DateTime.Now));
                canvasx.GetImage().SaveAsPng(fileName);
                LogHost.Default.Debug(string.Format("image saved to {0}", fileName));

                _drawnImage.Source = ImageSource.FromFile(fileName);
                // var s = colourString;
                // await MainPage.DisplayAlert("Done", string.Format("Done with {0}", s), "OK");
            }
            Device.BeginInvokeOnMainThread(async () => await this.DisplayAlert("All Done", "All Done", "OK"));
            
        }

    };

    /// <summary>
    /// Circular button control.
    /// </summary>
    public class AvatarButton : NControlView
    {
        private readonly Label _label;
        private readonly NControlView _circle;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AvatarButton"/> class.
        /// </summary>
        public AvatarButton()
        {
            HeightRequest = 44;
            WidthRequest = 44;

            _label = new Label
                     {
                         Text = "+",
                         TextColor = Xamarin.Forms.Color.White,
                         FontSize = 17,
                         BackgroundColor = Xamarin.Forms.Color.Transparent,
                         XAlign = Xamarin.Forms.TextAlignment.Center,
                         YAlign = Xamarin.Forms.TextAlignment.Center,
                     };

            _circle = new NControlView
                       {

                           DrawingFunction = (canvas1, rect) =>
                           {
                               var fillColor = new NGraphics.Color(FillColor.R,
                                                                   FillColor.G, FillColor.B, FillColor.A);

                               canvas1.FillEllipse(rect, fillColor);
                           }
                       };

            Content = new Grid
                      {
                          Children =
                          {
                              _circle,
                              _label,
                          }
                      };
        }

        /// <summary>
        /// The Command property.
        /// </summary>
        public static BindableProperty CommandProperty =
            BindableProperty.Create<AvatarButton, ICommand>(p => p.Command, null,
                                                                     propertyChanged: (bindable, oldValue, newValue) =>
                                                                     {
                                                                         var ctrl = (AvatarButton)bindable;
                                                                         ctrl.Command = newValue;
                                                                     });

        /// <summary>
        /// Gets or sets the Command of the AvatarButton instance.
        /// </summary>
        /// <value>The color of the buton.</value>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// The CommandParameter property.
        /// </summary>
        public static BindableProperty CommandParameterProperty =
            BindableProperty.Create<AvatarButton, object>(p => p.CommandParameter, null,
                                                                   propertyChanged: (bindable, oldValue, newValue) =>
                                                                   {
                                                                       var ctrl = (AvatarButton)bindable;
                                                                       ctrl.CommandParameter = newValue;
                                                                   });

        /// <summary>
        /// Gets or sets the CommandParameter of the AvatarButton instance.
        /// </summary>
        /// <value>The color of the buton.</value>
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// The FillColor property.
        /// </summary>
        public static BindableProperty FillColorProperty =
            BindableProperty.Create<AvatarButton, Xamarin.Forms.Color>(p => p.FillColor,
                                                                                Xamarin.Forms.Color.Gray,
                                                                                BindingMode.TwoWay,
                                                                                propertyChanged:
                                                                                    (bindable, oldValue, newValue) =>
                                                                                    {
                                                                                        var ctrl =
                                                                                            (AvatarButton)
                                                                                                bindable;
                                                                                        ctrl.FillColor = newValue;
                                                                                    });

        /// <summary>
        /// Gets or sets the FillColor of the AvatarButton instance.
        /// </summary>
        /// <value>The color of the buton.</value>
        public Xamarin.Forms.Color FillColor
        {
            get { return (Xamarin.Forms.Color)GetValue(FillColorProperty); }
            set
            {
                SetValue(FillColorProperty, value);
                _circle.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the FA icon.
        /// </summary>
        /// <value>The FA icon.</value>
        public string AvatarInitial
        {
            get { return _label.Text; }
            set { _label.Text = value; }
        }

        public override bool TouchesBegan(System.Collections.Generic.IEnumerable<NGraphics.Point> points)
        {
            base.TouchesBegan(points);
            this.ScaleTo(0.9, 65, Easing.CubicInOut);
            return true;
        }

        public override bool TouchesCancelled(System.Collections.Generic.IEnumerable<NGraphics.Point> points)
        {
            base.TouchesCancelled(points);
            this.ScaleTo(1.0, 65, Easing.CubicInOut);
            return true;
        }

        public override bool TouchesEnded(System.Collections.Generic.IEnumerable<NGraphics.Point> points)
        {
            base.TouchesEnded(points);
            this.ScaleTo(1.0, 65, Easing.CubicInOut);
            if(Command != null && Command.CanExecute(CommandParameter))
                Command.Execute(CommandParameter);

            return true;
        }
    }

    public class AvatarLabel00 : NControlView
    {
        private NGraphics.Color _custLineColour = NGraphics.Color.FromRGB(0);
        public NGraphics.Color CustLineColour
        {
            get
            {
                return _custLineColour;
            }
            set
            {
                _custLineColour = value;
                Invalidate();
            }
        }

        public AvatarLabel00()
        {
            //Content = new BoxView(){HeightRequest = 48, WidthRequest = 48, Color = Color.Navy};
            Content = new BoxView() { HeightRequest = 128, WidthRequest = 128, BackgroundColor = Color.Transparent};
        }

        public override void Draw(ICanvas canvas, Rect rect)
        {
            // base.Draw(canvas, rect);
            canvas.DrawLine(rect.Left, rect.Top, rect.Width, rect.Top, _custLineColour, 3);
            canvas.DrawLine(rect.Left, rect.Height, rect.Width, rect.Height, _custLineColour, 3);
            var font = new Font {Size = 20};
            var pen = new Pen(Colors.Blue, width: 10);
            var brush = new SolidBrush(Colors.White);
            canvas.DrawRectangle(new Point(20, 20), new Size(20), pen, brush);
            //canvas.DrawText("B", rect, font, TextAlignment.Center, pen);
        }
    }

}
