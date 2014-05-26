using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Raytracer.GeometricObjects.CompoundObjects
{
    public class Grid : Compound
    {        
        List<GeometricObject> cells = new List<GeometricObject>();
        int nx, ny, nz;
        BBox bbox;

        public Grid(string name)
            : base(name)
        {
        }

        Vector3 find_min_bounds()
        {
            BBox object_box;
            Vector3 p0 = new Vector3(float.MaxValue);
            int num_objects = Objects.Count;
            for (int j = 0; j < num_objects; j++)
            {
                object_box = Objects[j].GetBoundingBox();
                if (object_box.Min.X < p0.X)
                    p0.X = object_box.Min.X;
                if (object_box.Min.Y < p0.Y)
                    p0.Y = object_box.Min.Y;
                if (object_box.Min.Z < p0.Z)
                    p0.Z = object_box.Min.Z;
            }
            p0.X -= MathHelper.Epsilon; 
            p0.Y -= MathHelper.Epsilon;
            p0.Z -= MathHelper.Epsilon;
            return p0;
        }

        Vector3 find_max_bounds()
        {
            BBox object_box;
            Vector3 p1 = new Vector3(float.MinValue);
            int num_objects = Objects.Count;
            for (int j = 0; j < num_objects; j++)
            {
                object_box = Objects[j].GetBoundingBox();
                if (object_box.Max.X > p1.X)
                    p1.X = object_box.Max.X;
                if (object_box.Max.Y > p1.Y)
                    p1.Y = object_box.Max.Y;
                if (object_box.Max.Z > p1.Z)
                    p1.Z = object_box.Max.Z;
            }
            p1.X += MathHelper.Epsilon; p1.Y += MathHelper.Epsilon; p1.Z += MathHelper.Epsilon;
            return (p1);
        }

        void setupBoundingBox()
        {
            foreach (GeometricObject obj in Objects)
            {
                obj.SetBoundingBox();
                bbox.Union(obj.GetBoundingBox());
            }
        }

        public void SetupCells()
        {
            setupBoundingBox();
            int num_objects = Objects.Count;
            Vector3 p0 = bbox.Min;
            Vector3 p1 = bbox.Max;
            float wx = bbox.Max.X - bbox.Min.X;
            float wy = bbox.Max.Y - bbox.Min.Y;
            float wz = bbox.Max.Z - bbox.Min.Z;
            float multiplier = 2.0f;
            float s = MathHelper.Pow(wx * wy * wz / num_objects, 0.3333333f);
            nx = (int)(multiplier * wx / s + 1.0f);
            ny = (int)(multiplier * wy / s + 1.0f);
            nz = (int)(multiplier * wz / s + 1.0f);
            int num_cells = nx * ny * nz;
            for (int j = 0; j < num_cells; j++)
                cells.Add(null);
            List<int> counts = new List<int>(num_cells);
            for (int j = 0; j < num_cells; j++)
                counts.Add(0);
            BBox obj_bBox;
            int index;
            for (int j = 0; j < num_objects; j++)
            {
                obj_bBox = Objects[j].GetBoundingBox();
                int ixmin = clamp((obj_bBox.Min.X - p0.X) * nx / (p1.X - p0.X), 0, nx - 1);
                int iymin = clamp((obj_bBox.Min.Y - p0.Y) * ny / (p1.Y - p0.Y), 0, ny - 1);
                int izmin = clamp((obj_bBox.Min.Z - p0.Z) * nz / (p1.Z - p0.Z), 0, nz - 1);
                int ixmax = clamp((obj_bBox.Max.X - p0.X) * nx / (p1.X - p0.X), 0, nx - 1);
                int iymax = clamp((obj_bBox.Max.Y - p0.Y) * ny / (p1.Y - p0.Y), 0, ny - 1);
                int izmax = clamp((obj_bBox.Max.Z - p0.Z) * nz / (p1.Z - p0.Z), 0, nz - 1);
                for (int iz = izmin; iz <= izmax; iz++) 					// cells in z direction
                    for (int iy = iymin; iy <= iymax; iy++)					// cells in y direction
                        for (int ix = ixmin; ix <= ixmax; ix++)
                        {			// cells in x direction
                            index = ix + nx * iy + nx * ny * iz;

                            if (counts[index] == 0)
                            {
                                cells[index] = Objects[j];
                                counts[index] += 1;  						// now = 1
                            }
                            else
                            {
                                if (counts[index] == 1)
                                {
                                    Compound compound_ptr = new Compound(Name + "Compound" + j);	// construct a compound object
                                    compound_ptr.AddObject(cells[index]); // add object already in cell
                                    compound_ptr.AddObject(Objects[j]);  	// add the new object
                                    cells[index] = compound_ptr;			// store compound in current cell
                                    counts[index] += 1;  					// now = 2
                                }
                                else
                                {										// counts[index] > 1
                                    ((Compound)cells[index]).AddObject(Objects[j]);	// just add current object
                                    counts[index] += 1;						// for statistics only
                                }
                            }
                        }
            }
            Objects.Clear();
            int num_zeroes = 0;
            int num_ones = 0;
            int num_twos = 0;
            int num_threes = 0;
            int num_greater = 0;
            for (int j = 0; j < num_cells; j++)
            {
                if (counts[j] == 0)
                    num_zeroes += 1;
                if (counts[j] == 1)
                    num_ones += 1;
                if (counts[j] == 2)
                    num_twos += 1;
                if (counts[j] == 3)
                    num_threes += 1;
                if (counts[j] > 3)
                    num_greater += 1;
            }
            counts.Clear();
        }

        public override bool Hit(Ray ray, ref float t, ref ShadeRec sr)
        {
            float ox = ray.Position.X;
            float oy = ray.Position.Y;
            float oz = ray.Position.Z;
            float dx = ray.Direction.X;
            float dy = ray.Direction.Y;
            float dz = ray.Direction.Z;
            float x0 = bbox.Min.X;
            float y0 = bbox.Min.Y;
            float z0 = bbox.Min.Z;
            float x1 = bbox.Max.X;
            float y1 = bbox.Max.Y;
            float z1 = bbox.Max.Z;
            float tx_min, ty_min, tz_min;
            float tx_max, ty_max, tz_max;
            float a = 1.0f / dx;
            if (a >= 0)
            {
                tx_min = (x0 - ox) * a;
                tx_max = (x1 - ox) * a;
            }
            else
            {
                tx_min = (x1 - ox) * a;
                tx_max = (x0 - ox) * a;
            }
            float b = 1.0f / dy;
            if (b >= 0)
            {
                ty_min = (y0 - oy) * b;
                ty_max = (y1 - oy) * b;
            }
            else
            {
                ty_min = (y1 - oy) * b;
                ty_max = (y0 - oy) * b;
            }
            float c = 1.0f / dz;
            if (c >= 0)
            {
                tz_min = (z0 - oz) * c;
                tz_max = (z1 - oz) * c;
            }
            else
            {
                tz_min = (z1 - oz) * c;
                tz_max = (z0 - oz) * c;
            }
            float t0, t1;
            if (tx_min > ty_min)
                t0 = tx_min;
            else
                t0 = ty_min;

            if (tz_min > t0)
                t0 = tz_min;

            if (tx_max < ty_max)
                t1 = tx_max;
            else
                t1 = ty_max;

            if (tz_max < t1)
                t1 = tz_max;

            if (t0 > t1)
            {
                return (false);
            }
            else
            {
                Console.WriteLine("wtf");
            }
            int ix, iy, iz;
            if (bbox.Inside(ray.Position))
            {  			// does the ray start inside the grid?
                ix = clamp((ox - x0) * nx / (x1 - x0), 0, nx - 1);
                iy = clamp((oy - y0) * ny / (y1 - y0), 0, ny - 1);
                iz = clamp((oz - z0) * nz / (z1 - z0), 0, nz - 1);
            }
            else
            {
                Vector3 p = ray.Position + t0 * ray.Direction;  // initial hit point with grid's bounding box
                ix = clamp((p.X - x0) * nx / (x1 - x0), 0, nx - 1);
                iy = clamp((p.Y - y0) * ny / (y1 - y0), 0, ny - 1);
                iz = clamp((p.Z - z0) * nz / (z1 - z0), 0, nz - 1);
            }
            float dtx = (tx_max - tx_min) / nx;
            float dty = (ty_max - ty_min) / ny;
            float dtz = (tz_max - tz_min) / nz;
            float tx_next, ty_next, tz_next;
            int ix_step, iy_step, iz_step;
            int ix_stop, iy_stop, iz_stop;

            if (dx > 0)
            {
                tx_next = tx_min + (ix + 1) * dtx;
                ix_step = +1;
                ix_stop = nx;
            }
            else
            {
                tx_next = tx_min + (nx - ix) * dtx;
                ix_step = -1;
                ix_stop = -1;
            }

            if (dx == 0.0)
            {
                tx_next = float.MaxValue;
                ix_step = -1;
                ix_stop = -1;
            }
            if (dy > 0)
            {
                ty_next = ty_min + (iy + 1) * dty;
                iy_step = +1;
                iy_stop = ny;
            }
            else
            {
                ty_next = ty_min + (ny - iy) * dty;
                iy_step = -1;
                iy_stop = -1;
            }

            if (dy == 0.0)
            {
                ty_next = float.MaxValue;
                iy_step = -1;
                iy_stop = -1;
            }

            if (dz > 0)
            {
                tz_next = tz_min + (iz + 1) * dtz;
                iz_step = +1;
                iz_stop = nz;
            }
            else
            {
                tz_next = tz_min + (nz - iz) * dtz;
                iz_step = -1;
                iz_stop = -1;
            }

            if (dz == 0.0)
            {
                tz_next = float.MaxValue;
                iz_step = -1;
                iz_stop = -1;
            }
            while (true)
            {
                GeometricObject object_ptr = cells[ix + nx * iy + nx * ny * iz];
                if (tx_next < ty_next && tx_next < tz_next)
                {
                    if (object_ptr != null && object_ptr.Hit(ray, ref t, ref sr) && t < tx_next)
                    {
                        Material = object_ptr.GetMaterial();
                        return (true);
                    }
                    tx_next += dtx;
                    ix += ix_step;

                    if (ix == ix_stop)
                        return (false);
                }
                else
                {
                    if (ty_next < tz_next)
                    {
                        if (object_ptr != null && object_ptr.Hit(ray, ref t, ref sr) && t < ty_next)
                        {
                            Material = object_ptr.GetMaterial();
                            return (true);
                        }
                        ty_next += dty;
                        iy += iy_step;

                        if (iy == iy_stop)
                            return (false);
                    }
                    else
                    {
                        if (object_ptr != null && object_ptr.Hit(ray, ref t, ref sr) && t < tz_next)
                        {
                            Material = object_ptr.GetMaterial();
                            return (true);
                        }
                        tz_next += dtz;
                        iz += iz_step;

                        if (iz == iz_stop)
                            return (false);
                    }
                }
            }
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            float t = float.MaxValue;
            float ox = ray.Position.X;
            float oy = ray.Position.Y;
            float oz = ray.Position.Z;
            float dx = ray.Direction.X;
            float dy = ray.Direction.Y;
            float dz = ray.Direction.Z;
            float x0 = bbox.Min.X;
            float y0 = bbox.Min.Y;
            float z0 = bbox.Min.Z;
            float x1 = bbox.Max.X;
            float y1 = bbox.Max.Y;
            float z1 = bbox.Max.Z;
            float tx_min, ty_min, tz_min;
            float tx_max, ty_max, tz_max;
            float a = 1.0f / dx;
            if (a >= 0)
            {
                tx_min = (x0 - ox) * a;
                tx_max = (x1 - ox) * a;
            }
            else
            {
                tx_min = (x1 - ox) * a;
                tx_max = (x0 - ox) * a;
            }
            float b = 1.0f / dy;
            if (b >= 0)
            {
                ty_min = (y0 - oy) * b;
                ty_max = (y1 - oy) * b;
            }
            else
            {
                ty_min = (y1 - oy) * b;
                ty_max = (y0 - oy) * b;
            }
            float c = 1.0f / dz;
            if (c >= 0)
            {
                tz_min = (z0 - oz) * c;
                tz_max = (z1 - oz) * c;
            }
            else
            {
                tz_min = (z1 - oz) * c;
                tz_max = (z0 - oz) * c;
            }
            float t0, t1;
            if (tx_min > ty_min)
                t0 = tx_min;
            else
                t0 = ty_min;

            if (tz_min > t0)
                t0 = tz_min;

            if (tx_max < ty_max)
                t1 = tx_max;
            else
                t1 = ty_max;

            if (tz_max < t1)
                t1 = tz_max;

            if (t0 > t1)
                return (false);
            int ix, iy, iz;

            if (bbox.Inside(ray.Position))
            {  			// does the ray start inside the grid?
                ix = clamp((ox - x0) * nx / (x1 - x0), 0, nx - 1);
                iy = clamp((oy - y0) * ny / (y1 - y0), 0, ny - 1);
                iz = clamp((oz - z0) * nz / (z1 - z0), 0, nz - 1);
            }
            else
            {
                Vector3 p = ray.Position + t0 * ray.Direction;  // initial hit point with grid's bounding box
                ix = clamp((p.X - x0) * nx / (x1 - x0), 0, nx - 1);
                iy = clamp((p.Y - y0) * ny / (y1 - y0), 0, ny - 1);
                iz = clamp((p.Z - z0) * nz / (z1 - z0), 0, nz - 1);
            }
            float dtx = (tx_max - tx_min) / nx;
            float dty = (ty_max - ty_min) / ny;
            float dtz = (tz_max - tz_min) / nz;
            float tx_next, ty_next, tz_next;
            int ix_step, iy_step, iz_step;
            int ix_stop, iy_stop, iz_stop;

            if (dx > 0)
            {
                tx_next = tx_min + (ix + 1) * dtx;
                ix_step = +1;
                ix_stop = nx;
            }
            else
            {
                tx_next = tx_min + (nx - ix) * dtx;
                ix_step = -1;
                ix_stop = -1;
            }

            if (dx == 0.0)
            {
                tx_next = float.MaxValue;
                ix_step = -1;
                ix_stop = -1;
            }
            if (dy > 0)
            {
                ty_next = ty_min + (iy + 1) * dty;
                iy_step = +1;
                iy_stop = ny;
            }
            else
            {
                ty_next = ty_min + (ny - iy) * dty;
                iy_step = -1;
                iy_stop = -1;
            }

            if (dy == 0.0)
            {
                ty_next = float.MaxValue;
                iy_step = -1;
                iy_stop = -1;
            }

            if (dz > 0)
            {
                tz_next = tz_min + (iz + 1) * dtz;
                iz_step = +1;
                iz_stop = nz;
            }
            else
            {
                tz_next = tz_min + (nz - iz) * dtz;
                iz_step = -1;
                iz_stop = -1;
            }

            if (dz == 0.0)
            {
                tz_next = float.MaxValue;
                iz_step = -1;
                iz_stop = -1;
            }
            while (true)
            {
                GeometricObject object_ptr = cells[ix + nx * iy + nx * ny * iz];
                if (tx_next < ty_next && tx_next < tz_next)
                {
                    if (object_ptr != null && object_ptr.ShadowHit(ray, ref t) && t < tmin)
                    {		//This part is different from hit function, Notice it!
                        tmin = t;
                        return (true);
                    }

                    tx_next += dtx;
                    ix += ix_step;
                    if (ix == ix_stop)
                        return (false);
                }
                else
                {
                    if (ty_next < tz_next)
                    {
                        if (object_ptr != null && object_ptr.ShadowHit(ray, ref t) && t < tmin)
                        {
                            tmin = t;
                            return (true);
                        }

                        ty_next += dty;
                        iy += iy_step;

                        if (iy == iy_stop)
                            return (false);
                    }
                    else
                    {
                        if (object_ptr != null && object_ptr.ShadowHit(ray, ref t) && t < tmin)
                        {
                            tmin = t;
                            return (true);
                        }

                        tz_next += dtz;
                        iz += iz_step;

                        if (iz == iz_stop)
                            return (false);
                    }
                }
            }
        }

        public override BBox GetBoundingBox()
        {
            return bbox;
        }

        int clamp(float x, float min, float max)
        {
            return (int)(x < min ? 0 : (x > max ? max : x));
        }
    }
}
