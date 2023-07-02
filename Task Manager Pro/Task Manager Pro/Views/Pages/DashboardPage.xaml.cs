using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Task_Manager_Pro.Views.Windows;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Navigation;

namespace Task_Manager_Pro.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : INavigableView<ViewModels.DashboardViewModel>
    {
        public ViewModels.DashboardViewModel ViewModel
        {

            get;
        }

        public DashboardPage(ViewModels.DashboardViewModel viewModel)
        {
            InitializeComponent();
            GetAllProcesses();
        }
        System.Collections.Generic.List<Process> runningP;
        public void GetAllProcesses(string search = "")
        {
            listvi.Items.Clear();
            System.Collections.Generic.List<Process> runningProcesses = Process.GetProcesses().ToList();
            System.Collections.Generic.List<string> runningProcessNames = new();
            runningP = runningProcesses;
            int i = 0;
            foreach (Process p in runningProcesses)
            {
                if (runningProcessNames.Contains(p.ProcessName) || (!p.ProcessName.Contains(search) && search != string.Empty)) continue;
                runningProcessNames.Add(p.ProcessName);

                ListViewItem item = new ListViewItem();
                item.Content = p.ProcessName;
                item.Tag = p;
                listvi.Items.Add(item);
                i++;

            }

            if (search == string.Empty) processes.Text = $"Processes ({listvi.Items.Count})";
            else processes.Text = $"Processes Containing \"{search}\" ({listvi.Items.Count})";
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // refresh
            MainWindow.Instance.RootNavigation.Navigate(typeof(SettingsPage));
            MainWindow.Instance.RootNavigation.Navigate(typeof(DashboardPage));
        }
        [DllImport("shell32.dll", EntryPoint = "#61", CharSet = CharSet.Unicode)]
        public static extern int RunFileDlg(
        [In] IntPtr hWnd,
        [In] IntPtr icon,
        [In] string path,
        [In] string title,
        [In] string prompt,
        [In] uint flags);
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RunFileDlg(IntPtr.Zero, IntPtr.Zero, null, null, null, 0);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // end task
            foreach (ListViewItem selectedItem in listvi.SelectedItems)
            {
                Process selectedProcess = (Process)selectedItem.Tag;
                selectedProcess.Kill(true);
            }

            GetAllProcesses(searchbox.Text);
        }

        private void searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetAllProcesses(searchbox.Text);
        }
    }
}