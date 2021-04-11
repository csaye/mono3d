using Microsoft.Xna.Framework;
using System;

namespace Mono3D
{
    public class Noise
    {
        private readonly Random Random = new Random();

        private readonly int Width;
        private readonly int Height;

        private readonly float[,] RandomNoise;

        public Noise(int width, int height, int octaveCount)
        {
            Width = width;
            Height = height;

            RandomNoise = GenerateNoise(octaveCount);
        }

        // Returns an array of random values 0 through 1
        private float[,] GenerateWhiteNoise()
        {
            float[,] whiteNoise = new float[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    whiteNoise[x, y] = (float)Random.NextDouble();
                }
            }

            return whiteNoise;
        }

        // Returns a noise array of given size and octave
        private float[,] GenerateNoise(int octaveCount)
        {
            return GenerateWhiteNoise();
        }

        public void Draw(GameTime gameTime, Game1 game)
        {
            int gridWidth = Drawing.Width / Width;
            int gridHeight = Drawing.Height / Height;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Rectangle rect = new Rectangle(x * gridWidth, y * gridHeight, gridWidth, gridHeight);
                    float colorFactor = RandomNoise[x, y];
                    Color color = new Color(colorFactor, colorFactor, colorFactor);
                    Drawing.DrawRect(rect, color, game);
                }
            }
        }
    }
}
