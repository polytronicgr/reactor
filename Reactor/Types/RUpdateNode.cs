﻿// Author:
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
using Reactor.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Reactor.Types
{
    public class RUpdateNode : RSceneNode
    {
        public bool IsEnabled { get; set; }

        public event EventHandler OnUpdate;
        public virtual void Update()
        {
            UpdateMatrix();
            if (IsEnabled)
            {
                if (OnUpdate != null)
                    OnUpdate(this, null);
            }
        }
        internal Vector3 position = Vector3.Zero;
        internal Matrix matrix = Matrix.Identity;
        internal Quaternion rotation = Quaternion.CreateFromRotationMatrix(Matrix.Identity);
        internal Vector3 scale = Vector3.One;

        public Vector3 Position { get { return position; } set { position = value; } }
        public Matrix Matrix { get { return matrix; } set { matrix = value;} }
        public Quaternion Rotation { get { return rotation; } set { rotation = value; } }
        public Vector3 Scale { get { return scale; } set { scale = value; } }

        void UpdateMatrix()
        {
            //rotation = Quaternion.FromAxisAngle(Vector3.UnitX, rotation.X) * Quaternion.FromAxisAngle(Vector3.UnitY, rotation.Y) * Quaternion.FromAxisAngle(Vector3.UnitZ, rotation.Z);
            //rotation.Normalize();
            matrix = Matrix.Identity;
            BuildScalingMatrix(ref matrix);
            BuildRotationMatrix(ref matrix);
            BuildPositionMatrix(ref matrix);

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix GetRotationMatrix()
        {
            return rotation.ToMatrix();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quaternion GetRotation()
        {
            return rotation;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMatrix(ref Matrix Matrix)
        {
            matrix = Matrix;
            matrix.Decompose(out scale, out rotation, out position);
            //UpdateMatrix();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRotation(ref Quaternion rotation)
        {
            this.rotation = rotation;
            UpdateMatrix();
            
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateX(float value)
        {
            Rotate(value, 0, 0);
            UpdateMatrix();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateY(float value)
        {
            Rotate(0, value, 0);
            UpdateMatrix();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateZ(float value)
        {
            Rotate(0, 0, value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(Quaternion value)
        {
            rotation = value;
            UpdateMatrix();
        }
        public void Rotate(float x, float y, float z)
        {
            
            float dx = MathHelper.ToRadians(x);
            float dy = MathHelper.ToRadians(y);
            float dz = MathHelper.ToRadians(z);
            //rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateFromYawPitchRoll(y,x,z));
            rotation += Quaternion.CreateFromYawPitchRoll(dy, dx, dz);
            //UpdateMatrix();
        }
        public void SetPosition(Vector3 value)
        {
            position = value;
            //UpdateMatrix();
        }

        public void SetPosition(float X, float Y, float Z)
        {
            position.X = X;
            position.Y = Y;
            position.Z = Z;
            //UpdateMatrix();
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public void SetScale(Vector3 value)
        {
            scale = value;
        }

        public void SetScale(float value)
        {
            scale = new Vector3(value, value, value);
        }

        public void SetScale(float x, float y, float z)
        {
            scale.X = x;
            scale.Y = y;
            scale.Z = z;
        }

        public Vector3 GetScale()
        {
            return scale;
        }
        public void Move(float x, float y, float z)
        {
            position.X += x;
            position.Y += y;
            position.Z += z;
            //UpdateMatrix();
        }

        public void Move(Vector3 dir)
        {
            position += dir;
            //UpdateMatrix();
        }
        public void LookAt(Vector3 target)
        {
            matrix = Matrix.Identity * Matrix.CreateScale(scale) * Matrix.CreateLookAt(position, target, Vector3.UnitY);
            //UpdateMatrix();

        }
        internal void BuildRotationMatrix(ref Matrix m)
        {
            //rotation.Normalize();
            m *= Matrix.CreateFromAxisAngle(Vector3.Right, Rotation.X);
            m *= Matrix.CreateFromAxisAngle(Vector3.Up, Rotation.Y);
            m *= Matrix.CreateFromAxisAngle(Vector3.Forward, Rotation.Z);
            //m *= Matrix.CreateRotationX(rotation.X);
            //m *= Matrix.CreateRotationY(rotation.Y);
            //m *= Matrix.CreateRotationZ(rotation.Z);
            //return m;
        }
        internal void BuildScalingMatrix(ref Matrix m)
        {
            m *= Matrix.CreateScale(Scale);
            //return m;
        }
        internal void BuildPositionMatrix(ref Matrix m)
        {
            m *= Matrix.CreateTranslation(Position);
            //_transforms[_model.Meshes[0].ParentBone.Index] = m;
            //return m;
        }
    }
}
