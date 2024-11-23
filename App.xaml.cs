using Microsoft.Extensions.DependencyInjection;
using System;

namespace SimpleImmichFrame
{
	public partial class App : Application
	{
		private readonly IServiceProvider serviceProvider;

		public App(IServiceProvider serviceProvider)
		{
			InitializeComponent();
			this.serviceProvider = serviceProvider;
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			var aa =new NavigationPage(this.serviceProvider.GetRequiredService<PhotoPage>());
			return new Window(aa);
		}
	}
}