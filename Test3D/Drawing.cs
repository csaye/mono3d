using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test3D
{
    public static class Drawing
    {
        // Size of grid
        private const int Grid = 16;
        // Width and height of grid
        private const int GridWidth = 32;
        private const int GridHeight = 32;
        // Width and height of screen
        private const int Width = Grid * GridWidth;
        private const int Height = Grid * GridHeight;

        private static Texture2D blankTexture;

        public static void InitializeGraphics(Game1 game)
        {
            // Initialize screen size
            game.Graphics.PreferredBackBufferWidth = Width;
            game.Graphics.PreferredBackBufferHeight = Height;
            game.Graphics.ApplyChanges();

            // Initialize blank texture
            blankTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            blankTexture.SetData(new[] { Color.White });
        }

        // Draws given rect with given color to sprite batch
        public static void DrawRect(Rectangle rect, Color color, Game1 game)
        {
            game.SpriteBatch.Draw(blankTexture, rect, null, color);
        }
    }
}
