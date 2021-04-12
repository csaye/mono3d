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

        private readonly Player Player;
        private readonly Map Map;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Initialize player and map
            Player = new Player();
            Map = new Map(Player);
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

            // Update player
            Player.Update(gameTime, this);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(); // Begin sprite batch
            Map.Draw(gameTime, this); // Draw map
            SpriteBatch.End(); // End sprite batch

            base.Draw(gameTime);
        }

        private void ProcessKeyboardState()
        {
            KeyboardState state = KeyboardState;

            if (state.IsKeyDown(Keys.Escape)) Exit(); // Exit if escape key pressed
        }
    }
}
