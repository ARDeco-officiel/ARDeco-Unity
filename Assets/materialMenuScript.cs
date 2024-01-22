using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class materialMenuScript : MonoBehaviour
{
    public RectTransform listView;
    public RectTransform listViewSecondary;
    public GameObject PrefabPrimary;
    public GameObject PrefabSecondary;
    public GameObject colorMenu;

    public bool isPrimary;

    public static materialMenuScript instance;
    
    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (MultipleObjectPlacement.instance._lastObjectTouched == null)
            colorMenu.SetActive(false);
    }

    public void Close(GameObject colorMenu) {
        colorMenu.SetActive(false);
    }

    public void loadColorsMenu() 
    {
        this.ClearList();
        colorMenu.SetActive(true);
        StartCoroutine(LoadColorsPrimary());
        StartCoroutine(LoadColorsSecondary());
    }


    public void ClearList()
    {
        foreach (Transform child in listView)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in listViewSecondary)
        {
            Destroy(child.gameObject);
        }
    }

    public IEnumerator LoadColorsPrimary() 
    {
        GameObject newItem;
        if (MultipleObjectPlacement.instance._lastObjectTouched == null)
            yield break;
        List<Sprite> sprites = MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().matPrincipalTexture;

        foreach (Sprite sprite in sprites)
        {
            Debug.Log("Création d'un prefab pour le sprite : " + sprite.name);
            newItem = Instantiate(PrefabPrimary, listView);
            newItem.GetComponent<colorButton>().buttonIndex = sprites.IndexOf(sprite);
            Debug.Log("Index du bouton : " + newItem.GetComponent<colorButton>().buttonIndex);
            Transform thumbnailTransform = newItem.transform.Find("Texture");
            if (thumbnailTransform != null)
            {
                Image thumbnailImage = thumbnailTransform.GetComponent<Image>();
                if (thumbnailImage != null)
                {
                    thumbnailImage.sprite = sprite;
                }
            }
            yield return null;
        }
    }

    public IEnumerator LoadColorsSecondary() 
    {
        GameObject newItem;
        if (MultipleObjectPlacement.instance._lastObjectTouched == null)
            yield break;
        List<Sprite> sprites = MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().matSecondaryTexture;

        foreach (Sprite sprite in sprites)
        {
            Debug.Log("Création d'un prefab pour le sprite : " + sprite.name);
            newItem = Instantiate(PrefabSecondary, listViewSecondary);
            newItem.GetComponent<ColorButtonSecondary>().buttonIndex = sprites.IndexOf(sprite);
            Debug.Log("Index du bouton : " + newItem.GetComponent<ColorButtonSecondary>().buttonIndex);
            Transform thumbnailTransform = newItem.transform.Find("Texture");
            if (thumbnailTransform != null)
            {
                Image thumbnailImage = thumbnailTransform.GetComponent<Image>();
                if (thumbnailImage != null)
                {
                    thumbnailImage.sprite = sprite;
                }
            }
            yield return null;
        }
    }

    public void materialChangePrimary(int MaterialIndex) {
            List<int> matPrincipalIndex = MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().matPrincipalIndex;
            foreach (Transform child in MultipleObjectPlacement.instance._lastObjectTouched.transform)
            {
                Renderer childRenderer = child.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    if (matPrincipalIndex.Contains(child.GetSiblingIndex()))
                    {
                        childRenderer.material = MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().matPrincipal[MaterialIndex];
                        MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().indexMatPrincipal = MaterialIndex;
                    }
                }
            }
    }

    public void materialChangeSecondary(int MaterialIndex) {
            List<int> matSecondaireIndex = MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().matSecondaryIndex;
            foreach (Transform child in MultipleObjectPlacement.instance._lastObjectTouched.transform)
            {
                Renderer childRenderer = child.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    if (matSecondaireIndex.Contains(child.GetSiblingIndex()))
                    {
                        childRenderer.material = MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().matSecondary[MaterialIndex];
                    }
                }
            }
    }

}
