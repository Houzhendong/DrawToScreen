using SharpDX.Direct2D1;
using System.Numerics;

namespace DrawToScreen
{
    public interface ICanvas
    {
        void Draw(RenderTarget renderTarget);
        void Fill(Vector2 location, float radius, float[] fillColor);
        float Width { get; }
        float Height { get; }
    }
}
