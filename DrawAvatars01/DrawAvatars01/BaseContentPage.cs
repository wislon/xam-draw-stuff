using System;
using DrawAvatars01.ViewModels;
using Xamarin.Forms;

namespace DrawAvatars01
{
    public class BaseContentPage : ContentPage, IDisposable
    {
        private BaseViewModel _viewmodel = null;

        public T GetViewModel<T>() where T : BaseViewModel
        {
            return _viewmodel as T;
        }

        /// <summary>
        /// Attaches your view to the viewmodel you provide,
        /// and sets the BindingContext of your view to this viewmodel.
        /// </summary>
        /// <param name="vm"></param>
        public void SetViewModel(BaseViewModel vm)
        {
            if (vm != null)
            {
                _viewmodel = vm;
                BindingContext = _viewmodel;
            }
        }


        public BaseContentPage()
        {
            NavigationPage.SetBackButtonTitle(this, string.Empty);
            this.BackgroundColor = Color.White;
            this.Title = string.Empty;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public virtual void Init()
        {
            
        }

        public virtual void Dispose()
        {
            if (_viewmodel != null)
            {
                _viewmodel.Dispose();
            }
            Content = null;
        }
    }
}