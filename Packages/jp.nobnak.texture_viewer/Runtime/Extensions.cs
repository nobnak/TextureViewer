using UnityEngine;

namespace PIP {
    public static class GUITextureExtension {
        public static Texture DrawTexture(this Texture tex, Rect rect, Material mat = null) {
            if (Event.current.type == EventType.Repaint && tex != null) 
				Graphics.DrawTexture(rect, tex, mat);
            return tex;
        }

        public static Rect DrawTexture(this Texture tex, float texWidth, float texHeight, Material mat = null) {
            var rect = GUILayoutUtility.GetRect(texWidth, texHeight);
            tex.DrawTexture(rect, mat);
            return rect;
        }
        public static Rect DrawTexture(this PIPTextureHolder tex, float texWidth, float texHeight, PIPTextureMaterial mat = null) {
            var rect = GUILayoutUtility.GetRect(texWidth, texHeight);
            tex.Value.DrawTexture(rect, mat);
            return rect;
        }
    }

    public static class ObjectExtension {
        public static Object Dispose(this Object obj) {
            if (obj != null) {
                if (Application.isPlaying)
                    Object.Destroy(obj);
                else
                    Object.DestroyImmediate(obj);
            }
            return null;
        }
    }
}
