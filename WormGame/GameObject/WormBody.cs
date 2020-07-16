using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    public class WormBody : PoolableObject
    {
        public WormBody next;
        public Image graphic;
        public Vector2 target;
        public Vector2 direction;

        public WormBody(Config config)
        {
            graphic = Image.CreateCircle(config.imageSize / 2);
            graphic.Scale = (float)config.size / config.imageSize;
            graphic.Visible = false;
            graphic.CenterOrigin();
        }

        public void TargetFollow(Vector2 newTarget)
        {
            if (next != null)
                next.TargetFollow(target);
            target = newTarget;
        }

        public void DirectionFollow(Vector2 newDirection)
        {
            if (next != null)
                next.DirectionFollow(direction);
            direction = newDirection;
        }

        public ref Image GetGraphic(int wantedIndex = 0, int i = 0)
        {
            if (wantedIndex != i && next != null)
                return ref next.GetGraphic(wantedIndex, ++i);
            return ref graphic;
        }

        public ref WormBody GetNext(int wantedIndex = 0, int i = 0)
        {
            if (wantedIndex != i && next != null)
                return ref next.GetNext(wantedIndex, ++i);
            return ref next;
        }

        public ref Vector2 GetTarget(int wantedIndex = 0, int i = 0)
        {
            if (wantedIndex != i && next != null)
                return ref next.GetTarget(wantedIndex, ++i);
            return ref target;
        }

        public ref Vector2 GetDirection(int wantedIndex = 0, int i = 0)
        {
            if (wantedIndex != i && next != null)
                return ref next.GetDirection(wantedIndex, ++i);
            return ref direction;
        }

        public void SetColor(Color color)
        {
            if (next != null)
                next.SetColor(color);
            graphic.Color = color;
        }

        public override void Disable()
        {
            if (next != null)
                next.Disable();
            graphic.Visible = false;
            Enabled = false;
        }
    }
}
