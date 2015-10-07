using UnityEngine;
using System.Collections;

public class RandomPuppySkin : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponentInChildren<SkinnedMeshRenderer>().material = MaterialManager.Main.PuppySkins[Random.Range(0, MaterialManager.Main.PuppySkins.Length)];
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
