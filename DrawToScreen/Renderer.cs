using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Device = SharpDX.Direct3D11.Device;
using FeatureLevel = SharpDX.Direct3D.FeatureLevel;

namespace DrawToScreen
{
    public class Renderer
    {
        private readonly int height;
        private readonly int width;
        private readonly RenderTarget renderTarget;
        private SwapChain swapChain;

        public Renderer(int height, int width, IntPtr handle)
        {
            this.height = height;
            this.width = width;
            renderTarget = CreateRenderTarget(handle);

        }

        private RenderTarget CreateRenderTarget(IntPtr handle)
        {
            var swapChainDescription = new SwapChainDescription()
            {
                BufferCount = 2,
                ModeDescription = new ModeDescription(width, height, new Rational(), Format.B8G8R8A8_UNorm),
                IsWindowed = true,
                OutputHandle = handle,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
            };

            Device.CreateWithSwapChain(
                    SharpDX.Direct3D.DriverType.Hardware,
                    SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport,
                    new[] { FeatureLevel.Level_10_0, FeatureLevel.Level_11_1 },
                    swapChainDescription,
                    out Device device,
                    out swapChain);
            var pixelFormat = new PixelFormat(0, SharpDX.Direct2D1.AlphaMode.Premultiplied);
            var renderTargetProperty = new RenderTargetProperties(pixelFormat);
            var texture2D = SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(swapChain, 0);
            var surface = texture2D.QueryInterface<Surface>();
            var factory = new SharpDX.Direct2D1.Factory();

            return new RenderTarget(factory, surface, renderTargetProperty);
        }

        public void Render(ICanvas canvas)
        {
            if (renderTarget == null)
            {
                return;
            }

            renderTarget.BeginDraw();

            canvas.Draw(renderTarget);

            renderTarget.EndDraw();

            swapChain?.Present(0, PresentFlags.DoNotWait);
        }
    }
}
