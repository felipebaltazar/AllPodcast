using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AllPodcast.Extensions
{
    [Preserve(AllMembers = true)]
    [ContentProperty("Source")]
    public sealed class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider) =>
            string.IsNullOrEmpty(Source) ? null : ImageSource.FromResource(Source);
    }
}