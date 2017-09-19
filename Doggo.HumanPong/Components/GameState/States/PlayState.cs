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

        ScoreBoard scoreBoard;

        GameObject player1;
        GameObject player2;

        GameObject ball;
        bool ballMoving = false;

        Vector2 BallCenterPosition;
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

            // ScoreBoard
            scoreBoard = new ScoreBoard(GameRef);

            // Paddles
            Texture2D paddleTexture = content.Load<Texture2D>(@"Graphics\Sprites\Paddle");

            float distanceToEdge = Pong.TargetWidth * 0.01f;
            float centerOfPaddle = paddleTexture.Width / 2f;
            float y = (Pong.TargetHeight - paddleTexture.Height) / 2f;

            Vector2 paddleVelocity = new Vector2(0, 500);

            Vector2 positionP1 = new Vector2(distanceToEdge - centerOfPaddle, y);
            player1 = new GameObject(paddleTexture, positionP1, paddleVelocity);

            Vector2 positionP2 = new Vector2(Pong.TargetWidth - distanceToEdge - centerOfPaddle, y);
            player2 = new GameObject(paddleTexture, positionP2, paddleVelocity);

            // Ball
            Texture2D ballTexture = content.Load<Texture2D>(@"Graphics\Sprites\Ball");

            float ballX = (Pong.TargetWidth - ballTexture.Width) / 2f;
            float ballY = (Pong.TargetHeight - ballTexture.Height) / 2f;

            BallCenterPosition = new Vector2(ballX, ballY);
            Vector2 ballPosition = new Vector2(ballX, ballY);
            ball = new GameObject(ballTexture, ballPosition, new Vector2(-750, -500));
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
                float newPosition = player1.Position.Y - (delta * player1.Velocity.Y);
                player1.Position.Y = (newPosition < 0 ? 0 : newPosition);
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.S))// || Xin.KeyboardState.IsKeyDown(Keys.Down))
            {
                float newPosition = player1.Position.Y + (delta * player1.Velocity.Y);
                int maxHeight = Pong.TargetHeight - player1.BoundingBox.Height;
                player1.Position.Y = (newPosition > maxHeight ? maxHeight : newPosition);
            }

            if (Xin.KeyboardState.IsKeyDown(Keys.Up))
            {
                float newPosition = player2.Position.Y - (delta * player2.Velocity.Y);
                player2.Position.Y = (newPosition < 0 ? 0 : newPosition);
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.Down))
            {
                float newPosition = player2.Position.Y + (delta * player2.Velocity.Y);
                int maxHeight = Pong.TargetHeight - player2.BoundingBox.Height;
                player2.Position.Y = (newPosition > maxHeight ? maxHeight : newPosition);
            }

            // needs a bit of rework. sometimes it bounces off the air infront of the paddle and sometimes it goes inside
            if (ballMoving) ball.Position += ball.Velocity * delta;
            if ((ball.BoundingBox.Intersects(player1.BoundingBox) && ball.Velocity.X < 0) || (ball.BoundingBox.Intersects(player2.BoundingBox) && ball.Velocity.X > 0))
            {
                ball.Velocity.X *= -1;
            }

            if ((ball.BoundingBox.Top <= 0 && ball.Velocity.Y < 0) || (ball.BoundingBox.Bottom >= Pong.TargetHeight && ball.Velocity.Y > 0))
            {
                ball.Velocity.Y *= -1;
            }

            // update score
            if (ball.Position.X <= 0)
            {
                ballMoving = false;
                ball.Position = BallCenterPosition;
                scoreBoard.Player2Scored();
                ResetPaddles();
            }
            else if (ball.Position.X >= Pong.TargetWidth)
            {
                ballMoving = false;
                ball.Position = BallCenterPosition;
                scoreBoard.Player1Scored();
                ResetPaddles();
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
            scoreBoard.Draw(GameRef.SpriteBatch);

            // draw player paddles
            player1.Draw(GameRef.SpriteBatch);
            player2.Draw(GameRef.SpriteBatch);

            // draw ball
            ball.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetUpLocalMultiplayerGame()
        {
            
        }

        private void ResetPaddles()
        {
            player1.Position.Y = (Pong.TargetHeight - player1.BoundingBox.Height) / 2f;
            player2.Position.Y = (Pong.TargetHeight - player2.BoundingBox.Height) / 2f;
        }
        #endregion
    }
}
