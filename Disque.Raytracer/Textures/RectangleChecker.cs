using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Textures
{
    public class RectangleChecker : Texture
    {
        Vector3 color1, color2, line_color, p0, a, b;
        int num_x_checkers, num_z_checkers;
        float x_line_width, z_line_width;

        public void SetVector3(Vector3 p, Vector3 aa, Vector3 bb)
        {
            p0 = p;
            a = aa;
            b = bb;
        }

        public void SetColors(Vector3 c1, Vector3 c2, Vector3 lc)
        {
            color1 = c1;
            color2 = c2;
            line_color = lc;
        }

        public void SetWidths(float xw, float zw)
        {
            x_line_width = xw;
            z_line_width = zw;
        }

        public void SetNs(int nx, int nz)
        {
            num_x_checkers = nx;
            num_z_checkers = nz;
        }

        public override Vector3 GetColor(ShadeRec sr)
        {
            float x = sr.LocalHitPoint.X;
            float y = sr.LocalHitPoint.Y;
            float z = sr.LocalHitPoint.Z;
            float x_size = b.X / num_x_checkers;   	// in radians - azimuth angle
            float z_size = a.Z / num_z_checkers;
            int i_phi = MathHelper.Floor(x / x_size);
            int i_theta = MathHelper.Floor(z / z_size);
            float f_phi = x / x_size - i_phi;
            float f_theta = z / z_size - i_theta;
            float phi_line_width = 0.5f * x_line_width;
            float theta_line_width = 0.5f * z_line_width;
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
