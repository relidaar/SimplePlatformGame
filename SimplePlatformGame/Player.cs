using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimplePlatformGame
{
    public class Player : IGameObject
    {
        public Vector2 Position { get; private set; }
        public bool IsStatic { get; }
        public float Speed { get; private set; }
        
        public ISprite Sprite { get; }
        public int Width { get; }
        public int Height { get; }

        public Player(Vector2 position, float speed, ISprite sprite)
        {
            Position = position;
            Speed = speed;
            IsStatic = false;
            Sprite = sprite;
            Width = sprite.Width;
            Height = sprite.Height;
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    Position = new Vector2(Position.X - Speed, Position.Y);
                    break;
                case Direction.Right:
                    Position = new Vector2(Position.X + Speed, Position.Y);
                    break;
                case Direction.Up:
                    Position = new Vector2(Position.X, Position.Y - Speed);
                    break;
                case Direction.Down:
                    Position = new Vector2(Position.X , Position.Y + Speed);
                    break;
            }
        }

        public void Draw(SpriteBatch target)
        {
            target.Draw(Sprite.Texture, Position, Color.White);
        }
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}