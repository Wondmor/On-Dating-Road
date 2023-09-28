using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public Texture image;
    public float fadetime;
    private void FadeOut()
    {
        GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), image);

    }



}
