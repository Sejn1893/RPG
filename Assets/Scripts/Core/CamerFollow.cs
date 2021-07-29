using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CamerFollow : MonoBehaviour
    {
        public Transform Target;
        public float CamSpeed;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 TargetPos = Target.position;
            transform.position = Vector3.Lerp(transform.position, Target.position, CamSpeed * Time.deltaTime);
        }
    }
}
