using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mono3D
{
    public static class Drawing
    {
        // Size of grid
        public const int Grid = 8;
        // Width and height of grid
        public const int GridWidth = 64;
        public const int GridHeight = 64;
        // Width and height of screen
        public const int Width = Grid * GridWidth;
        public const int Height = Grid * GridHeight;

        private static Texture2D blankTexture;

        private static SpriteFont arialFont;

        public static void InitializeGraphics(Game1 game)
        {
            // Initialize screen size
            game.Graphics.PreferredBackBufferWidth = Width;
            game.Graphics.PreferredBackBufferHeight = Height;
            game.Graphics.ApplyChanges();

            // Initialize blank texture
            blankTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            blankTexture.SetData(new[] { Color.White });

            // Import arial font
            arialFont = game.Content.Load<SpriteFont>("Arial");
        }

        // Draws given rect with given color to sprite batch
        public static void DrawRect(Rectangle rect, Color color, Game1 game)
        {
            game.SpriteBatch.Draw(blankTexture, rect, null, color);
        }

        // Draws given text at given position
        public static void DrawText(string text, Vector2 position, Color color, Game1 game)
        {
            game.SpriteBatch.DrawString(arialFont, text, position, color);
        }
    }
}
