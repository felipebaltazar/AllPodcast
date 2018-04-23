using AllPodcast.Helpers;

namespace AllPodcast.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        #region Local Variables

        private string title = string.Empty;
        private bool isBusy = false;

        #endregion

        #region Acessible Variables
        
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }
        

		public IDataStore<Dog> DataStore => DependencyService.Get<IDataStore<Dog>>();

        #endregion
    }
}