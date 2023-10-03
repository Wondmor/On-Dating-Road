using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.OdinInspector;

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
}

public class DeliveryPersonFallCargoLauncher : MonoBehaviour
{
    public Vector2 leftCargoMinLaunchForce;
    public Vector2 leftCargoMaxLaunchForce;
    public Vector2 rightCargoMinLaunchForce;
    public Vector2 rightCargoMaxLaunchForce;
    public List<CargoRigidPair> cargoGOPairList;

    private void Awake()
    {
        foreach (CargoRigidPair cargoRigidPair in cargoGOPairList)
        {
            cargoRigidPair.leftCargoRigidOrigPos = cargoRigidPair.leftCargoRigid.transform.position;
            cargoRigidPair.rightCargoRigidOrigPos = cargoRigidPair.rightCargoRigid.transform.position;
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
            var leftjoint = cargoRigidPair.leftCargoRigid.GetComponent<FixedJoint2D>();
            leftjoint.enabled = false;
            leftjoint.connectedBody = null;
            cargoRigidPair.rightCargoRigid.gameObject.SetActive(false);
            cargoRigidPair.rightCargoRigid.bodyType = RigidbodyType2D.Kinematic;
            cargoRigidPair.rightCargoRigid.transform.position = cargoRigidPair.rightCargoRigidOrigPos;
            var rightJoint = cargoRigidPair.rightCargoRigid.GetComponent<FixedJoint2D>();
            rightJoint.enabled = false;
            rightJoint.connectedBody = null;
        }
    }

    public void LaunchCargo(string name)
    {
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
    }

    [Button]
    [HideInEditorMode]
    void LaunchTest()
    {
        Reset();
        LaunchCargo("дл╡Х");
    }
}
