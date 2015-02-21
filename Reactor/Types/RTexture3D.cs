﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reactor.Types
{
    public class RTexture3D : RTexture
    {
        public void Create(RPixelFormat format, ref RTexture2D posX, ref RTexture2D posY, ref RTexture2D posZ, ref RTexture2D negX, ref RTexture2D negY, ref RTexture2D negZ)
        {
            PixelInternalFormat inf = PixelInternalFormat.Rgba;
            PixelFormat pf = PixelFormat.Rgba;
            if(format == RPixelFormat.Bgr)
            {
                inf = PixelInternalFormat.Rgb;
                pf = PixelFormat.Bgr;
            }
            if (format == RPixelFormat.Rgb)
            {
                inf = PixelInternalFormat.Rgb;
                pf = PixelFormat.Rgb;
            }
            if (format == RPixelFormat.Bgra)
            {
                inf = PixelInternalFormat.Rgba;
                pf = PixelFormat.Bgra;
            }
            if (format == RPixelFormat.Rgba)
            {
                inf = PixelInternalFormat.Rgba;
                pf = PixelFormat.Rgba;
            }
            GL.ActiveTexture(TextureUnit.Texture0);
            var posXcolors = posX.GetData<RColor>();
            var posYcolors = posY.GetData<RColor>();
            var posZcolors = posZ.GetData<RColor>();
            var negXcolors = negX.GetData<RColor>();
            var negYcolors = negY.GetData<RColor>();
            var negZcolors = negZ.GetData<RColor>();

            Bounds = posX.Bounds;
            GL.GenTextures(1, out Id);
            textureTarget = OpenTK.Graphics.OpenGL.TextureTarget.TextureCubeMap;
            GL.BindTexture(textureTarget, Id);
            SetTextureMagFilter(RTextureMagFilter.Nearest);
            SetTextureMinFilter(RTextureMinFilter.Nearest);
            SetTextureWrapMode(RTextureWrapMode.ClampToBorder, RTextureWrapMode.ClampToBorder);
            GL.TexImage2D<RColor>(TextureTarget.TextureCubeMapPositiveX, 0, inf, Bounds.Width, Bounds.Height, 0, pf, PixelType.UnsignedByte, posXcolors);
            GL.TexImage2D<RColor>(TextureTarget.TextureCubeMapPositiveY, 0, inf, Bounds.Width, Bounds.Height, 0, pf, PixelType.UnsignedByte, posYcolors);
            GL.TexImage2D<RColor>(TextureTarget.TextureCubeMapPositiveZ, 0, inf, Bounds.Width, Bounds.Height, 0, pf, PixelType.UnsignedByte, posZcolors);
            GL.TexImage2D<RColor>(TextureTarget.TextureCubeMapNegativeX, 0, inf, Bounds.Width, Bounds.Height, 0, pf, PixelType.UnsignedByte, negXcolors);
            GL.TexImage2D<RColor>(TextureTarget.TextureCubeMapNegativeY, 0, inf, Bounds.Width, Bounds.Height, 0, pf, PixelType.UnsignedByte, negYcolors);
            GL.TexImage2D<RColor>(TextureTarget.TextureCubeMapNegativeZ, 0, inf, Bounds.Width, Bounds.Height, 0, pf, PixelType.UnsignedByte, negZcolors);
        }
    }
}
