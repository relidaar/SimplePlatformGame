﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplePlatformGame.Components;

namespace SimplePlatformGame.GameObjects
{
    public abstract class GameObject : IGameObject
    {
        public Vector2 Position { get; protected set; }
        public ISprite Sprite { get; }
        public int Width { get; }
        public int Height { get; }

        public GameObject(Vector2 position, ISprite sprite)
        {
            Position = position;
            Sprite = sprite;
            Width = sprite.Width;
            Height = sprite.Height;
        }

        public abstract void Update();

        public void Draw(SpriteBatch target)
        {
            target.Draw(Sprite.Texture, Position, Color.White);
        }
    }
}