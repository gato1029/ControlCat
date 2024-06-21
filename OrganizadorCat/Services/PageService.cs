using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using OrganizadorCat.Contracts.Services;
using OrganizadorCat.Helpers;
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
using OrganizadorCat.ViewModels.Persona;
using OrganizadorCat.Views.Persona;

namespace OrganizadorCat.Services
{
    public class PageService : IPageService
    {
        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();
        private readonly IServiceProvider _serviceProvider;

        public PageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Configure<MainViewModel, MainPage>();
            Configure<SettingsViewModel, SettingsPage>();
            Configure<AutoCompleteViewModel, AutoCompletePage>();
            Configure<BusyIndicatorViewModel, BusyIndicatorPage>();
            Configure<ChartsViewModel, ChartsPage>();
            Configure<DataGridViewModel, DataGridPage>();
            Configure<CarouselViewModel, CarouselPage>();
            Configure<GridControlViewModel, GridControlPage>();
            Configure<PropertyGridViewModel, PropertyGridPage>();
            Configure<UsuarioViewModel, UsuarioPage>();
            Configure<UsuarioMainViewModel, UsuarioMainPage>();
            Configure<EquipoMainViewModel, EquipoMainPage>();
            Configure<ProyectoMainViewModel, ProyectoMainPage>();
            Configure<AsignacionMainViewModel, AsignacionMainPage>();
            Configure<PersonaMainViewModel, PersonaMainPage>();
            Configure<InicioViewModel, Inicio>();
        }

        public Type GetPageType(string key)
        {
            Type pageType;
            lock (_pages)
            {
                if (!_pages.TryGetValue(key, out pageType))
                {
                    throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
                }
            }

            return pageType;
        }

        public Page GetPage(string key)
        {
            var pageType = GetPageType(key);
            return _serviceProvider.GetService(pageType) as Page;
        }

        private void Configure<VM, V>()
            where VM : Observable
            where V : Page
        {
            lock (_pages)
            {
                var key = typeof(VM).FullName;
                if (_pages.ContainsKey(key))
                {
                    throw new ArgumentException($"The key {key} is already configured in PageService");
                }

                var type = typeof(V);
                if (_pages.Any(p => p.Value == type))
                {
                    throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
                }

                _pages.Add(key, type);
            }
        }
    }
}
