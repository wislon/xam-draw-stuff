using DrawAvatars01.ViewModels;

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
        }

    }

    public class ImageButtonPageViewModel : BaseViewModel
    {
        public string MainText
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public void Init()
        {
            MainText = "Welcome to Page 2";
        }
    }
}
