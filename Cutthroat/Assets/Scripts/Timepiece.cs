using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timepiece : MonoBehaviour {

    Light Daylight;
    public float timeInDay;
    float currentTime = 0;
    Slider mySlider;
    public Text dayCountText;
    float daysubsection = .75f;

    public static int CurrentDay;

	// Use this for initialization

        void Awake()
    {
        mySlider = GetComponent<Slider>();
        Daylight = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
        Daylight.transform.Rotate(new Vector3(-360 * daysubsection / 2, 0));
        CurrentDay = 1;

    }

	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;
        if (currentTime >= timeInDay)
        {
            currentTime = 0;
            Daylight.transform.Rotate(new Vector3(360* (1-daysubsection), 0));
            CurrentDay++;
        }

        mySlider.value = currentTime / timeInDay;
        dayCountText.text = "" + CurrentDay;

        Daylight.transform.Rotate(new Vector3((360 * daysubsection) * Time.deltaTime / timeInDay, 0));
	
	}
}
