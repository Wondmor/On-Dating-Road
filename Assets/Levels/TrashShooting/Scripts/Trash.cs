using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

namespace TrashShooting
{
    public enum ETrashType
    {
        TrashL = 0,
        TrashU = 1,
        TrashR = 2,
        Trap = 3
    }

    public enum EShootType
    { 
        Perfect,
        Normal,
        Fail,
        Miss,
        TrapPerfect,
        TrapFail,
    }

    public class Trash : MonoBehaviour
    {

        [SerializeField] public Sprite escape;
        [SerializeField] public ETrashType type;
        public float unit = 0.0f;
        public bool Checked = false;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Shoot(EShootType _eShootType, Vector3 _target)
        {
            var pos = transform.position;
            switch (_eShootType)
            {
                case EShootType.Perfect:
                    {
                        Vector3[] path = new Vector3[3];
                        path[0] = pos; // start
                        path[1] = (pos + _target) / 2.0f + new Vector3(0, Mathf.Abs(_target.y - pos.y) * 0.75f); // peek
                        path[2] = _target; // end

                        iTween.MoveTo(gameObject, iTween.Hash(
                            "path", path,
                            "time", 0.5,
                            "easetype", iTween.EaseType.linear,
                            "oncomplete", "OnCompleteMethod",
                            "oncompletetarget", gameObject));
                    }
                    break;
                case EShootType.Normal:
                    {
                        var targetNormal = _target + new Vector3(0.2f, 0.1f, 0.0f);
                        Vector3[] path = new Vector3[4];
                        path[0] = pos; // start
                        path[1] = (pos + targetNormal) / 2.0f + new Vector3(0, Mathf.Abs(targetNormal.y - pos.y) * 0.75f); // peek
                        path[2] = targetNormal; // bounce
                        path[3] = _target; // end

                        iTween.MoveTo(gameObject, iTween.Hash(
                            "path", path,
                            "time", 0.55,
                            "easetype", iTween.EaseType.linear,
                            "oncomplete", "OnCompleteMethod",
                            "oncompletetarget", gameObject));
                    }
                    break;
                case EShootType.Fail:
                case EShootType.TrapFail:
                    {
                        var r = UnityEngine.Random.Range(-10.0f, 10.0f);
                        var targetFail = _target + new Vector3(r, 10.0f - r, 0.0f);
                        Vector3[] path = new Vector3[3];
                        path[0] = pos; // start
                        path[1] = (pos + targetFail) / 2.0f + new Vector3(0, Mathf.Abs(targetFail.y - pos.y) * 0.75f); // peek
                        path[2] = targetFail; // hit

                        iTween.MoveTo(gameObject, iTween.Hash(
                            "path", path,
                            "time", 0.45,
                            "easetype", iTween.EaseType.linear,
                            "oncomplete", "OnCompleteMethod",
                            "oncompletetarget", gameObject));
                    }
                    break;
            }
        }

        public EShootType GetInputRes(bool bLeft, bool bUp, bool bRight, bool bPerfect, ref int target)
        { 
            switch(type)
            {
                case ETrashType.TrashL:
                    if (bLeft && !bUp && !bRight)
                    {
                        target = (int)ETrashType.TrashL;
                        return bPerfect ? EShootType.Perfect : EShootType.Normal;
                    }
                    else if (!bLeft && !bUp && !bRight)
                    {
                        target = -1;
                        return EShootType.Miss;
                    }
                    else 
                    {
                        target = (bUp && bRight) ? UnityEngine.Random.Range(1, 3) : (bUp ? 1 : 2);
                        return EShootType.Fail;
                    }

                case ETrashType.TrashU:
                    if (!bLeft && bUp && !bRight)
                    {
                        target = (int)ETrashType.TrashU;
                        return bPerfect ? EShootType.Perfect : EShootType.Normal;
                    }
                    else if (!bLeft && !bUp && !bRight)
                    {
                        target = -1;
                        return EShootType.Miss;
                    }
                    else
                    {
                        target = (bLeft && bRight) ? UnityEngine.Random.Range(0, 2) * 2 : (bLeft ? 0 : 2);
                        return EShootType.Fail;
                    }

                case ETrashType.TrashR:
                    if (!bLeft && !bUp && bRight)
                    {
                        target = (int)ETrashType.TrashR;
                        return bPerfect ? EShootType.Perfect : EShootType.Normal;
                    }
                    else if (!bLeft && !bUp && !bRight)
                    {
                        target = -1;
                        return EShootType.Miss;
                    }
                    else
                    {
                        target = bLeft && bUp ? UnityEngine.Random.Range(0, 2) : (bLeft ? 0 : 1);
                        return EShootType.Fail;
                    }                
                default://case ETrashType.Trap:
                    {
                        if (!bLeft && !bUp && !bRight)
                        {
                            target = -1;
                            return EShootType.TrapPerfect;
                        }
                        else
                        {
                            List<int> numPool = new List<int>();
                            if (bLeft) numPool.Add(0);
                            if (bUp) numPool.Add(1);
                            if (bRight) numPool.Add(2);
                            target = numPool[UnityEngine.Random.Range(0, numPool.Count)];
                            return EShootType.TrapFail;
                        }
                    }
                    
            }
        }

        public void OnEscape()
        {
            GetComponent<SpriteRenderer>().sprite = escape;
        }

        public void OnCompleteMethod()
        {
            Destroy(gameObject);
        }
    }
}
