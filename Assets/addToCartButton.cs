using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class addToCartButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject itemPrefab;
    public Transform list;

    public TMP_Text quantityTextPrefab; // Assignez ceci dans l'inspecteur
public TMP_Text nameTextPrefab; // Assignez ceci dans l'inspecteur
public TMP_Text priceTextPrefab; // Assignez ceci dans l'inspecteur
public TMP_Text materialTextPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addItemToCart() {


        int qty = 1;


    // Assignez les valeurs string aux objets TMP_Text
  
            GameObject newItem = Instantiate(itemPrefab, list);
            newItem.GetComponent<CartScript>().Qte.text = "Qte: " + qty.ToString();
            newItem.GetComponent<CartScript>().Name.text = MultipleObjectPlacement.instance._lastObjectTouched.name;
            newItem.GetComponent<CartScript>().Price.text = "Price: " + MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().price.ToString();
            newItem.GetComponent<CartScript>().Material.text = "Material:" + MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().matPrincipal[MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().indexMatPrincipal];
            newItem.GetComponent<CartScript>().Brand.text = "Brand: " + MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().brand;
            
        Debug.Log(qty + " test ");

    }
}
