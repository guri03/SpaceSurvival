using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceSurvival
{
    public class HelpScene
    {
        private Game1 _game;
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        private Button _backButton;

        public HelpScene(Game1 game)
        {
            _game = game;
        }

        public void Show()
        {
            // Load resources
            _font = _game.Content.Load<SpriteFont>("Font");
            _buttonTexture = _game.Content.Load<Texture2D>("ButtonTexture");

            // Initialize the Back button
            _backButton = new Button(_buttonTexture, _font, "Back", new Vector2(300, 400));
        }

        public void Update()
        {
            // Get mouse state
            MouseState mouseState = Mouse.GetState();

            if (_backButton.IsClicked(mouseState))
            {
                _game.ChangeScene(GameState.Start); // Change to Start scene
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            string helpText =
                "Welcome to Space Survival!\n\n" +
                "How to Play:\n" +
                "- Enter your name and press enter to start.\n" +
                "Use W, A, S, D to move your ship.Spacebar to attack\n" +
                "Press space to attack ans escape to pause.\n" +
                "Collect coins to gain points\n" +
                "- Destroy enemy ships to earn points.\n\n" +
                "Objective:\n" +
                "- You have 5 lives for one game survive as long as possible!\n\n";
            spriteBatch.DrawString(_font, helpText, new Vector2(100, 100), Color.White);

            _backButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Hide()
        {
        }
    }
}
