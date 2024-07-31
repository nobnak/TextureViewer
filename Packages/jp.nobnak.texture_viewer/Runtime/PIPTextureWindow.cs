using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace PIP {

    [ExecuteAlways]
    public class PIPTextureWindow : MonoBehaviour {

        public Tuner tuner = new Tuner();
        public List<PIPTextureHolder> textures = new List<PIPTextureHolder>();

        protected Rect windowSize = new Rect(10, 10, 10, 10);
        protected PIPTextureMaterial mat;

        protected bool visibleOptions;
        protected int selectedTexIndex;
        protected GUIStyle foldoutOptions;

        #region unity
        private void OnEnable() {
            mat = new PIPTextureMaterial();
        }
        private void OnDisable() {
            mat?.Dispose();
            mat = null;
        }
        private void Update() {
            if (Input.GetKeyDown(tuner.keyToWindow)
				&& (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) {
                tuner.windowIsOpen = !tuner.windowIsOpen;
                windowSize = new Rect(10, 10, 10, 10);
            }
        }
        private void OnGUI() {
            if (!tuner.windowIsOpen) return;

            if (tuner.fullscreen && tuner.fullscreenTarget == FullscreenTarget.GUI) {
                if (selectedTexIndex >= 0 && selectedTexIndex < textures.Count) {
                    var screenSize = ScreenSize();
                    var tex = textures[selectedTexIndex];
                    tex.SetData(mat);
                    GlobalSet(mat);
                    tex.DrawTexture(screenSize.x, screenSize.y, mat);
                }
            }

            if (foldoutOptions == null)
                foldoutOptions = GenerateFoldout();

            windowSize.size = Vector2Int.zero;
            windowSize = GUILayout.Window(GetInstanceID(), windowSize, Window, name);
        }
        private void OnRenderObject() {
            if (!tuner.windowIsOpen) return;

            var c = Camera.current;
            if (c != Camera.main || (c.cullingMask & (1 << gameObject.layer)) == 0) return;

            if (tuner.fullscreen && tuner.fullscreenTarget == FullscreenTarget.MainCamera) {
                if (selectedTexIndex >= 0 && selectedTexIndex < textures.Count) {
                    var tex = textures[selectedTexIndex];

                    try {
                        GL.PushMatrix();
                        GL.LoadIdentity();
                        GL.LoadOrtho();
                        tex.SetData(mat);
                        GlobalSet(mat);
                        Graphics.DrawTexture(new Rect(0f, 1f, 1f, -1f), tex, mat);
                    } finally {
                        GL.PopMatrix();
                    }
                }
            }
        }
        #endregion

        #region member
        protected void Window(int id) { 
            var screenSize = ScreenSize();
            var texShare = tuner.texShare;
            var texHeight = Mathf.RoundToInt(texShare * screenSize.y);
            var texGap = tuner.texGap;
            var texOffset = (float)texGap;

            using (new GUILayout.VerticalScope()) {

                using (new GUILayout.HorizontalScope()) {
                    for (var i = 0; i < textures.Count; i++) {
                        var tex = textures[i];

                        if (tuner.compactList && i != selectedTexIndex) continue; 

                        if (tex != null && tex.isActiveAndEnabled && tex.Value != null) {
                            using (new GUILayout.VerticalScope()) {
                                var texSize = tex.Size;
                                var texAspect = (float)texSize.x / texSize.y;
                                var texWidth = texAspect * texHeight;

                                GUILayout.Label(tex.gameObject.name);
                                tex.SetData(mat);
                                GlobalSet(mat);
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

                var visibleTitle = $"{(visibleOptions ? "-" : "+")} Options";
                visibleOptions = GUILayout.Toggle(visibleOptions, visibleTitle, foldoutOptions);
                if (visibleOptions) {
                    using (new GUILayout.HorizontalScope()) {
                        var m = tuner.mixer;
                        if (GUILayout.Toggle(m == default, "RGB"))
                            m = default;
                        if (GUILayout.Toggle(m == PIPTextureMaterial.ChannelMixer.R, "R"))
                            m = PIPTextureMaterial.ChannelMixer.R;
                        if (GUILayout.Toggle(m == PIPTextureMaterial.ChannelMixer.G, "G"))
                            m = PIPTextureMaterial.ChannelMixer.G;
                        if (GUILayout.Toggle(m == PIPTextureMaterial.ChannelMixer.B, "B"))
                            m = PIPTextureMaterial.ChannelMixer.B;
                        if (GUILayout.Toggle(m == PIPTextureMaterial.ChannelMixer.A, "A"))
                            m = PIPTextureMaterial.ChannelMixer.A;
                        tuner.mixer = m;
                        GUILayout.FlexibleSpace();
                    }
                    using (new GUILayout.HorizontalScope()) {
                        tuner.forceTransparent = GUILayout.Toggle(tuner.forceTransparent, "Transparent");
                        tuner.opacity = GUILayout.HorizontalSlider(tuner.opacity, 0f, 1f, GUILayout.Width(100f));
                        GUILayout.FlexibleSpace();
                    }
                    using (new GUILayout.HorizontalScope()) {
                        tuner.compactList = GUILayout.Toggle(tuner.compactList, "Compact");
                        tuner.fullscreen = GUILayout.Toggle(tuner.fullscreen, "fullscreen");
                        GUILayout.FlexibleSpace();
                    }
                }
            }
            GUI.DragWindow();
        }

        protected void GlobalSet(PIPTextureMaterial mat) {
            var opacityOp = default(PIPTextureMaterial.OpacityOp);
            var opacity = 1f;

            if (tuner.mixer != default) {
                opacityOp = PIPTextureMaterial.OpacityOp.Override;
                mat.SetChannel(tuner.mixer);
            }

            if (tuner.forceTransparent && tuner.fullscreen) {
                opacity = tuner.opacity;
                mat.SrcBlend = BlendMode.SrcAlpha;
                mat.DstBlend = BlendMode.OneMinusSrcAlpha;
            }

            mat.K_OpacityOp = opacityOp;
            mat.Opacity = opacity;
        }
        protected int2 ScreenSize() => new int2(Screen.width, Screen.height);
        protected static GUIStyle GenerateFoldout() {
            var styleFoldout = new GUIStyle(GUI.skin.label) {
                alignment = TextAnchor.MiddleLeft
            };
            var coff = styleFoldout.normal.textColor;
            var con = Color.white;

            styleFoldout.onNormal.textColor = con;
            styleFoldout.onHover.textColor = con;
            styleFoldout.active.textColor = con;

            styleFoldout.normal.textColor = coff;
            styleFoldout.hover.textColor = coff;
            styleFoldout.onActive.textColor = coff;

            return styleFoldout;
        }
        #endregion

        #region classes
        public enum FullscreenTarget { GUI = 0, MainCamera }
        [System.Serializable]
        public class Tuner {
            public KeyCode keyToWindow = KeyCode.T;
            public bool windowIsOpen = true;

            [Range(0f, 1f)]
            public float texShare = 0.2f;
            public int texGap = 10;

            public bool compactList = true;
            public bool fullscreen = false;
            public FullscreenTarget fullscreenTarget = default;
            public PIPTextureMaterial.ChannelMixer mixer = default;

            public bool forceTransparent = false;
            [Range(0f, 1f)]
            public float opacity = 1f;
        }
        #endregion
    }
}
