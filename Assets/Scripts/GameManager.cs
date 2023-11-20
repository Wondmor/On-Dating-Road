using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GameManager{
    // events
    // GameManager is a Singleton that controls the whole game
    private static GameManager m_GameManager;
    // dummy gameobj to hold every sub component
    private static GameObject gameObject;
    public static GameManager Instance
    {
        get
        {
            if(m_GameManager == null)
            {
                m_GameManager = new GameManager();
                gameObject = new GameObject("_gameObj");
                // add to not destroy
                Object.DontDestroyOnLoad(gameObject);
            }
            return m_GameManager;
        }
    }

    // Input Controller
    private static CommonInputAction m_CommonInputAction;
    public CommonInputAction CommonInputAction
    {
        get
        {
            if (m_CommonInputAction == null)
            {
                Object commonInputActionPrefab = Resources.Load("Prefabs/CommonInputAction");

                GameObject.Instantiate(commonInputActionPrefab, gameObject.transform);

                m_CommonInputAction = gameObject.GetComponentInChildren<CommonInputAction>();
                m_CommonInputAction.enabled = true;
            }
            return m_CommonInputAction;
        }
    }

    private static RacingData m_RacingData;
    public RacingData RacingData
    {
        get
        {
            if(m_RacingData == null)
            {
                m_RacingData = new RacingData();
            }
            return m_RacingData;
        }
    }

    private static LogManager m_LogManager;
    public LogManager LogManager
    {
        get
        {
            if (m_LogManager == null)
            {
                m_LogManager = new LogManager();
            }
            return m_LogManager;
        }
    }

}
