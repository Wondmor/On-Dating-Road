using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public interface ICutsceneBehaviour
{
    public enum EInputType
    {
        None = 0,
        Confirm = 1,
        Cancel = 2,
        Directions = 3
    }
    public void Play();
}
public class CutsceneBehaviour : ICutsceneBehaviour
{
    public Dictionary<ICutsceneBehaviour.EInputType, CutsceneBehaviour> behavs;

    public CutsceneBehaviour(params KeyValuePair<ICutsceneBehaviour.EInputType, CutsceneBehaviour>[] pairs)
    {
        foreach (var pair in pairs) { behavs[pair.Key] = pair.Value; }
    }


    virtual public void Play()
    { 
    
    }
    public CutsceneBehaviour Next(ICutsceneBehaviour.EInputType eInputType)
    {
        if (behavs.ContainsKey(eInputType))
            return behavs[eInputType];
        else
            return null;
    }
}

public class SwitchPictureBehaviour : CutsceneBehaviour
{
    Sprite sprite;
    Image target;
    public SwitchPictureBehaviour(string picName, Image target,
        params KeyValuePair<ICutsceneBehaviour.EInputType, CutsceneBehaviour>[] pairs)
        : base(pairs)
    {
        this.sprite = Resources.Load<Sprite>(picName);
        this.target = target;

    }

    public void Init()
    {

    }

    public void Play()
    {
        target.sprite = sprite;
    }
}

//public class WaitInputBehaviour : CutsceneBehaviour
//{

//    EType type;

//    public WaitInputBehaviour(EType eType)
//    {
//        this.type = eType;
//    }

//    public void Init() { }
//    public void Play()
//    {

//    }
//}

class CutsceneStep
{

}