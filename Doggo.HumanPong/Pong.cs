﻿using Doggo.HumanPong.Components.GameObjects;
//using Doggo.HumanPong.Components.GameState;
using Doggo.HumanPong.Components.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Doggo.HumanPong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Pong : Game
    {
        #region Field Region
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public const int TargetWidth = 1920; //1280
        public const int TargetHeight = 1080; //720
        Matrix scaleMatrix;

        //GameStateManager gameStateManager;

        FrameRateCounter fpsCounter;

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

        #region Property Region
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public Matrix ScaleMatrix
        {
            get { return scaleMatrix; }
        }
        #endregion

        #region Constructor Region
        public Pong()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = false; 
            graphics.SynchronizeWithVerticalRetrace = false; //vsync
            //graphics.PreferredBackBufferFormat = SurfaceFormat.Alpha8;
            IsFixedTimeStep = false;
            if (IsFixedTimeStep)
                TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000.0f / 60);

            SetWindowResolution();
            IsMouseVisible = true;
        }
        #endregion

        #region Method Region
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // input helper
            Components.Add(new Xin(this));

            // fps counter
            fpsCounter = new FrameRateCounter(this);
            Components.Add(fpsCounter);

            // state manager
            //gameStateManager = new GameStateManager(this);
            //Components.Add(gameStateManager);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
            background = Content.Load<Texture2D>(@"Graphics\Backgrounds\Playfield");

            // Paddles
            Texture2D paddleTexture = Content.Load<Texture2D>(@"Graphics\Sprites\Paddle");

            float distanceToEdge = TargetWidth * 0.01f;
            float centerOfPaddle = paddleTexture.Width / 2f;
            float y = (TargetHeight - paddleTexture.Height) / 2f;

            Vector2 paddleVelocity = new Vector2(0, 500);

            Vector2 positionP1 = new Vector2(distanceToEdge - centerOfPaddle, y);
            Player1 = new GameObject(paddleTexture, positionP1, paddleVelocity);

            Vector2 positionP2 = new Vector2(TargetWidth - distanceToEdge - centerOfPaddle, y);
            Player2 = new GameObject(paddleTexture, positionP2, paddleVelocity);

            // Ball
            Texture2D ballTexture = Content.Load<Texture2D>(@"Graphics\Sprites\Ball");

            float ballX = (TargetWidth - ballTexture.Width) / 2f;
            float ballY = (TargetHeight - ballTexture.Height) / 2f;

            BallCenterPosition = new Vector2(ballX, ballY);
            Vector2 ballPosition = new Vector2(ballX, ballY);
            Ball = new GameObject(ballTexture, ballPosition, new Vector2(-750, -500));

            // Score font
            scoreFont = Content.Load<SpriteFont>(@"Fonts\ScoreFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // used to calculate movement according to time passed, so that even when the loops run faster/slower the distance moved is still correct
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // show/hide fps counter
            if (Xin.CheckKeyReleased(Keys.F1)) fpsCounter.IsVisible = !fpsCounter.IsVisible;

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
                int maxHeight = TargetHeight - Player1.BoundingBox.Height;
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
                int maxHeight = TargetHeight - Player2.BoundingBox.Height;
                Player2.Position.Y = (newPosition > maxHeight ? maxHeight : newPosition);
            }

            // needs a bit of rework. sometimes it bounces off the air infront of the paddle and sometimes it goes inside
            if (ballMoving) Ball.Position += Ball.Velocity * delta;
            if ((Ball.BoundingBox.Intersects(Player1.BoundingBox) && Ball.Velocity.X < 0) || (Ball.BoundingBox.Intersects(Player2.BoundingBox) && Ball.Velocity.X > 0))
            {
                Ball.Velocity.X *= -1;
            }

            if ((Ball.BoundingBox.Top <= 0 && Ball.Velocity.Y < 0) || (Ball.BoundingBox.Bottom >= TargetHeight && Ball.Velocity.Y > 0))
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
            else if (Ball.Position.X >= TargetWidth)
            {
                ballMoving = false;
                Ball.Position = BallCenterPosition;
                scorePlayer1++;
                // reset player paddles
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // the scaling will have a bug when in 4:3 fullscreen it won't display the pads in the correct position
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scaleMatrix);

            // draw background
            spriteBatch.Draw(background, new Rectangle(0, 0, TargetWidth, TargetHeight), Color.White);

            // draw scores
            // optimizations: calculation of score position can be put where the score is updated so it only recalculates when needed
            //                the offset from the top off the screen might be the same for both scores, need to check if the measurestring height is the same for every number
            float centerOfSCreenX = (TargetWidth / 2f);
            float scoreOffSetX = 75f;

            string scorePlayer1Str = scorePlayer1.ToString();
            Vector2 fontsizeScorePlayer1 = scoreFont.MeasureString(scorePlayer1Str);
            float score1Y = (TargetHeight - fontsizeScorePlayer1.Y) / 2f;
            float score1X = centerOfSCreenX - fontsizeScorePlayer1.X - scoreOffSetX;
            spriteBatch.DrawString(scoreFont, scorePlayer1Str, new Vector2(score1X, score1Y), Color.White);

            string scorePlayer2Str = scorePlayer2.ToString();
            Vector2 fontsizeScorePlayer2 = scoreFont.MeasureString(scorePlayer2Str);
            float score2Y = (TargetHeight - fontsizeScorePlayer2.Y) / 2f;
            float score2X = centerOfSCreenX + scoreOffSetX;
            spriteBatch.DrawString(scoreFont, scorePlayer2Str, new Vector2(score2X, score2Y), Color.White);

            // draw player paddles
            Player1.Draw(spriteBatch);
            Player2.Draw(spriteBatch);

            // draw ball
            Ball.Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetWindowResolution(int windowWidth = TargetWidth, int windowHeight = TargetHeight)
        {
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.ApplyChanges();

            float scaleWidth = graphics.GraphicsDevice.Viewport.Width / (float)TargetWidth;
            float scaleHeight = graphics.GraphicsDevice.Viewport.Height / (float)TargetHeight;

            scaleMatrix = Matrix.CreateScale(scaleWidth, scaleHeight, 1);
        }
        #endregion
    }
}
