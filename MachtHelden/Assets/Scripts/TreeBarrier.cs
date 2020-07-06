using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TreeBarrier : MonoBehaviour
{
    [SerializeField]
    GameObject[] treeModelPrefabs;
    [SerializeField]
    LayerMask instantiationInferenceCheck;
    BoxCollider col;
    GameObject theModel;
    [SerializeField]
    float InferenceRadius;

    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Spawned Tree");
        Collider[] collisions = Physics.OverlapSphere(transform.localPosition, InferenceRadius, instantiationInferenceCheck);
        if (collisions.Length>1)
        {
            Destroy(gameObject);
        }
        else
        {
            int randomIndex = Random.Range(0, treeModelPrefabs.Length);
            theModel = Instantiate(treeModelPrefabs[randomIndex], transform);
        }
    }

    void OnEnable()
    {

    }
}
