﻿//
// RTextureSprite.cs
//
// Author:
//       Gabriel Reiser <gabriel@reisergames.com>
//
// Copyright (c) 2015 2014
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
using Reactor.Math;

namespace Reactor.Types
{
    public class RTextureSprite : RTexture2D
    {
        public Vector2 Offset = new Vector2();
        public Vector2 Origin = new Vector2();
        public Rectangle ScaledBounds = new Rectangle();
        public RColor Color { get; set; }
        public RTextureSprite() : base()
        {
        }

        public void Render(bool flipped = false)
        {
            Rectangle bounds = new Rectangle((int)(Origin.X + Offset.X), (int)(Origin.Y + Offset.Y), this.Bounds.Width, this.Bounds.Height);
            if (flipped)
                RScreen.Instance.RenderTexture(this, bounds, Color, Matrix.CreateRotationY(MathHelper.ToRadians(180f)), false);
            else
                RScreen.Instance.RenderTexture(this, bounds, Color, Matrix.Identity, false);
        }
    }
}

