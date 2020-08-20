using System.Collections;
using Otter.Core;
using Otter.Utility;
using Otter.Graphics.Drawables;
using WormGame.Core;
using WormGame.Static;
using WormGame.Pooling;

namespace WormGame.Entities
{
    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Custom pooler that is the worm erasing system. Managed by Blocks and used from blockSpawner.
    /// </summary>
    public class Eraser : Pooler<EraserModule>
    {
        private readonly Collision collision;
        private readonly Entity renderer;
        private readonly Game game;
        private readonly float halfSize;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="game">Game, required for coroutine</param>
        /// <param name="scene">Scene</param>
        public Eraser(Settings settings, Game game, WormScene scene) : base(settings.moduleAmount)
        {
            this.game = game;
            collision = settings.collision;
            renderer = new Entity { Surface = settings.surface };
            halfSize = settings.halfSize;
            scene.Add(renderer);
            for (int i = 0; i < settings.moduleAmount; i++)
            {
                EraserModule eraser = new EraserModule(settings, renderer);
                eraser.Disable(false);
                pool[i] = eraser;
            }
        }


        /// <summary>
        /// Erases a part of a worm under a block module.
        /// </summary>
        /// <param name="blockModule">Block module</param>
        public void Erase(BlockModule blockModule)
        {
            Enable().Spawn(collision.EntityX(blockModule.X) - halfSize, collision.EntityY(blockModule.Y) - halfSize, blockModule.Width, blockModule.Height);
            game.Coroutine.Start(EraseEraser());
        }


        /// <summary>
        /// Erasers have to exists for a frame to actually erase.
        /// </summary>
        IEnumerator EraseEraser()
        {
            yield return Coroutine.Instance.WaitForFrames(1);
            Reset();
        }
    }


    /// @author Antti Harju
    /// @version v0.5
    /// <summary>
    /// Scalable eraser module. Matches itself to a blockModule which already covers a part of a worm.
    /// </summary>
    public class EraserModule : Poolable
    {
        private readonly Image graphic;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="renderer">Renderer entity</param>
        public EraserModule(Settings settings, Entity renderer)
        {
            graphic = Image.CreateRectangle(settings.size, settings.size, Colors.background);
            graphic.Visible = false;
            renderer.AddGraphic(graphic);
        }


        /// <summary>
        /// Disable eraser module.
        /// </summary>
        /// <param name="recursive">Disable recursively. Not relevant here.</param>
        public override void Disable(bool recursive = true)
        {
            base.Disable(recursive);
            graphic.Visible = false;
        }


        /// <summary>
        /// Spawn eraser module.
        /// </summary>
        /// <param name="X">Horizontal entity position</param>
        /// <param name="Y">Vertical entity position</param>
        /// <param name="width">Horizontal scale</param>
        /// <param name="height">Vertical scale</param>
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
