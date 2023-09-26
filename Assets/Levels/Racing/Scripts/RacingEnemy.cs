using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class RacingEnemy : MonoBehaviour
{
    Transform rider;
    Transform bike;
    Transform enemy;

    [SerializeField]
    RacingPlayerControl.BIKE_TYPE bikeType;

    Vector3 bikeOriginPos;
    Vector3 riderOriginPos;

    bool dying = false;

    public RacingPlayerControl.BIKE_TYPE BikeType { get => bikeType; set => bikeType = value; }

    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.Find("enemy");
        bike = transform.Find("bike");
        rider = transform.Find("rider");

        bikeOriginPos = bike.localPosition;
        riderOriginPos = rider.localPosition;
    }

    private void Update()
    {
    }

    public void ResetTransforms()
    {
        bike.localPosition = bikeOriginPos;
        rider.localPosition = riderOriginPos;

        bike.gameObject.SetActive(false);
        rider.gameObject.SetActive(false);

        enemy.gameObject.SetActive(true);

        dying = false;
    }

    public void Hit()
    {
        if (dying == true)
        {
            return;
        }
        dying = true;

        enemy.gameObject.SetActive(false);
        bike.gameObject.SetActive(true);
        rider.gameObject.SetActive(true);

        Vector3[] path = new Vector3[3];
        path[0] = bike.transform.position; // start
        path[2] = bike.transform.position + Vector3.right * 4.0f - Vector3.up * 3.0f; // end
        path[1] = bike.transform.position + Vector3.right * 1.5f + Vector3.up * 4.0f; // peek


        iTween.MoveTo(bike.gameObject, iTween.Hash(
            "path", path,
            "time", 0.8,
            "easetype", iTween.EaseType.easeInSine,
            "oncomplete", "OnComplete",
            "oncompletetarget", gameObject));

        Vector3[] path2 = new Vector3[3];
        path2[0] = rider.transform.position; // start
        path2[2] = rider.transform.position - Vector3.right * 4.0f - Vector3.up * 3.0f; // end
        path2[1] = rider.transform.position - Vector3.right * 1.5f + Vector3.up * 4.0f; // peek


        iTween.MoveTo(rider.gameObject, iTween.Hash(
            "path", path2,
            "time", 0.8,
            "easetype", iTween.EaseType.easeInSine,
            "oncomplete", "OnComplete",
            "oncompletetarget", gameObject));
    }

    public void OnComplete()
    {
        gameObject.SetActive(false);
    }
}
