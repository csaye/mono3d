using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Test3D
{
    public class Map
    {
        private BlockType[,,] map;

        private const int width = 32;
        private const int height = 32;
        private const int length = 32;

        private Vector3 position;
        private Vector2 angle;

        private Vector2 direction;
        private Vector2 rotation;

        private const float Speed = 1;
        private const float Spin = 1;

        public Map()
        {
            // Initialize map
            map = new BlockType[width, length, height];
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
            ProcessKeyboardState(game); // Process keyboard state
            MovePlayer(delta); // Move player by delta
        }

        public void Draw(Game1 game)
        {

        }

        private void ProcessKeyboardState(Game1 game)
        {
            KeyboardState state = game.KeyboardState;

            // Get movement direction
            if (state.IsKeyDown(Keys.W)) direction.Y = 1;
            else if (state.IsKeyDown(Keys.S)) direction.Y = -1;
            else direction.Y = 0;
            if (state.IsKeyDown(Keys.D)) direction.X = 1;
            else if (state.IsKeyDown(Keys.A)) direction.X = -1;
            else direction.X = 0;

            // Get movement rotation
            if (state.IsKeyDown(Keys.Up)) rotation.Y = 1;
            else if (state.IsKeyDown(Keys.Down)) rotation.Y = -1;
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

            // Update position
            position += new Vector3(direction.X, direction.Y, 0) * Speed * delta;
        }
    }
}
