
using UnityEngine;
using UnityEngine.InputSystem;

namespace TrashShooting
{
    public class TrashShootingInputAction : MonoBehaviour
    {
        public InputAction left;
        public InputAction up;
        public InputAction right;


        public void Awake()
        {
            // assign a callback for the "jump" action.
            left.performed += ctx => { OnLeft(); };
             up.performed += ctx => { OnUp(); };
            right.performed += ctx => { OnRight(); };
        }

        void OnLeft()
        {
            Debug.LogFormat("LeftPerformed");
        }

        void OnUp()
        {
            Debug.LogFormat("UpPerformed");
        }

        void OnRight()
        {
            Debug.LogFormat("RightPerformed");
        }

        public void OnEnable()
        {
            left.Enable();
            up.Enable();
            right.Enable();
        }

        public void OnDisable()
        {
            left.Disable();
            up.Disable();
            right.Disable();
        }
    }
}