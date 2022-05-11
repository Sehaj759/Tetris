using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    Color color;
    public Color MinoColor 
    {
        get => color;
        set
        {
            color = value;
            bodySprite.color = new Color(color.r, color.g, color.b, 1.0f);
        } 
    }

    SpriteRenderer bodySprite;
    void Awake()
    {
        bodySprite = transform.Find("Body").GetComponent<SpriteRenderer>();
    }

    public void SetScale(float newScale)
    {
        transform.localScale = new Vector3(newScale, newScale, 1);
    }
}
