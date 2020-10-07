using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimplePlatformGame
{
    public class PlatformGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IList<Level> _levels;
        private Level _currentLevel;
        private string _levelsDirectory;

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            var state = Keyboard.GetState();
            float timeDelta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (state.IsKeyDown(Keys.Up))
                _currentLevel.Player.Move(Direction.Up);
            if(state.IsKeyDown(Keys.Down))
                _currentLevel.Player.Move(Direction.Down);
            if (state.IsKeyDown(Keys.Left))
                _currentLevel.Player.Move(Direction.Left);
            if(state.IsKeyDown(Keys.Right))
                _currentLevel.Player.Move(Direction.Right);

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