using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectSelected : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private GameObject border;
    private RawImage image;

    private void Start()
    {
        image = border.GetComponent<RawImage>();
        image.color= Color.white;
        MenuManager.instance.onMapSelect += setHighlight;
    }

    public void setHighlight(int selectedValue)
    {
        image.color = selectedValue == id ? Color.green : Color.white;
    }
}
