using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class TextureEventSender : MonoBehaviour {

    public Texture tex = null;

    public TextureEvent OnUpdate = new TextureEvent();

    #region unity
    protected void OnEnable() {
        OnUpdate?.Invoke(tex);
    }
    protected void OnDisable() {
        OnUpdate?.Invoke(null);
    }
    #endregion

    #region definitions
    [System.Serializable]
    public class TextureEvent : UnityEvent<Texture> { }
    #endregion
}
