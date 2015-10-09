using UnityEngine;
using System.Collections;

public class RandomPuppySkin : MonoBehaviour {

	// Use this for initialization
	void Start () {

        int puppyID = Random.Range(0, MaterialManager.Main.PuppySkins.Length);

        GetComponentInChildren<SkinnedMeshRenderer>().material = MaterialManager.Main.PuppySkins[puppyID];
        GetComponent<Animator>().avatar = AvatarManager.Main.PuppyAvatars[puppyID];
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
