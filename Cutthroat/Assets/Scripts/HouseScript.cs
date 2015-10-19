using UnityEngine;
using System.Collections;

public class HouseScript : MonoBehaviour {

    public GameObject customer;
    int customersToAHouse = 6;

    public static int count;
    public  int myCount;

	// Use this for initialization
	void Start () {
        GameManager.AllHouses.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnCustomers()
    {
        for (int i = 0; i < customersToAHouse; i++)
        {
            GameObject o = (GameObject)Instantiate(customer, new Vector3(transform.position.x, -4, transform.position.z), transform.rotation);

            CustomerScript cust = o.GetComponent<CustomerScript>();
            cust.SetHome(gameObject);
        }
        myCount = count;
        count++;
    }
}
