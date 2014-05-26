using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Worlds
{
    public class PixelScreen
    {
        ViewPlane vp;
        public List<Pixel> Pixels = new List<Pixel>();

        public PixelScreen(ViewPlane vp)
        {
            this.vp = vp;
        }

        void addPixel(float x, float y, Color c)
        {
            if (c == null)
                throw new Exception();
            Pixels.Add(new Pixel() { X = x, Y = y, Color = c});
        }

        public void AddPixel(float r, float c, Vector3 L)
        {
            float x = c;
            float y = vp.VRes - r - 1.0f;
            addPixel(x, y, new Color(L));
        }
    }

    public struct Pixel
    {
        public float X;
        public float Y;
        public Color Color;
    }
}
