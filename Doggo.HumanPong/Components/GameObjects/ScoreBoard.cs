using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Doggo.HumanPong.Components.GameObjects
{
    public class ScoreBoard
    {
        #region Field Region
        // player 1 fields
        byte scorePlayer1;
        Vector2 scorePositionPlayer1;

        // player 2 fields
        byte scorePlayer2;
        Vector2 scorePositionPlayer2;

        // shared fields
        SpriteFont scoreFont;

        readonly float centerOfSCreenX = (Pong.TargetWidth / 2f);
        readonly float scoreOffSetX = 75f;
        #endregion

        #region Constructor Region
        public ScoreBoard(Pong game)
        {
            scorePlayer1 = 0;
            scorePlayer2 = 0;

            scorePositionPlayer1 = new Vector2();
            scorePositionPlayer2 = new Vector2();

            scoreFont = game.Content.Load<SpriteFont>(@"Fonts\ScoreFont");

            UpdateScorePositionPlayer1();
            UpdateScorePositionPlayer2();

        }
        #endregion

        public void Player1Scored()
        {
            scorePlayer1++;
            UpdateScorePositionPlayer1();
        }

        public void Player2Scored()
        {
            scorePlayer2++;
            UpdateScorePositionPlayer2();
        }

        private void UpdateScorePositionPlayer1()
        {
            string scoreStr = scorePlayer1.ToString();
            Vector2 fontsizeScore = scoreFont.MeasureString(scoreStr);
            scorePositionPlayer1.X = centerOfSCreenX - fontsizeScore.X - scoreOffSetX;
            scorePositionPlayer1.Y = (Pong.TargetHeight - fontsizeScore.Y) / 2f;
        }

        private void UpdateScorePositionPlayer2()
        {
            string scoreStr = scorePlayer2.ToString();
            Vector2 fontsizeScore = scoreFont.MeasureString(scoreStr);
            scorePositionPlayer2.X = centerOfSCreenX + scoreOffSetX;
            scorePositionPlayer2.Y = (Pong.TargetHeight - fontsizeScore.Y) / 2f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(scoreFont, scorePlayer1.ToString(), scorePositionPlayer1, Color.White);
            spriteBatch.DrawString(scoreFont, scorePlayer2.ToString(), scorePositionPlayer2, Color.White);
        }
    }
}
