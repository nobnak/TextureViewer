using Gist2.Extensions.ScreenExt;
using Gist2.Extensions.SizeExt;
using System.Collections.Generic;
using UnityEngine;

namespace PIP {

    [ExecuteAlways]
    public class PIPTextureWindow : MonoBehaviour {

        public Tuner tuner = new Tuner();
        public List<PIPTextureHolder> textures = new List<PIPTextureHolder>();

        protected Rect windowSize = new Rect(10, 10, 10, 10);
        protected PIPTextureMaterial mat;

        #region unity
        private void OnEnable() {
            mat = new PIPTextureMaterial();
        }
        private void OnDisable() {
            mat?.Dispose();
            mat = null;
        }
        private void Update() {
            if (Input.GetKeyDown(tuner.keyToWindow)) {
                tuner.windowIsOpen = !tuner.windowIsOpen;
                windowSize = new Rect(10, 10, 10, 10);
            }
        }
        private void OnGUI() {
            windowSize.size = Vector2Int.zero;
            if (tuner.windowIsOpen) windowSize = GUILayout.Window(GetInstanceID(), windowSize, Window, name);
        }
        #endregion

        #region member
        protected void Window(int id) { 
            var screenSize = ScreenExtension.ScreenSize();
            var texShare = tuner.texShare;
            var texHeight = Mathf.RoundToInt(texShare * screenSize.y);
            var texGap = tuner.texGap;
            var texOffset = (float)texGap;

            using (new GUILayout.HorizontalScope()) {
                foreach (var tex in textures) {
                    if (tex.Value != null) {
                        using (new GUILayout.VerticalScope()) {
                            var texSize = tex.Value.Size();
                            var texAspect = texSize.Aspect();
                            var texWidth = texAspect * texHeight;

                            GUILayout.Label(tex.gameObject.name);
                            tex.DrawTexture(texWidth, texHeight, mat);
                        }
                        GUILayout.Space(texGap);
                    }
                }
            }
            GUI.DragWindow();
        }
        #endregion

        #region classes
        [System.Serializable]
        public class Tuner {
            public KeyCode keyToWindow = KeyCode.T;
            public bool windowIsOpen = true;

            [Range(0f, 1f)]
            public float texShare = 0.2f;
            public int texGap = 10;
        }
        #endregion
    }
}
