using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawToScreen
{
    class Program
    {
        public static int Width => Screen.PrimaryScreen.Bounds.Width;
        public static int Height => Screen.PrimaryScreen.Bounds.Height;
        public static Random Random { get; set; } = new Random(DateTime.Now.Millisecond);
        public static List<Circle> Circles { get; set; } = new List<Circle>();

        static void Main(string[] args)
        {
            var render = new Renderer(Height, Width, Win32.Desktop());
            var canvas = new Canvas(new float[4], Width, Height);

            for (int i = 0; i < 200; i++)
            {
                var circle = new Circle()
                {
                    Location = new Vector2(Random.Next(0, Width), Random.Next(0, Height)),
                    Direction = new Vector2((float)Random.NextDouble(), (float)Random.NextDouble()),
                    Radius = Random.Next(5, 50),
                    FillColor = new float[] { 0, 1, 1, 1 },
                };
                Circles.Add(circle);
            }

            while (true)
            {
                Parallel.For(0, Circles.Count, i =>
                {
                    for (int j = 0; j < Circles.Count; j++)
                    {
                        if (i != j)
                        {
                            Circles[i].Collide(Circles[j]);
                        }
                    }
                    Circles[i].DoStuff();
                });

                foreach (var circle in Circles)
                {
                    //circle.DoStuff();
                    circle.Draw(canvas);
                }
                render.Render(canvas);
            }
        }
    }
}
