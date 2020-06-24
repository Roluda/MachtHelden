using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetDragon : MonoBehaviour
{
    [SerializeField]
    float averageHeight;
    [SerializeField]
    Renderer render;
    [SerializeField]
    public float amplitude;
    [SerializeField]
    public float frequency;
    [SerializeField]
    public float degreePerSecond;
    public Transform target;


    float timeAlive;

    // Start is called before the first frame update
    void Start()
    {
        averageHeight = transform.localPosition.y;
        render.material.color  = Random.ColorHSV(0, 1, .5f, .7f);
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        transform.RotateAround(target.position, Vector3.up, degreePerSecond * Time.deltaTime);
        float altitude = averageHeight + amplitude * Mathf.Sin(timeAlive * frequency *Mathf.PI*2);
        transform.localPosition = new Vector3(transform.localPosition.x, altitude, transform.localPosition.z);
    }
}
