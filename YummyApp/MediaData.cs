using System.Windows.Media.Imaging;
using System.IO;

namespace YummyApp
{
    /// <summary>
    /// Model for Media Data
    /// Author Maria Leticia Leoncio Barbosa
    /// </summary>
    
    // class to describe the displayable data
    public class MediaData
    {
        // refer the id from the table for this object media object
        public int Id { get; set; }
        
        // the title of the media object
        public string Title { get; set; }

        // the description of the media object
        public string Description { get; set; }

        // the image of the media object
        public BitmapImage ImageData { get; set; }

        // utility method to convert byte array to a displayable image
        public BitmapImage ByteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

    }
}
