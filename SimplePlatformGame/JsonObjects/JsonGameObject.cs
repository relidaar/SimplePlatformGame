using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SimplePlatformGame.JsonObjects
{
    public class JsonGameObject
    {
        public float X { get; set; }
        public float Y { get; set; }
        
        public int Width { get; set; }
        public int Height { get; set; }

        public static readonly Dictionary<string, Color> Colors = new Dictionary<string, Color>
        {
            {"Black", Color.Black},
            {"White", Color.White},
            {"DimGray", Color.DimGray},
            {"Red", Color.Red},
            {"Transparent", Color.Transparent},
            {"Gold", Color.Gold},
            {"Indigo", Color.Indigo}
        };
    }
}