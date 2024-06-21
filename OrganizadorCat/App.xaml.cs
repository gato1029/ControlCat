using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OrganizadorCat.Contracts.Services;
using OrganizadorCat.Contracts.Views;
using OrganizadorCat.Models;
using OrganizadorCat.Services;
using OrganizadorCat.ViewModels;
using OrganizadorCat.ViewModel;
using OrganizadorCat.ViewModels.Equipo;
using OrganizadorCat.ViewModels.Proyecto;
using OrganizadorCat.ViewModels.Usuario;
using OrganizadorCat.Views;
using OrganizadorCat.Views.Asignacion;
using OrganizadorCat.Views.Equipo;
using OrganizadorCat.Views.Proyecto;
using OrganizadorCat.Views.Usuario;
using OrganizadorCat.ViewModels.Dashboard;
using OrganizadorCat.Views.Dashboard;
using OrganizadorCat.Views.Login;
using OrganizadorCat.ViewModels.Persona;
using OrganizadorCat.Views.Persona;

namespace OrganizadorCat
{
    // For more inforation about application lifecyle events see https://docs.microsoft.com/dotnet/framework/wpf/app-development/application-management-overview

    // WPF UI elements use language en-US by default.
    // If you need to support other cultures make sure you add converters and review dates and numbers in your UI to ensure everything adapts correctly.
    // Tracking issue for improving this is https://github.com/dotnet/wpf/issues/1946
    public partial class App : Application
    {
        private IHost _host;

        public T GetService<T>()
            where T : class
            => _host.Services.GetService(typeof(T)) as T;

        public App()
        {
            // Add your Syncfusion license key for WPF platform with corresponding Syncfusion NuGet version referred in project. For more information about license key see https://help.syncfusion.com/common/essential-studio/licensing/license-key.
             Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzI0MzY2NEAzMjM1MmUzMDJlMzBGakYyM1lNVmZEMG1iL3Q2NFdqNkQ3WXlrUG1lSWIxN1Qyd0xCcWRKL0FZPQ=="); 
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {

       
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            // For more information about .NET generic host see  https://docs.microsoft.com/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
            _host = Host.CreateDefaultBuilder(e.Args)
                    .ConfigureAppConfiguration(c =>
                    {
                        c.SetBasePath(appLocation);
                    })
                    .ConfigureServices(ConfigureServices)
                    .Build();

            await _host.StartAsync();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // TODO WTS: Register your services, viewmodels and pages here

            // App Host
            services.AddHostedService<ApplicationHostService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Services
            services.AddSingleton<IWindowManagerService, WindowManagerService>();
            services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
            services.AddSingleton<ISystemService, SystemService>();
            services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Views and ViewModels
            services.AddTransient<IShellWindow, ShellWindow>();
            services.AddTransient<ShellViewModel>();

            services.AddTransient<InicioViewModel>();
            services.AddTransient<Inicio>();

            //services.AddTransient<MainViewModel>();
            //services.AddTransient<MainPage>();

            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddTransient<AutoCompleteViewModel>();
            services.AddTransient<AutoCompletePage>();

            services.AddTransient<BusyIndicatorViewModel>();
            services.AddTransient<BusyIndicatorPage>();

            services.AddTransient<ChartsViewModel>();
            services.AddTransient<ChartsPage>();

            services.AddTransient<DataGridViewModel>();
            services.AddTransient<DataGridPage>();

            services.AddTransient<CarouselViewModel>();
            services.AddTransient<CarouselPage>();

            services.AddTransient<GridControlViewModel>();
            services.AddTransient<GridControlPage>();

            services.AddTransient<PropertyGridViewModel>();
            services.AddTransient<PropertyGridPage>();


            services.AddTransient<UsuarioViewModel>();
            services.AddTransient<UsuarioPage>();

            services.AddTransient<UsuarioMainViewModel>();
            services.AddTransient<UsuarioMainPage>();

            services.AddTransient<EquipoMainViewModel>();
            services.AddTransient<EquipoMainPage>();

            services.AddTransient<ProyectoMainViewModel>();
            services.AddTransient<ProyectoMainPage>();

            services.AddTransient<AsignacionMainViewModel>();
            services.AddTransient<AsignacionMainPage>();

            services.AddTransient<PersonaMainViewModel>();
            services.AddTransient<PersonaMainPage>();


            services.AddTransient<IShellDialogWindow, ShellDialogWindow>();
            services.AddTransient<ShellDialogViewModel>();

            // Configuration
            services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            _host = null;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0
        }
    }
}
