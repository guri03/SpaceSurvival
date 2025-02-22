using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceSurvival
{
    public class AboutScene
    {
        private Game1 _game;
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        private Button _backButton;

        public AboutScene(Game1 game)
        {
            _game = game;
        }

        public void Show()
        {
            _font = _game.Content.Load<SpriteFont>("Font");
            _buttonTexture = _game.Content.Load<Texture2D>("ButtonTexture");

            _backButton = new Button(_buttonTexture, _font, "Back", new Vector2(300, 400));
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            if (_backButton.IsClicked(mouseState))
            {
                _game.ChangeScene(GameState.Start);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            string aboutText =
                "Space Survival\n\n" +
                "Developed by: Gursimar Kaur\n" +
                "Enjoy the adventure of space survival!\n\n" +
                "Press the Back button to return to the main menu.";
            spriteBatch.DrawString(_font, aboutText, new Vector2(100, 100), Color.White);

            _backButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Hide()
        {
        }
    }
}
