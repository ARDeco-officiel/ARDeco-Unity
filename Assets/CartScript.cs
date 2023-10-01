using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CartScript : MonoBehaviour
{
    public TMP_Text Qte;
    public TMP_Text Name;
    public TMP_Text Brand;
    public TMP_Text Price;
    public Sprite Thumbnail;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCartItemInfo(TMP_Text qte) {
        Qte.text = "Qte: " + qte.text;
        Brand.text = "Ikea";
        Price.text = "138€";
    }
}
