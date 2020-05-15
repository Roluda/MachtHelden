using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(SphereCollider))]
public class FireSpawn : MonoBehaviourPunCallbacks, IPunObservable
{
    public static byte eventCode = 199;
    [HideInInspector]
    public Vector2 position;
    [SerializeField]
    LayerMask spawnerLayerMask;
    public bool isNetworked;
    [SerializeField]
    GameObject fireZonePrefab;
    [SerializeField]
    float newFireCooldown;
    [SerializeField]
    float spawnRadius;
    float time;
    GameObject theFlame;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        time = newFireCooldown - 10;
        //FireSpawnList.Instance.AddEntryToList(this);
        if(Physics.OverlapSphere(transform.position,GetComponent<SphereCollider>().radius, spawnerLayerMask).Length > 0)
        {
            DestroyImmediate(gameObject);
        }
        else if (PhotonNetwork.AllocateViewID(photonView) &&!isNetworked)
        {
            object[] data = new object[]
            {
            transform.position, transform.rotation, photonView.ViewID
            };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache
            };

            ExitGames.Client.Photon.SendOptions sendOptions = new ExitGames.Client.Photon.SendOptions
            {
                Reliability = true
            };

            PhotonNetwork.RaiseEvent(eventCode, data, raiseEventOptions, sendOptions);
        }
        else
        {
            Debug.LogError("Failed to allocate a ViewId.");

            Destroy(this);
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            if (theFlame == null)
            {
                time += Time.deltaTime;
            }
            if (time >= newFireCooldown)
            {
                time = 0;
                Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
                theFlame = PhotonNetwork.InstantiateSceneObject(fireZonePrefab.name, transform.position + new Vector3(randomCircle.x, 0, randomCircle.y), Quaternion.identity, 0);
            }
        }
    }

    public static object Deserialize(byte[] data)
    {
        var result = new FireSpawn();
        result.position = new Vector2(data[0],data[1]);
        return result;
    }

    public static byte[] Serialize(object customType)
    {
        var c = (FireSpawn)customType;
        return new byte[] { (byte)c.position.x, (byte)c.position.y};
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
