using AllPodcast.Helpers;

namespace AllPodcast.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        #region Local Variables

        private string _title = string.Empty;
        private bool _isBusy = false;

        #endregion

        #region Acessible Variables
        
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public virtual void OnViewAppearing()
        {
        }

        public virtual void OnViewDisappearing()
        {
        }
        
		//public IDataStore<IPodcastEpisode> DataStore => DependencyService.Get<IDataStore<IPodcastEpisode>>();

        #endregion
    }
}