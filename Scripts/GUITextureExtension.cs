using Gist2.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PIP {

    public static class GUITextureExtension {

        public static Texture DrawTexture(this Texture tex, Rect rect, Material mat = null) {
            if (Event.current.type == EventType.Repaint) Graphics.DrawTexture(rect, tex, mat);
            return tex;
        }

        public static Rect DrawTexture(this Texture tex, float texWidth, float texHeight, Material mat = null) {
            var rect = GUILayoutUtility.GetRect(texWidth, texHeight);
            tex.DrawTexture(rect, mat);
            return rect;
        }
        public static Rect DrawTexture(this PIPTextureHolder tex, float texWidth, float texHeight, PIPTextureMaterial mat = null) {
            var rect = GUILayoutUtility.GetRect(texWidth, texHeight);
            if (mat != null) tex.SetData(mat);
            tex.Value.DrawTexture(rect, mat);
            return rect;
        }

        public static Rect DrawTexture(this IValue<Texture> tex, float texWidth, float texHeight, Material mat = null)
            => tex.Value.DrawTexture(texWidth, texHeight, mat);
    }
}
