using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AllPodcast
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
            MessagingCenter.Send((Page)this, "OnAppearing");
	    }

	    protected override void OnDisappearing()
	    {
	        base.OnDisappearing();
	        MessagingCenter.Send((Page)this, "OnDisappearing");
	    }
	}
}
