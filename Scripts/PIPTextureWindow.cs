using Gist2.Extensions.ScreenExt;
using Gist2.Extensions.SizeExt;
using Gist2.GUIExt;
using System.Collections.Generic;
using UnityEngine;

namespace PIP {

    [ExecuteAlways]
    public class PIPTextureWindow : MonoBehaviour {

        public Tuner tuner = new Tuner();
        public List<PIPTextureHolder> textures = new List<PIPTextureHolder>();

        protected Rect windowSize = new Rect(10, 10, 10, 10);
        protected PIPTextureMaterial mat;

        protected bool compactList;
        protected bool visibleOptions;
        protected int selectedTexIndex;
        protected bool fullscreen;

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
            var screenSize = ScreenExtension.ScreenSize();
            if (fullscreen && selectedTexIndex >= 0 && selectedTexIndex < textures.Count) {
                var tex = textures[selectedTexIndex];
                tex.DrawTexture(screenSize.x, screenSize.y, mat);
            }

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

            using (new GUILayout.VerticalScope()) {

                using (new GUILayout.HorizontalScope()) {
                    for (var i = 0; i < textures.Count; i++) {
                        var tex = textures[i];

                        if (compactList && i != selectedTexIndex) continue; 

                        if (tex != null && tex.isActiveAndEnabled && tex.Value != null) {
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

                using (new GUILayout.HorizontalScope()) {
                    if (GUILayout.Button("<")) selectedTexIndex--;
                    if (GUILayout.Button(">")) selectedTexIndex++;
                    GUILayout.Label($"{selectedTexIndex}");
                    GUILayout.FlexibleSpace();

                    var totalTex = textures.Count;
                    selectedTexIndex = (totalTex == 0) ? -1 : (selectedTexIndex + totalTex) % totalTex;
                }

                visibleOptions = visibleOptions.FoldOut("Options");
                if (visibleOptions) {
                    using (new GUILayout.VerticalScope()) {
                        compactList = GUILayout.Toggle(compactList, "Compact");
                        fullscreen = GUILayout.Toggle(fullscreen, "fullscreen");
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
