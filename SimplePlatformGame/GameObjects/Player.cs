using Microsoft.Xna.Framework;
using SimplePlatformGame.Components;

namespace SimplePlatformGame.GameObjects
{
    public class Player : GameObject
    {
        public float Speed { get; }
        public Collider Collider { get; }

        public Player(Vector2 position, float speed, ISprite sprite) : base(position, sprite)
        {
            Speed = speed;
            Collider = new PlayerCollider(sprite.Bounds, Position);
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

        public override void Update()
        {
            throw new System.NotImplementedException();
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