using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplePlatformGame.Components;
using SimplePlatformGame.GameObjects;

namespace SimplePlatformGame
{
    public class Level
    {
        public Player Player { get; private set; }
        public IList<GameObject> GameObjects { get; }
        public IList<Collider> Colliders { get; }

        private Vector2 _gravity;

        private readonly string _path;
        private readonly GraphicsDevice _graphics;
        
        public Level(string path, GraphicsDevice graphicsDevice)
        {
            GameObjects = new List<GameObject>();
            Colliders = new List<Collider>();
            _path = path;
            _graphics = graphicsDevice;
            _gravity = new Vector2(0, 10);
        }

        public void Update(float timeDelta)
        {
            Player.Update(_gravity * timeDelta);
            foreach (var gameObject in GameObjects)
            {
                gameObject.Update(_gravity * timeDelta);
            }
 
            foreach (var collider in Colliders)
            {
                foreach (var other in Colliders.Where(c => !c.Equals(collider)))
                {
                    var dynamicCollidable = collider;
                    var staticCollidable = other;

                    if (collider.IsStatic)
                    {
                        dynamicCollidable = other;
                        staticCollidable = collider;
                    }
                    
                    if (dynamicCollidable.Intersects(staticCollidable))
                        dynamicCollidable.ResolveCollision(staticCollidable);
                }
            }
        }

        public void Draw(SpriteBatch target, float timeDelta)
        {
            Player.Draw(target, timeDelta);
            foreach (var gameObject in GameObjects)
            {
                gameObject.Draw(target, timeDelta);
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
                switch (values[0])
                {
                    case "player":
                        Player = new Player(new Vector2(posX, posY), 5, 
                            new SolidColorSprite(width, height, Color.DimGray, _graphics));
                        Colliders.Add(Player.Collider);
                        break;
                    case "obstacle":
                        var obstacle = new Obstacle(new Vector2(posX, posY),
                            new SolidColorSprite(width, height, Color.White, _graphics));
                        GameObjects.Add(obstacle);
                        Colliders.Add(obstacle.Collider);
                        break;
                }
            }
        }

        public void Dispose()
        {
            Player = null;
            GameObjects.Clear();
            Colliders.Clear();
        }
    }
}