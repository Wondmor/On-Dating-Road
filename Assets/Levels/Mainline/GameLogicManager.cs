using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Shopping;


public struct GameData
{
    public float money { get; set; }
    public float positiveComment { get; set; }
    public float countDown { get; set; }
}

public struct BridgeData
{
    public bool bFadeIn { get; set; }
    public bool bLeft { get; set; }
    public List<string> subtitles { get; set; }
    public uint currentRoadMileStone { get; set; }
}

public enum EScene : int
{
    GameStart       = 0,
    Prelude         = GameStart + 1,
    Bridge          = Prelude + 1,

    InGame          = Bridge + 1,
    TrashShooting   = InGame,
    Racing          = TrashShooting + 1,
    BusGame         = Racing + 1,
    InGameEnd       = BusGame,

    CoinSkill       = InGameEnd + 1,
    Shopping        = CoinSkill + 1,
    Ending          = Shopping + 1,
    Test            = Ending + 1,
}

public enum ECoinSkillType
{
    Time,
    Money
}

public struct CoinSkillData
{
    public ECoinSkillType eType;
}

public enum EGift
{
    Bad,
    Normal,
    Good,
    Free,
    None,
}

public enum EEnding
{
    GoodCharacter,
    BadCharacter,
    BeLate
}

public struct EndingData
{
    public EGift eGift { get; set; }
    public EEnding eEnding { get; set; }
    public ShopItem giftInfo { get; set; }
}

public class GameLogicManager
{
    enum EState : uint
    {
        None,
        Prelude,
        DatingRoad,
        CoinSkill,
        Shopping,
        Ending,
        End
    }
    // Variables///////////////////////////////////////////////

    public GameData gameData { get; private set; }
    public BridgeData bridgeData { get; private set; }
    public CoinSkillData coinSkillData { get; private set; }
    public EndingData endingData { get; private set; }


    EState curState { get; set; }
    List<EScene> gamePool = null;
    uint currentRoadMilestone = 0;

    // Unless Restart() is called from beginning
    bool bTestMode = true;


    
    const uint c_RoadMilestoneCount = 5;

    public const float c_StandardGameDuration = 1200;
    const float c_StandardRoadDuration = 300;
    const float c_RefuseToHelpPeopleCost = 10;

    const float c_CoinSkillRequest = 50; // Coin skill check
    const float c_CoinSkillMoneyAdd = 50000; // The number Musk'll give you in RMB (Maybe with alipay).
    
    const float c_GoodCharacterEndingRequest = 75; // Ending check

    public static readonly Dictionary<EScene, string> c_sceneNames = new Dictionary<EScene, string>
    {
            {EScene.GameStart, "GameStart"},
            {EScene.Prelude, "Prelude"},
            {EScene.Bridge, "Bridge"},

            {EScene.TrashShooting, "TrashShooting"},
            {EScene.Racing, "Racing"},
            {EScene.BusGame, "BusGame"},
            {EScene.CoinSkill, "CoinSkill"},

            {EScene.Shopping, "Shopping"},
            {EScene.Ending, "Ending"},
            {EScene.Test, "TestMain"},
    };

    // Variables END///////////////////////////////////////////

