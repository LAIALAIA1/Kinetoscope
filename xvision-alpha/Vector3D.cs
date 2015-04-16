using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class Vector3D
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3D(Vector3D vect)
        {
            this.x = vect.x;
            this.y = vect.y;
            this.z = vect.z;
        }

        public void addSelf(Vector3D vect)
        {
            this.x += vect.x;
            this.y += vect.y;
            this.z += vect.z;
        }

        public Vector3D add(Vector3D vect)
        {
            return new Vector3D(x + vect.x, y + vect.y, z + vect.z);
        }

        public void subSelf(Vector3D vect)
        {
            this.x -= vect.x;
            this.y -= vect.y;
            this.z -= vect.z;
        }

        public Vector3D sub(Vector3D vect)
        {
            return new Vector3D(x - vect.x, y - vect.y, z - vect.z);
        }

        public void multSelf(double scalar)
        {
            this.x *= scalar;
            this.y *= scalar;
            this.z *= scalar;
        }

        public Vector3D mult(double scalar)
        {
            Vector3D copy = new Vector3D(this);
            copy.multSelf(scalar);
            return copy;
        }


        public double scalarProduct(Vector3D vect)
        {
            return x * vect.x + y * vect.y + z * vect.z;
        }

        public override string ToString()
        {
            return "[" + this.x + " ; " + this.y + " ; " + this.z + "]";
        }
    }
}
