using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace PIP {

    public class PIPTextureHolder : MonoBehaviour {

        public Texture tex = null;
        public Tuner tuner = new Tuner();
		public Events events = new Events();

        #region interface
        public Texture Value => tex;
        public int2 Size => tex != null ? new int2(tex.width, tex.height) : int2.zero;

		public void SetTexture(Texture tex) {
			this.tex = tex;
			events.OnSetTexture.Invoke(tex);
		}
        public PIPTextureHolder SetData(PIPTextureMaterial mat) {
            if (mat != null) {
                mat.SrcBlend = tuner.srcBlend;
                mat.DstBlend = tuner.dstBlend;

                mat.SetChannel(tuner.channel);
            }

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
		[System.Serializable]
		public class Events {
			public TextureEvent OnSetTexture = new TextureEvent();

			[System.Serializable]
			public class TextureEvent : UnityEvent<Texture> { }
		}
        #endregion
    }
}
