using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mono3D
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        public KeyboardState KeyboardState { get; private set; }

        private Map map = new Map();

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Initialize graphics
            Drawing.InitializeGraphics(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            // Get and process keyboard state
            KeyboardState = Keyboard.GetState();
            ProcessKeyboardState();

            // Update map
            map.Update(gameTime, this);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();

            // Draw map
            map.Draw(gameTime, this);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void ProcessKeyboardState()
        {
            KeyboardState state = KeyboardState;

            if (state.IsKeyDown(Keys.Escape)) Exit();
        }
    }
}
