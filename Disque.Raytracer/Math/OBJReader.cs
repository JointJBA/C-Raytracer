using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disque.Raytracer.GeometricObjects.Triangles;
using Disque.Raytracer.GeometricObjects;
using Disque.Math;

namespace Disque.Raytracer.Math
{
    public class OBJReader
    {
        string[] lines;
        Mesh ret = new Mesh(GeometricObject.GetUniqueRandomName());
        bool hasNormals = false;

        public OBJReader(string txt)
        {
            lines = txt.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            parseVertices();
            parseNormals();
            parseFaces();
        }

        void parseVertices()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i][0] == 'v' && lines[i][1] != 'n')
                {
                    ret.Vertices.Add(ToVector3(lines[i].Replace("v ", "").Replace(" ", ",")));
                }
            }
        }

        void parseNormals()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i][0] == 'v' && lines[i][1] == 'n')
                {
                    hasNormals = true;
                    ret.Normals.Add(ToVector3(lines[i].Replace("vn ", "").Replace(" ", ",")));
                }
            }
        }

        void parseFaces()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i][0] == 'f')
                {
                    string[] w = lines[i].Replace("f ", "").Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < w.Length - 2; j++)
                    {
                        if (hasNormals)
                        {
                            SmoothMeshTriangle fmt = new SmoothMeshTriangle(int.Parse(w[j]) - 1, int.Parse(w[j + 1]) - 1, int.Parse(w[j + 2]) - 1, ret, i + "" + j + "smsh" + "Mesh" + ret.Name);
                            ret.AddObject(fmt);
                        }
                        else
                        {
                            FlatMeshTriangle fmt = new FlatMeshTriangle(int.Parse(w[j]) - 1, int.Parse(w[j + 1]) - 1, int.Parse(w[j + 2]) - 1, ret, i + "" + j + "fmsh" + "Mesh" + ret.Name);
                            fmt.ComputeNormal(false);
                            ret.AddObject(fmt);
                        }
                    }
                }
            }
        }

        public Mesh GetMesh()
        {
            return ret;
        }

        public string ToRml()
        {
            string head = "<Mesh>\r\n";
            head += "\t<Mesh.Vertices>\r\n";
            foreach (Vector3 v in ret.Vertices)
            {
                head += "\t\t<Vertex Position='" + v.X + "," + v.Y + "," + v.Z + "'/>\r\n";
            }
            head += "\t</Mesh.Vertices>\r\n";
            if (ret.Normals.Count > 0)
            {
                head += "\t<Mesh.Normals>\r\n";
                foreach (Vector3 v in ret.Normals)
                {
                    head += "\t\t<Normal Vector='" + v.X + "," + v.Y + "," + v.Z + "'/>\r\n";
                }
                head += "\t</Mesh.Normals>\r\n";
            }
            head += "\t<Mesh.Faces>\r\n";
            foreach (MeshTriangle m in ret.Objects)
            {
                head += "\t\t<Face Indices='" + m.Index0 + "," + m.Index1 + "," + m.Index2 + "'/>\r\n";
            }
            head += "\t</Mesh.Faces>\r\n";
            head += "</Mesh>";
            return head;
        }

        static Vector3 ToVector3(string v)
        {
            Vector3 vector = Vector3.Zero;
            if (v.Contains(','))
            {
                string[] nums = v.Split(new string[] { ", ", ",", " , " }, StringSplitOptions.RemoveEmptyEntries);
                vector.X = float.Parse(nums[0]);
                vector.Y = float.Parse(nums[1]);
                vector.Z = float.Parse(nums[2]);
            }
            else
            {
                vector = new Vector3(float.Parse(v));
            }
            return vector;
        }
    }
}
