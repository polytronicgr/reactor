﻿using Reactor;
using Reactor.Math;
using Reactor.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBed
{
    class Game : RGame
    {
        RMesh sponza;
        public override void Dispose()
        {
            
        }

        public override void Init()
        {
            Engine.InitGameWindow(1280, 720, RWindowStyle.Normal);
            GameWindow.VSync = OpenTK.VSyncMode.On;
            Engine.SetShowFPS(true);
            Engine.SetViewport(new RViewport(0,0,800, 600));
            GameWindow.CursorVisible = false;
            sponza = Engine.Scene.Create<RMesh>("sponza");
            sponza.LoadSourceModel("/models/sponza.fbx");
            sponza.CullEnable = true;
            sponza.CullMode = Reactor.Types.States.RCullMode.CullCounterClockwiseFace;
            sponza.SetScale(1f);
            sponza.SetPosition(0, 0, 0);
            var cam = Engine.GetCamera();
            cam.SetPosition(0, 20, -1f);
            cam.LookAt(Vector3.Zero);
            cam.SetClipPlanes(0.0001f, 1000000f);


            RTexture2D posX = (RTexture2D)Engine.Textures.CreateTexture<RTexture2D>("posX", "/textures/sky-posX.png");
            RTexture2D posY = (RTexture2D)Engine.Textures.CreateTexture<RTexture2D>("posY", "/textures/sky-posY.png");
            RTexture2D posZ = (RTexture2D)Engine.Textures.CreateTexture<RTexture2D>("posZ", "/textures/sky-posZ.png");
            RTexture2D negX = (RTexture2D)Engine.Textures.CreateTexture<RTexture2D>("negX", "/textures/sky-negX.png");
            RTexture2D negY = (RTexture2D)Engine.Textures.CreateTexture<RTexture2D>("negY", "/textures/sky-negY.png");
            RTexture2D negZ = (RTexture2D)Engine.Textures.CreateTexture<RTexture2D>("negZ", "/textures/sky-negZ.png");

            var skyboxTexture = new RTexture3D();
            skyboxTexture.Create(RPixelFormat.Rgb, ref posX, ref posY, ref posZ, ref negX, ref negY, ref negZ);

            Engine.Atmosphere.CreateSkyBox(skyboxTexture);
            
            
        }

        public override void Render()
        {
            Engine.Clear(RColor.Gray);
            Engine.Atmosphere.RenderSkybox();
            sponza.Render();
            Engine.Present();
        }

        public override void Resized(int Width, int Height)
        {
            Engine.SetViewport(new RViewport(0,0,Width, Height));
        }
        Vector2 panning = Vector2.Zero;
        Vector3 direction = Vector3.Zero;
        public override void Update()
        {
            int X,Y,Wheel = 0;
            Engine.Input.GetMouse(out X, out Y, out Wheel);
            Vector2 center_of_window = new Vector2(GameWindow.Width / 2, GameWindow.Height / 2);
            Vector2 mouse_direction = new Vector2(X, Y);
            mouse_direction = (mouse_direction-center_of_window);
            mouse_direction.X /= GameWindow.Width;
            mouse_direction.Y /= GameWindow.Height;
            mouse_direction *= 80f;
            var cam = Engine.GetCamera();
            if (Engine.Input.IsKeyDown(RKey.W))
                direction -= cam.Matrix.Forward * 0.1f;
            if (Engine.Input.IsKeyDown(RKey.S))
                direction -= cam.Matrix.Backward * 0.1f;
            if (Engine.Input.IsKeyDown(RKey.A))
                direction -= cam.Matrix.Left * 0.1f;
            if (Engine.Input.IsKeyDown(RKey.D))
                direction -= cam.Matrix.Right * 0.1f;
            panning.X += -mouse_direction.X * 2f;
            panning.Y += mouse_direction.Y * 2f;
            sponza.Update();
            direction *= 0.90f;
            cam.RotateX(mouse_direction.Y * 2f);
            if (cam.Rotation.X > Math.PI/2)
                cam.Rotation = new Quaternion((float)Math.PI/2, cam.Rotation.Y, cam.Rotation.Z, cam.Rotation.W);
            if (cam.Rotation.X < -Math.PI/2)
                cam.Rotation = new Quaternion(-(float)Math.PI/2, cam.Rotation.Y, cam.Rotation.Z, cam.Rotation.W);
                
            cam.RotateY(-mouse_direction.X * 2f);
            cam.Move(direction);
            cam.Update();
            if(GameWindow.Focused)
                Engine.Input.CenterMouse();
            if (Engine.Input.IsKeyDown(RKey.Escape))
                GameWindow.Exit();
        }
    }
}
