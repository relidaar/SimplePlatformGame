using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimplePlatformGame
{
    public class Level
    {
        public Player Player { get; private set; }
        public IList<IGameObject> GameObjects { get; }

        private readonly string _path;
        private readonly GraphicsDevice _graphics;
        
        public Level(string path, GraphicsDevice graphicsDevice)
        {
            GameObjects = new List<IGameObject>();
            _path = path;
            _graphics = graphicsDevice;
        }

        public void Update()
        {
            Player.Update();
            foreach (var gameObject in GameObjects)
            {
                gameObject.Update();
            }
        }

        public void Draw(SpriteBatch target)
        {
            Player.Draw(target);
            foreach (var gameObject in GameObjects)
            {
                gameObject.Draw(target);
            }
        }

        public void Load()
        {
            var valuesStrings = System.IO.File.ReadAllLines(_path)
                .Select(x => x)
                .Where(x => !string.IsNullOrWhiteSpace(x));
            foreach (var values in valuesStrings.Select(x => x.Split(" ")))
            {
                float posX = float.Parse(values[1]);
                float posY = float.Parse(values[2]);
                int width = int.Parse(values[3]);
                int height = int.Parse(values[4]);
                bool isStatic = bool.Parse(values[5]);
                switch (values[0])
                {
                    case "player":
                        Player = new Player(new Vector2(posX, posY), 5, 
                            new SolidColorSprite(width, height, Color.DimGray, _graphics));
                        break;
                    case "obstacle":
                        var obstacle = new GameObject(new Vector2(posX, posY), isStatic,
                            new SolidColorSprite(width, height, Color.White, _graphics));
                        GameObjects.Add(obstacle);
                        break;
                }
            }
        }

        public void Dispose()
        {
            Player = null;
            GameObjects.Clear();
        }
    }
}