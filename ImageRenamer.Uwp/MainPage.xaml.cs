using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace ImageRenamer.Uwp
{
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

                var path = file.Path;
                var name = file.Name;
                var fileType = file.FileType;

                var newName = "";
                try
                {
                    newName = $"{FileRenamer.GetNewName(path, tmpStream)}{fileType}";

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
