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
            spriteRenderer.color = color;
        } 
    }

    SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetScale(float newScale)
    {
        transform.localScale = new Vector3(newScale, newScale, 1);
    }
}
