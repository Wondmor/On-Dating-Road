using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class SwitchPicturePlayable : BasicPlayableBehaviour
{
    [Header("换图片")]
    public ExposedReference<Image> target;
    public Sprite sprite;

    private Image _target;

    [Header("复数对象")]

    public ExposedReference<Image>[] targets = null;
    public Sprite[] sprites = null;

    private List<Image> _targets = new List<Image>();
    //public ExposedReference<Text> dialog;
    //private Text _dialog;
    //[Multiline(3)]
    //public string dialogStr;

    public override void OnGraphStart(Playable playable)
    {
        _target = target.Resolve(playable.GetGraph().GetResolver());
        //sprite = target.Resolve(playable.GetGraph().GetResolver());
        if (targets != null && targets.Length > 0)
        {
            _targets.Clear();
            foreach (var tar in targets)
            {
                _targets.Add(tar.Resolve(playable.GetGraph().GetResolver()));
            }
        }
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        _target.gameObject.SetActive(true);
        _target.sprite = sprite;

        for(int i = 0; i < _targets.Count; i++)
        {
            if (_targets[i] != null && sprites[i] != null)
            {
                _targets[i].gameObject.SetActive(true);
                _targets[i].sprite = sprites[i];
            }
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (_target)
        {
            //_target.gameObject.SetActive(false);
        }
    }
}