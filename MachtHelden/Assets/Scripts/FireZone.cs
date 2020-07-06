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
                _flameHealth = 0;
            }
            else
            {
                particles.transform.localScale = InitialScale * (value / 100);
                _flameHealth = value;
            }
        }
    }

    List<PlayerController> helpers = new List<PlayerController>();
    public bool mainHeroNearby;

    private void OnTriggerStay(Collider other)
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected) {
            if (other.gameObject.layer == 8)
            {
                FlameHealth -= decayRate * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        PlayerController helper = other.gameObject.GetComponent<PlayerController>();
        if (helper != null && !helpers.Contains(helper)) {
            helpers.Add(helper);
        }
        if (helper != null && helper.photonView.IsMine || !PhotonNetwork.IsConnected) {
            GlobalLight.Instance.DangerMode();
            mainHeroNearby = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        PlayerController leaver = other.gameObject.GetComponent<PlayerController>();
        if (leaver != null && leaver.photonView.IsMine || !PhotonNetwork.IsConnected) {
            GlobalLight.Instance.NormalMode();
            mainHeroNearby = false;
        }
    }

    public void LowerFire(int value) {
        photonView.RPC("RpcLowerFire", RpcTarget.MasterClient, value);
    }

    [PunRPC]
    void RpcLowerFire(int value) {
        if (FlameHealth > 0) {
            FlameHealth -= value;
            if (FlameHealth <= 0) {
                Extinguish();
            }
        }
    }

    void Awake()
    {
        InitialScale = particles.transform.localScale;
        _flameHealth = 100;
    }

    void Extinguish()
    {
        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
        {
            photonView.RPC("RpcExtinguish", RpcTarget.All);
            QuestManager.Instance.CompleteStage();
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    void RpcExtinguish() {
        if (mainHeroNearby) {
            GlobalLight.Instance.NormalMode();
        }

        foreach (PlayerController helper in helpers) {
            //Future me: maybe check if helper is owned by client. It now ok because power level is
            //secure to increse
            helper.powerLevel++;
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
