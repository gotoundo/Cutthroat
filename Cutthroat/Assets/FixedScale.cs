using UnityEngine;
using System.Collections;

public class FixedScale : MonoBehaviour {

    public Vector3 MyScale = new Vector3(1, 1, 1);
    // Use this for initialization
    void Start () {
        transform.localScale = MyScale;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
