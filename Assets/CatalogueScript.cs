using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatalogueScript : MonoBehaviour
{
    public RectTransform listView;
    public GameObject Prefab;
    public Transform cartList;

    public static CatalogueScript instance;
    
    void Start()
    {
        instance = this;
    }

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

        yield return StartCoroutine(NetworkManager.instance.GetCatalogue());

        NetworkManager.instance.allCatalog.ForEach((item) =>
        {
            newItem = Instantiate(Prefab, listView);
            newItem.GetComponent<CatalogueItemScript>().Name.text = item.name;
            Debug.Log(item.price + " test du prix " + newItem.GetComponent<CatalogueItemScript>().Price.text);
            newItem.GetComponent<CatalogueItemScript>().Price.text = item.price.ToString();
            newItem.GetComponent<CatalogueItemScript>().BrandName.text = item.company_name;
            newItem.GetComponent<CatalogueItemScript>().list = cartList;
        });
    }


    public IEnumerator CreateItemsWithFilters() 
    {
        GameObject newItem;
        yield return StartCoroutine(NetworkManager.instance.ApplyFilter());
        NetworkManager.instance.filteredCatalog.ForEach((item) =>
        {
            newItem = Instantiate(Prefab, listView);
            Debug.Log(item.name + " test " + newItem.GetComponent<CatalogueItemScript>().Name.text);
            newItem.GetComponent<CatalogueItemScript>().Name.text = item.name;
            Debug.Log(item.price + " test du prix " + newItem.GetComponent<CatalogueItemScript>().Price.text);
            newItem.GetComponent<CatalogueItemScript>().Price.text = item.price.ToString();
            newItem.GetComponent<CatalogueItemScript>().list = cartList;
            Transform thumbnailTransform = newItem.transform.Find("Thumbnail");
            int randomItem = Random.Range(0, MultipleObjectPlacement.instance.TexturesList.Count);
            if (thumbnailTransform != null)
            {
                RawImage thumbnailImage = thumbnailTransform.GetComponent<RawImage>();
                if (thumbnailImage != null)
                {
                    thumbnailImage.texture = MultipleObjectPlacement.instance.TexturesList[randomItem];
                }
            }
            GameObject TryAR = newItem.transform.Find("TryAR").gameObject;
            Button TryARButton = TryAR.GetComponent<Button>();
            TryARButton.onClick.AddListener(() => {
                MultipleObjectPlacement.instance.mainButtons.SetActive(true);
                MultipleObjectPlacement.instance.scanText.SetActive(true);
                MultipleObjectPlacement.instance.spawnObject(MultipleObjectPlacement.instance.ModelsList[randomItem]);
            });
        });
    }
}
