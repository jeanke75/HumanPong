using Doggo.HumanPong.Components.GameObjects;
using Doggo.HumanPong.Components.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Doggo.HumanPong.Components.GameState.States
{
    public class PlayState : BaseGameState
    {
        #region Field Region
        Texture2D background;

        GameObject Player1;
        GameObject Player2;

        GameObject Ball;
        bool ballMoving = false;

        Vector2 BallCenterPosition;

        SpriteFont scoreFont;
        byte scorePlayer1 = 0;
        byte scorePlayer2 = 0;
        #endregion

        #region Constructor Region
        public PlayState(Game game) : base(game)
        {
            game.Services.AddService(typeof(PlayState), this);
        }
        #endregion

        #region Method Region
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            /*
            // create a solid color texture (without a png file), can be replaced by a premade image file using Content.Load<Texture2D>("contentFolderStructure\fileNameWithoutExtension")
            int paddleWidth = 5;
            int paddleHeight = 100;

            Texture2D paddleTexture = new Texture2D(GraphicsDevice, paddleWidth, paddleHeight);
            Color[] paddleTextureData = new Color[paddleTexture.Width * paddleTexture.Height];
            for (int i = 0; i < paddleTextureData.Length; ++i)
                paddleTextureData[i] = Color.Red;
            paddleTexture.SetData(paddleTextureData);*/

            // Background
            background = content.Load<Texture2D>(@"Graphics\Backgrounds\Playfield");

            // Paddles
            Texture2D paddleTexture = content.Load<Texture2D>(@"Graphics\Sprites\Paddle");

            float distanceToEdge = Pong.TargetWidth * 0.01f;
            float centerOfPaddle = paddleTexture.Width / 2f;
            float y = (Pong.TargetHeight - paddleTexture.Height) / 2f;

            Vector2 paddleVelocity = new Vector2(0, 500);

            Vector2 positionP1 = new Vector2(distanceToEdge - centerOfPaddle, y);
            Player1 = new GameObject(paddleTexture, positionP1, paddleVelocity);

            Vector2 positionP2 = new Vector2(Pong.TargetWidth - distanceToEdge - centerOfPaddle, y);
            Player2 = new GameObject(paddleTexture, positionP2, paddleVelocity);

            // Ball
            Texture2D ballTexture = content.Load<Texture2D>(@"Graphics\Sprites\Ball");

            float ballX = (Pong.TargetWidth - ballTexture.Width) / 2f;
            float ballY = (Pong.TargetHeight - ballTexture.Height) / 2f;

            BallCenterPosition = new Vector2(ballX, ballY);
            Vector2 ballPosition = new Vector2(ballX, ballY);
            Ball = new GameObject(ballTexture, ballPosition, new Vector2(-750, -500));

            // Score font
            scoreFont = content.Load<SpriteFont>(@"Fonts\ScoreFont");
        }

        public override void Update(GameTime gameTime)
        {
            // used to calculate movement according to time passed, so that even when the loops run faster/slower the distance moved is still correct
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // release the ball to start playing
            if (!ballMoving && Xin.CheckKeyReleased(Keys.Space)) ballMoving = true;

            // move the left paddle up and down
            if (Xin.KeyboardState.IsKeyDown(Keys.Z))// || Xin.KeyboardState.IsKeyDown(Keys.Up))
            {
                float newPosition = Player1.Position.Y - (delta * Player1.Velocity.Y);
                Player1.Position.Y = (newPosition < 0 ? 0 : newPosition);
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.S))// || Xin.KeyboardState.IsKeyDown(Keys.Down))
            {
                float newPosition = Player1.Position.Y + (delta * Player1.Velocity.Y);
                int maxHeight = Pong.TargetHeight - Player1.BoundingBox.Height;
                Player1.Position.Y = (newPosition > maxHeight ? maxHeight : newPosition);
            }

            if (Xin.KeyboardState.IsKeyDown(Keys.Up))
            {
                float newPosition = Player2.Position.Y - (delta * Player2.Velocity.Y);
                Player2.Position.Y = (newPosition < 0 ? 0 : newPosition);
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.Down))
            {
                float newPosition = Player2.Position.Y + (delta * Player2.Velocity.Y);
                int maxHeight = Pong.TargetHeight - Player2.BoundingBox.Height;
                Player2.Position.Y = (newPosition > maxHeight ? maxHeight : newPosition);
            }

            // needs a bit of rework. sometimes it bounces off the air infront of the paddle and sometimes it goes inside
            if (ballMoving) Ball.Position += Ball.Velocity * delta;
            if ((Ball.BoundingBox.Intersects(Player1.BoundingBox) && Ball.Velocity.X < 0) || (Ball.BoundingBox.Intersects(Player2.BoundingBox) && Ball.Velocity.X > 0))
            {
                Ball.Velocity.X *= -1;
            }

            if ((Ball.BoundingBox.Top <= 0 && Ball.Velocity.Y < 0) || (Ball.BoundingBox.Bottom >= Pong.TargetHeight && Ball.Velocity.Y > 0))
            {
                Ball.Velocity.Y *= -1;
            }

            // update score
            if (Ball.Position.X <= 0)
            {
                ballMoving = false;
                Ball.Position = BallCenterPosition;
                scorePlayer2++;
                // reset player paddles
            }
            else if (Ball.Position.X >= Pong.TargetWidth)
            {
                ballMoving = false;
                Ball.Position = BallCenterPosition;
                scorePlayer1++;
                // reset player paddles
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // the scaling will have a bug when in 4:3 fullscreen it won't display the pads in the correct position
            GameRef.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, GameRef.ScaleMatrix);

            // draw background
            GameRef.SpriteBatch.Draw(background, new Rectangle(0, 0, Pong.TargetWidth, Pong.TargetHeight), Color.White);

            // draw scores
            // optimizations: calculation of score position can be put where the score is updated so it only recalculates when needed
            //                the offset from the top off the screen might be the same for both scores, need to check if the measurestring height is the same for every number
            float centerOfSCreenX = (Pong.TargetWidth / 2f);
            float scoreOffSetX = 75f;

            string scorePlayer1Str = scorePlayer1.ToString();
            Vector2 fontsizeScorePlayer1 = scoreFont.MeasureString(scorePlayer1Str);
            float score1Y = (Pong.TargetHeight - fontsizeScorePlayer1.Y) / 2f;
            float score1X = centerOfSCreenX - fontsizeScorePlayer1.X - scoreOffSetX;
            GameRef.SpriteBatch.DrawString(scoreFont, scorePlayer1Str, new Vector2(score1X, score1Y), Color.White);

            string scorePlayer2Str = scorePlayer2.ToString();
            Vector2 fontsizeScorePlayer2 = scoreFont.MeasureString(scorePlayer2Str);
            float score2Y = (Pong.TargetHeight - fontsizeScorePlayer2.Y) / 2f;
            float score2X = centerOfSCreenX + scoreOffSetX;
            GameRef.SpriteBatch.DrawString(scoreFont, scorePlayer2Str, new Vector2(score2X, score2Y), Color.White);

            // draw player paddles
            Player1.Draw(GameRef.SpriteBatch);
            Player2.Draw(GameRef.SpriteBatch);

            // draw ball
            Ball.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetUpLocalMultiplayerGame()
        {
            
        }
        #endregion
    }
}
