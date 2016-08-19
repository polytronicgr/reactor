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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactor.Math;
namespace Reactor
{
    public class RCamera2d : RCamera
    {
        public RCamera2d():base()
        {

            this.OnUpdate += RCamera2d_OnUpdate;
        }

        void RCamera2d_OnUpdate(object sender, EventArgs e)
        {
            RViewport viewport = REngine.Instance._viewport;
            float x_max = viewport.Width - 1.0f;
            float y_max = viewport.Height - 1.0f;
            
            /*this.Projection = new Matrix(
                2 / y_max, 0, 0, -1,
                0, 2 / x_max, 0, -1,
                0, 0, -1, 0,
                0, 0, 0, 1);
             */
            this.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height,0, -1, 1);
            this.View = Matrix.Identity;
        }
    }
}
