﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactor;
namespace Reactor.Types.States
{
    public partial class RBlendState : IDisposable
    {
        private readonly RTargetBlendState[] _targetBlendState;

        private RColor _blendFactor;

        private int _multiSampleMask;

        private bool _independentBlendEnable;

        private string _name;

        /// <summary>
        /// Returns the target specific blend state.
        /// </summary>
        /// <param name="index">The 0 to 3 target blend state index.</param>
        /// <returns>A target blend state.</returns>
        public RTargetBlendState this[int index]
        {
            get { return _targetBlendState[index]; }
        }

        public RBlendFunc AlphaBlendFunction
        {
            get { return _targetBlendState[0].AlphaBlendFunction; }
            set
            {
                _targetBlendState[0].AlphaBlendFunction = value;
            }
        }

        public RBlend AlphaDestinationBlend
        {
            get { return _targetBlendState[0].AlphaDestinationBlend; }
            set
            {
                _targetBlendState[0].AlphaDestinationBlend = value;
            }
        }

        public RBlend AlphaSourceBlend
        {
            get { return _targetBlendState[0].AlphaSourceBlend; }
            set
            {
                _targetBlendState[0].AlphaSourceBlend = value;
            }
        }

        public RBlendFunc ColorBlendFunction
        {
            get { return _targetBlendState[0].ColorBlendFunction; }
            set
            {
                _targetBlendState[0].ColorBlendFunction = value;
            }
        }

        public RBlend ColorDestinationBlend
        {
            get { return _targetBlendState[0].ColorDestinationBlend; }
            set
            {
                _targetBlendState[0].ColorDestinationBlend = value;
            }
        }

        public RBlend ColorSourceBlend
        {
            get { return _targetBlendState[0].ColorSourceBlend; }
            set
            {
                _targetBlendState[0].ColorSourceBlend = value;
            }
        }

        public RColorWriteChannels ColorWriteChannels
        {
            get { return _targetBlendState[0].ColorWriteChannels; }
            set
            {
                _targetBlendState[0].ColorWriteChannels = value;
            }
        }

        public RColorWriteChannels ColorWriteChannels1
        {
            get { return _targetBlendState[1].ColorWriteChannels; }
            set
            {
                _targetBlendState[1].ColorWriteChannels = value;
            }
        }

        public RColorWriteChannels ColorWriteChannels2
        {
            get { return _targetBlendState[2].ColorWriteChannels; }
            set
            {
                _targetBlendState[2].ColorWriteChannels = value;
            }
        }

        public RColorWriteChannels ColorWriteChannels3
        {
            get { return _targetBlendState[3].ColorWriteChannels; }
            set
            {
                _targetBlendState[3].ColorWriteChannels = value;
            }
        }

        public RColor BlendFactor
        {
            get { return _blendFactor; }
            set
            {
                _blendFactor = value;
            }
        }

        public int MultiSampleMask
        {
            get { return _multiSampleMask; }
            set
            {
                _multiSampleMask = value;
            }
        }

        /// <summary>
        /// Enables use of the per-target blend states.
        /// </summary>
        public bool IndependentBlendEnable
        {
            get { return _independentBlendEnable; }
            set
            {
                _independentBlendEnable = value;
            }
        }

        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        private static readonly Utilities.ObjectFactory<RBlendState> _additive;
        private static readonly Utilities.ObjectFactory<RBlendState> _alphaBlend;
        private static readonly Utilities.ObjectFactory<RBlendState> _nonPremultiplied;
        private static readonly Utilities.ObjectFactory<RBlendState> _opaque;

        public static RBlendState Additive { get { return _additive.Value; } }
        public static RBlendState AlphaBlend { get { return _alphaBlend.Value; } }
        public static RBlendState NonPremultiplied { get { return _nonPremultiplied.Value; } }
        public static RBlendState Opaque { get { return _opaque.Value; } }

