# TextureViewer-Unity

Drawing textures with debug-specified shader in IMGUI's GUI Layout manager on Unity.

## Installation
Released as [UPM package on OpenUPM](https://openupm.com/packages/jp.nobnak.texture_viewer/).
- Add URL "https://package.openupm.com" in a Scoped Registry
- Add scope "jp.nobnak"
- Add package "Texture Viewer" in Package Manager.

## Demo
[![Demo](http://img.youtube.com/vi/HpbL3llkxhU/mqdefault.jpg)](https://youtu.be/HpbL3llkxhU)

## Example
```csharp
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
