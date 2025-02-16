using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using CurveFittingApp.Services;
using CurveFittingApp.ViewModels;

namespace CurveFittingApp
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ICurveFittingService, CurveFittingService>();
            // Add other services here
        }
    }
}

