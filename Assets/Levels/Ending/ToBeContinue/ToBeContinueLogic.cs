using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToBeContinueLogic : MonoBehaviour
{
    [SerializeField] RestartStreetSign streetSign = null;
    [SerializeField] string RestartSceneName = "GameStart";

    public void SetBG(Sprite sprite)
    {
        transform.Find("Canvas/BG").GetComponent<Image>().sprite = sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        streetSign.Select(RestartStreetSign.EState.None);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == CommonInputAction.EType.Directions)
        {
            var directions = GameManager.Instance.CommonInputAction.directions.ReadValue<Vector2>();
            if(directions.x < 0 || directions.y < 0)
            {
                streetSign.Select(RestartStreetSign.EState.Restart);
            }
            else if(directions.x > 0 || directions.y > 0)
            {
                streetSign.Select(RestartStreetSign.EState.Exit);
            }
        }
        else if (GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == CommonInputAction.EType.Enter)
        {
            switch(streetSign.GetState())
            {
                case RestartStreetSign.EState.Restart:
                    GameLogicManager.Instance.OnEndingFinished();
                    break;
                case RestartStreetSign.EState.Exit:
                    Application.Quit();
                    break;
                default: 
                    break;
            }
        }
    }
}
