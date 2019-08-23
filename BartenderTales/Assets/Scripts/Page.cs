using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PageSide
{
    public RawImage[] m_ingredientImages;
    public RawImage m_potionImage;
}

public class Page : MonoBehaviour
{
    private Collider m_collider;
    [SerializeField]
    private PageSide[] m_recipePageSides;

    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(ray, out var hit, 1f)
            && hit.transform.GetComponent<Page>())
        {
            m_collider.enabled = false;
        }
        else
        {
            m_collider.enabled = true;
        }
    }

    /// <summary>
    /// Sets the ingredient and potion images for the specified page
    /// </summary>
    /// <param name="ingredient1"> The image of the first ingredient type of the recipe </param>
    /// <param name="ingredient2"> The image of the second ingredient type of the recipe </param>
    /// <param name="potion"> The image of the potion the recipe creates </param>
    /// <param name="sideIndex"> Which side of the page to display recipe on (0/1) </param>
    public void SetImages(Texture ingredient1, Texture ingredient2, Texture potion, int sideIndex)
    {
        int index = m_recipePageSides.Length > 1 ? sideIndex : 0;
        m_recipePageSides[index].m_ingredientImages[0].texture = ingredient1;
        m_recipePageSides[index].m_ingredientImages[1].texture = ingredient2;
        m_recipePageSides[index].m_potionImage.texture = potion;
    }
}
