using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;
using System;
using Sirenix.OdinInspector;

public class DeliveryPersonFallHand : MonoBehaviour
{
    public float speed = 10;
    public InputAction moveAction;

    [HideInInspector]
    public Camera cam;

    ReactiveProperty<bool> handFull = new(false);
    Subject<GameObject> catchCargo = new();
    bool handMovable = false;

    Rigidbody2D rigid;
    Collider2D trigger;

    public IObservable<GameObject> CatchCargo => catchCargo;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trigger = GetComponent<Collider2D>();
        trigger.OnCollisionEnter2DAsObservable()
            .Where(c =>
            {
                if (handFull.Value)
                    return false;
                GameObject other = c.gameObject;
                BoxCollider2D boxCollider2D = other.GetComponent<BoxCollider2D>();
                string otherLayerName = LayerMask.LayerToName(other.layer);
                bool isDeliveryCargo = otherLayerName.StartsWith("DeliveryPerson");
                Transform otherTrans = other.transform;
                Vector3 otherPos = otherTrans.position;
                Vector3 handPos = transform.position;
                bool isYAbove = otherPos.y > handPos.y + boxCollider2D.size.y * otherTrans.lossyScale.y / 2;
                bool isXAbove = otherPos.x < handPos.x + boxCollider2D.size.x * otherTrans.lossyScale.x / 2 && otherPos.x > handPos.x - boxCollider2D.size.x * otherTrans.lossyScale.x / 2;
                return isDeliveryCargo && isYAbove && isXAbove;
            })
            .Subscribe(c =>
            {
                GameObject other = c.gameObject;
                Rigidbody2D otherRigid = other.GetComponent<Rigidbody2D>();
                FixedJoint2D fixedJoint2D = other.GetComponent<FixedJoint2D>();
                fixedJoint2D.connectedBody = rigid;
                fixedJoint2D.enabled = true;
                handFull.Value = true;
                catchCargo.OnNext(other);
            });
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (handMovable || cam == null)
            return;
        float moveAmount = moveAction.ReadValue<float>();
        float posX = transform.position.x;
            posX = Mathf.Clamp(posX + moveAmount * speed * Time.deltaTime, cam.ViewportToWorldPoint(new Vector3(0f, 0, 0)).x, cam.ViewportToWorldPoint(new Vector3(1f, 0, 0)).x);
        rigid.MovePosition(new Vector3(posX, transform.position.y, transform.position.z));
    }

    public void Reset()
    {
        handFull.Value = false;
    }

    public void SetMovable(bool movable)
    {
        handMovable = movable;
    }

    [Button, HideInEditorMode]
    public void SwitchMovable()
    {
        handMovable = !handMovable;
    }
}
