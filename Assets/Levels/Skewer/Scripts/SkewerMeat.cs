using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkewerMeat : MonoBehaviour
{
    public enum Type
    {
        LEAN,
        FAT
    }

    [SerializeField]
    Sprite[] sprites;

    Type meatType = Type.LEAN;

    public Type MeatType { get => meatType; private set => meatType = value; }

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetupMeat(Type type)
    {
        meatType = type;
        spriteRenderer.sprite = sprites[(int)meatType];
    }

}
