using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mono3D
{
    public class Player
    {
        private const float Pi = (float)Math.PI;

        private Vector3 position; // Player position
        private Vector2 angle; // Player angle

        private Vector3 direction; // Current player direction
        private Vector2 rotation; // Current player rotation

        private const float Speed = 3; // Player movement speed
        private const float Spin = 1; // Player rotation speed

        private const float BaseFov = Pi / 4; // Base field of view
        private readonly Vector2 Fov;

        public Vector3 GetPosition() => position;
        public Vector2 GetAngle() => angle;
        public Vector2 GetFov() => Fov;

        public Player()
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
        }

        public void Update(GameTime gameTime, Game1 game)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds; // Get time delta
            ProcessKeyboardState(game); // Process keyboard state
            MovePlayer(delta); // Move player by delta
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
