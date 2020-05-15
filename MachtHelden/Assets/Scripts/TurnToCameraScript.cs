using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToCameraScript : MonoBehaviour
{
    Transform _target;
    Transform Target
    {
        get
        {
            if (_target == null)
            {
                if (CameraController.Instance != null) _target = CameraController.Instance.transform;
            }
            return _target;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            return;
        }
        Vector3 relativePos = Target.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(new Vector3(relativePos.x*2,0,relativePos.z*2), Vector3.up);
        transform.rotation = rotation;
    }
}
