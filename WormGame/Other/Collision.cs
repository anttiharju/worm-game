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
        /// <param name="worm"></param>
        /// <param name="target"></param>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        /// <param name="noclip"></param>
        /// <returns></returns>
        public bool WormCheck(Worm worm, Vector2 target, int deltaX, int deltaY, bool noclip)
        {
            // Actual collision
            Vector2 next = target + new Vector2(deltaX, deltaY);
            int nextReverseX = playArea.X(next.X);
            int nextReverseY = playArea.Y(next.Y);
            if (nextReverseX <= -1 ||
                nextReverseX >= playArea.Width ||
                nextReverseY <= -1 ||
                nextReverseY >= playArea.Height ||
                (playArea.Get(next) != null && !noclip))
                return false;

            // Update worm collision data
            playArea.Update(next, worm[0]);
            playArea.Update(next, worm[^1]);
            playArea.Update(worm[^1].Target, null);
            for (int i = worm.Length - 1; i > 0; i--)
                playArea.Update(worm[i - 1].Target, worm[i]);
            playArea.Update(next, worm[0]);
            return true;
        }
    }
}
