using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingDialog : MonoBehaviour
{
    [SerializeField]
    Image buttonTrue, buttonFalse, item;

    [SerializeField]
    TextMeshProUGUI moneyText, buyText;

    [SerializeField]
    GameObject canvas, notEnough;


    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
    }

    public void Setup(Sprite sprite, float price)
    {
        item.sprite = sprite;
        moneyText.text = string.Format("\uFFE5{0}", price);
        canvas.SetActive(true);
        notEnough.SetActive(false);
    }

    public void Select(bool buy)
    {
        if(buy)
        {
        }
        if (!buy)
        {
        }

    }

    public void Unshow()
    {
        canvas.SetActive(false);
    }

    public void NoEnough()
    {
        notEnough.SetActive(true);
    }

    public void ShowGet(Sprite sprite, string name)
    {
        item.sprite = sprite;
        item.SetNativeSize();
        moneyText.text = "";
        canvas.SetActive(true);
        notEnough.SetActive(false);
        buyText.text = string.Format("得到了{0}", name);
    }
}
