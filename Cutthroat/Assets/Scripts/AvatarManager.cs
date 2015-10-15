using UnityEngine;
using System.Collections;

public class AvatarManager : MonoBehaviour {

    public Avatar[] PuppyAvatars;

    public static AvatarManager Main;

	// Use this for initialization
	void Awake () {
        Main = this;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
