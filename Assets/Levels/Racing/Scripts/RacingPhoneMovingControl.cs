using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RacingPhoneMovingControl : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;

    [SerializeField]
    Vector3[] positions, cameraPositions;

    bool moving = false;
    int currentPos = 0;
    Camera phoneCamera;
    RacingPlayerControl.BIKE_TYPE[] bikeType = { RacingPlayerControl.BIKE_TYPE.FIRE, RacingPlayerControl.BIKE_TYPE.WATER, RacingPlayerControl.BIKE_TYPE.GRASS };
    SubtitlePlayer player;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.Find("Canvas/SubtitleBG").GetComponent<SubtitlePlayer>();
        phoneCamera = GetComponentInChildren<Camera>();

        transform.parent.Find("Canvas/SubtitleBG").gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destPos = positions[currentPos];
        Vector3 destCamPos = cameraPositions[currentPos];

        if(Vector3.Distance(transform.localPosition, destPos) > 0.1f)
        {
            moving = true;
            transform.localPosition = Vector3.Lerp(transform.localPosition, destPos, speed * Time.deltaTime);
            phoneCamera.transform.localPosition = Vector3.Lerp(phoneCamera.transform.localPosition, destCamPos, speed * Time.deltaTime);
        }
        else
        {
            moving = false;
        }

        var performedThisFrame = GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame();
        if(!moving && performedThisFrame == CommonInputAction.EType.Enter)
        {
            Debug.Log("BIKE SELECTED " + bikeType[currentPos].ToString());
            GameManager.Instance.RacingData.BikeType = bikeType[currentPos];
            // set up racing times to 0
            GameManager.Instance.RacingData.RaceTime = 0;
            player.LoadSubtitle("racing_subtitle_selection_common");
            transform.parent.Find("Canvas/Images").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        if(GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == CommonInputAction.EType.Directions)
        {
            float direction = GameManager.Instance.CommonInputAction.directions.ReadValue<Vector2>().x;

            currentPos += direction > 0 ? -1 : 1;
            currentPos = Mathf.Clamp(currentPos, 0, positions.Length - 1);
        }
    }
}
