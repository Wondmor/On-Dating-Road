using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public enum EGirlFriendMood : int
{
    Praise = 0,
    SadBack = 1,
    Expect = 2,
    Sad = 3,
    Unhappy = 4,
    Happy = 5,
    None = 6
}

public class GirlFriendPlayable : BasicPlayableBehaviour
{
    public override double duration => 1 / 300.0f;
    [SerializeField] GirlFriend girlFriend = null;
    [SerializeField] uint eMood = (int)EGirlFriendMood.None;
    [SerializeField] [ReadOnly][Multiline] string prompt = "Praise = 0, SadBack = 1, Expect = 2, Sad = 3, " +
        "Unhappy = 4, Happy = 5, None = 6";
    //[SerializeField] EGirlFriendMood eMood = EGirlFriendMood.None;
    // Start is called before the first frame update

    public override void OnGraphStart(Playable playable)
    {

    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        girlFriend.gameObject.SetActive(true);
        girlFriend.SetMood((EGirlFriendMood)eMood);
        
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }
}
