using System;
using System.Windows;
using System.Windows.Controls;
using Task_Manager_Pro.ViewModels;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Task_Manager_Pro.Views.Pages;
namespace Task_Manager_Pro.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INavigationWindow
    {
        public static MainWindow Instance;
        public ViewModels.MainWindowViewModel ViewModel
        {
            get;
        }

        public MainWindow(ViewModels.MainWindowViewModel viewModel, IPageService pageService, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;
            Instance = this;
            InitializeComponent();
            SetPageService(pageService);

            navigationService.SetNavigationControl(RootNavigation);

             Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark,     // Theme type
                                           Wpf.Ui.Appearance.BackgroundType.Mica, // Background type
                                           true                                   // Whether to change accents automatically
            );
        }

        #region INavigationWindow methods

        public Frame GetFrame()
            => RootFrame;

        public INavigation GetNavigation()
            => RootNavigation;

        public bool Navigate(Type pageType)
            => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService)
            => RootNavigation.PageService = pageService;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate(typeof(SettingsPage));
            topbutton.Icon = Wpf.Ui.Common.SymbolRegular.ArrowLeft28;
            
            topbutton.Click -= Button_Click;
            topbutton.Click += Button_Click2;
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate(typeof(DashboardPage));
            topbutton.Icon = Wpf.Ui.Common.SymbolRegular.Settings28;

            topbutton.Click += Button_Click;
            topbutton.Click -= Button_Click2;
        }
    }
}