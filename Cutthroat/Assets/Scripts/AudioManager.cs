using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public static AudioManager Main;
    public AudioSource Source;
    public AudioClip[] Music;
    public AudioClip[] DogBarks;
    public AudioClip[] BuyUpgrade;

    public AudioClip UIButtonDefault;
    public AudioClip SaleMade;



    // Use this for initialization
    void Awake () {
        Source = GetComponent<AudioSource>();
        if (Main == null)
        {
            Main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }

    // Update is called once per frame

    public bool BarkingDogs = false;
    float BarkMinWait = 0.25f;
    float BarkMaxWait = 2f;
    float CurrentBarkdown = 1f;
    float BarkVolume = .1f;
    void Update () {
        if(BarkingDogs)
        {
            CurrentBarkdown -= Time.deltaTime;
            if(CurrentBarkdown<=0)
            {
                CurrentBarkdown = Random.Range(BarkMinWait, BarkMaxWait);
                Source.PlayOneShot(DogBarks[Random.Range(0, DogBarks.Length)], BarkVolume);
            }
        }
	
	}
}
