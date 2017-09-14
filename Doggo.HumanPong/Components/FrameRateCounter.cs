using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Doggo.HumanPong.Components
{
    public class FrameRateCounter : DrawableGameComponent
    {
        protected Pong GameRef;
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public FrameRateCounter(Game game) : base(game)
        {
            GameRef = (Pong)game;
            DrawOrder = int.MaxValue; //always on top
        }

        protected override void LoadContent()
        {
            spriteFont = GameRef.Content.Load<SpriteFont>(@"Fonts\FrameRateFont");
        }

        protected override void UnloadContent()
        {
            GameRef.Content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format("fps: {0}", frameRate);

            GameRef.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, GameRef.ScaleMatrix);

            GameRef.SpriteBatch.DrawString(spriteFont, fps, new Vector2(10, 10), Color.Black);
            GameRef.SpriteBatch.DrawString(spriteFont, fps, new Vector2(11, 11), Color.White);

            GameRef.SpriteBatch.End();
        }
    }
}
