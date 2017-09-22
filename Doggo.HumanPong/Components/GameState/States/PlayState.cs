using System.Collections.Generic;
using Doggo.HumanPong.Components.Controller;
using Doggo.HumanPong.Components.GameObjects;
using Doggo.HumanPong.Components.ParticleEffects;
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

        IPlayer player1, player2;

        GameObject paddlePlayer1;
        GameObject paddlePlayer2;

        GameObject ball;
        bool ballMoving = false;

        Vector2 BallCenterPosition;

        ParticleEngine particleEngine;
        #endregion

        #region Constructor Region
        public static PlayState CreateAIGame(Game game)
        {
            return new PlayState(game, new AIPlayer(), new AIPlayer());
        }

        public static PlayState CreateSinglePlayerGame(Game game)
        {
            return new PlayState(game, new HumanPlayer(Keys.Z, Keys.S), new AIPlayer());
        }

        public static PlayState CreateLocalMultiPlayerGame(Game game)
        {
            return new PlayState(game, new HumanPlayer(Keys.Z, Keys.S), new HumanPlayer(Keys.Up, Keys.Down));
        }

        private PlayState(Game game, IPlayer player1, IPlayer player2) : base(game)
        {
            game.Services.AddService(typeof(PlayState), this);
            this.player1 = player1;
            this.player2 = player2;
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
            paddlePlayer1 = new GameObject(paddleTexture, positionP1, paddleVelocity);

            Vector2 positionP2 = new Vector2(Pong.TargetWidth - distanceToEdge - centerOfPaddle, y);
            paddlePlayer2 = new GameObject(paddleTexture, positionP2, paddleVelocity);

            // Ball
            Texture2D ballTexture = content.Load<Texture2D>(@"Graphics\Sprites\Ball");

            float ballX = (Pong.TargetWidth - ballTexture.Width) / 2f;
            float ballY = (Pong.TargetHeight - ballTexture.Height) / 2f;

            BallCenterPosition = new Vector2(ballX, ballY);
            Vector2 ballPosition = new Vector2(ballX, ballY);
            ball = new GameObject(ballTexture, ballPosition, new Vector2(-750, -500));

            // Particle Engine
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(GameRef.Content.Load<Texture2D>(@"Graphics\Particles\Circle"));
            particleEngine = new ParticleEngine(textures, BallCenterPosition);
        }

        public override void Update(GameTime gameTime)
        {
            // used to calculate movement according to time passed, so that even when the loops run faster/slower the distance moved is still correct
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // release the ball to start playing
            if (!ballMoving && Xin.CheckKeyReleased(Keys.Space)) ballMoving = true;

            // move the left paddle up and down
            HandlePlayerState(player1.GetState(ball, paddlePlayer1), paddlePlayer1, delta);
            HandlePlayerState(player2.GetState(ball, paddlePlayer2), paddlePlayer2, delta);


            /*if (Xin.KeyboardState.IsKeyDown(Keys.Z))
            {
                float newPosition = player1.Position.Y - (delta * player1.Velocity.Y);
                player1.Position.Y = (newPosition < 0 ? 0 : newPosition);
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.S))
            {
                float newPosition = player1.Position.Y + (delta * player1.Velocity.Y);
                int maxHeight = Pong.TargetHeight - player1.BoundingBox.Height;
                player1.Position.Y = (newPosition > maxHeight ? maxHeight : newPosition);
            }*/

            /*if (Xin.KeyboardState.IsKeyDown(Keys.Up))
            {
                float newPosition = player2.Position.Y - (delta * player2.Velocity.Y);
                player2.Position.Y = (newPosition < 0 ? 0 : newPosition);
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.Down))
            {
                float newPosition = player2.Position.Y + (delta * player2.Velocity.Y);
                int maxHeight = Pong.TargetHeight - player2.BoundingBox.Height;
                player2.Position.Y = (newPosition > maxHeight ? maxHeight : newPosition);
            }*/

            if (ballMoving)
            {
                particleEngine.EmitterLocation = new Vector2(ball.BoundingBox.Center.X, ball.BoundingBox.Center.Y);
                particleEngine.Update(delta);

                ball.Position += ball.Velocity * delta;

                // all positions are calculated using their position float values instead of the boundingbox rounded int values for accuracy
                // ball <> player collision
                if (ball.BoundingBox.Intersects(paddlePlayer1.BoundingBox) && ball.Velocity.X < 0)
                {
                    ball.Velocity.X *= -1;
                    float collisionDepth = paddlePlayer1.Position.X + paddlePlayer1.BoundingBox.Width - ball.Position.X;
                    ball.Position.X += collisionDepth * 2;
                }
                else if (ball.BoundingBox.Intersects(paddlePlayer2.BoundingBox) && ball.Velocity.X > 0)
                {
                    ball.Velocity.X *= -1;
                    float collisionDepth = ball.Position.X + ball.BoundingBox.Width - paddlePlayer2.Position.X;
                    ball.Position.X -= collisionDepth * 2;
                }

                // ball <> top/bottom screen collision
                if (ball.BoundingBox.Top <= 0 && ball.Velocity.Y < 0)
                {
                    ball.Velocity.Y *= -1;
                    ball.Position.Y *= -1;
                }
                else if (ball.BoundingBox.Bottom >= Pong.TargetHeight && ball.Velocity.Y > 0)
                {
                    ball.Velocity.Y *= -1;
                    float collisionDepth = ball.Position.Y + ball.BoundingBox.Height - Pong.TargetHeight;
                    ball.Position.Y -= collisionDepth * 2;
                }

                // update score
                if (ball.Position.X <= 0)
                {
                    ballMoving = false;
                    ball.Position = BallCenterPosition;
                    scoreBoard.Player2Scored();
                    ResetPaddles();
                    particleEngine.RemoveAllParticles();
                }
                else if (ball.Position.X >= Pong.TargetWidth)
                {
                    ballMoving = false;
                    ball.Position = BallCenterPosition;
                    scoreBoard.Player1Scored();
                    ResetPaddles();
                    particleEngine.RemoveAllParticles();
                }
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
            paddlePlayer1.Draw(GameRef.SpriteBatch);
            paddlePlayer2.Draw(GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();

            particleEngine.Draw(GameRef.SpriteBatch, GameRef.ScaleMatrix);

            GameRef.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, GameRef.ScaleMatrix);
            // draw ball
            ball.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandlePlayerState(PlayerState state, GameObject paddle, float delta)
        {
            if (state == PlayerState.UP)
            {
                float newPosition = paddle.Position.Y - (delta * paddle.Velocity.Y);
                paddle.Position.Y = (newPosition < 0 ? 0 : newPosition);
            }
            else if (state == PlayerState.DOWN)
            {
                float newPosition = paddle.Position.Y + (delta * paddle.Velocity.Y);
                int maxHeight = Pong.TargetHeight - paddle.BoundingBox.Height;
                paddle.Position.Y = (newPosition > maxHeight ? maxHeight : newPosition);
            }
        }

        private void ResetPaddles()
        {
            paddlePlayer1.Position.Y = (Pong.TargetHeight - paddlePlayer1.BoundingBox.Height) / 2f;
            paddlePlayer2.Position.Y = (Pong.TargetHeight - paddlePlayer2.BoundingBox.Height) / 2f;
        }
        #endregion
    }
}
