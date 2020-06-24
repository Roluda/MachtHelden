using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class FireZone : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    float decayRate;
    [SerializeField]
    GameObject particles;
    Vector3 InitialScale = Vector3.one;
    float _flameHealth = 100;
    float FlameHealth
    {
        get
        {
            return _flameHealth;
        }
        set
        {
            if (value <= 0)
            {
                value = 0;
                Extinguish();
            }
            else
            {
                particles.transform.localScale = InitialScale * (value / 100);
                _flameHealth = value;
            }
        }
    }

    List<PlayerController> helpers = new List<PlayerController>();

    private void OnTriggerStay(Collider other)
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected) {
            if (other.gameObject.layer == 8)
            {
                FlameHealth -= decayRate * Time.deltaTime;
                PlayerController helper = other.gameObject.GetComponent<PlayerController>();
                if (helper != null && !helpers.Contains(helper)) {
                    helpers.Add(helper);
                }
            }
        }
    }

    void Start()
    {
        InitialScale = particles.transform.localScale;
        _flameHealth = 100;
    }

    void Extinguish()
    {
        foreach(PlayerController helper in helpers) {
            helper.powerLevel++;
        }
        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
        {
            QuestManager.Instance.CompleteStage();
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(FlameHealth);
        }
        else
        {
            FlameHealth = (float)stream.ReceiveNext();
        }
    }
}
