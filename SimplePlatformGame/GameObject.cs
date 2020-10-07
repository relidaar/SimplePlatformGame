using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimplePlatformGame
{
    public class GameObject : IGameObject
    {
        public Vector2 Position { get; }
        public bool IsStatic { get; }

        public ISprite Sprite { get; }
        public int Width { get; }
        public int Height { get; }

        public GameObject(Vector2 position, bool isStatic, ISprite sprite)
        {
            Position = position;
            IsStatic = isStatic;
            Sprite = sprite;
            Width = sprite.Width;
            Height = sprite.Height;
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void Draw(SpriteBatch target)
        {
            target.Draw(Sprite.Texture, Position, Color.White);
        }
    }
}