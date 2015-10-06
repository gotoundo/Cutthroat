using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InspectorUI : MonoBehaviour {

    public Text TargetName;
    public Text TargetDescription;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GameObject SelectedObject = GameManager.singleton.SelectedObject;

        if (SelectedObject != null)
        {
            TargetName.text = SelectedObject.name;

            CustomerScript customer = SelectedObject.GetComponent<CustomerScript>();
            if (customer != null)
            {
                TargetDescription.text = "";
                foreach (string line in customer.debugStringArray)
                    TargetDescription.text += line + "\n";
            }
        }

    }
}
