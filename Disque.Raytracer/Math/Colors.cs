using System;
using System.Collections.Generic;
using System.Text;
using Disque.Math;

namespace Disque.Math
{
    public static class Colors
    {
        /// <summary>
        /// A list of common Colors.
        /// </summary>
        public static Vector3 Gold
        {
            get
            {
                return new Vector3(1, 0.83984375f, 0);
            }
        }
        public static Vector3 Red
        {
            get
            {
                return new Vector3(1, 0, 0);
            }
        }
        public static Vector3 Green
        {
            get
            {
                return new Vector3(0, 1, 0);
            }
        }
        public static Vector3 Blue
        {
            get
            {
                return new Vector3(0, 0, 1);
            }
        }
        public static Vector3 Black
        {
            get
            {
                return Vector3.Zero;
            }
        }
        public static Vector3 White
        {
            get
            {
                return Vector3.One;
            }
        }
        public static Vector3 Yellow
        {
            get
            {
                return Green + Red;
            }
        }
        public static Vector3 Pink
        {
            get
            {
                return new Vector3(1, 0.796875f, 0.89453125f);
            }
        }
    }
}
