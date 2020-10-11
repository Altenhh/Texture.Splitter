using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
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

            using (var fileStream = new FileStream(Path.GetFileNameWithoutExtension(plistFile) + ".png", FileMode.Open))
            using (var image = new Bitmap(fileStream))
            {
                foreach (var frame in spriteSheet.Frames)
                {
                    write($"Extracting {frame.SpriteName}... ");
                    var sprite = Splitter.GetSprite(frame, image);

                    using (var graphics = Graphics.FromImage(sprite))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;

                        #region Tiling
                        if (arguments.Any(a => a == "-t" || a == "--tile"))
                        {
                            Console.Error.WriteLine("Tiling feature is currently unavailable.");

                            return;

                            var tiledImage = new Bitmap(frame.SourceSize.Width, frame.SourceSize.Height);
                            var brush = new TextureBrush(sprite) { WrapMode = WrapMode.TileFlipXY };

                            using (var g = Graphics.FromImage(tiledImage))
                            {
                                g.FillRegion(brush, new Region(new Rectangle(0, 0, frame.SourceSize.Width, frame.SourceSize.Height)));
                                
                                tiledImage.Save(Path.Combine(outputDirectory, frame.SpriteName));
                            }
                        }
                        else
                        {
                            var brush = new TextureBrush(sprite);
                            graphics.FillRectangle(brush, 0, 0, sprite.Width, sprite.Height);
                            sprite.Save(Path.Combine(outputDirectory, frame.SpriteName));
                        }
                        #endregion
                    
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                        write($"Extracting {frame.SpriteName}... Done!", ConsoleColor.Yellow);
                        
                        graphics.Dispose();
                    }
                }
                
                image.Dispose();
                fileStream.Dispose();
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