using Gist2.Extensions.ComponentExt;
using Gist2.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace PIP {

    public class PIPTextureMaterial : System.IDisposable, IValue<Material> {

        public enum ChannelMixer { None = 0, R, G, B, A }

        public const string PATH = "TextureViewer-Unity/PIPTexture";

        public static readonly int P_SrcBlend = Shader.PropertyToID("_SrcBlend");
        public static readonly int P_DstBlend = Shader.PropertyToID("_DstBlend");
        public static readonly int P_ChannelMixer = Shader.PropertyToID("_ChannelMixer");
        public static readonly int P_Opacity = Shader.PropertyToID("_Opacity");

        #region properties
        public Matrix4x4 ChannelMixerMatrix { get; set; }
        public BlendMode SrcBlend { get; set; }
        public BlendMode DstBlend { get; set; }
        public float Opacity { get; set; }
        #endregion

        protected Material mat;

        public PIPTextureMaterial() {
            mat = new Material(Resources.Load<Shader>(PATH));
            SetChannel(default);
        }

        #region interface
        #region IValue
        public Material Value {
            get {
                mat.shaderKeywords = null;
                mat.SetInt(P_SrcBlend, (int)SrcBlend);
                mat.SetInt(P_DstBlend, (int)DstBlend);
                mat.SetMatrix(P_ChannelMixer, ChannelMixerMatrix);
                mat.SetFloat(P_Opacity, Opacity);
                return mat;
            }
        }
        #endregion

        #region IDisposable
        public void Dispose() {
            mat.Destroy();
        }
        #endregion

        public PIPTextureMaterial SetChannel(ChannelMixer mix) {
            switch (mix) {
                case ChannelMixer.R:
                    ChannelMixerMatrix = new Matrix4x4() { m00 = 1, m33 = 1 };
                    break;
                case ChannelMixer.G:
                    ChannelMixerMatrix = new Matrix4x4() { m11 = 1, m33 = 1 };
                    break;
                case ChannelMixer.B:
                    ChannelMixerMatrix = new Matrix4x4() { m22 = 1, m33 = 1 };
                    break;
                case ChannelMixer.A:
                    ChannelMixerMatrix = new Matrix4x4() { m03 = 1, m13 = 1, m23 = 1, m33 = 1 };
                    break;
                default:
                    ChannelMixerMatrix = Matrix4x4.identity;
                    break;
            }
            return this;
        }

        #region static
        public static implicit operator Material (PIPTextureMaterial piptm)
            => piptm != null ? piptm.Value : null;
        #endregion
        #endregion
    }
}
