using UnityEngine;
using System.Collections;

public class FloatingIcon : MonoBehaviour {

    public Transform Target;
    public Vector3 Offset;
	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

        SnapToPosition();
    }

    public void SnapToPosition()
    {
        if (Target != null) {
            Vector3 wantedPos = Camera.main.WorldToScreenPoint(Target.position) + Offset;
            transform.position = wantedPos;
        }
    }
}
