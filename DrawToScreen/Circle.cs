using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawToScreen
{
    public class Circle
    {
        public static float MaxSize = 50f;
        public Vector2 Location { get; set; }
        public Vector2 Direction { get; set; }

        public Vector2 Mass => new Vector2(Radius / MaxSize, Radius / MaxSize);

        public float Radius { get; set; }
        public float[] FillColor { get; set; }

        public void DoStuff()
        {
            Location += Direction;
            Direction = new Vector2(Direction.X, Direction.Y + Mass.Y);
            KeepWithinScreenBounds();
            CollideMouse();
        }

        public void Draw(ICanvas canvas)
        {
            canvas.Fill(Location, Radius, FillColor);
        }

        public void Collide(Circle v, float springForce = 0.5f, float damping = 0.02f, float shearForce = 0.1f)
        {
            if (v != null)
            {
                var relPos = v.Location - Location;
                var dist = relPos.Length();
                var collideDist = Radius + v.Radius;
                if (dist < collideDist)
                {
                    var relVel = v.Direction - Direction;
                    var norm = relPos / (dist + 0.00001f);
                    var force = norm * -springForce * (collideDist - dist);
                    force += damping * relVel;
                    force += shearForce * (relVel - Vector2.Dot(relVel, norm) * norm);
                    Direction += force * v.Mass;
                    v.Direction -= force * Mass;
                }

            }
        }

        public void CollideMouse(float springForce = 0.5f, float damping = 0.02f, float shearForce = 0.1f)
        {
            var mouse = new Vector2(Cursor.Position.X, Cursor.Position.Y);
            var mouseSize = 50f;
            var relPos = Location - mouse;
            var dist = relPos.Length();
            var collideDist = Radius + mouseSize;
            if (dist < collideDist)
            {
                var relVel = Direction; 
                var norm = relPos / (dist + 0.00001f);
                var force = norm * -springForce * (collideDist - dist);
                force += damping * relVel;
                force += shearForce * (relVel - Vector2.Dot(relVel, norm) * norm);
                Direction -= force * Mass;
            }

        }
        private void KeepWithinScreenBounds()
        {
            Left();
            Right((int)(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / 1.25));
            Top();
            Bottom(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height);
        }

        private void Bottom(int height)
        {
            if (Location.Y >= height - Radius)
            {
                if (Direction.Y > 0)
                {
                    Direction *= new Vector2(1, -0.7f);
                }
                Location = new Vector2(Location.X, height - Radius);

            }
        }

        private void Top()
        {
            if (Location.Y <= Radius)
            {
                if (Direction.Y < 0)
                {
                    Direction *= new Vector2(1, -0.7f);
                }
                Location = new Vector2(Location.X, Radius);
            }
        }

        private void Right(int width)
        {
            if (Location.X + Radius >= width)
            {
                if (Direction.X > 0)
                {
                    Direction *= new Vector2(-0.7f, 1);
                }
                Location = new Vector2(width - Radius, Location.Y);
            }
        }

        private void Left()
        {
            if (Location.X <= Radius)
            {
                if (Location.X < 0)
                {
                    Direction *= new Vector2(-0.7f, 1);
                }
                Location = new Vector2(Radius, Location.Y);
            }
        }
    }
}
