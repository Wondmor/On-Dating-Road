using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartStreetSign : MonoBehaviour
{
    public enum EState
    {
        Restart,
        Exit,
        None,
    }

    [SerializeField] GameObject Restart = null;
    [SerializeField] GameObject Exit = null;

    EState eState = EState.None;

    // Start is called before the first frame update
    void Start()
    {
        eState = EState.None;
        Restart.SetActive(eState == EState.Restart);
        Exit.SetActive(eState == EState.Exit);
    }

    public void Select(EState eState)
    {
        this.eState = eState;
        Restart.SetActive(eState == EState.Restart);
        Exit.SetActive(eState == EState.Exit);
    }

    public EState GetState() { return eState; }

    // Update is called once per frame
    void Update()
    {
        
    }
}
