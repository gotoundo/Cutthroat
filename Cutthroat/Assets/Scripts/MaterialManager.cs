using UnityEngine;
using System.Collections;

public class MaterialManager : MonoBehaviour {

    public static MaterialManager Main;

    public Material[] PuppySkins;

	// Use this for initialization
	void Awake() {
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
