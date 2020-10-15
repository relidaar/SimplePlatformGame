using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplePlatformGame.Components;

namespace SimplePlatformGame.GameObjects
{
    public class Player : GameObject
    {
        public bool Alive { get; set; } = true;
        private readonly float _runSpeed;
        private readonly float _jumpSpeed;

        public Player(Vector2 position, float runSpeed, float jumpSpeed, Sprite sprite) 
            : base(position, sprite, new PlayerCollider(sprite.Bounds, position))
        {
            _runSpeed = runSpeed;
            _jumpSpeed = jumpSpeed;
        }

        public void Move(Direction direction)
        {
            if (!Alive) return;
            switch (direction)
            {
                case Direction.Left:
                    Collider.Velocity = new Vector2(-_runSpeed, Collider.Velocity.Y);
                    break;
                case Direction.Right:
                    Collider.Velocity = new Vector2(_runSpeed, Collider.Velocity.Y);
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
            }
        }

        public void Jump()
        {
            if (!Alive) return;
            var collider = (PlayerCollider) Collider;
            if (!collider.Grounded) return;
            
            collider.Velocity = new Vector2(collider.Velocity.X, -_jumpSpeed);
            collider.Grounded = false;
        }

        public override void Update(Vector2 gravity)
        {
            if (!Alive) return;
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
}