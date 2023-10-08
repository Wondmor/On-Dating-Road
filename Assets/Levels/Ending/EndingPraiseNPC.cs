using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;


[System.Serializable]
public class EndingNPCInfo
{
    public Image npc = null;
    public string line = "";
}



public class EndingPraiseNPC : MonoBehaviour
{
    [SerializeField] public EndingNPCInfo[] endingNPCInfo = null;
    Dictionary<EScene, EndingNPCInfo> dicInfo = null;

    static readonly Dictionary<string, EScene> dic = new Dictionary<string, EScene>
    {
        { "TrashShooting", EScene.TrashShooting},
        { "Racing", EScene.Racing},
        { "BusGame", EScene.BusGame},
        { "Delivery", EScene.Delivery},
        { "Skewer", EScene.Skewer},
        { "CoinSkill", EScene.CoinSkill},
    };

    static public readonly EScene[] Order = 
    {
        EScene.Skewer,
        EScene.BusGame,
        EScene.Delivery,
        EScene.TrashShooting,
        EScene.CoinSkill,
        EScene.Racing    
    };

    // Start is called before the first frame update
    void Start()
    {
        dicInfo = new Dictionary<EScene, EndingNPCInfo>();
        foreach(var info in endingNPCInfo)
        {
            if (dic.ContainsKey(info.npc.gameObject.name))
                dicInfo[dic[info.npc.gameObject.name]] = info;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SetNPCAndGetInfo(EScene eScene, ref EndingNPCInfo _info)
    {
        if (dicInfo.ContainsKey(eScene))
        {
            var info = dicInfo[eScene];
            foreach (var toDisactive in endingNPCInfo)
            {
                toDisactive.npc.gameObject.SetActive(false);
            }

            info.npc.gameObject.SetActive(true);

            _info = info;
            return true;
        }
        return false;
    }

    public void HideAll()
    {
        foreach (var toDisactive in endingNPCInfo)
        {
            toDisactive.npc.gameObject.SetActive(false);
        }
    }
}
