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

    public void setCartItemInfo(TMP_Text qte, TMP_Text name, TMP_Text price) {
        Qte.text = "Qte: " + qte.text;
        Name.text = name.text;
        Price.text = price.text;
    }
}
