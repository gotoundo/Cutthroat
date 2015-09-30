using UnityEngine;
using System.Collections;

public class TreeRandomize : MonoBehaviour {
    public bool randomSize = true;
    public bool randomRotation = true;
    public float maxScale = 3f;
    public float minScale = .75f;

	// Use this for initialization
	void Start () {
        if (randomSize)
        {
            float newScale = Random.Range(minScale, maxScale);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
        if (randomRotation)
            transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
