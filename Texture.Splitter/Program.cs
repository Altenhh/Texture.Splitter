using System;
using System.Diagnostics;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Texture.Splitter.SpriteSheets;

namespace Texture.Splitter
{
    public static class Program
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();

        public static void Main(string[] args)
        {
            var resultDirectory = Directory.GetCurrentDirectory() + "/Result/";

            if (!Directory.Exists(resultDirectory))
            {
                Directory.CreateDirectory(resultDirectory);
                Console.WriteLine("Result directory has been created automatically");

                // TODO: Remove.
                return;
            }

            write("Starting stopwatch");
            stopwatch.Start();

            write("Lexing file.plist");
            var plist = new Plist(resultDirectory + "file.plist");
            write("Loading spritesheet file");
            var spriteSheet = SpriteSheet.LoadSpriteSheet(plist);

            write("Done! Splitting sprites...");

            using (var image = Image.Load(resultDirectory + spriteSheet.Metadata.FileName))
            {
                foreach (var frame in spriteSheet.Frames)
                {
                    write($"Extracting {frame.SpriteName}... ");

                    if (frame.Rotated)
                    {
                        var (x, y, width, height) = frame.TextureRect;

                        frame.TextureRect = new Rectangle(x, y, height, width);
                    }

                    var sprite = image.Clone(i =>
                    {
                        i.Crop(frame.TextureRect);

                        if (frame.Rotated)
                            i.Rotate(RotateMode.Rotate270);
                    });

                    sprite.SaveAsPng(resultDirectory + "/h/" + frame.SpriteName);

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