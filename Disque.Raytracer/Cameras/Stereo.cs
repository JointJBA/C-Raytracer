using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.Worlds;
using Disque.Math;

namespace Disque.Raytracer.Cameras
{
    public enum ViewType
    {
        Parallel, Transverse
    }

    public class Stereo : Camera
    {
        public ViewType ViewType;
        public int PixelGap;
        public float Beta;
        public Camera LeftCamera, RightCamera;

        public void SetupCamera() { }

        public override void RenderScene(World world)
        {
            ViewPlane vp = world.ViewPlane;
            int hres = vp.HRes;
            int vreh = vp.VRes;
            float r = Vector3.Distance(Position, Target);
            float x = r * MathHelper.Tan(0.5f * Beta * MathHelper.PIOn180);
            switch (ViewType)
            {
                case ViewType.Parallel:
                    LeftCamera.RenderStereo(world, x, 0);
                    RightCamera.RenderStereo(world, -x, hres + PixelGap);
                    break;
                case ViewType.Transverse:
                    RightCamera.RenderStereo(world, -x, 0);
                    LeftCamera.RenderStereo(world, x, hres + PixelGap);
                    break;
            }

        }
    }
}
