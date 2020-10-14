using Microsoft.Xna.Framework.Graphics;

namespace SimplePlatformGame.Components
{
    public class Sprite : ISprite
    {
        public Texture2D Texture { get; }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }
    }
}