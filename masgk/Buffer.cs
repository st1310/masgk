﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace masgk
{
    public class Buffer : IDisposable
    {
        public Bitmap colorBuffer;
        public int[] bitsBuffer;

        protected GCHandle bitsHandle;

        public readonly int width;
        public readonly int height;

        public Buffer(int x, int y, Color col)
        {
            width = x;
            height = y;

            bitsBuffer = new int[x * y];
            bitsHandle = GCHandle.Alloc(bitsBuffer, GCHandleType.Pinned);
            colorBuffer = new Bitmap(x, y, x * 4, PixelFormat.Format32bppPArgb, bitsHandle.AddrOfPinnedObject());
            Clear(col);
        }

        public void SetPixel(int x, int y, Color col)
        {
            int index = x + (y * width);
            int c = col.ToArgb();

            bitsBuffer[index] = c;
        }

        public void Clear(Color col)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    {
                        SetPixel(i, j, col);
                    }
        }

        public void Save(string filename)
        {
            colorBuffer.Save(filename + ".png", ImageFormat.Png);
        }

        public void Dispose()
        {
            colorBuffer.Dispose();
            bitsHandle.Free();
        }
    }
}