        public RBlendState()
        {
            _targetBlendState = new RTargetBlendState[4];
            _targetBlendState[0] = new RTargetBlendState();
            _targetBlendState[1] = new RTargetBlendState();
            _targetBlendState[2] = new RTargetBlendState();
            _targetBlendState[3] = new RTargetBlendState();

            _blendFactor = RColor.White;
            _multiSampleMask = Int32.MaxValue;
            _independentBlendEnable = false;
        }

        static RBlendState()
        {
            _additive = new Utilities.ObjectFactory<RBlendState>(() => new RBlendState
            {
                Name = "BlendState.Additive",
                ColorSourceBlend = RBlend.SourceAlpha,
                AlphaSourceBlend = RBlend.SourceAlpha,
                ColorDestinationBlend = RBlend.One,
                AlphaDestinationBlend = RBlend.One
            });

            _alphaBlend = new Utilities.ObjectFactory<RBlendState>(() => new RBlendState()
            {
                Name = "BlendState.AlphaBlend",
                ColorSourceBlend = RBlend.One,
                AlphaSourceBlend = RBlend.One,
                ColorDestinationBlend = RBlend.InverseSourceAlpha,
                AlphaDestinationBlend = RBlend.InverseSourceAlpha
            });

            _nonPremultiplied = new Utilities.ObjectFactory<RBlendState>(() => new RBlendState()
            {
                Name = "BlendState.NonPremultiplied",
                ColorSourceBlend = RBlend.SourceAlpha,
                AlphaSourceBlend = RBlend.SourceAlpha,
                ColorDestinationBlend = RBlend.InverseSourceAlpha,
                AlphaDestinationBlend = RBlend.InverseSourceAlpha
            });

            _opaque = new Utilities.ObjectFactory<RBlendState>(() => new RBlendState()
            {
                Name = "BlendState.Opaque",
                ColorSourceBlend = RBlend.One,
                AlphaSourceBlend = RBlend.One,
                ColorDestinationBlend = RBlend.Zero,
                AlphaDestinationBlend = RBlend.Zero
            });
        }

        internal static void ResetStates()
        {
            _additive.Reset();
            _alphaBlend.Reset();
            _nonPremultiplied.Reset();
            _opaque.Reset();
        }
        internal void PlatformApplyState()
        {
            var blendEnabled = !(this.ColorSourceBlend == RBlend.One && 
                                 this.ColorDestinationBlend == RBlend.Zero &&
                                 this.AlphaSourceBlend == RBlend.One &&
                                 this.AlphaDestinationBlend == RBlend.Zero);
            if (blendEnabled)
                GL.Enable(EnableCap.Blend);
            else
                GL.Disable(EnableCap.Blend);
            REngine.CheckGLError();

            GL.BlendColor(
                this.BlendFactor.R / 255.0f,      
                this.BlendFactor.G / 255.0f, 
                this.BlendFactor.B / 255.0f, 
                this.BlendFactor.A / 255.0f);
            REngine.CheckGLError();

            GL.BlendEquationSeparate(
                this.ColorBlendFunction.GetBlendEquationMode(),
                this.AlphaBlendFunction.GetBlendEquationMode());
            REngine.CheckGLError();

            GL.BlendFuncSeparate(
                this.ColorSourceBlend.GetBlendFactorSrc(), 
                this.ColorDestinationBlend.GetBlendFactorDest(), 
                this.AlphaSourceBlend.GetBlendFactorSrc(), 
                this.AlphaDestinationBlend.GetBlendFactorDest());
            REngine.CheckGLError();

            GL.ColorMask(
                (this.ColorWriteChannels & RColorWriteChannels.Red) != 0,
                (this.ColorWriteChannels & RColorWriteChannels.Green) != 0,
                (this.ColorWriteChannels & RColorWriteChannels.Blue) != 0,
                (this.ColorWriteChannels & RColorWriteChannels.Alpha) != 0);
            REngine.CheckGLError();
        }

        public void Dispose()
        {
        }
    }
}
