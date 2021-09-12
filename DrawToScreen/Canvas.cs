using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DrawToScreen
{
    public class Canvas : ICanvas
    {
        private readonly RawColor4 backColor;
        private SolidColorBrush solidColorBrush;
        private List<Action<RenderTarget>> drawActions = new List<Action<RenderTarget>>();

        public Canvas(float[] backColor, float width, float height)
        {
            this.backColor = new RawColor4(backColor[0], backColor[1], backColor[2], backColor[3]);
            Width = width;
            Height = height;
        }

        public float Width { get; }

        public float Height { get; }

        public void Draw(RenderTarget renderTarget)
        {
            renderTarget?.Clear(backColor);
            foreach (var item in drawActions)
            {
                item?.Invoke(renderTarget);
            }

            drawActions.Clear();
        }

        public void Fill(Vector2 location, float radius, float[] fillColor)
        {
            drawActions.Add(renderTarget => { Fill(renderTarget, location, radius, fillColor); });
        }

        private void Fill(RenderTarget renderTarget, Vector2 location, float radius, float[] fillColor)
        {
            if(solidColorBrush == null)
            {
                solidColorBrush = new SolidColorBrush(renderTarget, new RawColor4(fillColor[0], fillColor[1], fillColor[2], fillColor[3]));
            }
            else
            {
                solidColorBrush.Color = new RawColor4(fillColor[0], fillColor[1], fillColor[2], fillColor[3]);
            }

            renderTarget.FillEllipse(new Ellipse(new RawVector2(location.X, location.Y), radius, radius), solidColorBrush);
        }
    }
}
