using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Textures
{
    public class DiskChecker : Texture
    {
        Vector3 color1, color2, line_color, center;
        float angular_line_width, radial_line_width, radius;
        int num_angular_checkers, num_radial_checkers;

        public void SetRadius(float r)
        {
            radius = r;
        }

        public void SetColors(Vector3 c1, Vector3 c2, Vector3 lnclr)
        {
            color1 = c1;
            color2 = c2;
            line_color = lnclr;
        }

        public void SetWidths(float awidth, float rwidth)
        {
            angular_line_width = awidth;
            radial_line_width = rwidth;
        }

        public void SetCheckerNumbers(int ha, int hr)
        {
            num_angular_checkers = ha;
            num_radial_checkers = hr;
        }

        public void SetCenter(Vector3 c)
        {
            center = c;
        }

        public override Vector3 GetColor(ShadeRec sr)
        {
            float x = sr.LocalHitPoint.X - center.X;
            float y = sr.LocalHitPoint.Y - center.Y;
            float z = sr.LocalHitPoint.Z - center.Z;
            float phi = MathHelper.Atan2(x, z);
            float phi_size = 2 * MathHelper.PI / num_angular_checkers;
            float theta_size = radius / num_radial_checkers;
            float ra = MathHelper.Sqrt(x * x + z * z) / radius;
            int i_phi = MathHelper.Floor(phi / phi_size);
            int i_theta = MathHelper.Floor(ra / theta_size);
            float f_phi = phi / phi_size - i_phi;
            float f_theta = ra / theta_size - i_theta;
            float phi_line_width = 0.5f * angular_line_width;
            float theta_line_width = 0.5f * radial_line_width;
            bool in_outline = (f_phi < phi_line_width || f_phi > 1.0 - phi_line_width) ||
                                (f_theta < theta_line_width || f_theta > 1.0 - theta_line_width);
            if ((i_phi + i_theta) % 2 == 0)
            {
                if (!in_outline)
                    return (color1);
            }
            else
            {
                if (!in_outline)
                    return (color2);
            }
            return (line_color);
        }
    }
}
