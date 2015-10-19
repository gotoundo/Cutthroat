using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timepiece : MonoBehaviour {

    public float SecondsInDay = 60f;
    public float CurrentTime = 0;
    public float TimeLeftInDay
    {
        get { return SecondsInDay - CurrentTime; }
    }

    Light Daylight;
   // float timeInDay;
    
    Slider mySlider;
    public Text dayCountText;
    float daysubsection = 1f;

    public static int CurrentDay;

    public static Timepiece Main;

	// Use this for initialization

    void Awake()
    {
        Main = this;
        mySlider = GetComponent<Slider>();
        Daylight = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
        Daylight.transform.Rotate(new Vector3(-360 * daysubsection / 2, 0));
        CurrentDay = 1;

    }

    void Start()
    {
        //NewDay();
    }

	
	// Update is called once per frame
	void Update () {

        if (GameManager.Main.gameRunning)
        {
            CurrentTime += Time.deltaTime;
            if (CurrentTime >= SecondsInDay)
                NewDay();

            mySlider.value = CurrentTime / SecondsInDay;
            dayCountText.text = "" + CurrentDay;

            Daylight.transform.Rotate(new Vector3((360 * daysubsection) * Time.deltaTime / SecondsInDay, 0));
        }
	}

    void NewDay()
    {
        CurrentTime = 0;
        Daylight.transform.Rotate(new Vector3(360 * (1 - daysubsection), 0));
        CurrentDay++;

        Zeitgeist.RandomizePopularities();
        IngredientStore.Main.RefreshPrices();
        ProductsPaneUI.UpdateRecipesStatus();
    }
}
