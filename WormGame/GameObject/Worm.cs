﻿using Otter.Graphics;
using Otter.Utility.MonoGame;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.GameObject
{
    /// @author Antti Harju
    /// @version 18.07.2020
    /// <summary>
    /// Worm entity. Worms are modular entities; it consists of one Otter2d entity and several normal objects so it can grow and be any length.
    /// </summary>
    /// TODO: Weird behaviour if turn into a brick in a tight space.
    public class Worm : PoolableEntity
    {
        private readonly Collision field;
        private readonly float step;
        private readonly int size;

        private int currentLength;
        private bool moving;
        private bool grow;
        private WormScene scene;
        private Vector2 target;
        private WormModule newModule;
        private WormModule lastModule;
        private Graphic newGraphic;
        private Pooler<WormModule> modules;

        public WormModule firstModule;


        /// <summary>
        /// We can access scene through player.
        /// </summary>
        public Player Player { get; set; }


        /// <summary>
        /// Get worm length.
        /// </summary>
        public int Length { get; private set; }


        /// <summary>
        /// Get and set worm color.
        /// </summary>
        public override Color Color { get { return firstModule.Graphic.Color ?? null; } set { SetColor(value); } }


        /// <summary>
        /// Get and set the worm direction.
        /// </summary>
        public Vector2 Direction { get { return direction; } set { if (Help.ValidateDirection(field, firstModule.Target, size, value)) direction = value; } }
        private Vector2 direction;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config"></param>
        public Worm(Config config) : base()
        {
            size = config.size;
            step = config.step;
            field = config.field;
        }


        /// <summary>
        /// Spawns the worm.
        /// </summary>
        /// <param name="wormModules">WormBody pool so the worm can grow.</param>
        /// <param name="x">Horizontal field position</param>
        /// <param name="y">Vertical field position</param>
        /// <param name="length">Worm length</param>
        /// <param name="color">Worm color</param>
        /// <returns>Worm</returns>
        public Worm Spawn(Pooler<WormModule> wormModules, int x, int y, int length, Color color)
        {
            scene = (WormScene)Scene;
            modules = wormModules;
            X = field.EntityX(x);
            Y = field.EntityY(y);
            target = Position;
            Length = length;
            currentLength = 1;
            moving = true;

            firstModule = wormModules.Enable();
            firstModule.Target = Position;
            AddGraphic(firstModule.Graphic);


            lastModule = firstModule;
            for (int i = 1; i < Length; i++)
                Grow(true);

            direction = Random.ValidDirection(field, Position, size);
            field.Set(this, x, y);
            Color = color;
            return this;
        }


        private void Grow(bool spawning = false)
        {
            newModule = modules.Enable();
            if (newModule == null) return;
            newModule.Graphic.X = lastModule.Graphic.X;
            newModule.Graphic.Y = lastModule.Graphic.Y;
            newModule.GetTarget().X = X + lastModule.Graphic.X;
            newModule.GetTarget().Y = Y + lastModule.Graphic.Y;
            newModule.Graphic.Color = Color;
            AddGraphic(newModule.Graphic);
            lastModule.Next = newModule;
            lastModule.GetDirection().X = 0;
            lastModule.GetDirection().Y = 0;
            lastModule = newModule;
            if (!spawning)
                Length++;
        }


        /// <summary>
        /// Updates worms directions, targets and updates its position on the collision field.
        /// </summary>
        public void Move()
        {
            if (grow)
                Grow();
            grow = false;
            moving = true;
            bool retry = true;
        Retry:
            target = firstModule.Target + Direction * size;
            int nextPos = field.Check(target, true);
            if (nextPos >= 3) // If fruit (3) or free (4)
            {
                if (nextPos == 3) // If fruit
                    grow = true;
                if (currentLength < Length)
                    currentLength++;
                else
                    field.Set(null, lastModule.Target);
                firstModule.DirectionFollow(direction);
                firstModule.TargetFollow(target);
                field.Set(this, target);
            }
            else
            {
                if (!retry)
                {
                    scene.SpawnBlock(this, currentLength);
                    Disable();
                }
                else if (Player == null)
                {
                    direction = Random.ValidDirection(field, firstModule.Target, size);
                    retry = false;
                    goto Retry;
                }
                moving = false;
            }
        }


        /// <summary>
        /// Moves worms graphics.
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (moving)
            {
                Vector2 positionDelta = firstModule.Direction * step;
                Position += positionDelta;
                firstModule.Next.GraphicFollow(positionDelta, step);
            }
        }


        /// <summary>
        /// Sets worms color.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            firstModule.SetColor(color);
        }


        /// <summary>
        /// Disables the worm.
        /// </summary>
        public override void Disable()
        {
            firstModule.Disable();
            ClearGraphics();
            Enabled = false;
            lastModule = null;
        }
    }
}
