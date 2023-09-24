using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TrashShooting
{
    public class TempoBox : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetIntensity(float _Intensity)
        {
            var col = gameObject.GetComponent<SpriteRenderer>().color;
            col.a = _Intensity;
            gameObject.GetComponent<SpriteRenderer>().color = col;
        }
    }
}