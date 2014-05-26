using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Raytracer.Worlds;

namespace Disque.Raytracer.Tracers
{
    public class PhotonTracer : PathTrace
    {
        public PhotonTracer(World world)
            : base(world)
        {
        }
    }
}