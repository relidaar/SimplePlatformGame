using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimplePlatformGame.GameObjects;

namespace SimplePlatformGame
{
    public class PlatformGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IList<Level> _levels;
        private Level _currentLevel;
        private string _levelsDirectory;
        private KeyboardState _state;

        public PlatformGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _levelsDirectory = "Levels";
            
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _state = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _levels = new List<Level>();
            foreach (string filePath in Directory.GetFiles( $"{Content.RootDirectory}/{_levelsDirectory}/"))
            {
                _levels.Add(new Level(filePath, _graphics.GraphicsDevice));
            }
            
            _currentLevel = _levels[0];
            _currentLevel.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            var currentState = Keyboard.GetState();
            float timeDelta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_state.IsKeyDown(Keys.Escape))
                Exit();

            var movement = new[]
            {
                (Keys.Up, Direction.Up),
                (Keys.Down, Direction.Down),
                (Keys.Left, Direction.Left),
                (Keys.Right, Direction.Right),
            };

            foreach (var (key, direction) in movement)
            {
                if (currentState.IsKeyDown(key) && _state.IsKeyUp(key))
                    _currentLevel.Player.Move(direction);
                if (currentState.IsKeyUp(key) && _state.IsKeyDown(key))
                    _currentLevel.Player.Stop(direction);
            }

            if (currentState.IsKeyDown(Keys.Space))
            {
                _currentLevel.Player.Jump();
            }

            _state = currentState;
            
            _currentLevel.Update(timeDelta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _currentLevel.Draw(_spriteBatch);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}