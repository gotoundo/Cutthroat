using UnityEngine;
using System.Collections;

public class AssetManager : MonoBehaviour {

    public static AssetManager singleton;

    public Sprite[] OverheadIcons;

	// Use this for initialization
	void Start () {
        singleton = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
