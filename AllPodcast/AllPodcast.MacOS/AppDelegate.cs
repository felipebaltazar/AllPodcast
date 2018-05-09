using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace AllPodcast.MacOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        private const NSWindowStyle Style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

        public AppDelegate()
        {
            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);

            MainWindow = new NSWindow(rect, Style, NSBackingStore.Buffered, false)
            {
                Title = "AllPodcast",
                TitleVisibility = NSWindowTitleVisibility.Hidden
            };
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Forms.Init();
            LoadApplication(new App());
            base.DidFinishLaunching(notification);
        }

        public override NSWindow MainWindow { get; }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}