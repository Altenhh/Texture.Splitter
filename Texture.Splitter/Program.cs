using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using Texture.Splitter.SpriteSheets;

namespace Texture.Splitter
{
    public static class Program
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();

        public static void Main(string[] args)
        {
            var arguments = args.ToList();
            var plistFile = Path.GetFullPath(arguments[0]);
            var outputDirectory = Path.GetFullPath(arguments[arguments.IndexOf("-o") + 1]);

            if (!File.Exists(plistFile))
                throw new ArgumentException("File does not exist.");
            
            if (!Directory.Exists(outputDirectory))
                throw new ArgumentException("Directory does not exist.");

            write("Starting stopwatch");
            stopwatch.Start();

            write("Lexing file.plist");
            var plist = new Plist(plistFile);
            write("Loading spritesheet file");
            var spriteSheet = SpriteSheet.LoadSpriteSheet(plist);

            write("Done! Splitting sprites...");

            using (var image = Image.Load(Path.GetFileNameWithoutExtension(plistFile) + ".png"))
            {
                foreach (var frame in spriteSheet.Frames)
                {
                    write($"Extracting {frame.SpriteName}... ");

                    new Task(() => Splitter.GetSprite(frame, image).SaveAsPng(Path.Combine(outputDirectory, frame.SpriteName))).Start();

                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                    write($"Extracting {frame.SpriteName}... Done!", ConsoleColor.Yellow);
                }
            }

            write("Finished!");
            stopwatch.Stop();
        }

        private static void write(string message, ConsoleColor col = ConsoleColor.Gray)
        {
            if (stopwatch.ElapsedMilliseconds > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(stopwatch.ElapsedMilliseconds.ToString().PadRight(8));
            }

            Console.ForegroundColor = col;
            Console.WriteLine(message);
        }
    }
}