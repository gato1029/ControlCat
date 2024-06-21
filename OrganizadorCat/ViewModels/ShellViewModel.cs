using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using BTCat.Generico;
using OrganizadorCat.Contracts.Services;
using OrganizadorCat.Helpers;
using OrganizadorCat.Models;
using OrganizadorCat.Properties;
using OrganizadorCat.Repositorios;
using OrganizadorCat.ViewModel;
using OrganizadorCat.ViewModels.Dashboard;
using OrganizadorCat.ViewModels.Equipo;
using OrganizadorCat.ViewModels.Persona;
using OrganizadorCat.ViewModels.Proyecto;
using OrganizadorCat.ViewModels.Usuario;
using Syncfusion.UI.Xaml.NavigationDrawer;
using Syncfusion.Windows.Shared;

namespace OrganizadorCat.ViewModels
{
    public class ShellViewModel : Observable
    {
        private ICommand _optionsMenuItemInvokedCommand;

        private readonly INavigationService _navigationService;
        private object _selectedMenuItem;
        private RelayCommand _goBackCommand;
        private ICommand _menuItemInvokedCommand;
        private ICommand _loadedCommand;
        private ICommand _unloadedCommand;

        public object SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set
            {
                if (value as NavigationPaneItem == null)
                {
                    Set(ref _selectedMenuItem, ((FrameworkElement)value).DataContext, "SelectedMenuItem");
                }
                else
                {
                    Set(ref _selectedMenuItem, value, "SelectedMenuItem");
                }
                //NavigateTo((_selectedMenuItem as NavigationPaneItem).TargetType);
				if (_selectedMenuItem is NavigationPaneItem navigationPaneItem && navigationPaneItem.TargetType != null)
                {
                    NavigateTo(navigationPaneItem.TargetType);
                }
            }
        }

        public void UpdateFillColor(SolidColorBrush FillColor)
        {
            foreach (var item in MenuItems)
            {
                (item as NavigationPaneItem).Path.Fill = FillColor;
            }
            SetttingsIconColor = FillColor;
        }

        private SolidColorBrush setttingsIconColor;

        public SolidColorBrush SetttingsIconColor
        {
            get { return setttingsIconColor; }
            set
            {
                setttingsIconColor = value;
                OnPropertyChanged(nameof(SetttingsIconColor));
            }
        }

        // TODO WTS: Change the icons and titles for all HamburgerMenuItems here.
        public ObservableCollection<NavigationPaneItem> MenuItems { get; set; } = new ObservableCollection<NavigationPaneItem>()
        {                
        };


