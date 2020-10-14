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
        private readonly IList<Obstacle> _obstacles;
        private IList<Coin> _coins;

        private readonly Vector2 _gravity;

        private readonly string _path;
        private readonly GraphicsDevice _graphics;

        public Level(string path, GraphicsDevice graphicsDevice)
        {
            _obstacles = new List<Obstacle>();
            _coins = new List<Coin>();
            
            _path = path;
            _graphics = graphicsDevice;
            _gravity = new Vector2(0, 10);
        }

        public void Update(float timeDelta)
        {
            Player.Update(_gravity * timeDelta);

            foreach (var obstacle in _obstacles)
            {
                if (Player.Collider.Intersects(obstacle.Collider))
                {
                    Player.Collider.ResolveCollision(obstacle.Collider);
                }
            }

            _coins = _coins.Where(coin => !Player.Collider.Intersects(coin.Collider)).ToList();
        }

        public void Draw(SpriteBatch target, float timeDelta)
        {
            Player.Draw(target, timeDelta);
            foreach (var gameObject in _obstacles)
            {
                gameObject.Draw(target, timeDelta);
            }

            foreach (var coin in _coins)
            {
                coin.Draw(target, timeDelta);
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

            foreach (var token in json["obstacles"])
            {
                var jsonObstacle = JsonConvert.DeserializeObject<JsonGameObject>(token.ToString());
                _obstacles.Add(new Obstacle(
                    new Vector2(jsonObstacle.X, jsonObstacle.Y),
                    new SolidColorSprite(jsonObstacle.Width, jsonObstacle.Height, 
                        JsonGameObject.Colors[(string)token["Color"]], _graphics)
                ));
            }

            foreach (var token in json["coins"])
            {
                var jsonObstacle = JsonConvert.DeserializeObject<JsonGameObject>(token.ToString());
                _coins.Add(new Coin(
                    new Vector2(jsonObstacle.X, jsonObstacle.Y),
                    new SolidColorSprite(jsonObstacle.Width, jsonObstacle.Height, 
                        JsonGameObject.Colors[(string)token["Color"]], _graphics)
                ));
            }
        }

        public void Dispose()
        {
            Player = null;
            _obstacles.Clear();
            _coins.Clear();
        }
    }
}