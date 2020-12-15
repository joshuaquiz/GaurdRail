﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using GuardRail.Core.CommandLine;
using GuardRail.LocalClient.Data;
using GuardRail.LocalClient.Data.Interfaces;
using GuardRail.LocalClient.Data.Local;
using GuardRail.LocalClient.Data.Remote;
using GuardRail.LocalClient.Implementations;
using GuardRail.LocalClient.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GuardRail.LocalClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Service provider for the application.
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Configuration for the application.
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Initial app startup configuration.
        /// </summary>
        public App()
        {
            Startup += Application_Startup;
            Exit += Application_Exit;
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", false, true);
            Configuration = builder.Build();
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            await ConfigureDatabase();
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            StartupHelper.Startup(
                CommandLineArguments.Create(e.Args),
                mainWindow);
            mainWindow.Show();
            var dataStore = ServiceProvider.GetRequiredService<IDataStore>();
            dataStore.StartSync();
        }
        
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<IGuardRailFileProvider>(new GuardRailFileProvider(Directory.GetCurrentDirectory()));
            services.AddSingleton<IGuardRailApiClient, GuardRailApiClient>();
            services.AddDbContext<GuardRailContext>(x => x.UseSqlite("Data Source=LocalDataStore.db"));
            services.AddSingleton<IDataSink, EntityFrameWorkDataSink>();
            services.AddSingleton<IDisposable, EntityFrameWorkDataSink>();
            services.AddSingleton<IDataSink, RemoteDataSink>();
            services.AddSingleton<IDisposable, RemoteDataSink>();
            services.AddSingleton<IEnumerable<IDataSink>>(x =>
                new IDataSink[]
                {
                    x.GetRequiredService<EntityFrameWorkDataSink>(),
                    x.GetRequiredService<RemoteDataSink>()
                });
            services.AddSingleton<IDataStore, DataStore>();
            services.AddSingleton<IDisposable, DataStore>();
        }
        
        private async Task ConfigureDatabase()
        {
            using var serviceScope = ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<GuardRailContext>();
            await context.Database.MigrateAsync();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            var disposables = ServiceProvider.GetRequiredService<IEnumerable<IDisposable>>();
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }
    }
}