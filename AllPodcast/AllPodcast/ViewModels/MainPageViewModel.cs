using PodcastManager.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllPodcast.Extensions;
using PodcastManager;
using PodcastManager.Enums;

namespace AllPodcast.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private List<IPodcastEpisode> _podcastCollection;
        private readonly Manager _podcastManager;
        
        public List<PodcastType> SignedPodcasts { get; set; }

        public List<IPodcastEpisode> PodcastCollection
        {
            get => _podcastCollection;
            set => SetProperty(ref _podcastCollection, value);
        }
        
        public MainPageViewModel()
        {
            _podcastManager = new Manager();
            PodcastCollection = new List<IPodcastEpisode>();

            SignedPodcasts = new List<PodcastType>()
            {
                PodcastType.NerdCast,
                PodcastType.NaoOuvo
            };
        }

        public override async void OnViewAppearing()
        {
            base.OnViewAppearing();
            await GetSignedPodcastsList();
        }

        private async Task GetSignedPodcastsList()
        {
            IsBusy = true;

            var managers = SignedPodcasts
                .Select(type => _podcastManager.GetManager(type))
                .Where(m => m != null);
            
            var collection = await managers.SelectManyAsync(m=> m.GetPodcastListAsync());
            var episodeCollection = collection.OrderBy(p=> p.Title).Take(20).ToList();

            PodcastCollection = episodeCollection;

            IsBusy = false;
        }
    }
}
