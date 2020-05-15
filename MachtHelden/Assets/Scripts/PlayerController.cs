using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.Location;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    SpriteRenderer avatarRender;
    [SerializeField]
    TMP_Text flyingName;

    ILocationProvider _locationProvider;
    ILocationProvider LocationProvider
    {
        get
        {
            if (_locationProvider == null)
            {
                _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            }
            return _locationProvider;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            flyingName.text = photonView.Owner.NickName;
            avatarRender.sprite = HeroPool.Instance.heroes[(int)photonView.Owner.CustomProperties["Hero"]].pic;
        }
        else
        {
            flyingName.text = "Daraban";
            avatarRender.sprite = HeroPool.Instance.heroes[4].pic;
        }
        if (photonView.IsMine || !PhotonNetwork.IsConnected) {
            Debug.Log("AttachCamera");
            CameraController.Instance.Target = gameObject;
            //int index = (int)PhotonNetwork.LocalPlayer.CustomProperties["Hero"];
            //photonView.RPC("SetAvatarPicture", RpcTarget.All, index);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.mapInitialized&&(photonView.IsMine || !PhotonNetwork.IsConnected))
        {
            var map = LocationProviderFactory.Instance.mapManager;
            transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
            Debug.Log(transform.localPosition);
        }
    }

    [PunRPC]
    void SetAvatarPicture(int index)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = HeroPool.Instance.heroes[index].pic;
    }
}
