using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpaceSurvival
{
    public class Coin
    {
        public Vector2 Position { get; private set; }
        private Texture2D _texture { get; set; }
        private Vector2 _coinVelocity;
        private SoundEffect _coinSound;

        public Coin(Texture2D texture, Vector2 position, SoundEffect coinSound,Vector2 coinVelocity)
        {
            _texture = texture;
            Position = position;
            _coinSound = coinSound;
            _coinVelocity = coinVelocity;
        }

        public void Update(float deltaTime)
        {
            // Move the coin downward (adjust speed as needed)
            Position += _coinVelocity * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}
