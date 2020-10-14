using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplePlatformGame.Components;

namespace SimplePlatformGame.GameObjects
{
    public abstract class GameObject
    {
        public Vector2 Position { get; protected set; }
        public ISprite Sprite { get; }
        public int Width { get; }
        public int Height { get; }

        protected GameObject(Vector2 position, ISprite sprite)
        {
            Position = position;
            Sprite = sprite;
            Width = sprite.Width;
            Height = sprite.Height;
        }

        public abstract void Update(Vector2 gravity);

        public virtual void Draw(SpriteBatch target)
        {
            target.Draw(Sprite.Texture, Position, Color.White);
        }
    }
}