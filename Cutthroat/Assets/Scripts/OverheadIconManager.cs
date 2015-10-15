using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OverheadIconManager : MonoBehaviour {
    public Vector3 Offset;
    public Vector3 FloatSpeed;

    public GameObject overheadIconTemplate;
    public Canvas MainCanvas;
    public GameObject myOverheadIcon;
    public Image myOverheadImage;
    public FloatingIcon icon;


    GameObject FloatingIconDump;

	// Use this for initialization
	void Start () {
        if (FloatingIconDump == null)
            FloatingIconDump = GameObject.FindGameObjectWithTag("FloatingIconDump");


        MainCanvas = ((GameObject)GameObject.FindGameObjectWithTag("Canvas")).GetComponent<Canvas>() ;
        myOverheadIcon = (GameObject)Instantiate(overheadIconTemplate);
        myOverheadIcon.transform.SetParent(FloatingIconDump.transform);
        icon = myOverheadIcon.GetComponent<FloatingIcon>();
        icon.Target = transform;
        

        myOverheadImage = myOverheadIcon.GetComponent<Image>();
        

        myOverheadIcon.SetActive(false);
	}

    float remainingTime = 0;
    //Vector3 newOffset;
    // Update is called once per frame
    void Update () {
        if (myOverheadIcon.activeSelf)
        {
            remainingTime -= Time.deltaTime;
            icon.Offset += FloatSpeed * Time.deltaTime;
            if (remainingTime <= 0)
            {
                myOverheadIcon.SetActive(false);
                
            }
        }
	}

    
    public void ShowIcon(Sprite sprite, float duration)
    {
        myOverheadIcon.SetActive(true);
        myOverheadImage.overrideSprite = sprite;
        remainingTime = duration;
        icon.Offset = new Vector3(Offset.x, Offset.y, Offset.z);
        icon.SnapToPosition();
       

    }

}
