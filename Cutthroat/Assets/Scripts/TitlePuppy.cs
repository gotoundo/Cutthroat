using UnityEngine;
using System.Collections;

public class TitlePuppy : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        GetComponent<Animator>().SetBool("DemoMode", true);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
