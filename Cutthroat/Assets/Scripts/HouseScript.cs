using UnityEngine;
using System.Collections;

public class HouseScript : MonoBehaviour {

    public GameObject customer;
    public int customersToAHouse = 4;

    public static int count;
    public  int myCount;

	// Use this for initialization
	void Start () {
        
        for (int i = 0; i < customersToAHouse; i++)
        {
            GameObject o = (GameObject)Instantiate(customer, new Vector3(transform.position.x,-4,transform.position.z), transform.rotation);
            
            CustomerScript cust = o.GetComponent<CustomerScript>();
            cust.home = gameObject;
        }
        myCount = count;
        count++;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
