using UnityEngine;
using System.Collections;

public class Inspectable : MonoBehaviour {

    public bool deselectionObject = false;

    Color startingColor;
    public Color selectionColor = Color.red;
    
    // Use this for initialization
    void Start () {
        startingColor = GetComponent<Renderer>().material.color;
    }

    public void Select()
    {
        GetComponent<Renderer>().material.color = selectionColor;
    }

    public void Deselect()
    {
        GetComponent<Renderer>().material.color = startingColor;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

   // private Vector3 screenPoint;
   // private Vector3 offset;
    void OnMouseDown()
    {
        /*screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        destroyBalls = true;*/
        if (deselectionObject)
            GameManager.singleton.CloseInspector();
        else
            GameManager.singleton.MakeSelection(gameObject);
    }
}
