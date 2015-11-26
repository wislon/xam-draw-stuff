
using DrawAvatars01.ViewModels;
using Xamarin.Forms;

namespace DrawAvatars01
{
    public partial class ImageButtonPage : BaseContentPage
    {
        private readonly string _dataPath;

        public ImageButtonPageViewModel ViewModel { get; private set; }

        public ImageButtonPage(string dataPath)
        {
            InitializeComponent();

            SetViewModel(new ImageButtonPageViewModel());
            ViewModel = GetViewModel<ImageButtonPageViewModel>();
            ViewModel.Init();

            _dataPath = dataPath;

            //var pickupbtn = new XLabs.Forms.Controls.ImageButton()
            //                {
            //                    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //                    WidthRequest = 200,
            //                    HeightRequest = 54,
            //                    ImageHeightRequest = 48,
            //                    ImageWidthRequest = 48,
            //                    Orientation = ImageOrientation.ImageToLeft,
            //                    Text = "John Wilson",
            //                    Source = "https://d2ojpxxtu63wzl.cloudfront.net/static/2bacd1af951486806f4f0c452c5549aa_9b5a49c8032bb699fd25f9d8ba090b26aa519e27ab686e66aa51457444be2699",
            //                    BorderRadius = 27,
            //                    BackgroundColor = Color.Silver,
            //                    BorderColor = Color.Silver,
            //                    BorderWidth = 2,
            //                };
            //containerLayout.Children.Add(pickupbtn);

        }

        public class ImageButtonPageViewModel : BaseViewModel
        {
            public string MainText
            {
                get { return GetValue<string>(); }
                set { SetValue(value); }
            }

            public string ImageFileName
            {
                get { return GetValue<string>(); }
                set { SetValue(value); }
            }

            public string ButtonText
            {
                get { return GetValue<string>(); }
                set { SetValue(value); }
            }

            public void Init()
            {
                ButtonText = "Is there a big green image?";
                MainText = "Welcome to Page 2 (from ViewModel)";
                ImageFileName = "BigGreenButton";
            }
        }
    }
}
