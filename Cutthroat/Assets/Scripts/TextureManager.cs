using UnityEngine;
using System.Collections;

public class TextureManager : MonoBehaviour {

    public static TextureManager singleton;

    public Sprite[] OverheadIcons;

	// Use this for initialization
	void Start () {
        singleton = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
