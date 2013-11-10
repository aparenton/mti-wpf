using System.IO;

namespace parent_bMedecine.Utilities
{
    /// <summary>
    /// Class to handle images
    /// </summary>
    public class ImageManager
    {
        /// <summary>
        /// Get the byte[] from a file path
        /// </summary>
        /// <param name="imageFile">the image file path</param>
        /// <returns>byte array data</returns>
        public static byte[] GetBytesFromImage(string imageFile)
        {
            byte[] b = File.ReadAllBytes(imageFile);
            return b;
        }
    }
}