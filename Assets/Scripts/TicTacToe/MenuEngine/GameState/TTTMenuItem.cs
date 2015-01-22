using UnityEngine;
using System.Collections;

public class TTTMenuItem : StaticObject, IUnityRenderable {

    public string prefab;
    public bool selected;
    public string message;
    public string target;
    public AudioClip audioMessage;

    public TTTMenuItem(string prefab, string message, string target, AudioClip audioMessage, Vector3 position, bool hidden, bool selected = false) : base(position, hidden) {
        this.prefab = prefab;
        this.message = message;
        this.target = target;
        this.audioMessage = audioMessage;
        this.selected = selected;
	}

    public string getPrefab() {
        return prefab;
    }

}