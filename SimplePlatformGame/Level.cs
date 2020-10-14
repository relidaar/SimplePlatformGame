using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimplePlatformGame.Components;
using SimplePlatformGame.GameObjects;
using SimplePlatformGame.JsonObjects;

namespace SimplePlatformGame
{
    public class Level
    {
        public Player Player { get; private set; }
        private readonly IList<Collider> _colliders;
        private readonly IList<GameObject> _gameObjects;

        private readonly Vector2 _gravity;

        private readonly string _path;
        private readonly GraphicsDevice _graphics;

        public Level(string path, GraphicsDevice graphicsDevice)
        {
            _gameObjects = new List<GameObject>();
            _colliders = new List<Collider>();
            _path = path;
            _graphics = graphicsDevice;
            _gravity = new Vector2(0, 10);
        }

        public void Update(float timeDelta)
        {
            Player.Update(_gravity * timeDelta);
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update(_gravity * timeDelta);
            }
 
            foreach (var collider in _colliders)
            {
                foreach (var other in _colliders.Where(c => !c.Equals(collider)))
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
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Draw(target, timeDelta);
            }
        }

        public void Load()
        {
            var jsonString = File.ReadAllText(_path);
            var json = JObject.Parse(jsonString);

            var jsonToken = json["player"];
            var jsonPlayer = JsonConvert.DeserializeObject<JsonPlayer>(jsonToken.ToString());
            
            Player = new Player(
                new Vector2(jsonPlayer.X, jsonPlayer.Y), 
                jsonPlayer.RunSpeed, jsonPlayer.JumpSpeed, 
                new SolidColorSprite(jsonPlayer.Width, jsonPlayer.Height, 
                    JsonGameObject.Colors[(string)jsonToken["Color"]], _graphics)
            );
            _colliders.Add(Player.Collider);

            foreach (var token in json["obstacles"])
            {
                var jsonObstacle = JsonConvert.DeserializeObject<JsonGameObject>(token.ToString());
                var obstacle = new Obstacle(
                    new Vector2(jsonObstacle.X, jsonObstacle.Y),
                    new SolidColorSprite(jsonObstacle.Width, jsonObstacle.Height, 
                        JsonGameObject.Colors[(string)token["Color"]], _graphics)
                );
                _gameObjects.Add(obstacle);
                _colliders.Add(obstacle.Collider);
            }
        }

        public void Dispose()
        {
            Player = null;
            _gameObjects.Clear();
            _colliders.Clear();
        }
    }
}