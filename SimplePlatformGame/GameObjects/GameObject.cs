﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplePlatformGame.Components;

namespace SimplePlatformGame.GameObjects
{
    public abstract class GameObject
    {
        protected Vector2 Position { get; set; }
        private ISprite Sprite { get; }

        protected GameObject(Vector2 position, ISprite sprite)
        {
            Position = position;
            Sprite = sprite;
        }

        public abstract void Update(Vector2 gravity);

        public virtual void Draw(SpriteBatch target, float timeDelta)
        {
            target.Draw(Sprite.Texture, Position, Color.White);
        }
    }
}