using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.Core.Helpers
{
    public static class SoundHelper
    {
        // Play embedded WAV file
        public static void PlayEmbeddedWav(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"DashBoard.Core.Assets.sounds.{fileName}";  // Modify with your namespace and folder structure

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using (var player = new SoundPlayer(stream))
                {
                    player.Play();
                }
            }
            else
            {
                Debug.WriteLine($"Embedded WAV file '{fileName}' not found.");
            }
        }
    }
}
