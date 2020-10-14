using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplePlatformGame.Components;

namespace SimplePlatformGame.GameObjects
{
    public class Player : GameObject
    {
        public float Speed { get; }
        public PlayerCollider Collider { get; }

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
                    Collider.Velocity = new Vector2(-Speed, Collider.Velocity.Y);
                    break;
                case Direction.Right:
                    Collider.Velocity = new Vector2(Speed, Collider.Velocity.Y);
                    break;
                case Direction.Up:
                    Collider.Velocity = new Vector2(Collider.Velocity.X, -Speed);
                    break;
                case Direction.Down:
                    Collider.Velocity = new Vector2(Collider.Velocity.X, Speed);
                    break;
            }
        }

        public void Stop(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    Collider.Velocity = new Vector2(0, Collider.Velocity.Y);
                    break;
                case Direction.Right:
                    Collider.Velocity = new Vector2(0, Collider.Velocity.Y);
                    break;
                case Direction.Up:
                    Collider.Velocity = new Vector2(Collider.Velocity.X, 0);
                    break;
                case Direction.Down:
                    Collider.Velocity = new Vector2(Collider.Velocity.X, 0);
                    break;
            }
        }

        public void Jump()
        {
            if (!Collider.Grounded) return;
            
            Collider.Velocity = new Vector2(Collider.Velocity.X, -Speed);
            Collider.Grounded = false;
        }

        public override void Update(Vector2 gravity)
        {
            Collider.OldPosition = Collider.Position;
            Collider.Position += Collider.Velocity;
            Collider.Velocity += gravity;
        }

        public override void Draw(SpriteBatch target)
        {
            Position = Collider.Position;
            base.Draw(target);
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