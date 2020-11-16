using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace masgk
{
    public class DepthBuffer : IDisposable
    {
        public Bitmap depthBuffer;
        public UInt32[] bitsBuffer;
        public float[] bitsFloatBuffer;

        protected GCHandle bitsHandle;

        public DepthBuffer(int x, int y, float col)
        {
            bitsBuffer = new UInt32[x * y];
            bitsFloatBuffer = new float[x * y];
            bitsHandle = GCHandle.Alloc(bitsBuffer, GCHandleType.Pinned);
            depthBuffer = new Bitmap(x, y, x * 4, PixelFormat.Format32bppArgb, bitsHandle.AddrOfPinnedObject());
            Clear(col);
        }

        public void SetDepth(int x, int y, float col)
        {
            int index = x + (y * depthBuffer.Width);

            bitsFloatBuffer[index] = col;
            bitsBuffer[index] = (UInt32)(0xff000000 | ((byte)(col * 255f) << 16));
        }

        public float GetDepth(int x, int y)
        {
            int index = x + (y * depthBuffer.Width);

            return bitsFloatBuffer[index];
        }

        public void Clear(float col)
        {
            for (int i = 0; i < depthBuffer.Width; i++)
                for (int j = 0; j < depthBuffer.Height; j++)
                {
                    SetDepth(i, j, col);
                }
        }

        public void Save(string filename)
        {
            depthBuffer.Save(filename + ".png", ImageFormat.Png);
        }

        public void Dispose()
        {
            depthBuffer.Dispose();
            bitsHandle.Free();
        }
    }
}
