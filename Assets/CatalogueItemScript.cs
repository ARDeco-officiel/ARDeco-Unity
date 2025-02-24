using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;


public class CatalogueItemScript : MonoBehaviour
{
    // Start is called before the first frame update


    [System.Serializable]
    public struct itemStruct
    {
        public TMP_Text Qte;
        public TMP_Text Price;
        public Sprite Thumbnail;
    }

    public GameObject itemPrefab;
    public Transform list;
    public TMP_Text Name;
    public TMP_Text BrandName;
    public TMP_Text Price;

    public TMP_Text Material;

    public int id;
    public int price;
    public string room;
    public string style;
    public string color;
    int i = 0;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addItemToCart(TMP_Text Quantity) {
        int qty = int.Parse(Quantity.text);
        //Debug.Log(qty + " test ");
        GameObject newItem = Instantiate(itemPrefab, list);
        newItem.GetComponent<CartScript>().setCartItemInfo(Quantity, Name, Price, Material);
    }
}
