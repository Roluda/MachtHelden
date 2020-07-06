using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.Location;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable {
    [SerializeField]
    SpriteRenderer avatarRender;
    [SerializeField]
    TMP_Text flyingName;

    public int powerLevel {
        get {
            if (PhotonNetwork.IsConnected) {
                return (int)photonView.Owner.CustomProperties["PowerLevel"];
            } else {
                return PlayerPrefs.GetInt("PowerLevel");
            }
        }
        set {
            if (PhotonNetwork.IsConnected) {
                if (photonView.IsMine) {
                    PlayerPrefs.SetInt("PowerLevel", value);
                    ExitGames.Client.Photon.Hashtable current = photonView.Owner.CustomProperties;
                    current["PowerLevel"] = value;
                    photonView.Owner.SetCustomProperties(current);
                }
            } else {
                PlayerPrefs.SetInt("PowerLevel", value);
            }
        }
    }

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
        if (!PlayerPrefs.HasKey("PowerLevel")) {
            PlayerPrefs.SetInt("PowerLevel", 0);
        }
        powerLevel = PlayerPrefs.GetInt("PowerLevel");
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
            transform.localPosition = Vector3.Lerp(transform.localPosition, map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude), Time.deltaTime*5);
            Shader.SetGlobalVector("_PlayerPosition", transform.position);
        }
    }

    [PunRPC]
    void SetAvatarPicture(int index)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = HeroPool.Instance.heroes[index].pic;
    }
}
