using Otter;
using WormGame.GameObject;

namespace WormGame.Other
{
    /// @author anvemaha
    /// @version 01.07.2020
    /// <summary>
    /// Collision class. Closely tied to play area.
    /// </summary>
    class Collision
    {
        private readonly PlayArea playArea;

        public Collision(PlayArea playArea)
        {
            this.playArea = playArea;
        }

        /// <summary>
        /// Checks worms collision
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public bool Check(Vector2 next)
        {
            // Actual collision detection
            int nextReverseX = playArea.X(next.X);
            int nextReverseY = playArea.Y(next.Y);

            if (nextReverseX <= -1 ||
                nextReverseX >= playArea.Width ||
                nextReverseY <= -1 ||
                nextReverseY >= playArea.Height ||
                playArea.Get(next) != null)
                return false;
            return true;
        }

        /// <summary>
        /// Checks worms collision
        /// </summary>
        /// <param name="worm"></param>
        /// <param name="nextTarget"></param>
        /// <param name="noclip"></param>
        /// <returns></returns>
        public bool WormCheck(Worm worm, Vector2 nextTarget)
        {
            // Actual collision detection
            int nextReverseX = playArea.X(nextTarget.X);
            int nextReverseY = playArea.Y(nextTarget.Y);
            if (nextReverseX <= -1 ||
                nextReverseX >= playArea.Width ||
                nextReverseY <= -1 ||
                nextReverseY >= playArea.Height ||
                playArea.Get(nextTarget) != null)
                return false;

            // Update collision data
            playArea.Update(worm[^1].target, null);
            for (int i = worm.Length - 1; i > 0; i--)
                playArea.Update(worm[i - 1].target, worm[i]);
            playArea.Update(nextTarget, worm[0]);
            return true;
        }
    }
}
