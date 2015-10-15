using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Inspectable : MonoBehaviour {

    public bool deselectionObject = false;
    public GameObject IndicatorObject;
    public string Name;
    public List<string> Updates;
    public const int MaxUpdates = 6;

    public bool newData = false;

    public void AddUpdate(string updateString)
    {
        Updates.Add(updateString);
        while (Updates.Count > MaxUpdates)
            Updates.RemoveAt(0);
        newData = true;
    }

    void Awake () {
        Updates = new List<string>();
    }

    public void Select()
    {
            if (IndicatorObject != null)
                IndicatorObject.SetActive(true);
    }

    public void Deselect()
    {
        if (IndicatorObject != null)
            IndicatorObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if (deselectionObject)
            GameManager.Main.CloseInspector();
        else if (!EventSystem.current.IsPointerOverGameObject())
            GameManager.Main.MakeSelection(gameObject);
    }
}
