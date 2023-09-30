using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RacingSubtitleCallback : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnSection(int section)
    {
        // when we hit section, load next subtitle
        string filename = string.Format("racing_subtitle_selection_{0}", (int)GameManager.Instance.RacingData.BikeType);
        Debug.Log(filename);
        GetComponent<SubtitlePlayer>().LoadSubtitle(filename);
    }

    public void OnEnd()
    {
        SceneManager.LoadScene("Racing");
    }
}
