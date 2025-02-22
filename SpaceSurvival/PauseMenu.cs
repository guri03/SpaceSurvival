using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace SpaceSurvival
{
    public class PauseMenu
    {
        private Game1 _game;
        private SpriteFont _font;
        private Button _resumeButton;
        private Button _mainMenuButton;
        private Texture2D _buttonTexture;

        public PauseMenu(Game1 game)
        {
            _game = game;
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Font");
            _buttonTexture= _game.Content.Load<Texture2D>("ButtonTexture");

            _resumeButton = new Button(_buttonTexture, _font, "Resume", new Vector2(300,100));
            _mainMenuButton = new Button(_buttonTexture, _font, "MainMenu", new Vector2(300, 200));
        }

        public void Update(GameTime gameTime)
        {

            var mouseState = Mouse.GetState();

                if (_resumeButton.IsClicked(mouseState))
                {
                    _game.ResumeGame();
                }
                else if (_mainMenuButton.IsClicked(mouseState))
                {
                    _game.ChangeScene(GameState.Start);
                }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Game Paused", new Vector2(350, 100), Color.White);
            _resumeButton.Draw(spriteBatch);
            _mainMenuButton.Draw(spriteBatch);
            spriteBatch.End();
        }
    }

}
