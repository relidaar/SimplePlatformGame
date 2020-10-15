using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplePlatformGame.Components;

namespace SimplePlatformGame.GameObjects
{
    public class GameObject
    {
        public Collider Collider { get; }
        protected Vector2 Position { get; set; }
        private Sprite Sprite { get; }

        public GameObject(Vector2 position, Sprite sprite, Collider collider)
        {
            Collider = collider;
            Position = position;
            Sprite = sprite;
        }

        public GameObject(Vector2 position, Sprite sprite, bool isStatic)
            : this(position, sprite, new Collider(sprite.Bounds, position, isStatic))
        {
        }

        public virtual void Update(Vector2 gravity)
        {
        }

        public virtual void Draw(SpriteBatch target, float timeDelta)
        {
            target.Draw(Sprite.Texture, Position, Color.White);
        }
    }
}