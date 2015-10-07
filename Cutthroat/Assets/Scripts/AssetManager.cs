using UnityEngine;
using System.Collections;

public class AssetManager : MonoBehaviour {
    public static AssetManager Main;

	// Use this for initialization
	void Start () {
        if (Main == null)
        {
            Main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
