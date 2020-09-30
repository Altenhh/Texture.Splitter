using System;
using System.IO;
using Texture.Splitter.SpriteSheets;

namespace Texture.Splitter
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var resultDirectory = Directory.GetCurrentDirectory() + "/Result";

            if (!Directory.Exists(resultDirectory))
            {
                Directory.CreateDirectory(resultDirectory);
                Console.WriteLine("Result directory has been created automatically");
                
                // TODO: Remove.
                return;
            }

            var plist = new Plist(resultDirectory + "/file.plist");
            var spriteSheet = SpriteSheet.LoadSpriteSheet(plist);
            Console.WriteLine(spriteSheet);
        }
    }
}