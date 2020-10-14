using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimplePlatformGame.Components
{
    public class Sprite : ISprite
    {
        public Texture2D Texture { get; }
        public Rectangle Bounds => Texture.Bounds;
        public int Width => Texture.Width;
        public int Height => Texture.Height;

        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }
    }
}