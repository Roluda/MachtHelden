using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSortByDistance : MonoBehaviour
{
    Renderer render;
    Transform target;
    // Start is called before the first frame update
    void Awake()
    {
        render = GetComponent<Renderer>();
        if (render == null)
        {
            Debug.Log("No REnderer");
            render = GetComponent<ParticleSystemRenderer>();
        }
    }

    void Start()
    {
        target = CameraController.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        render.sortingOrder = -(int)((target.position - transform.position).magnitude*100);
    }
}
