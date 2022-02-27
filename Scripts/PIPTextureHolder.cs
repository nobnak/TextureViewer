using Gist2.Interfaces;
using UnityEngine;
using UnityEngine.Rendering;

namespace PIP {

    public class PIPTextureHolder : MonoBehaviour, IValue<Texture> {

        public Texture tex = null;
        public Tuner tuner = new Tuner();


        #region interface
        public Texture Value => tex;
        public void SetTexture(Texture tex) => this.tex = tex;
        public PIPTextureHolder SetData(PIPTextureMaterial mat) {
            mat.SrcBlend = tuner.srcBlend;
            mat.DstBlend = tuner.dstBlend;

            mat.SetChannel(tuner.channel);

            return this;
        }
        #endregion

        #region static
        public static implicit operator Texture (PIPTextureHolder pip) => (pip != null) ? pip.tex : null;
        #endregion

        #region classes
        [System.Serializable]
        public class Tuner {
            public PIPTextureMaterial.ChannelMixer channel = default;
            public BlendMode srcBlend = BlendMode.One;
            public BlendMode dstBlend = BlendMode.Zero;
        }
        #endregion
    }
}
