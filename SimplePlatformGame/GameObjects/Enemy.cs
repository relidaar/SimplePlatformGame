using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplePlatformGame.Components;

namespace SimplePlatformGame.GameObjects
{
    public class Enemy : GameObject
    {
        private readonly float _speed;
        private Direction _currentDirection;
        
        public Enemy(Vector2 position, float speed, Direction direction, Sprite sprite) 
            : base(position, sprite, new Collider(sprite.Bounds, position))
        {
            _speed = speed;
            _currentDirection = direction;
        }

        private void Move()
        {
            switch (_currentDirection)
            {
                case Direction.Left:
                    Collider.Velocity = new Vector2(-_speed, Collider.Velocity.Y);
                    break;
                case Direction.Right:
                    Collider.Velocity = new Vector2(_speed, Collider.Velocity.Y);
                    break;
            }
        }

        public void ChangeDirection()
        {
            switch (_currentDirection)
            {
                case Direction.Left:
                    _currentDirection = Direction.Right;
                    break;
                case Direction.Right:
                    _currentDirection = Direction.Left;
                    break;
            }
        }

        public override void Update(Vector2 gravity)
        {
            Move();
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