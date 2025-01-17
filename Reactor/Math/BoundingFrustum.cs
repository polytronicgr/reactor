﻿
// Author:
//       Gabriel Reiser <gabe@reisergames.com>
//
// Copyright (c) 2010-2016 Reiser Games, LLC.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using OpenTK;

namespace Reactor.Math
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    [StructLayout(LayoutKind.Sequential)]
    public class BoundingFrustum : IEquatable<BoundingFrustum>
    {
        #region Private Fields

        private Matrix matrix;
        private readonly Vector3[] corners = new Vector3[CornerCount];
        private readonly Plane[] planes = new Plane[PlaneCount];

        private const int PlaneCount = 6;

        #endregion Private Fields

        #region Public Fields
        public const int CornerCount = 8;
        #endregion

        #region Public Constructors
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundingFrustum(Matrix value)
        {
            this.matrix = value;
            this.CreatePlanes();
            this.CreateCorners();
        }

        #endregion Public Constructors


        #region Public Properties

        public Matrix Matrix
        {
            get { return this.matrix; }
            set
            {
                this.matrix = value;
                this.CreatePlanes();    // FIXME: The odds are the planes will be used a lot more often than the matrix
            	this.CreateCorners();   // is updated, so this should help performance. I hope ;)
			}
        }

        public Plane Near
        {
            get { return this.planes[0]; }
        }
        
        public Plane Far
        {
            get { return this.planes[1]; }
        }
        
        public Plane Left
        {
            get { return this.planes[2]; }
        }

        public Plane Right
        {
            get { return this.planes[3]; }
        }

        public Plane Top
        {
            get { return this.planes[4]; }
        }

        public Plane Bottom
        {
            get { return this.planes[5]; }
        }

        #endregion Public Properties


        #region Public Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundingFrustum a, BoundingFrustum b)
        {
            if (object.Equals(a, null))
                return (object.Equals(b, null));

            if (object.Equals(b, null))
                return (object.Equals(a, null));

            return a.matrix == (b.matrix);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundingFrustum a, BoundingFrustum b)
        {
            return !(a == b);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ContainmentType Contains(BoundingBox box)
        {
            var result = default(ContainmentType);
            this.Contains(ref box, out result);
            return result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            var intersects = false;
            for (var i = 0; i < PlaneCount; ++i)
            {
                var planeIntersectionType = default(PlaneIntersectionType);
                box.Intersects(ref this.planes[i], out planeIntersectionType);
                switch (planeIntersectionType)
                {
                case PlaneIntersectionType.Front:
                    result = ContainmentType.Disjoint; 
                    return;
                case PlaneIntersectionType.Intersecting:
                    intersects = true;
                    break;
                }
            }
            result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        /*
        public ContainmentType Contains(BoundingFrustum frustum)
        {
            if (this == frustum)                // We check to see if the two frustums are equal
                return ContainmentType.Contains;// If they are, there's no need to go any further.

            throw new NotImplementedException();
        }
        */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ContainmentType Contains(BoundingSphere sphere)
        {
            var result = default(ContainmentType);
            this.Contains(ref sphere, out result);
            return result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            var intersects = false;
            for (var i = 0; i < PlaneCount; ++i) 
            {
                var planeIntersectionType = default(PlaneIntersectionType);

                sphere.Intersects(ref this.planes[i], out planeIntersectionType);
                switch (planeIntersectionType)
                {
                case PlaneIntersectionType.Front:
                    result = ContainmentType.Disjoint; 
                    return;
                case PlaneIntersectionType.Intersecting:
                    intersects = true;
                    break;
                }
            }
            result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ContainmentType Contains(Vector3 point)
        {
            var result = default(ContainmentType);
            this.Contains(ref point, out result);
            return result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            for (var i = 0; i < PlaneCount; ++i)
            {
                // TODO: we might want to inline this for performance reasons
                if (PlaneHelper.ClassifyPoint(ref point, ref this.planes[i]) > 0)
                {
                    result = ContainmentType.Disjoint;
                    return;
                }
            }
            result = ContainmentType.Contains;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundingFrustum other)
        {
            return (this == other);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            BoundingFrustum f = obj as BoundingFrustum;
            return (object.Equals(f, null)) ? false : (this == f);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3[] GetCorners()
        {
            return (Vector3[])this.corners.Clone();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetCorners(Vector3[] corners)
        {
			if (corners == null) throw new ArgumentNullException("corners");
		    if (corners.Length < CornerCount) throw new ArgumentOutOfRangeException("corners");

            this.corners.CopyTo(corners, 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return this.matrix.GetHashCode();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(BoundingBox box)
        {
			var result = false;
			this.Intersects(ref box, out result);
			return result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Intersects(ref BoundingBox box, out bool result)
        {
			var containment = default(ContainmentType);
			this.Contains(ref box, out containment);
			result = containment != ContainmentType.Disjoint;
		}

        /*
        public bool Intersects(BoundingFrustum frustum)
        {
            throw new NotImplementedException();
        }
        */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(BoundingSphere sphere)
        {
            var result = default(bool);
            this.Intersects(ref sphere, out result);
            return result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Intersects(ref BoundingSphere sphere, out bool result)
        {
            var containment = default(ContainmentType);
            this.Contains(ref sphere, out containment);
            result = containment != ContainmentType.Disjoint;
        }

        /*
        public PlaneIntersectionType Intersects(Plane plane)
        {
            throw new NotImplementedException();
        }

        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            throw new NotImplementedException();
        }

        public Nullable<float> Intersects(Ray ray)
        {
            throw new NotImplementedException();
        }

        public void Intersects(ref Ray ray, out Nullable<float> result)
        {
            throw new NotImplementedException();
        }
        */


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(256);
            sb.Append("{Near:");
            sb.Append(this.planes[0].ToString());
            sb.Append(" Far:");
            sb.Append(this.planes[1].ToString());
            sb.Append(" Left:");
            sb.Append(this.planes[2].ToString());
            sb.Append(" Right:");
            sb.Append(this.planes[3].ToString());
            sb.Append(" Top:");
            sb.Append(this.planes[4].ToString());
            sb.Append(" Bottom:");
            sb.Append(this.planes[5].ToString());
            sb.Append("}");
            return sb.ToString();
        }

        #endregion Public Methods


        #region Private Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreateCorners()
        {
            IntersectionPoint(ref this.planes[0], ref this.planes[2], ref this.planes[4], out this.corners[0]);
            IntersectionPoint(ref this.planes[0], ref this.planes[3], ref this.planes[4], out this.corners[1]);
            IntersectionPoint(ref this.planes[0], ref this.planes[3], ref this.planes[5], out this.corners[2]);
            IntersectionPoint(ref this.planes[0], ref this.planes[2], ref this.planes[5], out this.corners[3]);
            IntersectionPoint(ref this.planes[1], ref this.planes[2], ref this.planes[4], out this.corners[4]);
            IntersectionPoint(ref this.planes[1], ref this.planes[3], ref this.planes[4], out this.corners[5]);
            IntersectionPoint(ref this.planes[1], ref this.planes[3], ref this.planes[5], out this.corners[6]);
            IntersectionPoint(ref this.planes[1], ref this.planes[2], ref this.planes[5], out this.corners[7]);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreatePlanes()
        {            
            this.planes[0] = new Plane(-this.matrix.M13, -this.matrix.M23, -this.matrix.M33, -this.matrix.M43);
            this.planes[1] = new Plane(this.matrix.M13 - this.matrix.M14, this.matrix.M23 - this.matrix.M24, this.matrix.M33 - this.matrix.M34, this.matrix.M43 - this.matrix.M44);
            this.planes[2] = new Plane(-this.matrix.M14 - this.matrix.M11, -this.matrix.M24 - this.matrix.M21, -this.matrix.M34 - this.matrix.M31, -this.matrix.M44 - this.matrix.M41);
            this.planes[3] = new Plane(this.matrix.M11 - this.matrix.M14, this.matrix.M21 - this.matrix.M24, this.matrix.M31 - this.matrix.M34, this.matrix.M41 - this.matrix.M44);
            this.planes[4] = new Plane(this.matrix.M12 - this.matrix.M14, this.matrix.M22 - this.matrix.M24, this.matrix.M32 - this.matrix.M34, this.matrix.M42 - this.matrix.M44);
            this.planes[5] = new Plane(-this.matrix.M14 - this.matrix.M12, -this.matrix.M24 - this.matrix.M22, -this.matrix.M34 - this.matrix.M32, -this.matrix.M44 - this.matrix.M42);
            
            this.NormalizePlane(ref this.planes[0]);
            this.NormalizePlane(ref this.planes[1]);
            this.NormalizePlane(ref this.planes[2]);
            this.NormalizePlane(ref this.planes[3]);
            this.NormalizePlane(ref this.planes[4]);
            this.NormalizePlane(ref this.planes[5]);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void IntersectionPoint(ref Plane a, ref Plane b, ref Plane c, out Vector3 result)
        {
            // Formula used
            //                d1 ( N2 * N3 ) + d2 ( N3 * N1 ) + d3 ( N1 * N2 )
            //P =   -------------------------------------------------------------------------
            //                             N1 . ( N2 * N3 )
            //
            // Note: N refers to the normal, d refers to the displacement. '.' means dot product. '*' means cross product
            
            Vector3 v1, v2, v3;
            Vector3 cross;
            
            Vector3.Cross(ref b.Normal, ref c.Normal, out cross);
            
            float f;
            Vector3.Dot(ref a.Normal, ref cross, out f);
            f *= -1.0f;
            
            Vector3.Cross(ref b.Normal, ref c.Normal, out cross);
            Vector3.Multiply(ref cross, a.D, out v1);
            //v1 = (a.D * (Vector3.Cross(b.Normal, c.Normal)));
            
            
            Vector3.Cross(ref c.Normal, ref a.Normal, out cross);
            Vector3.Multiply(ref cross, b.D, out v2);
            //v2 = (b.D * (Vector3.Cross(c.Normal, a.Normal)));
            
            
            Vector3.Cross(ref a.Normal, ref b.Normal, out cross);
            Vector3.Multiply(ref cross, c.D, out v3);
            //v3 = (c.D * (Vector3.Cross(a.Normal, b.Normal)));
            
            result.X = (v1.X + v2.X + v3.X) / f;
            result.Y = (v1.Y + v2.Y + v3.Y) / f;
            result.Z = (v1.Z + v2.Z + v3.Z) / f;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void NormalizePlane(ref Plane p)
        {
            float factor = 1f / p.Normal.Length();
            p.Normal.X *= factor;
            p.Normal.Y *= factor;
            p.Normal.Z *= factor;
            p.D *= factor;
        }

        #endregion
    }
}

