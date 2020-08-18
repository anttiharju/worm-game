using Otter.Core;
using Otter.Graphics;
using Otter.Graphics.Drawables;
using Otter.Utility;
using System.Collections;
using WormGame.Core;
using WormGame.Pooling;

namespace WormGame.Entities
{
    public class Eraser : Pooler<EraserModule>
    {
        private readonly Collision collision;
        private readonly Entity renderer;
        private readonly Game game;
        private readonly float halfSize;

        public Eraser(Config config, Game game, WormScene scene) : base(config.moduleAmount)
        {
            this.game = game;
            collision = config.collision;
            renderer = new Entity { Surface = config.surface };
            halfSize = config.halfSize;
            scene.Add(renderer);
            for (int i = 0; i < config.moduleAmount; i++)
            {
                EraserModule eraser = new EraserModule(config, renderer);
                eraser.Disable(false);
                pool[i] = eraser;
            }
        }

        public void Erase(BlockModule blockModule)
        {
            Enable().Spawn(collision.EntityX(blockModule.X) - halfSize, collision.EntityY(blockModule.Y) - halfSize, blockModule.Width, blockModule.Height);
            game.Coroutine.Start(EraseEraser());
        }

        IEnumerator EraseEraser()
        {
            yield return Coroutine.Instance.WaitForFrames(1);
            Reset();
        }
    }


    public class EraserModule : Poolable
    {
        private readonly Image graphic;

        public EraserModule(Config config, Entity renderer)
        {
            graphic = Image.CreateRectangle(config.size, config.size, config.backgroundColor);
            graphic.Visible = false;
            renderer.AddGraphic(graphic);
        }

        public override void Disable(bool recursive = true)
        {
            base.Disable(recursive);
            graphic.Visible = false;
        }

        public void Spawn(float X, float Y, int width, int height)
        {
            graphic.X = X;
            graphic.Y = Y;
            graphic.ScaleX = width;
            graphic.ScaleY = height;
            graphic.Visible = true;
        }
    }
}
