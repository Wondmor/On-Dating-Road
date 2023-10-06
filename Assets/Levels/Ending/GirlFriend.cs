using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlFriend : MonoBehaviour
{
    [SerializeField] public GameObject[] moods = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMood(EGirlFriendMood _mood)
    {
        for(EGirlFriendMood mood = EGirlFriendMood.Praise; mood < EGirlFriendMood.None; ++mood)
        {
            if(mood == _mood)
            {
                moods[(int)mood].SetActive(true);
            }
            else
            {
                moods[(int)mood].SetActive(false);
            }

        }
    }
}
