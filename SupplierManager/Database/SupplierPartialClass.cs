using System.IO;
using System.Windows.Media.Imaging;

namespace SupplierManager.Database
{
    public partial class Supplier
    {
        private BitmapImage image;
        
        public BitmapImage Picture => image ?? CreateImage();

        private BitmapImage CreateImage()
        {
            if (PictureBytes == null || PictureBytes.Length == 0) return null;

            image = new BitmapImage();
            using (var stream = new MemoryStream(PictureBytes))
            {
                stream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
            }
            
            image.Freeze();
            return image;
        }
    }
}