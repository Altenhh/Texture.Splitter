using System.Drawing;
using System.Numerics;

namespace Texture.Splitter.SpriteSheets
{
    /// <summary>
    /// Contains all sprite frames.
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// The name of the sprite.
        /// </summary>
        public string SpriteName { get; set; }

        /// <summary>
        /// A reference to sprites containing the same image data.
        /// </summary>
        public object[] Aliases { get; set; }

        /// <summary>
        /// The offset of the sprite's untrimmed center to the sprite's trimmed center.
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Size of the trimmed sprite.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Size of the untrimmed sprite.
        /// </summary>
        public Size SourceSize { get; set; }

        /// <summary>
        /// Sprite's position and size in the texture.
        /// </summary>
        public Rectangle TextureRect { get; set; }

        /// <summary>
        /// True if the sprite is rotated.
        /// </summary>
        public bool Rotated { get; set; }

        public override string ToString() => SpriteName;
    }
}