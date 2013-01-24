using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.IO.IsolatedStorage;

namespace WeiboSdk
{
    public class ISHelper
    {
        public static void CopyFromContentToStorage(string fileName)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            using (var src = Application.GetResourceStream(new Uri(fileName, UriKind.Relative)).Stream)
            using (var dest = new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, store))
            {
                src.Position = 0;
                CopyStream(src, dest);
                dest.Flush();
            }
        }

        private static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        public static bool FileExist(string fileName)
        {
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
                return (iso.FileExists(fileName));
        }
    }
}
