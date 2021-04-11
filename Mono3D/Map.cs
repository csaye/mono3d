using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mono3D
{
    public class Map
    {
        private const float Pi = (float)Math.PI;

        // Map data
        private byte[,,] map;

        private const int Width = 32;
        private const int Height = 16;
        private const int Length = 32;

        private const bool ShowSky = false;

        private const int SmoothingIters = 3;
        private const float SmoothingFactor = 0.7f;
        private readonly Noise Noise = new Noise();

        private float fps;

        // Player data
        private Vector3 position;
        private Vector2 angle;

        private Vector3 direction;
        private Vector2 rotation;

        private const float Speed = 3;
        private const float Spin = 1;

        private const float BaseFov = Pi / 4;
        private readonly Vector2 Fov;

        // Ray data
        private const float RayStepDist = 0.2f;
        private const float MaxDepth = 32;

        public Map()
        {
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

            // Initialize map
            float[,] smoothNoise = Noise.GenerateSmoothNoise(Width, Length, SmoothingIters, SmoothingFactor);
            map = new byte[Width, Height, Length];
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Length; z++)
                {
                    float yLevel = Height - (smoothNoise[x, z] * Height);
                    for (int y = Height - 1; y > yLevel; y--)
                    {
                        map[x, y, z] = (byte)BlockType.White;
                    }
                }
            }

            // Initialize player
            position = new Vector3(0, 0, 0);
            angle = new Vector2(0, 0);
        }

        public void Update(GameTime gameTime, Game1 game)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds; // Get time delta
            ProcessKeyboardState(game); // Process keyboard state
            MovePlayer(delta); // Move player by delta
        }

        public void Draw(GameTime gameTime, Game1 game)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds; // Get time delta
            fps = 1 / delta; // Set fps

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
                    bool hitWall = false;
                    BlockType hitBlock = BlockType.Empty;

                    // While nothing hit and ray not exceeded max depth
                    while (hitBlock == BlockType.Empty && rayDistance < MaxDepth)
                    {
                        // If in bounds, check whether wall hit
                        if (
                            rayPosition.X >= 0 && rayPosition.X < Width &&
                            rayPosition.Y >= 0 && rayPosition.Y < Height &&
                            rayPosition.Z >= 0 && rayPosition.Z < Length
                            )
                        {
                            // Get ray coordiates on map
                            int mapX = (int)rayPosition.X;
                            int mapY = (int)rayPosition.Y;
                            int mapZ = (int)rayPosition.Z;

                            // Set hit block
                            hitBlock = (BlockType)map[mapX, mapY, mapZ];
                        }

                        // If wall not hit, increment ray position and distance
                        if (!hitWall)
                        {
                            rayPosition += rayStep;
                            rayDistance += RayStepDist;
                        }
                    }

                    // Skip draw if showing sky
                    if (ShowSky && rayDistance >= MaxDepth) continue;

                    // Draw pixel based on ray distance
                    float closeFactor = 1 - (rayDistance / MaxDepth);
                    Rectangle rect = new Rectangle(x * Drawing.Grid, y * Drawing.Grid, Drawing.Grid, Drawing.Grid);
                    byte colorFactor = (byte)(255 * closeFactor);
                    Color color = new Color(colorFactor, colorFactor, colorFactor);
                    // Tint color based on hit block
                    //switch (hitBlock)
                    //{
                    //    case BlockType.Red:
                    //        color.G /= 2;
                    //        color.B /= 2;
                    //        break;
                    //    case BlockType.Green:
                    //        color.R /= 2;
                    //        color.B /= 2;
                    //        break;
                    //    case BlockType.Blue:
                    //        color.R /= 2;
                    //        color.G /= 2;
                    //        break;
                    //}
                    Drawing.DrawRect(rect, color, game);
                }
            }

            // Draw data text
            Drawing.DrawText("pos: " + position.ToString(), new Vector2(8, 8), Color.White, game);
            Drawing.DrawText("angle: " + angle.ToString(), new Vector2(8, 24), Color.White, game);
            Drawing.DrawText("fps: " + fps.ToString(), new Vector2(8, 40), Color.White, game);

            // Draw crosshair
            Rectangle crosshairRect = new Rectangle(Drawing.Width / 2 - 4, Drawing.Height / 2 - 1, 8, 2);
            Drawing.DrawRect(crosshairRect, Color.White, game);
            crosshairRect = new Rectangle(Drawing.Width / 2 - 1, Drawing.Height / 2 - 4, 2, 8);
            Drawing.DrawRect(crosshairRect, Color.White, game);
        }

        private void ProcessKeyboardState(Game1 game)
        {
            KeyboardState state = game.KeyboardState;

            // Get movement direction
            if (state.IsKeyDown(Keys.W)) direction.X = 1;
            else if (state.IsKeyDown(Keys.S)) direction.X = -1;
            else direction.X = 0;
            if (state.IsKeyDown(Keys.D)) direction.Z = 1;
            else if (state.IsKeyDown(Keys.A)) direction.Z = -1;
            else direction.Z = 0;
            if (state.IsKeyDown(Keys.LeftShift)) direction.Y = 1;
            else if (state.IsKeyDown(Keys.Space)) direction.Y = -1;
            else direction.Y = 0;

            // Get movement rotation
            if (state.IsKeyDown(Keys.Up)) rotation.Y = -1;
            else if (state.IsKeyDown(Keys.Down)) rotation.Y = 1;
            else rotation.Y = 0;
            if (state.IsKeyDown(Keys.Right)) rotation.X = 1;
            else if (state.IsKeyDown(Keys.Left)) rotation.X = -1;
            else rotation.X = 0;
        }

        // Moves player based on given delta
        private void MovePlayer(float delta)
        {
            // Update angle
            angle += rotation * Spin * delta;
            // Clamp angle Y
            angle.Y = Math.Clamp(angle.Y, Pi / -2, Pi / 2);

            // Update position frontways
            position.X += direction.X * (float)Math.Cos(angle.X) * (float)Math.Cos(angle.Y) * Speed * delta;
            position.Y += direction.X * (float)Math.Sin(angle.Y) * Speed * delta;
            position.Z += direction.X * (float)Math.Sin(angle.X) * (float)Math.Cos(angle.Y) * Speed * delta;
            // Update position sideways
            position.X -= direction.Z * (float)Math.Sin(angle.X) * Speed * delta;
            position.Z += direction.Z * (float)Math.Cos(angle.X) * Speed * delta;
            // Update position vertically
            position.Y += direction.Y * Speed * delta;
        }
    }
}
