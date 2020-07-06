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

    Color standardColor;
    float timeAlive;

    // Start is called before the first frame update
    void Start()
    {
        averageHeight = transform.localPosition.y;
        standardColor = Random.ColorHSV(0, 1, .5f, .7f);
        render.material.color = standardColor;
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        transform.RotateAround(target.position, Vector3.up, degreePerSecond * Time.deltaTime);
        float altitude = averageHeight + amplitude * Mathf.Sin(timeAlive * frequency *Mathf.PI*2);
        transform.localPosition = new Vector3(transform.localPosition.x, altitude, transform.localPosition.z);
    }

    public void TakeHit() {
        StartCoroutine(BlinkWhite());
    }

    IEnumerator BlinkWhite() {
        render.material.color = Color.white;
        yield return new WaitForSeconds(.1f);
        render.material.color = standardColor;
    }
}
