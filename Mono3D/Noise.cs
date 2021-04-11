using System;

namespace Mono3D
{
    public class Noise
    {
        private readonly Random Random;

        public Noise()
        {
            Random = new Random();
        }

        public Noise(int seed)
        {
            Random = new Random(seed);
        }

        // Returns an array of random values 0 through 1
        public float[,] GenerateWhiteNoise(int width, int height)
        {
            float[,] whiteNoise = new float[width, height];

            // For each pixel
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Set to random float
                    whiteNoise[x, y] = (float)Random.NextDouble();
                }
            }

            return whiteNoise;
        }

        // Returns white noise of given size smoothed by given iterations
        public float[,] GenerateSmoothNoise(int width, int height, int iterations, float smoothing)
        {
            float[,] smoothNoise = GenerateWhiteNoise(width, height);

            // For each iteration
            for (int i = 0; i < iterations; i++)
            {
                // For each pixel
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        // Get surrounding pixel range
                        int xa = x == 0 ? x : x - 1;
                        int ya = y == 0 ? y : y - 1;
                        int xb = x == width - 1 ? x : x + 1;
                        int yb = y == height - 1 ? y : y + 1;

                        // Get average of values around pixel
                        float total = 0;
                        int count = 0;
                        for (int xx = xa; xx <= xb; xx++)
                        {
                            for (int yy = ya; yy <= yb; yy++)
                            {
                                if (x == xx && y == yy) continue;
                                total += smoothNoise[xx, yy];
                                count++;
                            }
                        }

                        // Smooth based on surrounding average
                        total /= count;
                        float val = smoothNoise[x, y];
                        smoothNoise[x, y] = val * (1 - smoothing) + total * smoothing;
                    }
                }
            }

            return smoothNoise;
        }
    }
}
