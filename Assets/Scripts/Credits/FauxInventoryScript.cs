using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FauxInventoryScript : MonoBehaviour
{

    [SerializeField] private GameObject[] inventoryList;
    private void Start()
    {
        inventoryList = new GameObject[gameObject.transform.childCount];

        for(int i =0; i < gameObject.transform.childCount; i++)
        {
            inventoryList[i] = gameObject.transform.GetChild(i).gameObject;
            setItemEnabled(inventoryList[i], false);
        }


        DialogueManager.instance.onDialogueBeginInitializers += () =>
        {
            foreach (var g in inventoryList)
                g.SetActive(false);
                
        };

        DialogueManager.instance.onDialogueFinishedInitializers += () =>
        {
            foreach (var g in inventoryList)
                g.SetActive(true);
        };

    }

    private void setItemEnabled(GameObject g, bool willEnable)
    {
        Color color = g.GetComponent<Image>().color;
        color.a = willEnable ? 1f: 0f;
        g.GetComponent<Image>().color = color;

        g.GetComponent<Button>().enabled = willEnable;

    }

    public void AddItem(string itemName)
    {
        GameObject toAdd = Array.Find(inventoryList, item => item.name == itemName);
        if(toAdd != null)
        {
            setItemEnabled(toAdd, true);
            sortInventory();
        }
    }

    public void RemoveItem(string itemName)
    {
        GameObject toRemove = Array.Find(inventoryList, item => item.name == itemName);
        if (toRemove != null)
        {
            setItemEnabled(toRemove, false);
            sortInventory();
        }
    }

    public bool isItemObtained(string itemName)
    {
        GameObject toFind = Array.Find(inventoryList, item => item.name == itemName);
        if(toFind == null)
            return false;

        return toFind.GetComponent<Button>().enabled;
       
    }

    public bool isInventoryEmpty()
    {
        return Array.Find(inventoryList, item => item.GetComponent<Image>().color.a != 0) == null;
    }
    private void sortInventory()
    {
        List<GameObject> temp = new List<GameObject>();


      

        foreach (GameObject g in inventoryList)
        {
            if (g.GetComponent<Image>().color.a == 0)
            {
                temp.Add(g);
            }
        }

        foreach (GameObject g in inventoryList)
        {
            if (g.GetComponent<Image>().color.a == 1)
            {
                temp.Add(g);
            }
        }



        for(int i = 0; i < inventoryList.Length; i++)
        {
            temp[i].transform.SetSiblingIndex(i);
        }


    }

    #region singleton

    public static FauxInventoryScript instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }


    #endregion


}
