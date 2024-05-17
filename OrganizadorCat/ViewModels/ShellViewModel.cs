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
using OrganizadorCat.Properties;
using OrganizadorCat.Repositorios;
using OrganizadorCat.ViewModel;
using OrganizadorCat.ViewModels.Equipo;
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
        	new NavigationPaneItem() { 
                        Label = Resources.ShellMainPage,
                        Path = new Path()
                        {
                            Width = 15,
                            Height = 15,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Data = Geometry.Parse("M28.414 4H7V44H39V14.586ZM29 7.414 35.586 14H29ZM9 42V6H27V16H37V42Z"),
                            Fill = new SolidColorBrush(Colors.Black),
                            Stretch = Stretch.Fill,
                        },
                        TargetType = typeof(MainViewModel) 
            },
        	new NavigationPaneItem() { 
                        Label = Resources.ShellAutoCompletePage,
                        Path = new Path()
                        {
                            Width = 15,
                            Height = 15,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Data = Geometry.Parse("M4 0C1.79086 0 0 1.79086 0 4V14C0 16.2091 1.79086 18 4 18H44C46.2091 18 48 16.2091 48 14V4C48 1.79086 46.2091 0 44 0H4ZM2 4C2 2.89543 2.89543 2 4 2H44C45.1046 2 46 2.89543 46 4V14C46 15.1046 45.1046 16 44 16H4C2.89543 16 2 15.1046 2 14V4ZM2 21C2 20.4477 1.55228 20 1 20C0.447715 20 0 20.4477 0 21V41C0 44.866 3.13401 48 7 48H41C44.866 48 48 44.866 48 41V21C48 20.4477 47.5523 20 47 20C46.4477 20 46 20.4477 46 21V41C46 43.7614 43.7614 46 41 46H7C4.23858 46 2 43.7614 2 41V21ZM5 9C5 7.34315 6.34315 6 8 6H22C23.6569 6 25 7.34315 25 9C25 10.6569 23.6569 12 22 12H8C6.34315 12 5 10.6569 5 9ZM8 8C7.44772 8 7 8.44772 7 9C7 9.55228 7.44772 10 8 10H22C22.5523 10 23 9.55228 23 9C23 8.44771 22.5523 8 22 8H8ZM8 24C6.34315 24 5 25.3431 5 27C5 28.6569 6.34315 30 8 30H30C31.6569 30 33 28.6569 33 27C33 25.3431 31.6569 24 30 24H8ZM7 27C7 26.4477 7.44772 26 8 26H30C30.5523 26 31 26.4477 31 27C31 27.5523 30.5523 28 30 28H8C7.44772 28 7 27.5523 7 27ZM5 37C5 35.3431 6.34315 34 8 34H38C39.6569 34 41 35.3431 41 37C41 38.6569 39.6569 40 38 40H8C6.34315 40 5 38.6569 5 37ZM8 36C7.44772 36 7 36.4477 7 37C7 37.5523 7.44772 38 8 38H38C38.5523 38 39 37.5523 39 37C39 36.4477 38.5523 36 38 36H8ZM30 5C30 4.44772 29.5523 4 29 4C28.4477 4 28 4.44772 28 5V13C28 13.5523 28.4477 14 29 14C29.5523 14 30 13.5523 30 13V5Z"),
                            Fill = new SolidColorBrush(Colors.Black),
                            Stretch = Stretch.Fill,
                        },
                        TargetType = typeof(AutoCompleteViewModel) 
            },
        	new NavigationPaneItem() { 
                        Label = Resources.ShellBusyIndicatorPage,
                        Path = new Path()
                        {
                            Width = 15,
                            Height = 15,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Data = Geometry.Parse("M0 8C0 4.68629 2.68629 2 6 2H42C45.3137 2 48 4.68629 48 8V40C48 43.3137 45.3137 46 42 46H6C2.68629 46 0 43.3137 0 40V8ZM6 4C3.79086 4 2 5.79086 2 8V40C2 42.2091 3.79086 44 6 44H42C44.2091 44 46 42.2091 46 40V8C46 5.79086 44.2091 4 42 4H6ZM28.8571 36.3929C28.8571 38.4048 27.2262 40.0357 25.2143 40.0357C23.2024 40.0357 21.5714 38.4048 21.5714 36.3929C21.5714 34.381 23.2024 32.75 25.2143 32.75C27.2262 32.75 28.8571 34.381 28.8571 36.3929ZM41 21.8214C41 19.8095 39.369 18.1785 37.3572 18.1785C35.3453 18.1785 33.7143 19.8095 33.7143 21.8214C33.7143 23.8333 35.3453 25.4642 37.3572 25.4642C39.369 25.4642 41 23.8333 41 21.8214ZM30.5342 12.0398C31.6213 10.9527 33.3838 10.9527 34.4709 12.0398C35.558 13.1269 35.558 14.8895 34.4709 15.9766C33.3838 17.0637 31.6213 17.0637 30.5342 15.9766C29.4471 14.8895 29.4471 13.1269 30.5342 12.0398ZM37.8202 34.8605C39.1489 33.5318 39.1489 31.3776 37.8202 30.0489C36.4915 28.7203 34.3373 28.7203 33.0086 30.0489C31.68 31.3776 31.68 33.5318 33.0086 34.8605C34.3373 36.1892 36.4915 36.1892 37.8202 34.8605ZM26.4286 10.8928C26.4286 12.2341 25.3412 13.3214 24 13.3214C22.6587 13.3214 21.5714 12.2341 21.5714 10.8928C21.5714 9.55154 22.6587 8.46423 24 8.46423C25.3412 8.46423 26.4286 9.55154 26.4286 10.8928ZM11.8572 23.6428C11.8572 24.6488 11.0417 25.4643 10.0357 25.4643C9.02978 25.4643 8.21429 24.6488 8.21429 23.6428C8.21429 22.6369 9.02978 21.8214 10.0357 21.8214C11.0417 21.8214 11.8572 22.6369 11.8572 23.6428ZM11.2881 31.0739C11.892 30.4699 12.8712 30.4699 13.4751 31.0739C14.0791 31.6778 14.0791 32.657 13.4751 33.261C12.8712 33.8649 11.892 33.8649 11.2881 33.261C10.6841 32.657 10.6841 31.6778 11.2881 31.0739ZM16.3119 16.3391C17.1574 15.4935 17.1574 14.1227 16.3119 13.2772C15.4663 12.4316 14.0955 12.4316 13.2499 13.2772C12.4044 14.1227 12.4044 15.4935 13.2499 16.3391C14.0955 17.1846 15.4663 17.1846 16.3119 16.3391Z"),
                            Fill = new SolidColorBrush(Colors.Black),
                            Stretch = Stretch.Fill,
                        },
                        TargetType = typeof(BusyIndicatorViewModel) 
            },
        	new NavigationPaneItem() { 
                        Label = Resources.ShellChartsPage,
                        Path = new Path()
                        {
                            Width = 15,
                            Height = 15,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Data = Geometry.Parse("M22 0C19.7909 0 18 1.79086 18 4V44C18 46.2091 19.7909 48 22 48H26C28.2091 48 30 46.2091 30 44V4C30 1.79086 28.2091 0 26 0H22ZM20 4C20 2.89543 20.8954 2 22 2H26C27.1046 2 28 2.89543 28 4V44C28 45.1046 27.1046 46 26 46H22C20.8954 46 20 45.1046 20 44V4ZM4 22C1.79086 22 0 23.7909 0 26V44C0 46.2091 1.79086 48 4 48H8C10.2091 48 12 46.2091 12 44V26C12 23.7909 10.2091 22 8 22H4ZM2 26C2 24.8954 2.89543 24 4 24H8C9.10457 24 10 24.8954 10 26V44C10 45.1046 9.10457 46 8 46H4C2.89543 46 2 45.1046 2 44V26ZM40 15C37.7909 15 36 16.7909 36 19V44C36 46.2091 37.7909 48 40 48H44C46.2091 48 48 46.2091 48 44V19C48 16.7909 46.2091 15 44 15H40ZM38 19C38 17.8954 38.8954 17 40 17H44C45.1046 17 46 17.8954 46 19V44C46 45.1046 45.1046 46 44 46H40C38.8954 46 38 45.1046 38 44V19Z"),
                            Fill = new SolidColorBrush(Colors.Black),
                            Stretch = Stretch.Fill,
                        },
                        TargetType = typeof(ChartsViewModel) 
            },
        	new NavigationPaneItem() { 
                        Label = Resources.ShellDataGridPage,
                        Path = new Path()
                        {
                            Width = 15,
                            Height = 15,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Data = Geometry.Parse("M0 6C0 2.68629 2.68629 0 6 0H42C45.3137 0 48 2.68629 48 6V42C48 45.3137 45.3137 48 42 48H6C2.68629 48 0 45.3137 0 42V6ZM6 2C3.79086 2 2 3.79086 2 6V13H46V6C46 3.79086 44.2091 2 42 2H6ZM46 15H36V24H46V15ZM46 26H36V35H46V26ZM46 37H36V46H42C44.2091 46 46 44.2091 46 42V37ZM34 46V37H25V46H34ZM23 46V37H14V46H23ZM12 46V37H2V42C2 44.2091 3.79086 46 6 46H12ZM2 35H12V26H2V35ZM2 24H12V15H2V24ZM14 15V24H23V15H14ZM25 15V24H34V15H25ZM34 26H25V35H34V26ZM23 35V26H14V35H23Z"),
                            Fill = new SolidColorBrush(Colors.Black),
                            Stretch = Stretch.Fill,
                        },
                        TargetType = typeof(DataGridViewModel) 
            },
        	new NavigationPaneItem() { 
                        Label = Resources.ShellCarouselPage,
                        Path = new Path()
                        {
                            Width = 15,
                            Height = 15,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Data = Geometry.Parse("M12 5H36C38.2091 5 40 6.79086 40 9V22.3692L31.0529 26.6394C29.97 27.1563 28.6789 26.6099 28.296 25.4727L26.4338 19.9427C25.4189 16.9287 21.4626 16.2472 19.4977 18.7479L8.01607 33.361C8.00543 33.2421 8 33.1217 8 33V9C8 6.79086 9.79086 5 12 5ZM8.87811 35.501C9.61119 36.4149 10.7373 37 12 37H36C38.2091 37 40 35.2091 40 33V24.5853L31.9143 28.4444C29.7486 29.4781 27.1664 28.3853 26.4006 26.111L24.5384 20.581C24.031 19.074 22.0528 18.7332 21.0704 19.9836L8.87811 35.501ZM6 9C6 5.68629 8.68629 3 12 3H36C39.3137 3 42 5.68629 42 9V33C42 36.3137 39.3137 39 36 39H12C8.68629 39 6 36.3137 6 33V9ZM2 13C2 12.4477 1.55228 12 1 12C0.447715 12 0 12.4477 0 13V29C0 29.5523 0.447714 30 0.999999 30C1.55228 30 2 29.5523 2 29L2 13ZM46 12C46.5523 12 47 12.4477 47 13V29C47 29.5523 46.5523 30 46 30C45.4477 30 45 29.5523 45 29V13C45 12.4477 45.4477 12 46 12ZM30 10C28.8954 10 28 10.8954 28 12C28 13.1046 28.8954 14 30 14C31.1046 14 32 13.1046 32 12C32 10.8954 31.1046 10 30 10ZM26 12C26 9.79086 27.7909 8 30 8C32.2091 8 34 9.79086 34 12C34 14.2091 32.2091 16 30 16C27.7909 16 26 14.2091 26 12ZM13 45C13 45.5523 12.5523 46 12 46C11.4477 46 11 45.5523 11 45C11 44.4477 11.4477 44 12 44C12.5523 44 13 44.4477 13 45ZM15 45C15 46.6569 13.6569 48 12 48C10.3431 48 9 46.6569 9 45C9 43.3431 10.3431 42 12 42C13.6569 42 15 43.3431 15 45ZM24 48C25.6569 48 27 46.6569 27 45C27 43.3431 25.6569 42 24 42C22.3431 42 21 43.3431 21 45C21 46.6569 22.3431 48 24 48ZM37 45C37 45.5523 36.5523 46 36 46C35.4477 46 35 45.5523 35 45C35 44.4477 35.4477 44 36 44C36.5523 44 37 44.4477 37 45ZM39 45C39 46.6569 37.6569 48 36 48C34.3431 48 33 46.6569 33 45C33 43.3431 34.3431 42 36 42C37.6569 42 39 43.3431 39 45Z"),
                            Fill = new SolidColorBrush(Colors.Black),
                            Stretch = Stretch.Fill,
                        },
                        TargetType = typeof(CarouselViewModel) 
            },
        	new NavigationPaneItem() { 
                        Label = Resources.ShellGridControlPage,
                        Path = new Path()
                        {
                            Width = 15,
                            Height = 15,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Data = Geometry.Parse("M0 6C0 2.68629 2.68629 0 6 0H42C45.3137 0 48 2.68629 48 6V14V25V27C48 27.5523 47.5523 28 47 28C46.4477 28 46 27.5523 46 27V26H35H25V36V46H27C27.5523 46 28 46.4477 28 47C28 47.5523 27.5523 48 27 48H24H13H6C2.68629 48 0 45.3137 0 42V36V25V14V6ZM2 26V35H12V26H2ZM12 24H2V15H12V24ZM14 26V35H23V26H14ZM23 24H14V15H23V24ZM25 24H34V15H25V24ZM24 13H35H46V6C46 3.79086 44.2091 2 42 2H6C3.79086 2 2 3.79086 2 6V13H13H24ZM46 15H36V24H46V15ZM23 37H14V46H23V37ZM12 46V37H2V42C2 44.2091 3.79086 46 6 46H12ZM34.7151 28.6816C35.5946 26.701 38.4054 26.701 39.2849 28.6816L39.9068 30.0821C40.0149 30.3256 40.2948 30.4415 40.5434 30.3458L41.9734 29.7953C43.9958 29.0166 45.9834 31.0042 45.2047 33.0266L44.6542 34.4566C44.5585 34.7052 44.6744 34.9851 44.9179 35.0932L46.3184 35.7151C48.299 36.5946 48.299 39.4054 46.3184 40.2849L44.9179 40.9068C44.6744 41.0149 44.5585 41.2948 44.6542 41.5434L45.2047 42.9734C45.9834 44.9958 43.9958 46.9834 41.9734 46.2047L40.5434 45.6542C40.2948 45.5585 40.0149 45.6744 39.9068 45.9179L39.2849 47.3184C38.4054 49.299 35.5946 49.299 34.7151 47.3184L34.0932 45.9179C33.9851 45.6744 33.7052 45.5585 33.4566 45.6542L32.0266 46.2047C30.0042 46.9834 28.0166 44.9958 28.7953 42.9734L29.3458 41.5434C29.4415 41.2948 29.3256 41.0149 29.0821 40.9068L27.6816 40.2849C25.701 39.4054 25.701 36.5946 27.6816 35.7151L29.0821 35.0932C29.3256 34.9851 29.4415 34.7052 29.3458 34.4566L28.7953 33.0266C28.0166 31.0042 30.0042 29.0166 32.0266 29.7953L33.4566 30.3458C33.7052 30.4415 33.9851 30.3256 34.0932 30.0821L34.7151 28.6816ZM37.457 29.4933C37.2811 29.0971 36.7189 29.0971 36.543 29.4933L35.9211 30.8938C35.3807 32.111 33.9809 32.6908 32.738 32.2123L31.308 31.6617C30.9035 31.506 30.506 31.9035 30.6617 32.308L31.2123 33.738C31.6908 34.9809 31.111 36.3807 29.8938 36.9211L28.4933 37.543C28.0971 37.7189 28.0971 38.2811 28.4933 38.457L29.8938 39.0789C31.111 39.6193 31.6908 41.0191 31.2123 42.262L30.6617 43.692C30.506 44.0965 30.9035 44.494 31.308 44.3383L32.738 43.7877C33.9809 43.3092 35.3807 43.889 35.9211 45.1062L36.543 46.5067C36.7189 46.9029 37.2811 46.9029 37.457 46.5067L38.0789 45.1062C38.6193 43.889 40.0191 43.3092 41.262 43.7877L42.692 44.3383C43.0965 44.494 43.494 44.0965 43.3383 43.692L42.7877 42.262C42.3092 41.0191 42.889 39.6193 44.1062 39.0789L45.5067 38.457C45.9029 38.2811 45.9029 37.7189 45.5067 37.543L44.1062 36.9211C42.889 36.3807 42.3092 34.9809 42.7877 33.738L43.3383 32.308C43.494 31.9035 43.0965 31.506 42.692 31.6617L41.262 32.2123C40.0191 32.6908 38.6193 32.111 38.0789 30.8938L37.457 29.4933ZM35 38C35 36.8954 35.8954 36 37 36C38.1046 36 39 36.8954 39 38C39 39.1046 38.1046 40 37 40C35.8954 40 35 39.1046 35 38ZM37 34C34.7909 34 33 35.7909 33 38C33 40.2091 34.7909 42 37 42C39.2091 42 41 40.2091 41 38C41 35.7909 39.2091 34 37 34Z"),
                            Fill = new SolidColorBrush(Colors.Black),
                            Stretch = Stretch.Fill,
                        },
                        TargetType = typeof(GridControlViewModel) 
            },
        	new NavigationPaneItem() { 
                        Label = Resources.ShellPropertyGridPage,
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
                        TargetType = typeof(PropertyGridViewModel) 
            },

                   new NavigationPaneItem() {
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
            },

                     new NavigationPaneItem() {
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
            },
                             new NavigationPaneItem() {
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
            },
                                              new NavigationPaneItem() {
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
            },
        };


        public ICommand OptionsMenuItemInvokedCommand => _optionsMenuItemInvokedCommand ?? (_optionsMenuItemInvokedCommand = new RelayCommand(OnOptionsMenuItemInvoked));

        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, CanGoBack));

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand(OnUnloaded));

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SetttingsIconColor = new SolidColorBrush(Colors.Black);

            var dbContext = MongoDBContext.Instance;
            var _usuarioRepository = new UsuarioRepository(dbContext);

            var usuario = UsuarioLogeado.Login(_usuarioRepository.GetUsuarioById("66466a0478499aa2997494e2"));

            var _equipoRepository = new EquipoRepository(dbContext);

            var data = _equipoRepository.GetEquiposPorUsuario(usuario);
            usuario.Equipos = data;
            EquipoActual.SetearEquipo(usuario.Equipos[0]);

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
