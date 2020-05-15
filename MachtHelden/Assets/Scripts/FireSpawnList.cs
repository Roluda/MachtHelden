using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FireSpawnList : MonoBehaviourPunCallbacks, IPunObservable
{
    public static FireSpawnList Instance;
    [SerializeField]
    float SpawnRadius;
    [SerializeField]
    GameObject FireSpawnPrefab;
    [SerializeField]
    public FireSpawn[] spawnPoints = new FireSpawn[0];

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(spawnPoints);
        }
        else
        {
            spawnPoints = (FireSpawn[])stream.ReceiveNext();
        }
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            DestroyImmediate(this);
        }
        else
        {
            Instance = this;
            //Setup();
        }
    }

    void Setup()
    {
        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected) {
            for (int i = 0; i < 10; i++)
            {
                Vector2 newPosition = Random.insideUnitCircle*SpawnRadius;
                Instantiate(FireSpawnPrefab, new Vector3(newPosition.x, 0, newPosition.y), Quaternion.identity);
            }
        }
    }

    public void AddEntryToList(FireSpawn entry)
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
        {
            Destroy(entry.gameObject);
            return;
        }
        FireSpawn[] newArray = new FireSpawn[spawnPoints.Length+1];
        for(int i=0; i < spawnPoints.Length; i++)
        {
            newArray[i] = spawnPoints[i];
        }
        newArray[spawnPoints.Length] = entry;
        spawnPoints = newArray;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Entries " + spawnPoints.Length);
    }
}
