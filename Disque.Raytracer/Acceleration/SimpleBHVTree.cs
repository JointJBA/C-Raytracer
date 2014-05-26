using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.GeometricObjects.CompoundObjects;
using Disque.Raytracer.GeometricObjects;
using Disque.Math;

namespace Disque.Raytracer.Acceleration
{
    public enum GroupingMethod
    {
        DistanceR, DistanceM
    }

    public class SimpleBVHTree : Compound
    {
        BBox box;
        GroupingMethod grouping;
        int n, gcount = 0;

        List<GeometricObject> temp = new List<GeometricObject>();

        public SimpleBVHTree(GroupingMethod gm, int num_per_group, string name)
            : base(name)
        {
            box = new BBox();
            grouping = gm;
            n = num_per_group;
        }

        public override void AddObject(GeometricObject o)
        {
            if (grouping == GroupingMethod.DistanceR)
            {
                float t = float.MaxValue;
                GeometricObject a = o;
                for (int i = 0; i < Objects.Count; i++)
                {
                    GeometricObject b = Objects[i];
                    if (o != b)
                    {
                        float temp = distance(o.GetBoundingBox(), b.GetBoundingBox());
                        if (temp < t)
                        {
                            t = temp;
                            a = b;
                            break;
                        }
                    }
                }
                if (Objects.Count > 1)
                {
                    Objects.Remove(a);
                    Group g = new Group(Name + "Group" + gcount);
                    gcount++;
                    g.AddObject(a);
                    g.AddObject(o);
                    Objects.Add(g);
                }
                else
                {
                    Objects.Add(a);
                }
            }
            else if (grouping == GroupingMethod.DistanceM)
            {
                temp.Add(o);
            }
        }

        public void PrepareObjects()
        {
            if (grouping == GroupingMethod.DistanceR)
            {
                foreach (GeometricObject obj in Objects)
                {
                    box.Union(obj.GetBoundingBox());
                }
            }
            else if (grouping == GroupingMethod.DistanceM)
            {
                temp.Sort(new SortByBoundingBox());
                int div = temp.Count / n;
                int rem = temp.Count % n;
                for (int i = 0; i < div * n; i += n)
                {
                    Group group = new Group(Name + "Group" + gcount);
                    gcount++;
                    for (int f = 0; f < n; f++)
                    {
                        int num = i + f;
                        group.AddObject(temp[num]);
                    }
                    Objects.Add(group);
                    box.Union(group.GetBoundingBox());
                }
                for (int i = 0; i < rem; i++)
                {
                    Objects.Add(temp[temp.Count - 1 - rem]);
                }
                temp.Clear();
                while (Objects.Count > n)
                {
                    temp = new List<GeometricObject>(Objects);
                    Objects.Clear();
                    temp.Sort(new SortByBoundingBox());
                    int div2 = temp.Count / n;
                    int rem2 = temp.Count % n;
                    for (int i = 0; i < div2 * n; i += n)
                    {
                        Group group = new Group(Name + "Group" + gcount);
                        gcount++;
                        for (int f = 0; f < n; f++)
                        {
                            int num = i + f;
                            group.AddObject(temp[num]);
                        }
                        Objects.Add(group);
                        box.Union(group.GetBoundingBox());
                    }
                    for (int i = 0; i < rem2; i++)
                    {
                        Objects.Add(temp[temp.Count - 1 - rem2]);
                    }
                    temp.Clear();
                }
            }
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            if (box.Hit(ray))
                return base.Hit(ray, ref tmin, ref sr);
            else
                return false;
        }

        public override BBox GetBoundingBox()
        {
            return box;
        }

        float distance(BBox b1, BBox b2)
        {
            Vector3 mid1 = (b1.Min + b1.Max) / 2.0f;
            Vector3 mid2 = (b2.Min + b2.Max) / 2.0f;
            return Vector3.Distance(mid1, mid2);
        }
    }

    public class SortByBoundingBox : IComparer<GeometricObject>
    {
        Vector3 getcenter(Vector3 a, Vector3 b)
        {
            return (a + b) / 2.0f;
        }

        public int Compare(GeometricObject z, GeometricObject f)
        {
            BBox x = z.GetBoundingBox();
            BBox y = f.GetBoundingBox();
            Vector3 a = getcenter(x.Min, x.Max);
            Vector3 b = getcenter(y.Min, y.Max);
            if (MathHelper.Abs(a.X + a.Y + a.Z) > MathHelper.Abs(b.X + b.Y + b.Z))
                return 1;
            else if (MathHelper.Abs(a.X + a.Y + a.Z) < MathHelper.Abs(b.X + b.Y + b.Z))
                return -1;
            else
                return 0;
        }
    }
}
