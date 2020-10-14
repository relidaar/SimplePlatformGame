﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimplePlatformGame.Components
{
    public class SolidColorSprite : ISprite
    {
        public Texture2D Texture { get; }

        public SolidColorSprite(int width, int height, Color color, GraphicsDevice graphics)
        {
            Texture = new Texture2D(graphics, width, height);
            var data = new Color[width * height];
            for (int i = 0; i < data.Length; i++) data[i] = color;
            Texture.SetData(data);
        }
    }
}