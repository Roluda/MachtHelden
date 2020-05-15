using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffLocationController : MonoBehaviour
{
    [SerializeField]
    float movementSpeed = 1;
    [SerializeField]
    Vector2 minMax_X = new Vector2(-10, 10);
    [SerializeField]
    Vector2 minMax_Y = new Vector2(-10, 10);

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        /*
        if (transform.position.x <= minMax_X.x && horizontal < 0) horizontal = 0;
        if (transform.position.x >= minMax_X.y && horizontal > 0) horizontal = 0;
        if (transform.position.z <= minMax_Y.x && vertical < 0) vertical = 0;
        if (transform.position.z >= minMax_Y.y && vertical > 0) vertical = 0;
        */
        transform.Translate(horizontal * Time.deltaTime * movementSpeed, 0, vertical * Time.deltaTime * movementSpeed);
    }
}
