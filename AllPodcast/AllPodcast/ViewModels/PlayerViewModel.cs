using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace AllPodcast.ViewModels
{
    public sealed class PlayerViewModel : BaseViewModel
    {
        public static PlayerViewModel Instance { get; internal set; }

        private Stream _mediaStream;
        private string _mediaTitle;
        private int _progressbarWidth;
        
        public Stream MediaStream
        {
            get => _mediaStream;
            set => SetProperty(ref _mediaStream, value);
        }

        public string MediaTitle
        {
            get => _mediaTitle;
            set => SetProperty(ref _mediaTitle, value);
        }

        public int ProgressbarWidth
        {
            get => _progressbarWidth;
            set => SetProperty(ref _progressbarWidth, value);
        }

        public PlayerViewModel()
        {
            ProgressbarWidth = 0;
            MediaTitle = string.Empty;
            Instance = this;
        }
    }
}