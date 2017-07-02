using System;
using Windows.Media.Core;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExtendedAppBarButtonOnMediaCommandBar
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnPickVideoClick(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.VideosLibrary
            };

            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".mkv");
            openPicker.FileTypeFilter.Add(".avi");

            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                Media.Source = MediaSource.CreateFromStorageFile(file);
                Media.MediaPlayer.Play();
            }
        }
    }
}
