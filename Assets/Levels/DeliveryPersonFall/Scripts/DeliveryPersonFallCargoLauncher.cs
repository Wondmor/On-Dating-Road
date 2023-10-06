using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.OdinInspector;
using DG.Tweening;

[Serializable]
public class CargoRigidPair
{
    public string name;
    public Rigidbody2D leftCargoRigid;
    public Rigidbody2D rightCargoRigid;

    [HideInInspector]
    public Vector3 leftCargoRigidOrigPos;
    [HideInInspector]
    public Vector3 rightCargoRigidOrigPos;
    [HideInInspector]
    public Vector3 leftCargoRigidOrigScale;
    [HideInInspector]
    public Vector3 rightCargoRigidOrigScale;
}

public class DeliveryPersonFallCargoLauncher : MonoBehaviour
{
    public float cargoScaleTarget;
    public Vector2 leftCargoMinLaunchForce;
    public Vector2 leftCargoMaxLaunchForce;
    public Vector2 rightCargoMinLaunchForce;
    public Vector2 rightCargoMaxLaunchForce;
    public List<CargoRigidPair> cargoGOPairList;
    public AudioSource launchAudioSource;

    private void Awake()
    {
        foreach (CargoRigidPair cargoRigidPair in cargoGOPairList)
        {
            cargoRigidPair.leftCargoRigidOrigPos = cargoRigidPair.leftCargoRigid.transform.position;
            cargoRigidPair.rightCargoRigidOrigPos = cargoRigidPair.rightCargoRigid.transform.position;
            cargoRigidPair.leftCargoRigidOrigScale = cargoRigidPair.leftCargoRigid.transform.localScale;
            cargoRigidPair.rightCargoRigidOrigScale = cargoRigidPair.rightCargoRigid.transform.localScale;
        }
        Reset();
    }

    [Button]
    [HideInEditorMode]
    public void Reset()
    {
        foreach (CargoRigidPair cargoRigidPair in cargoGOPairList)
        {
            cargoRigidPair.leftCargoRigid.gameObject.SetActive(false);
            cargoRigidPair.leftCargoRigid.bodyType = RigidbodyType2D.Kinematic;
            cargoRigidPair.leftCargoRigid.transform.position = cargoRigidPair.leftCargoRigidOrigPos;
            cargoRigidPair.leftCargoRigid.transform.localScale = cargoRigidPair.leftCargoRigidOrigScale;
            var leftjoint = cargoRigidPair.leftCargoRigid.GetComponent<FixedJoint2D>();
            leftjoint.enabled = false;
            leftjoint.connectedBody = null;
            var leftColli = cargoRigidPair.leftCargoRigid.GetComponent<Collider2D>();
            leftColli.enabled = true;
            cargoRigidPair.rightCargoRigid.gameObject.SetActive(false);
            cargoRigidPair.rightCargoRigid.bodyType = RigidbodyType2D.Kinematic;
            cargoRigidPair.rightCargoRigid.transform.position = cargoRigidPair.rightCargoRigidOrigPos;
            cargoRigidPair.rightCargoRigid.transform.localScale = cargoRigidPair.rightCargoRigidOrigScale;
            var rightJoint = cargoRigidPair.rightCargoRigid.GetComponent<FixedJoint2D>();
            rightJoint.enabled = false;
            rightJoint.connectedBody = null;
            var rightColli = cargoRigidPair.rightCargoRigid.GetComponent<Collider2D>();
            rightColli.enabled = true;

        }
    }

    public void LaunchCargo(string name)
    {
        launchAudioSource.Play();
        var targetCargo = cargoGOPairList.FirstOrDefault(pair => pair.name == name);
        if(targetCargo == null)
        {
            Debug.LogError(Equals($"No cargo with name {name} found"));
            return;
        }
        targetCargo.leftCargoRigid.gameObject.SetActive(true);
        targetCargo.leftCargoRigid.bodyType = RigidbodyType2D.Dynamic;
        targetCargo.rightCargoRigid.gameObject.SetActive(true);
        targetCargo.rightCargoRigid.bodyType = RigidbodyType2D.Dynamic;
        targetCargo.leftCargoRigid.AddForce(
            new Vector2(
                UnityEngine.Random.Range(leftCargoMinLaunchForce.x, leftCargoMaxLaunchForce.x), 
                UnityEngine.Random.Range(leftCargoMinLaunchForce.y, leftCargoMaxLaunchForce.y)
                ),
            ForceMode2D.Impulse);
        targetCargo.rightCargoRigid.AddForce(
            new Vector2(
                UnityEngine.Random.Range(rightCargoMinLaunchForce.x, rightCargoMaxLaunchForce.x),
                UnityEngine.Random.Range(rightCargoMinLaunchForce.y, rightCargoMaxLaunchForce.y)
                ),
            ForceMode2D.Impulse);
        Sequence seq = DOTween.Sequence();
        targetCargo.leftCargoRigid.GetComponent<Collider2D>().enabled = false;
        targetCargo.rightCargoRigid.GetComponent<Collider2D>().enabled = false;
        seq.Append(targetCargo.leftCargoRigid.transform.DOScale(targetCargo.leftCargoRigidOrigScale * cargoScaleTarget, 1f));
        seq.Join(targetCargo.rightCargoRigid.transform.DOScale(targetCargo.rightCargoRigidOrigScale * cargoScaleTarget, 1f));
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            targetCargo.leftCargoRigid.GetComponent<Collider2D>().enabled = true;
            targetCargo.rightCargoRigid.GetComponent<Collider2D>().enabled = true;
        });
    }

    [Button]
    [HideInEditorMode]
    void LaunchTest()
    {
        Reset();
        LaunchCargo("дл╡Х");
    }
}
