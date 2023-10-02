using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    [SerializeField]
    RenderTexture renderTexture;

    SpriteRenderer spriteRenderer;

    Texture2D texture;
    Rect rect;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
    }

    // Update is called once per frame
    void Update()
    {
        int width = renderTexture.width;
        int height = renderTexture.height;

        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        RenderTexture.active = renderTexture;
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();
        spriteRenderer.sprite = Sprite.Create(texture, rect, Vector2.zero);
    }
}
