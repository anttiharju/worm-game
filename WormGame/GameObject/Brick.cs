using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility.MonoGame;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Brick class. Work in progress.
    /// </summary>
    /// TODO: Upgrade into a modular entity, doesn't work with longer than minimum length worms.
    public class Brick : PoolableEntity
    {
        private readonly Collision field;
        private readonly Vector2[] positions;
        private readonly Vector2[] next;
        private readonly Image[] graphics;
        private readonly int kickLimit = 2;
        private readonly int maxLength;
        private readonly int size;

        private int anchorIndex;
        private int kickCounter;


        /// <summary>
        /// How many individual bricks does the brick consist of.
        /// </summary>
        public int Count { get; private set; }


        /// <summary>
        /// Posessee. Required so we can kick it out when the brick can no longer fall.
        /// </summary>
        public Player Player { get; set; }


        /// <summary>
        /// Set and get brick color.
        /// </summary>
        public override Color Color { get { return graphics[0].Color ?? null; } set { SetColor(value); } }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config"></param>
        public Brick(Config config)
        {
            size = config.size;
            field = config.field;
            maxLength = config.minWormLength;
            positions = new Vector2[maxLength];
            graphics = new Image[maxLength];
            next = new Vector2[maxLength];
            for (int i = 0; i < maxLength; i++)
            {
                Image tmp = Image.CreateRectangle(config.imageSize);
                tmp.Scale = (float)config.size / config.imageSize;
                tmp.Visible = false;
                tmp.CenterOrigin();
                graphics[i] = tmp;
                AddGraphic(tmp);
            }
        }


        /// <summary>
        /// Replaces a worm with a brick.
        /// </summary>
        /// <param name="worm">Worm to replace</param>
        /// <returns>Brick</returns>
        public Brick Spawn(Worm worm)
        {
            Position = worm.GetTarget(0);
            Count = worm.Length;
            Color = worm.Color;
            anchorIndex = (int)Count / 2;
            kickCounter = 0;
            for (int i = 0; i < Count; i++)
            {
                field.Set(this, worm.GetTarget(i));
                positions[i] = worm.GetTarget(i) - worm.GetTarget(0);
                graphics[i].X = positions[i].X;
                graphics[i].Y = positions[i].Y;
                graphics[i].Visible = true;
            }
            return this;
        }


        /// <summary>
        /// Move brick to right by one.
        /// </summary>
        /// <param name="amount"></param>
        public void Right(int amount = 1)
        {
            SetNull();
            for (int i = 0; i < Count; i++)
            {
                next[i].X = graphics[i].X;
                next[i].Y = graphics[i].Y;
                next[i].X += size * amount;
                if (field.Check(next[i] + Position) == 2)
                {
                    Reset(); return;
                }
            }
            Set();
        }


        /// <summary>
        /// Move brick to left by one.
        /// </summary>
        public void Left()
        {
            Right(-1);
        }


        /// <summary>
        /// Drop brick by one.
        /// </summary>
        public void SoftDrop()
        {
            SetNull();
            for (int i = 0; i < Count; i++)
            {
                next[i].X = graphics[i].X;
                next[i].Y = graphics[i].Y;
                next[i].Y += size;
                if (field.Check(next[i] + Position) == 2)
                {
                    if (Player != null)
                    {
                        kickCounter++;
                        if (kickCounter >= kickLimit)
                        {
                            Player.LeaveBrick();
                        }
                    }
                    Reset();
                    return;
                }
            }
            kickCounter = 0;
            Set();
        }


        /// <summary>
        /// Drop brick as much as possible.
        /// </summary>
        public void HardDrop()
        {
            SetNull();
            int dropAmount = HardDropAmount();
            for (int i = 0; i < Count; i++)
            {
                next[i].X = graphics[i].X;
                next[i].Y = graphics[i].Y;
                next[i].Y += size * dropAmount;
            }
            Set();
        }


        /// <summary>
        /// Calculates how much we can drop brick.
        /// </summary>
        /// <returns>Drop amount</returns>
        public int HardDropAmount()
        {
            int endX = Rightmost();
            int startX = Leftmost();
            int startY = Lowest() - 1;
            for (int x = startX; x <= endX; x++)
                for (int y = startY; y >= 0; y--)
                    if (field.Check(x, y) == 2)
                        return startY - y;
            return startY + 1;
        }


        /// <summary>
        /// Rotates brick clockwise or counter-clockwise.
        /// </summary>
        /// <param name="clockwise">Rotation direction</param>
        public void Rotate(bool clockwise = false)
        {
            SetNull();
            next[anchorIndex].X = graphics[anchorIndex].X;
            next[anchorIndex].Y = graphics[anchorIndex].Y;
            for (int i = 0; i < Count; i++)
            {
                if (i == anchorIndex) i++;
                next[i].X = graphics[i].X;
                next[i].Y = graphics[i].Y;
                Vector2 rotationVector = next[i] - next[anchorIndex];
                rotationVector = clockwise ? Mathf.RotateCW(rotationVector) : Mathf.RotateCCW(rotationVector);
                next[i] = next[anchorIndex] + rotationVector;
            }
            for (int i = 0; i < Count; i++)
                if (field.Check(Position + next[i]) == 2)
                {
                    Reset();
                    return;
                }
            Set();
        }


        /// <summary>
        /// Removes brick from collision field so it doesn't collide with itself.
        /// </summary>
        private void SetNull()
        {
            for (int i = 0; i < Count; i++)
                field.Set(null, X + graphics[i].X, Y + graphics[i].Y);
        }


        /// <summary>
        /// Put brick back on collision field without applying any changes.
        /// </summary>
        private void Reset()
        {
            for (int i = 0; i < Count; i++)
                field.Set(this, X + graphics[i].X, Y + graphics[i].Y);
        }


        /// <summary>
        /// Put brick on collision field and apply changes.
        /// </summary>
        private void Set()
        {
            graphics[0].X = 0;
            graphics[0].Y = 0;
            Position += next[0];
            field.Set(this, Position);
            for (int i = 1; i < Count; i++)
            {
                field.Set(this, Position + next[i] - next[0]);
                graphics[i].X = next[i].X - next[0].X;
                graphics[i].Y = next[i].Y - next[0].Y;
            }
        }


        /// <summary>
        /// Gets bricks leftmost individual brick position.
        /// </summary>
        /// <returns></returns>
        private int Leftmost()
        {
            float leftmost = float.MaxValue;
            for (int i = 0; i < Count; i++)
                leftmost = Mathf.Smaller(graphics[i].X, leftmost);
            return field.X(X + leftmost);
        }


        /// <summary>
        /// Gets bricks rightmost individual brick position.
        /// </summary>
        /// <returns></returns>
        private int Rightmost()
        {
            float rightmost = float.MinValue;
            for (int i = 0; i < Count; i++)
                rightmost = Mathf.Bigger(graphics[i].X, rightmost);
            return field.X(X + rightmost);
        }


        /// <summary>
        /// Gets bricks lowest individual brick position.
        /// </summary>
        /// <returns></returns>
        private int Lowest()
        {
            float lowest = float.MinValue;
            for (int i = 0; i < Count; i++)
                lowest = Mathf.Bigger(graphics[i].Y, lowest);
            return field.Y(Y + lowest);
        }


        /// <summary>
        /// Set brick color.
        /// </summary>
        /// <param name="color">Color</param>
        public void SetColor(Color color)
        {
            for (int i = 0; i < Count; i++)
                graphics[i].Color = color;
        }
    }
}
