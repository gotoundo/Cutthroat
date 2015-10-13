using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InspectorUI : MonoBehaviour {

    public Text TargetName;
    public Text TargetDescription;
    Inspectable InspectedObject;


	// Use this for initialization
	void Start () {
        InspectedObject = GameManager.singleton.SelectedObject.GetComponent<Inspectable>();
        InspectedObject.newData = true;
    }
	
	// Update is called once per frame
	void Update () {

        Inspectable NewSelectedObject = GameManager.singleton.SelectedObject.GetComponent<Inspectable>();
        if (InspectedObject != NewSelectedObject)
        {
            InspectedObject = NewSelectedObject;
            ReloadText();
        }
        else if(InspectedObject.newData)
            ReloadText();
    }

    void ReloadText()
    {
        TargetName.text = InspectedObject.Name;

        TargetDescription.text = "";
        foreach (string line in InspectedObject.Updates)
            TargetDescription.text += line + "\n";

        InspectedObject.newData = false;
    }
}
