using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogueScript : MonoBehaviour
{
    public RectTransform listView;
    public GameObject Prefab;
    public Transform cartList;

    public static CatalogueScript instance;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close(GameObject catalogue) {
        catalogue.SetActive(false);
    }

    public void ClearList()
    {
        foreach (Transform child in listView)
        {
            Destroy(child.gameObject);
        }
    }

    public IEnumerator LoadCatalogue() 
    {
        GameObject newItem;

        // Attendre la fin de la coroutine GetCatalogue
        yield return StartCoroutine(NetworkManager.instance.GetCatalogue());

        NetworkManager.instance.allCatalog.ForEach((item) =>
        {
            newItem = Instantiate(Prefab, listView);
            newItem.GetComponent<CatalogueItemScript>().Name.text = item.name;
     //       newItem.GetComponent<CatalogueItemScript>().BrandName.text = item.company_name;
       //     newItem.GetComponent<CatalogueItemScript>().Price.text = item.price.ToString();
       //     newItem.GetComponent<CatalogueItemScript>().list = cartList;
        });
    }


    public IEnumerator CreateItemsWithFilters() 
    {
        GameObject newItem;

        // Attendre la fin de la coroutine ApplyFilter
        yield return StartCoroutine(NetworkManager.instance.ApplyFilter());

        Debug.Log(NetworkManager.instance.filteredCatalog.Count + " test ");

        NetworkManager.instance.filteredCatalog.ForEach((item) =>
        {
            newItem = Instantiate(Prefab, listView);
            Debug.Log(item.name + " test " + newItem.GetComponent<CatalogueItemScript>().Name.text);
            newItem.GetComponent<CatalogueItemScript>().Name.text = item.name;
      //      newItem.GetComponent<CatalogueItemScript>().BrandName.text = "test";
    //        newItem.GetComponent<CatalogueItemScript>().Price.text = item.company_name;
            newItem.GetComponent<CatalogueItemScript>().list = cartList;
        });
    }

    // public void CreateItemsWithFilters() {
    //     NetworkManager.instance.GetCatalogue();
    //     GameObject newItem = Instantiate(Prefab, listView);
    //     StartCoroutine(NetworkManager.instance.ApplyFilter());
    //     Debug.Log(NetworkManager.instance.filteredCatalog.Count + " test ");
    //     NetworkManager.instance.filteredCatalog.ForEach((item) =>
    //     {
    //         newItem = Instantiate(Prefab, listView);
    //         Debug.Log(item.name + " test "+ newItem.GetComponent<CatalogueItemScript>().Name.text);
    //         newItem.GetComponent<CatalogueItemScript>().Name.text = item.name;
    //     });
    // }
}
