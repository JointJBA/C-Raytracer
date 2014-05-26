using System;
using System.Collections.Generic;
using System.Text;
using Disque.Raytracer.GeometricObjects.Triangles;
using Disque.Math;
using Disque.Raytracer.Materials;

namespace Disque.Raytracer.GeometricObjects.Meshes
{
    public class SmoothHeightMap : GeometricObject
    {
        BBox box = new BBox();
        int w, h;
        float height, cellSize;
        byte[] map;
        Mesh m;

        public SmoothHeightMap(byte[] heightpoints, int width, int length, float csize, float _height, string name)
            : base(name)
        {
            m = new Mesh(name + "Mesh");
            w = width;
            h = length;
            height = _height;
            cellSize = csize;
            map = heightpoints;
            createMesh();
        }

        void createMesh()
        {
            createVertices();
            createIndices();
        }

        void createVertices()
        {
            Vector3 offsetToCenter = -new Vector3((w / 2.0f) * cellSize, 0, (h / 2.0f) * cellSize);
            for (int x = 0; x < w; x++)
            {
                for (int z = 0; z < h; z++)
                {
                    Vector3 position = new Vector3(x * cellSize, (((float)map[x + z * w]) / 255.0f) * height, z * cellSize) + offsetToCenter;
                    //Vector2 uv = new Vector2(x / ((float)width), z / ((float)length));
                    m.Vertices.Add(position);
                }
            }
        }

        void createIndices()
        {
            for (int x = 0; x < w - 1; x++)
                for (int z = 0; z < h - 1; z++)
                {
                    int upperLeft = (z * w + x);
                    int upperRight = (upperLeft + 1);
                    int lowerLeft = (upperLeft + w);
                    int lowerRight = (lowerLeft + 1);
                    SmoothMeshTriangle fmt = new SmoothMeshTriangle(upperLeft, upperRight, lowerLeft, m, Name + x + "" + z);
                    box = BBox.Join(box, fmt.GetBoundingBox());
                    m.AddObject(fmt);
                    SmoothMeshTriangle fmt2 = new SmoothMeshTriangle(lowerLeft, upperRight, lowerRight, m, Name + x + "" + z + "2");
                    box = BBox.Join(box, fmt2.GetBoundingBox());
                    m.AddObject(fmt2);
                }
        }

        public override void SetMaterial(Materials.Material mm)
        {
            m.SetMaterial(mm);
        }

        public override Material GetMaterial()
        {
            return m.GetMaterial();
        }

        public override bool Hit(Ray ray, ref float tmin, ref ShadeRec sr)
        {
            if (box.Hit(ray))
            {
                return m.Hit(ray, ref tmin, ref sr);
            }
            return false;
        }

        public override bool ShadowHit(Ray ray, ref float tmin)
        {
            if (box.Hit(ray))
            {
                return m.ShadowHit(ray, ref tmin);
            }
            return false;
        }
    }
}