        public ICommand OptionsMenuItemInvokedCommand => _optionsMenuItemInvokedCommand ?? (_optionsMenuItemInvokedCommand = new RelayCommand(OnOptionsMenuItemInvoked));

        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, CanGoBack));

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand(OnUnloaded));

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SetttingsIconColor = new SolidColorBrush(Colors.Black);
           // Configuracion();
        }

        public void Configuracion()
        {
          var resumen =  new NavigationPaneItem()
            {
                Label = "Resumen",
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M0 6C0 2.68629 2.68629 0 6 0H42C45.3137 0 48 2.68629 48 6V42C48 45.3137 45.3137 48 42 48H6C2.68629 48 0 45.3137 0 42V6ZM6 2C3.79086 2 2 3.79086 2 6V13H46V6C46 3.79086 44.2091 2 42 2H6ZM46 15H2V42C2 44.2091 3.79086 46 6 46H42C44.2091 46 46 44.2091 46 42V15ZM7 23C7 21.3431 8.34315 20 10 20H12C13.6569 20 15 21.3431 15 23V25C15 26.6569 13.6569 28 12 28H10C8.34315 28 7 26.6569 7 25V23ZM10 22C9.44772 22 9 22.4477 9 23V25C9 25.5523 9.44772 26 10 26H12C12.5523 26 13 25.5523 13 25V23C13 22.4477 12.5523 22 12 22H10ZM19 21C19 20.4477 19.4477 20 20 20H40C40.5523 20 41 20.4477 41 21C41 21.5523 40.5523 22 40 22H20C19.4477 22 19 21.5523 19 21ZM19 27C19 26.4477 19.4477 26 20 26H34C34.5523 26 35 26.4477 35 27C35 27.5523 34.5523 28 34 28H20C19.4477 28 19 27.5523 19 27ZM7 36C7 34.3431 8.34315 33 10 33H12C13.6569 33 15 34.3431 15 36V38C15 39.6569 13.6569 41 12 41H10C8.34315 41 7 39.6569 7 38V36ZM10 35C9.44772 35 9 35.4477 9 36V38C9 38.5523 9.44772 39 10 39H12C12.5523 39 13 38.5523 13 38V36C13 35.4477 12.5523 35 12 35H10ZM19 34C19 33.4477 19.4477 33 20 33H40C40.5523 33 41 33.4477 41 34C41 34.5523 40.5523 35 40 35H20C19.4477 35 19 34.5523 19 34ZM19 40C19 39.4477 19.4477 39 20 39H34C34.5523 39 35 39.4477 35 40C35 40.5523 34.5523 41 34 41H20C19.4477 41 19 40.5523 19 40Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(InicioViewModel)
            };

          var personas =  new NavigationPaneItem()
            {
                Label = "Personas",
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M0 6C0 2.68629 2.68629 0 6 0H42C45.3137 0 48 2.68629 48 6V42C48 45.3137 45.3137 48 42 48H6C2.68629 48 0 45.3137 0 42V6ZM6 2C3.79086 2 2 3.79086 2 6V13H46V6C46 3.79086 44.2091 2 42 2H6ZM46 15H2V42C2 44.2091 3.79086 46 6 46H42C44.2091 46 46 44.2091 46 42V15ZM7 23C7 21.3431 8.34315 20 10 20H12C13.6569 20 15 21.3431 15 23V25C15 26.6569 13.6569 28 12 28H10C8.34315 28 7 26.6569 7 25V23ZM10 22C9.44772 22 9 22.4477 9 23V25C9 25.5523 9.44772 26 10 26H12C12.5523 26 13 25.5523 13 25V23C13 22.4477 12.5523 22 12 22H10ZM19 21C19 20.4477 19.4477 20 20 20H40C40.5523 20 41 20.4477 41 21C41 21.5523 40.5523 22 40 22H20C19.4477 22 19 21.5523 19 21ZM19 27C19 26.4477 19.4477 26 20 26H34C34.5523 26 35 26.4477 35 27C35 27.5523 34.5523 28 34 28H20C19.4477 28 19 27.5523 19 27ZM7 36C7 34.3431 8.34315 33 10 33H12C13.6569 33 15 34.3431 15 36V38C15 39.6569 13.6569 41 12 41H10C8.34315 41 7 39.6569 7 38V36ZM10 35C9.44772 35 9 35.4477 9 36V38C9 38.5523 9.44772 39 10 39H12C12.5523 39 13 38.5523 13 38V36C13 35.4477 12.5523 35 12 35H10ZM19 34C19 33.4477 19.4477 33 20 33H40C40.5523 33 41 33.4477 41 34C41 34.5523 40.5523 35 40 35H20C19.4477 35 19 34.5523 19 34ZM19 40C19 39.4477 19.4477 39 20 39H34C34.5523 39 35 39.4477 35 40C35 40.5523 34.5523 41 34 41H20C19.4477 41 19 40.5523 19 40Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(PersonaMainViewModel)
            };

          var usuarios=   new NavigationPaneItem()
            {
                Label = "Usuarios",
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M0 6C0 2.68629 2.68629 0 6 0H42C45.3137 0 48 2.68629 48 6V42C48 45.3137 45.3137 48 42 48H6C2.68629 48 0 45.3137 0 42V6ZM6 2C3.79086 2 2 3.79086 2 6V13H46V6C46 3.79086 44.2091 2 42 2H6ZM46 15H2V42C2 44.2091 3.79086 46 6 46H42C44.2091 46 46 44.2091 46 42V15ZM7 23C7 21.3431 8.34315 20 10 20H12C13.6569 20 15 21.3431 15 23V25C15 26.6569 13.6569 28 12 28H10C8.34315 28 7 26.6569 7 25V23ZM10 22C9.44772 22 9 22.4477 9 23V25C9 25.5523 9.44772 26 10 26H12C12.5523 26 13 25.5523 13 25V23C13 22.4477 12.5523 22 12 22H10ZM19 21C19 20.4477 19.4477 20 20 20H40C40.5523 20 41 20.4477 41 21C41 21.5523 40.5523 22 40 22H20C19.4477 22 19 21.5523 19 21ZM19 27C19 26.4477 19.4477 26 20 26H34C34.5523 26 35 26.4477 35 27C35 27.5523 34.5523 28 34 28H20C19.4477 28 19 27.5523 19 27ZM7 36C7 34.3431 8.34315 33 10 33H12C13.6569 33 15 34.3431 15 36V38C15 39.6569 13.6569 41 12 41H10C8.34315 41 7 39.6569 7 38V36ZM10 35C9.44772 35 9 35.4477 9 36V38C9 38.5523 9.44772 39 10 39H12C12.5523 39 13 38.5523 13 38V36C13 35.4477 12.5523 35 12 35H10ZM19 34C19 33.4477 19.4477 33 20 33H40C40.5523 33 41 33.4477 41 34C41 34.5523 40.5523 35 40 35H20C19.4477 35 19 34.5523 19 34ZM19 40C19 39.4477 19.4477 39 20 39H34C34.5523 39 35 39.4477 35 40C35 40.5523 34.5523 41 34 41H20C19.4477 41 19 40.5523 19 40Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(UsuarioMainViewModel)
            };

          var equipos = new NavigationPaneItem()
            {
                Label = "Equipos",
                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M0 6C0 2.68629 2.68629 0 6 0H42C45.3137 0 48 2.68629 48 6V42C48 45.3137 45.3137 48 42 48H6C2.68629 48 0 45.3137 0 42V6ZM6 2C3.79086 2 2 3.79086 2 6V13H46V6C46 3.79086 44.2091 2 42 2H6ZM46 15H2V42C2 44.2091 3.79086 46 6 46H42C44.2091 46 46 44.2091 46 42V15ZM7 23C7 21.3431 8.34315 20 10 20H12C13.6569 20 15 21.3431 15 23V25C15 26.6569 13.6569 28 12 28H10C8.34315 28 7 26.6569 7 25V23ZM10 22C9.44772 22 9 22.4477 9 23V25C9 25.5523 9.44772 26 10 26H12C12.5523 26 13 25.5523 13 25V23C13 22.4477 12.5523 22 12 22H10ZM19 21C19 20.4477 19.4477 20 20 20H40C40.5523 20 41 20.4477 41 21C41 21.5523 40.5523 22 40 22H20C19.4477 22 19 21.5523 19 21ZM19 27C19 26.4477 19.4477 26 20 26H34C34.5523 26 35 26.4477 35 27C35 27.5523 34.5523 28 34 28H20C19.4477 28 19 27.5523 19 27ZM7 36C7 34.3431 8.34315 33 10 33H12C13.6569 33 15 34.3431 15 36V38C15 39.6569 13.6569 41 12 41H10C8.34315 41 7 39.6569 7 38V36ZM10 35C9.44772 35 9 35.4477 9 36V38C9 38.5523 9.44772 39 10 39H12C12.5523 39 13 38.5523 13 38V36C13 35.4477 12.5523 35 12 35H10ZM19 34C19 33.4477 19.4477 33 20 33H40C40.5523 33 41 33.4477 41 34C41 34.5523 40.5523 35 40 35H20C19.4477 35 19 34.5523 19 34ZM19 40C19 39.4477 19.4477 39 20 39H34C34.5523 39 35 39.4477 35 40C35 40.5523 34.5523 41 34 41H20C19.4477 41 19 40.5523 19 40Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(EquipoMainViewModel)
            };

          var proyectos = new NavigationPaneItem()
            {
                Label = "Proyectos",

                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M0 6C0 2.68629 2.68629 0 6 0H42C45.3137 0 48 2.68629 48 6V42C48 45.3137 45.3137 48 42 48H6C2.68629 48 0 45.3137 0 42V6ZM6 2C3.79086 2 2 3.79086 2 6V13H46V6C46 3.79086 44.2091 2 42 2H6ZM46 15H2V42C2 44.2091 3.79086 46 6 46H42C44.2091 46 46 44.2091 46 42V15ZM7 23C7 21.3431 8.34315 20 10 20H12C13.6569 20 15 21.3431 15 23V25C15 26.6569 13.6569 28 12 28H10C8.34315 28 7 26.6569 7 25V23ZM10 22C9.44772 22 9 22.4477 9 23V25C9 25.5523 9.44772 26 10 26H12C12.5523 26 13 25.5523 13 25V23C13 22.4477 12.5523 22 12 22H10ZM19 21C19 20.4477 19.4477 20 20 20H40C40.5523 20 41 20.4477 41 21C41 21.5523 40.5523 22 40 22H20C19.4477 22 19 21.5523 19 21ZM19 27C19 26.4477 19.4477 26 20 26H34C34.5523 26 35 26.4477 35 27C35 27.5523 34.5523 28 34 28H20C19.4477 28 19 27.5523 19 27ZM7 36C7 34.3431 8.34315 33 10 33H12C13.6569 33 15 34.3431 15 36V38C15 39.6569 13.6569 41 12 41H10C8.34315 41 7 39.6569 7 38V36ZM10 35C9.44772 35 9 35.4477 9 36V38C9 38.5523 9.44772 39 10 39H12C12.5523 39 13 38.5523 13 38V36C13 35.4477 12.5523 35 12 35H10ZM19 34C19 33.4477 19.4477 33 20 33H40C40.5523 33 41 33.4477 41 34C41 34.5523 40.5523 35 40 35H20C19.4477 35 19 34.5523 19 34ZM19 40C19 39.4477 19.4477 39 20 39H34C34.5523 39 35 39.4477 35 40C35 40.5523 34.5523 41 34 41H20C19.4477 41 19 40.5523 19 40Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(ProyectoMainViewModel)
            };

          var asignaciones = new NavigationPaneItem()
            {
                Label = "Asignaciones",

                Path = new Path()
                {
                    Width = 15,
                    Height = 15,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Data = Geometry.Parse("M0 6C0 2.68629 2.68629 0 6 0H42C45.3137 0 48 2.68629 48 6V42C48 45.3137 45.3137 48 42 48H6C2.68629 48 0 45.3137 0 42V6ZM6 2C3.79086 2 2 3.79086 2 6V13H46V6C46 3.79086 44.2091 2 42 2H6ZM46 15H2V42C2 44.2091 3.79086 46 6 46H42C44.2091 46 46 44.2091 46 42V15ZM7 23C7 21.3431 8.34315 20 10 20H12C13.6569 20 15 21.3431 15 23V25C15 26.6569 13.6569 28 12 28H10C8.34315 28 7 26.6569 7 25V23ZM10 22C9.44772 22 9 22.4477 9 23V25C9 25.5523 9.44772 26 10 26H12C12.5523 26 13 25.5523 13 25V23C13 22.4477 12.5523 22 12 22H10ZM19 21C19 20.4477 19.4477 20 20 20H40C40.5523 20 41 20.4477 41 21C41 21.5523 40.5523 22 40 22H20C19.4477 22 19 21.5523 19 21ZM19 27C19 26.4477 19.4477 26 20 26H34C34.5523 26 35 26.4477 35 27C35 27.5523 34.5523 28 34 28H20C19.4477 28 19 27.5523 19 27ZM7 36C7 34.3431 8.34315 33 10 33H12C13.6569 33 15 34.3431 15 36V38C15 39.6569 13.6569 41 12 41H10C8.34315 41 7 39.6569 7 38V36ZM10 35C9.44772 35 9 35.4477 9 36V38C9 38.5523 9.44772 39 10 39H12C12.5523 39 13 38.5523 13 38V36C13 35.4477 12.5523 35 12 35H10ZM19 34C19 33.4477 19.4477 33 20 33H40C40.5523 33 41 33.4477 41 34C41 34.5523 40.5523 35 40 35H20C19.4477 35 19 34.5523 19 34ZM19 40C19 39.4477 19.4477 39 20 39H34C34.5523 39 35 39.4477 35 40C35 40.5523 34.5523 41 34 41H20C19.4477 41 19 40.5523 19 40Z"),
                    Fill = new SolidColorBrush(Colors.Black),
                    Stretch = Stretch.Fill,
                },
                TargetType = typeof(AsignacionMainViewModel)
            };

          var usuarioActual = UsuarioLogeado.UsuarioActual;
            var dbContext = MongoDBContext.Instance;
            var _equipoRepository = new EquipoRepository(dbContext);
            var data = _equipoRepository.GetEquiposPorUsuario(usuarioActual);

            if (data.Count > 0)
            {
                MenuItems.Add(resumen);
                MenuItems.Add(proyectos);
                MenuItems.Add(asignaciones);
            }
            else
            {
                //MessageBoxCat msg = new MessageBoxCat();
                //msg.Mensaje("Mientras no tenga un equipo asociado, no podra asignar proyectos");
            }
            if (usuarioActual.Privilegios !=null)
            {
                if (usuarioActual.Privilegios.Contains("Referente"))
                {
                    MenuItems.Add(personas);
                    MenuItems.Add(usuarios);
                    MenuItems.Add(equipos);
                }
            }
            
           
            
        }

        private void OnLoaded()
        {
            _navigationService.Navigated += OnNavigated;
        }

        private void OnUnloaded()
        {
            _navigationService.Navigated -= OnNavigated;
        }

        private bool CanGoBack()
            => _navigationService.CanGoBack;

        private void OnGoBack()
            => _navigationService.GoBack();

        private void NavigateTo(Type targetViewModel)
        {
            if (targetViewModel != null)
            {
                _navigationService.NavigateTo(targetViewModel.FullName);
            }
        }

        private void OnNavigated(object sender, string viewModelName)
        {
            var item = MenuItems
                        .OfType<NavigationPaneItem>()
                        .FirstOrDefault(i => viewModelName == i.TargetType?.FullName);
            if (item != null)
            {
                SelectedMenuItem = item;
            }

            GoBackCommand.OnCanExecuteChanged();
        }

        private void OnOptionsMenuItemInvoked()
            => NavigateTo(typeof(SettingsViewModel));
    }

    public class NavigationPaneItem
    {
        public string Label { get; set; }
        public Path Path { get; set; }
        public Type TargetType { get; set; }

    }
}
