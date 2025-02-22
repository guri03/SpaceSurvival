using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceSurvival
{
    public class HighscoreScene
    {
        private Game1 _game;
        private SpriteFont _font;
        private Texture2D _buttonTexture;

        private Button _backButton;

        private List<int> _highScores;
        private ScoreManager score = new ScoreManager();

        public HighscoreScene(Game1 game)
        {
            _game = game;
            _highScores = new List<int>();
        }
        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            // Load assets
            _font = content.Load<SpriteFont>("Font");
            _buttonTexture = content.Load<Texture2D>("ButtonTexture");

            // Initialize back button
            _backButton = new Button(_buttonTexture, _font, "Back", new Vector2(300, 350));
            _backButton.OnClick = () =>
            {
                _game.ChangeScene(GameState.GameOver);
            };

            // Load high scores
            _highScores = score.LoadScores();
        }

        public void Update(GameTime gameTime)
        {
            // Handle button clicks
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_backButton.IsClicked(mouseState))
                {
                    _backButton.OnClick.Invoke();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(_font, "High Scores", new Vector2(300, 50), Color.White);

            for (int i = 0; i < _highScores.Count; i++)
            {
                spriteBatch.DrawString(
                    _font,
                    $"{i + 1}. {_highScores[i]}",
                    new Vector2(300, 100 + i * 30),
                    Color.White
                );
            }

            _backButton.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
