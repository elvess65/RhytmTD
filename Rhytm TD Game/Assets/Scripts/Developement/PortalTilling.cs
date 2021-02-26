using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmTD.Developement
{
    public class PortalTilling : MonoBehaviour
    {
        public float speed = 2;
        public float radius = 1;

        float angle = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            angle += speed * Time.deltaTime;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            GetComponent<MeshRenderer>().material.SetVector("_Offset", new Vector2(x, y));
        }
    }
}
