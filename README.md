# TextureViewer-Unity

Drawing textures with debug-specified shader in IMGUI's GUI Layout manager on Unity.

## Demo
[![Demo](http://img.youtube.com/vi/G90Q0EHsT5g/mqdefault.jpg)](https://youtu.be/G90Q0EHsT5g)

## Example
```csharp
using Gist2.Extensions.ScreenExt;
using Gist2.Extensions.SizeExt;
using UnityEngine;

public PIPTextureHolder tex;

protected Rect windowSize = new Rect(10, 10, 10, 10);
protected PIPTextureMaterial mat;

private void OnEnable() {
    mat = new PIPTextureMaterial();
}
private void OnDisable() {
    mat?.Dispose();
    mat = null;
}
private void OnGUI() {
    windowSize = GUILayout.Window(GetInstanceID(), windowSize, Window, name);
}

protected void Window(int id) { 
    using (new GUILayout.VerticalScope()) {
        GUILayout.Label(tex.name);
        tex.DrawTexture(texWidth, texHeight, mat);
    }
    GUI.DragWindow();
}

```
