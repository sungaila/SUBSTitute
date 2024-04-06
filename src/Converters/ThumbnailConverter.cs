using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Sungaila.SUBSTitute.Converters
{
    public class ThumbnailConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not char driveLetter)
                return DependencyProperty.UnsetValue;

            StorageFolder folder;

            try
            {
                folder = StorageFolder.GetFolderFromPathAsync($"{driveLetter}:\\").GetAwaiter().GetResult();
            }
            catch
            {
                return null;
            }

            StorageItemThumbnail thumbnail;

            if (uint.TryParse(parameter as string, out var size))
                thumbnail = folder.GetThumbnailAsync(ThumbnailMode.SingleItem, size, ThumbnailOptions.UseCurrentScale).GetAwaiter().GetResult();
            else
                thumbnail = folder.GetThumbnailAsync(ThumbnailMode.SingleItem).GetAwaiter().GetResult();

            var bitmap = new BitmapImage();
            bitmap.SetSource(thumbnail);

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}