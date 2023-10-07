using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletItem : MonoBehaviour
{
    [SerializeField]
    Image tile, item;

    public void Set(Sprite tilesprite, Sprite itemsprite)
    {
        if(itemsprite)
        {
            item.sprite = itemsprite;
            item.gameObject.SetActive(true);
        }
        else
        {
            item.gameObject.SetActive(false);
        }
        tile.sprite = tilesprite;
    }
}
