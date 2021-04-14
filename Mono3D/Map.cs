using Microsoft.Xna.Framework;
using System;

namespace Mono3D
{
    public class Map
    {
        private const float Pi = (float)Math.PI;

        private readonly Player Player;

        private const bool ShowSky = false; // Whether sky is shown
        private const bool ShowColors = false; // Whether colors are shown

        private const int SmoothingIters = 3; // Noise smoothing iterations
        private const float SmoothingFactor = 0.7f; // Noise smoothing factor
        private readonly Noise Noise = new Noise();

        private float fps;

        private const float RayStepDist = 0.2f; // Ray step distance
        private const float MaxDepth = 32; // Maximum ray depth

        private const float BaseFov = Pi / 4; // Base field of view
        private readonly Vector2 Fov;

        public Map(Player player)
        {
            // Initialize player
            Player = player;

#pragma warning disable CS0162 // Unreachable code detected

            // Initialize field of view
            if (Drawing.GridWidth == Drawing.GridHeight)
            {
                Fov = new Vector2(BaseFov, BaseFov);
            }
            else if (Drawing.GridWidth > Drawing.GridHeight)
            {
                float factor = (float)Drawing.GridWidth / Drawing.GridHeight;
                Fov = new Vector2(BaseFov * factor, BaseFov);
            }
            else
            {
                float factor = (float)Drawing.GridHeight / Drawing.GridWidth;
                Fov = new Vector2(BaseFov, BaseFov * factor);
            }

#pragma warning restore CS0162 // Unreachable code detected
        }

        public void Draw(GameTime gameTime, Game1 game)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds; // Get time delta
            fps = 1 / delta; // Set fps

            Vector2 angle = Player.GetAngle();
            Vector3 position = Player.GetPosition();

            // Cast ray for each grid
            for (int x = 0; x < Drawing.GridWidth; x++)
            {
                for (int y = 0; y < Drawing.GridHeight; y++)
                {
                    // Get ray angle
                    float rayX = angle.X - (Fov.X / 2) + (Fov.X * ((float)x / Drawing.GridWidth));
                    float rayY = angle.Y - (Fov.Y / 2) + (Fov.Y * ((float)y / Drawing.GridHeight));
                    Vector2 rayAngle = new Vector2(rayX, rayY);

                    // Get ray steps
                    float cosY = (float)Math.Cos(rayAngle.Y);
                    float rayStepX = RayStepDist * (float)Math.Cos(rayAngle.X) * cosY;
                    float rayStepY = RayStepDist * (float)Math.Sin(rayAngle.Y);
                    float rayStepZ = RayStepDist * (float)Math.Sin(rayAngle.X) * cosY;
                    Vector3 rayStep = new Vector3(rayStepX, rayStepY, rayStepZ);

                    // Initialize ray
                    Vector3 rayPosition = position;
                    float rayDistance = 0;
                    BlockType hitBlock = BlockType.Empty;

                    // While nothing hit and ray not exceeded max depth
                    while (hitBlock == BlockType.Empty && rayDistance < MaxDepth)
                    {
                        // Increment ray position and distance
                        rayPosition += rayStep;
                        rayDistance += RayStepDist;

                        // Set hit block
                        hitBlock = GetBlock(rayPosition);
                    }

                    // Skip draw if showing sky
                    if (ShowSky && rayDistance >= MaxDepth) continue;

                    // Set rect and color based on ray distance
                    float closeFactor = 1 - (rayDistance / MaxDepth);
                    closeFactor = Math.Clamp(closeFactor, 0, 1);
                    Rectangle rect = new Rectangle(x * Drawing.Grid, y * Drawing.Grid, Drawing.Grid, Drawing.Grid);
                    byte colorFactor = (byte)(255 * closeFactor);
                    Color color = new Color(colorFactor, colorFactor, colorFactor);

                    // Tint color based on hit block
                    if (ShowColors && (int)hitBlock > 1)
                    {
                        switch (hitBlock)
                        {
                            case BlockType.Red:
                                color.G /= 2;
                                color.B /= 2;
                                break;
                            case BlockType.Green:
                                color.R /= 2;
                                color.B /= 2;
                                break;
                            case BlockType.Blue:
                                color.R /= 2;
                                color.G /= 2;
                                break;
                        }
                    }

                    // Draw pixel
                    Drawing.DrawRect(rect, color, game);
                }
            }

            // Draw data text
            Drawing.DrawText($"pos: {position}", new Vector2(8, 8), Color.Blue, game);
            Drawing.DrawText($"angle: {angle}", new Vector2(8, 24), Color.Blue, game);
            Drawing.DrawText($"fps: {fps}", new Vector2(8, 40), Color.Blue, game);

            // Draw crosshair
            Rectangle crosshairRect = new Rectangle(Drawing.Width / 2 - 4, Drawing.Height / 2 - 1, 8, 2);
            Drawing.DrawRect(crosshairRect, Color.White, game);
            crosshairRect = new Rectangle(Drawing.Width / 2 - 1, Drawing.Height / 2 - 4, 2, 8);
            Drawing.DrawRect(crosshairRect, Color.White, game);
        }

        private BlockType GetBlock(Vector3 pos) => GetBlock(pos.X, pos.Y, pos.Z);
        private BlockType GetBlock(float x, float y, float z)
        {
            int mapX = (int)Math.Floor(x);
            int mapY = (int)Math.Floor(y);
            int mapZ = (int)Math.Floor(z);

            if (mapX % 4 == 0 && mapY % 4 == 0 && mapZ % 4 == 0) return BlockType.White;
            else return BlockType.Empty;
        }
    }
}
