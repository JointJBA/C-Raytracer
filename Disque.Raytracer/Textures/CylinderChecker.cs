using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.Textures
{
    public class CylinderChecker : Texture
    {
        Vector3 color1, color2, line_color;
        float horizontal_line_width, vertical_line_width, h;
        int num_horizontal_checkers, num_vertical_checkers;

        public CylinderChecker(float _h)
        {
            h = _h;
        }

        public void SetColors(Vector3 c1, Vector3 c2, Vector3 lnclr)
        {
            color1 = c1;
            color2 = c2;
            line_color = lnclr;
        }

        public void SetWidths(float hwidth, float vwidth)
        {
            horizontal_line_width = hwidth;
            vertical_line_width = vwidth;
        }

        public void SetCheckerNumbers(int hc, int hv)
        {
            num_horizontal_checkers = hc;
            num_vertical_checkers = hv;
        }

        public override Vector3 GetColor(ShadeRec sr)
        {
            float x = sr.LocalHitPoint.X;
            float y = sr.LocalHitPoint.Y;
            float z = sr.LocalHitPoint.Z;
            float ph = y;
            float phi = MathHelper.Atan2(x, z);
            if (phi < 0.0)
                phi += 2.0f * MathHelper.PI;
            float phi_size = 2.0f * MathHelper.PI / num_horizontal_checkers;
            float theta_size = h / num_vertical_checkers;
            int i_phi = MathHelper.Floor(phi / phi_size);
            int i_theta = MathHelper.Floor(ph / theta_size);
            float f_phi = phi / phi_size - i_phi;
            float f_theta = ph / theta_size - i_theta;
            float phi_line_width = 0.5f * vertical_line_width;
            float theta_line_width = 0.5f * horizontal_line_width;
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
