using UnityEngine;
using System.Collections;

public class WindmillRotate : MonoBehaviour {
    public GameObject MillWings;
    public bool RotationEnabled = false;
    public float rotationSpeed = 20f;
	// Use this for initialization
	void Start () {
        
	
	}
	
	// Update is called once per frame
	void Update () {
        if(GameManager.Main.gameRunning && RotationEnabled)
        {
            MillWings.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
	
	}
}
