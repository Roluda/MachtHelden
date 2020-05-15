using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    GameObject _target = null;
    Camera cam;
    public GameObject Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
            Debug.Log("target set: " + _target.transform.position);
            Invoke("Initialize", 3f);
        }
    }
    [SerializeField]
    Vector3 offSet;
    [SerializeField]
    float smoothSpeed = 50;
    [SerializeField]
    float smoothTime = 0.5f;
    [SerializeField]
    Vector2 minMaxZoom;
    [SerializeField]
    float zoomSpeed;
    [SerializeField]
    float dragSpeed;
    [SerializeField]
    float dragDistance;
    float idleTime;
    float Zoom
    {
        get
        {
            return cam.fieldOfView;
        }
        set
        {
            if (value < minMaxZoom.x)
            {
                value = minMaxZoom.x;
            }else if(value > minMaxZoom.y)
            {
                value = minMaxZoom.y;
            }
            cam.fieldOfView = value;
        }
    }



    private Vector3 currentVelocity;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cam.enabled = false;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            return;
        }
        if (Input.touchCount == 2)
        {
            idleTime = 0;
            ZoomCamera();
        }else if(Input.touchCount==1)
        {
            idleTime = 0;
            DragCamera();
        }
        else
        {
            idleTime += Time.deltaTime;
            if (idleTime >= 3)
            {
                FocusOnTarget();
            }
        }
    }

    void ZoomCamera()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
        float previousTouchDeltaMag = (touchZeroPrevPos-touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
        float deltaMagnitudeDiff = previousTouchDeltaMag - touchDeltaMag;
        Zoom += deltaMagnitudeDiff * zoomSpeed;
    }

    void DragCamera()
    {
        Touch touch = Input.GetTouch(0);
        Vector2 previousPos = touch.position - touch.deltaPosition;
        Vector3 translation = new Vector3(-touch.deltaPosition.x / Screen.width, 0, -touch.deltaPosition.y / Screen.width);
        float curDistance = (transform.position - Target.gameObject.transform.position).magnitude;
        float nextDistace = (transform.position - Target.gameObject.transform.position + translation).magnitude;
        if (curDistance < nextDistace)
        {
            transform.Translate(translation * Time.deltaTime * dragSpeed * -((curDistance - dragDistance) / dragDistance), Space.World);
        }
        else
        {
            transform.Translate(translation * Time.deltaTime * dragSpeed, Space.World);
        }
    }

    void FocusOnTarget()
    {
        if (Target == null)
        {
            return;
        }
        float distance = (transform.position - (Target.transform.position + offSet)).magnitude;
        if (distance >= 0.05f)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Target.transform.position + offSet, ref currentVelocity, smoothTime, smoothSpeed);
        }
    }

    void Initialize()
    {
        idleTime = 3;
        cam.enabled = true;
        transform.position = Target.transform.position + offSet;
    }
}
