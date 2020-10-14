using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplePlatformGame.Components;

namespace SimplePlatformGame.GameObjects
{
    public class Player : GameObject
    {
        private readonly float _runSpeed;
        private readonly float _jumpSpeed;

        public PlayerCollider Collider { get; }

        public Player(Vector2 position, float runSpeed, float jumpSpeed, ISprite sprite) : base(position, sprite)
        {
            _runSpeed = runSpeed;
            _jumpSpeed = jumpSpeed;
            Collider = new PlayerCollider(sprite.Bounds, Position);
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    Collider.Velocity = new Vector2(-_runSpeed, Collider.Velocity.Y);
                    break;
                case Direction.Right:
                    Collider.Velocity = new Vector2(_runSpeed, Collider.Velocity.Y);
                    break;
                case Direction.Up:
                    Collider.Velocity = new Vector2(Collider.Velocity.X, -_runSpeed);
                    break;
                case Direction.Down:
                    Collider.Velocity = new Vector2(Collider.Velocity.X, _runSpeed);
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
            
            Collider.Velocity = new Vector2(Collider.Velocity.X, -_jumpSpeed);
            Collider.Grounded = false;
        }

        public override void Update(Vector2 gravity)
        {
            Collider.OldPosition = Collider.Position;
            Collider.Position += Collider.Velocity;
            Collider.Velocity += gravity;
        }

        public override void Draw(SpriteBatch target, float timeDelta)
        {
            Position = Collider.Position * timeDelta + Collider.OldPosition * (1f - timeDelta);
            base.Draw(target, timeDelta);
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