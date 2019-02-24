using ImageRenamer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ImageRenamerUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        IReadOnlyList<IStorageItem> files;
        ObservableCollection<string> viewList = new ObservableCollection<string>();
        public FileActivatedEventArgs fileEventArgs { get; set; } = null;

        public MainPage()
        {
            this.InitializeComponent();
            PreviewList.ItemsSource = viewList;
        }

        public void FileActivateRename()
        {
            files = fileEventArgs.Files;
            UpdateListViewAsync();
        }

        private async void OpenButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            viewList.Clear();
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".mp4");

            files = await picker.PickMultipleFilesAsync();

            UpdateListViewAsync();
        }

        private async void UpdateListViewAsync()
        {
            foreach (StorageFile file in files)
            {
                var tmpStream = await file.OpenStreamForReadAsync();
                viewList.Add($"{file.Name} --> {FileRenamer.GetNewName(file.Path, tmpStream)}{file.FileType}");
            }
        }

        private async void RenameButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            foreach (StorageFile file in files)
            {
                var tmpStream = await file.OpenStreamForReadAsync();
                var newName = $"{FileRenamer.GetNewName(file.Path, tmpStream)}{file.FileType}";

                while (true)
                {
                    try
                    {
                        await file.RenameAsync(newName);
                        break;
                    }
                    catch
                    {
                        newName = $"extra_{newName}";
                        // TODO: Make this hack less disgusting. 
                    }
                }
            }
            viewList.Clear();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            viewList.Clear();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.Core.CoreApplication.Exit();
        }
    }
}
