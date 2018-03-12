using System;
using System.IO;

using Mobile.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndLoad))]
namespace Mobile.Droid
{
    public class SaveAndLoad : ISaveAndLoad
    {
        public string LoadText(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return System.IO.File.ReadAllText(filePath);
        }

        public void SaveText(string filename, string text)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            System.IO.File.WriteAllText(filePath, text);
        }
    }
}