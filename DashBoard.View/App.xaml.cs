using DashBoard.Core.EventAggregator;
using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.Model;
using DashBoard.Model.interfaces;
using DashBoard.Model.Services;
using DashBoard.Model.Services.Interface;
using DashBoard.ViewModel;
using DashBoard.ViewModel.interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace DashBoard.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Register ViewModels
            services.AddSingleton<IShellViewModel, ShellViewModel>();

            //Register Factories
            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddSingleton<IModelFactory, ModelFactory>();
            
            // Register Views
            services.AddSingleton<Shell>();

            // Add other services as needed
            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddSingleton<IConfigService, ConfigService>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<Shell>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<IShellViewModel>();
            mainWindow.Show();
        }
    }
}