    // Singleton///////////////////////////////////////////////
    private static GameLogicManager m_Instance = null;
    public static GameLogicManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameLogicManager();
            }
            return m_Instance;
        }
    }
    // Singleton END///////////////////////////////////////////

    // Public Functions////////////////////////////////////////
    public GameLogicManager()
    {
        Init();
    }

    public void GameStart()
    {
        Init();
        bTestMode = false;

        yieldToScene(EScene.Prelude);
    }

    public void TestModeStart()
    {
        Init();
        bTestMode = true;
    }

    public void OnPreludeFinished()
    {
        if (bTestMode)
            onTestFinished();
        else
        {
            curState = EState.DatingRoad;

            datingRoadControl();
        }
    }
    public void OnBridgeFinished()
    {
        if (bTestMode)
            onTestFinished();
        else
        {
            if (curState != EState.DatingRoad)
                throw new System.Exception("OnBridgeFinished() during wrong state.");

            if (gamePool.Count <= 0)
                throw new System.Exception("Not enough game in gamePool.");

            var nextGameIdx = UnityEngine.Random.Range(0, gamePool.Count);
            EScene nextGame = gamePool[nextGameIdx];
            gamePool.RemoveAt(nextGameIdx);

            yieldToScene(nextGame);
        }
    }

    public void OnMiniGameRefused()
    {
        if (bTestMode)
            onTestFinished();
        else
        {
            OnMiniGameFinished(gameData.money, gameData.positiveComment - c_RefuseToHelpPeopleCost);
        }
    }

    public void OnMiniGameFinished(float money, float positiveComment, float countDown)
    {
        if (bTestMode)
            onTestFinished();
        else
        {
            var _gameData = gameData;
            _gameData.countDown = Mathf.Max(0, countDown);
            gameData = _gameData;

            OnMiniGameFinished(money, positiveComment);
        }
    }
    public void OnMiniGameFinished(float money, float positiveComment)
    {
        if (bTestMode)
            onTestFinished();
        else
        {
            var _gameData = gameData;
            _gameData.money = Mathf.Max(0, money);
            _gameData.positiveComment = Mathf.Max(0, positiveComment);
            _gameData.countDown = Mathf.Max(0, _gameData.countDown - c_StandardRoadDuration);
            gameData = _gameData;

            currentRoadMilestone++;


            datingRoadControl();
        }
    }

    public void OnCoinSkillFinished(float money, float positiveComment)
    {
        if (bTestMode)
            onTestFinished();
        else
        {
            var _gameData = gameData;
            _gameData.money = Mathf.Max(0, money);
            _gameData.positiveComment = Mathf.Max(0, positiveComment);
            gameData = _gameData;


            if (curState != EState.CoinSkill)
                throw new System.Exception("OnCoinSkillFinished() during wrong state.");
            curState = EState.Shopping;
            shoppingControl();
        }
    }

    public void OnShoppingFinished(EGift gift, ShopItem giftInfo)
    {
        if (bTestMode)
            onTestFinished();
        else
        {
            if (curState != EState.Shopping)
                throw new System.Exception("OnShoppingFinished() during wrong state.");
            curState = EState.Ending;

            EEnding eEnding = gameData.positiveComment >= c_GoodCharacterEndingRequest ? EEnding.GoodCharacter : EEnding.BadCharacter;

            endingControl(eEnding, gift, giftInfo);
        }
    }
    public void OnEndingFinished()
    {
        if (bTestMode)
            onTestFinished();
        else
        {
            yieldToScene(EScene.GameStart);
        }
    }
    // Public Functions END///////////////////////////////////////////

    // Private FUnctions//////////////////////////////////////////////
    void Init()
    {
        bTestMode = true;

        GameData _gameData = new GameData();
        _gameData.money = 0;
        _gameData.positiveComment = 0;
        _gameData.countDown = 7200; // 2h
        this.gameData = _gameData;


        gamePool = new List<EScene>();

        // TODO: shouldn't repeat but we don't have more than them for now.
        gamePool.Add(EScene.TrashShooting);
        gamePool.Add(EScene.Racing);
        gamePool.Add(EScene.BusGame);
        gamePool.Add(EScene.TrashShooting);
        gamePool.Add(EScene.Racing);
        gamePool.Add(EScene.BusGame);


        currentRoadMilestone = 0;

        curState = EState.Prelude;
        EndingData _endingData = new EndingData();
        _endingData.eGift = EGift.None;
        this.endingData = _endingData;
    }

    void yieldToScene(EScene eScene)
    {
        SceneManager.LoadScene(c_sceneNames[eScene]);
    }

    void onTestFinished()
    {
        yieldToScene(EScene.Test);
    }

    void datingRoadControl()
    {
        if (curState != EState.DatingRoad)
            throw new System.Exception("roadControl() during wrong state.");

        if (currentRoadMilestone >= c_RoadMilestoneCount) // Arrive?
        {
            // Arrived
            curState = EState.CoinSkill;
            coinSkillControl(ECoinSkillType.Money);
        }
        else if (gameData.countDown < c_StandardGameDuration) // Will be late?
        {
            // Will be late
            curState = EState.CoinSkill;
            coinSkillControl(ECoinSkillType.Time);
        }
        else
        {
            // Still on the road

            BridgeData _bridgeData = new BridgeData();
            _bridgeData.bFadeIn = true;
            _bridgeData.bLeft = UnityEngine.Random.Range(0, 2) > 0;
            _bridgeData.subtitles = new List<string>();
            _bridgeData.currentRoadMileStone = currentRoadMilestone;

            yieldToScene(EScene.Bridge);
        }
    }

    void coinSkillControl(ECoinSkillType eType)
    {
        if (curState != EState.CoinSkill)
            throw new System.Exception("coinSkillControl() during wrong state.");

        bool bCoinSkill = gameData.positiveComment >= c_CoinSkillRequest;

        if (bCoinSkill)
        {
            CoinSkillData _coinSkillData = new CoinSkillData();
            _coinSkillData.eType = eType;
            coinSkillData = _coinSkillData;
            yieldToScene(EScene.CoinSkill);
        }
        else 
        {
            if(eType == ECoinSkillType.Time)
            {
                curState = EState.Ending;
                endingControl(EEnding.BeLate, EGift.None, new ShopItem());
            }
            else
            {
                curState = EState.Shopping;
                shoppingControl();
            }
        }


    }

    void shoppingControl()
    {
        yieldToScene(EScene.Shopping);
    }

    void endingControl(EEnding eEnding, EGift eGift, ShopItem giftInfo)
    {
        if (curState != EState.Ending)
            throw new System.Exception("endingControl() during wrong state.");

        var _endingData = endingData;
        _endingData.eEnding = eEnding;
        _endingData.eGift = eGift;
        _endingData.giftInfo = giftInfo;
        endingData = _endingData;

        yieldToScene(EScene.Ending);
    }


    // Private FUnctions END//////////////////////////////////////////
}
