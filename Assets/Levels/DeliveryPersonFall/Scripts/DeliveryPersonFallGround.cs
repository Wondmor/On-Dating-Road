using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeliveryPersonFallGround : MonoBehaviour
{
    Rigidbody2D rigid;
    Collider2D colli;

    List<GameObject> cargoGOList = new();

    public IObservable<int> CatchCargo => catchCargo;

    Subject<int> catchCargo = new();

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        colli = GetComponent<Collider2D>();
        colli.OnCollisionEnter2DAsObservable()
            .Where(c =>
            {
                GameObject other = c.gameObject;
                if (cargoGOList.Contains(other))
                    return false;
                BoxCollider2D boxCollider2D = other.GetComponent<BoxCollider2D>();
                string otherLayerName = LayerMask.LayerToName(other.layer);
                bool isDeliveryCargo = otherLayerName.StartsWith("DeliveryPerson");
                return isDeliveryCargo;
            })
            .Subscribe(c =>
            {
                GameObject other = c.gameObject;
                Rigidbody2D otherRigid = other.GetComponent<Rigidbody2D>();
                FixedJoint2D fixedJoint2D = other.GetComponent<FixedJoint2D>();
                fixedJoint2D.connectedBody = rigid;
                fixedJoint2D.enabled = true;
                catchCargo.OnNext(cargoGOList.Count);
            });
    }

    public void Reset()
    {
        cargoGOList.Clear();
    }
}
