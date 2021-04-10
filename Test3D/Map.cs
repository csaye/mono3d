using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Test3D
{
    public class Map
    {
        private float fps;

        // Map data
        private BlockType[,,] map;

        private const int width = 32;
        private const int height = 32;
        private const int length = 32;

        // Player data
        private Vector3 position;
        private Vector2 angle;

        private Vector3 direction;
        private Vector2 rotation;

        private const float Speed = 3;
        private const float Spin = 1;

        private const float Fov = (float)Math.PI / 4;

        // Ray data
        private const float RayStep = 0.4f;
        private const float MaxDepth = 64;

        public Map()
        {
            // Initialize map
            map = new BlockType[width, height, length];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        // Calculate grass positions
                        int floorLevel = height / 4;
                        if (y < floorLevel)
                        {
                            int centered = Math.Abs((x + z) - (width / 2 + length / 2));
                            int centered01 = 1 - (centered / (width / 2 + length / 2));
                            if (centered01 * floorLevel < y)
                            {
                                map[x, y, z] = BlockType.Grass;
                            }
                        }
                    }
                }
            }

            // Initialize player
            position = new Vector3(width / 2, height / 2, length / 2);
            angle = new Vector2(0, 0);
        }

        public void Update(GameTime gameTime, Game1 game)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds; // Get time delta
            fps = 1 / delta; // Set fps
            ProcessKeyboardState(game); // Process keyboard state
            MovePlayer(delta); // Move player by delta
        }

        public void Draw(Game1 game)
        {
            // Cast ray for each grid
            for (int x = 0; x < Drawing.GridWidth; x++)
            {
                for (int y = 0; y < Drawing.GridHeight; y++)
                {
                    // Get ray angle
                    float rayX = angle.X - (Fov / 2) + (Fov * ((float)x / Drawing.GridWidth));
                    float rayY = angle.Y - (Fov / 2) + (Fov * ((float)y / Drawing.GridHeight));
                    Vector2 rayAngle = new Vector2(rayX, rayY);

                    // Initialize ray
                    Vector3 rayPosition = position;
                    float rayDistance = 0;
                    bool hitWall = false;

                    // While wall not hit and ray not exceeded max depth
                    while (!hitWall && rayDistance < MaxDepth)
                    {
                        // If in bounds, check whether wall hit
                        if (
                            rayPosition.X >= 0 && rayPosition.X < width &&
                            rayPosition.Y >= 0 && rayPosition.Y < height &&
                            rayPosition.Z >= 0 && rayPosition.Z < length
                            )
                        {
                            int mapX = (int)rayPosition.X;
                            int mapY = (int)rayPosition.Y;
                            int mapZ = (int)rayPosition.Z;

                            try
                            {
                                if (map[mapX, mapY, mapZ] != BlockType.Air) hitWall = true;
                            }
                            catch
                            {
                                Console.WriteLine($"{mapX}, {mapY}, {mapZ}");
                            }
                        }

                        // If wall not hit, increment ray distance
                        if (!hitWall)
                        {
                            rayPosition.X += RayStep * (float)Math.Cos(rayAngle.X) * (float)Math.Cos(rayAngle.Y);
                            rayPosition.Y += RayStep * (float)Math.Sin(rayAngle.Y);
                            rayPosition.Z += RayStep * (float)Math.Sin(rayAngle.X) * (float)Math.Cos(rayAngle.Y);
                            rayDistance += RayStep;
                        }
                    }

                    //if (rayDistance >= MaxDepth) continue;

                    // Draw pixel based on ray distance
                    float closeFactor = 1 - (rayDistance / MaxDepth);
                    //Console.WriteLine(closeFactor);
                    Rectangle rect = new Rectangle(x * Drawing.Grid, y * Drawing.Grid, Drawing.Grid, Drawing.Grid);
                    int colorFactor = (int)(255 * closeFactor);
                    Color color = new Color(colorFactor, colorFactor, colorFactor);
                    Drawing.DrawRect(rect, color, game);
                }
            }

            // Draw data text
            Drawing.DrawText("pos: " + position.ToString(), new Vector2(8, 8), Color.White, game);
            Drawing.DrawText("angle: " + angle.ToString(), new Vector2(8, 24), Color.White, game);
            Drawing.DrawText("fps: " + fps.ToString(), new Vector2(8, 40), Color.White, game);
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
            angle.Y = Math.Clamp(angle.Y, (float)Math.PI / -2, (float)Math.PI / 2);

            // Update position frontways
            position.X += direction.X * (float)Math.Cos(angle.X) * (float)Math.Cos(angle.Y) * Speed * delta;
            position.Y += direction.X * (float)Math.Sin(angle.Y) * Speed * delta;
            position.Z += direction.X * (float)Math.Sin(angle.X) * (float)Math.Cos(angle.Y) * Speed * delta;
            // Update position sideways
            position.X += direction.Z * (float)Math.Sin(angle.X) * Speed * delta;
            position.Z += direction.Z * (float)Math.Cos(angle.X) * Speed * delta;
            // Update position vertically
            position.Y += direction.Y * Speed * delta;
        }
    }
}
