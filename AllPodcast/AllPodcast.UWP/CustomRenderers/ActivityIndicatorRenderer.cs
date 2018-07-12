using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using AllPodcast.Views;
using Color = Windows.UI.Color;

[assembly: ExportRenderer(typeof(BusyIndicator), typeof(ActivityIndicatorRenderer))]

namespace AllPodcast.UWP.CustomRenderers
{
    public class ActivityIndicatorRenderer : ViewRenderer<BusyIndicator, ProgressRing>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<BusyIndicator> e)
        {
            base.OnElementChanged(e);

            if (Control != null) { return; }
            var progressRing = new ProgressRing
            {
                IsActive = true,
                Visibility = Windows.UI.Xaml.Visibility.Visible,
                IsEnabled = true,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))
            };

            SetNativeControl(progressRing);
        }
    }
}