using System;
using Doggo.HumanPong.Components.GameObjects;
using Microsoft.Xna.Framework;

namespace Doggo.HumanPong.Components.Controller
{
    public class AIPlayer : IPlayer
    {
        #region Field Region
        Random random;
        #endregion
        
        #region Constructor Region
        public AIPlayer()
        {
            random = new Random();
        }
        #endregion

        #region Method Region
        public PlayerState GetState(GameObject ball, GameObject paddle)
        {
            if ((paddle.Position.X < ball.Position.X && ball.Velocity.X < 0) || (paddle.Position.X > ball.Position.X && ball.Velocity.X > 0))
            {
                // ball coming towards the ai player
                return Defend(ball, paddle);
            }
            else
            {
                // ball going away from the ai player
                return GoToCenter(paddle);
            }
        }

        private PlayerState Defend(GameObject ball, GameObject paddle)
        {
            var ballYRandomized = MathHelper.Clamp(ball.BoundingBox.Center.Y + random.Next(-paddle.BoundingBox.Height / 2, paddle.BoundingBox.Height / 2), 0, Pong.TargetHeight - (ball.BoundingBox.Height / 2));

            if (Math.Abs(paddle.Position.X - ball.Position.X) < (Pong.TargetWidth / 3f))
            {
                if (paddle.BoundingBox.Center.Y > ballYRandomized)//ball.BoundingBox.Center.Y)
                {
                    return PlayerState.UP;
                }
                else if (paddle.BoundingBox.Center.Y < ballYRandomized)//< ball.BoundingBox.Center.Y)
                {
                    return PlayerState.DOWN;
                }
            }

            // already in position
            return PlayerState.IDLE;
        }

        private PlayerState GoToCenter(GameObject paddle)
        {
            float distanceFromMiddle = (Pong.TargetHeight / 2f) - paddle.BoundingBox.Center.Y;

            // slight offset from the middle to allow for lag
            if (distanceFromMiddle < -1)
            {
                return PlayerState.UP;
            }
            else if (distanceFromMiddle > 1)
            {
                return PlayerState.DOWN;
            }

            return PlayerState.IDLE;
        }
        #endregion
    }
}
