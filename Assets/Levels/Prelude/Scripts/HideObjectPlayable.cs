using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class HideObjectPlayable : BasicPlayableBehaviour
{
    [Header("显示对象")]
    public ExposedReference<GameObject>[] showGos = null;
    [Header("隐藏对象")]
    public ExposedReference<GameObject>[] hideGos = null;
    List<GameObject> _showGos = new List<GameObject>();
    List<GameObject> _hideGos = new List<GameObject>();

    public override void OnGraphStart(Playable playable)
    {
        _showGos.Clear();
        _hideGos.Clear();
        foreach (var g in showGos)
        {
            _showGos.Add(g.Resolve(playable.GetGraph().GetResolver()));
        }
        foreach (var g in hideGos)
        {
            _hideGos.Add(g.Resolve(playable.GetGraph().GetResolver()));
        }
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        foreach (var g in _showGos)
        {
            if(g != null)
                g.gameObject.SetActive(true);
        }
        foreach (var g in _hideGos)
        {
            if (g != null)
                g.gameObject.SetActive(false);
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    { 
    }

}
