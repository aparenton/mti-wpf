using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parent_bMedecine.Utilities
{
    public class ImageManager
    {
        public static byte[] GetBytesFromImage(string imageFile)
        {
            byte[] b = File.ReadAllBytes(imageFile);
            return b;
        }
    }
}
