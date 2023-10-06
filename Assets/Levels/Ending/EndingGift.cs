using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingGift : MonoBehaviour
{
    [SerializeField] Image giftImage = null;
    [SerializeField] TextMeshProUGUI giftText = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGift(Sprite giftSprite, string text)
    {
        giftImage.sprite = giftSprite;
        giftImage.rectTransform.sizeDelta = giftSprite.rect.size;
        giftText.text = text;
    }
}
