using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ImageRenamer;
using System.Globalization;

namespace ImageRenamer.Wpf
{
    public class MyColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;

            if (text.Contains("renamed")) { return Brushes.DarkGray; }
            if (text.Contains("find")) { return Brushes.Red; }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] files;
        ObservableCollection<string> viewList = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            PreviewList.ItemsSource = viewList;

            if (App.Args.Length > 0)
            {
                files = App.Args.Where(x => IsMedia(x)).ToArray();
                UpdateListView();
            }
            else
            {
                files = new string[0];
            }
        }

        private bool IsMedia(string arg)
        {
            var extension = Path.GetExtension(arg);
            if (extension == ".jpg") return true;
            if (extension == ".jpeg") return true;
            if (extension == ".mp4") return true;
            if (extension == ".png") return true;
            return false;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Media";
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Photos/Videos | *.jpg; *.jpeg; *.mp4; *.png";
            dlg.Multiselect = true;

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                files = dlg.FileNames;
            }

            UpdateListView();
        }

        private void UpdateListView()
        {
            foreach (string file in files)
            {
                var name = Path.GetFileName(file);
                var path = Path.GetFullPath(file);
                var fileType = Path.GetExtension(file);

                var newName = "";
                try
                {
                    newName = $"{FileRenamer.GetNewName(path)}{fileType}";

                    if (newName == name)
                    {
                        name = $"✔ {name}";
                        newName = "Already renamed 💯";
                    }
                    else
                    {
                        name = $"⚙ {name}";
                    }
                }
                catch (Exception)
                {
                    name = $"💥 {name}";
                    newName = "Can't find a new name ☹";
                }

                viewList.Add($"{name} ➡ {newName}");
            }
        }

        private async void RenameButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            var renamer = new FileRenamer(new Mover());
            await Task.Run(() => renamer.RenameFiles(files.ToList()));
            viewList.Clear();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            viewList.Clear();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
