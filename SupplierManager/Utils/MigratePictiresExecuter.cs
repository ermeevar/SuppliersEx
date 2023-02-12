using System;
using System.IO;
using System.Net.Mime;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SupplierManager.Utils
{
    /// <summary>
    /// Добавить картинки в БД
    /// </summary>
    public static class MigratePictiresExecuter
    {
        private const string pathPrefix = @"C:\Users\ermee\OneDrive\Рабочий стол\Work\ктитс\практика\лопушок\products\";
        public static void Migrate()
        {
            foreach (var supplier in MainWindow.Context.Suppliers)
            {
                var image = new BitmapImage(new Uri(pathPrefix + supplier.PathString));
                var encoder = new JpegBitmapEncoder();  
                encoder.Frames.Add(BitmapFrame.Create(image)); 
                
                using (var stream = new MemoryStream())  
                {  
                    encoder.Save(stream);  
                    supplier.PictureBytes = stream.ToArray();  
                }  

            }
            
            MainWindow.Context.SaveChanges();
        }
    }
}