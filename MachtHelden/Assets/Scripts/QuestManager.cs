using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuestManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static QuestManager Instance;

    public delegate void QuestUpdateAction(Quest quest);
    public static event QuestUpdateAction OnQuestUpdated;
    public delegate void QuestCompletedAction(Quest quest);
    public static event QuestCompletedAction OnQuestCompleted;

    [SerializeField]
    Quest[] quests;
    int currentId = 0;
    int currentStage = 0;

    public int CurrentStage
    {
        get
        {
            return currentStage;
        }
        set
        {
            CurrentQuest.stage = value;
            if (value == CurrentQuest.maxStage)
            {
                OnQuestCompleted?.Invoke(CurrentQuest);
                CurrentId++;
                value = 0;
            }
            currentStage = value;
            CurrentQuest.stage = currentStage;
            OnQuestUpdated?.Invoke(CurrentQuest);
        }
    }
    public int CurrentId
    {
        get
        {
            return currentId;
        }
        set
        {
            if (value >= quests.Length)
            {
                value = 0;
            }
            currentId = value;
        }
    }
    public Quest CurrentQuest
    {
        get
        {
            return quests[CurrentId];
        }
        set
        {
            quests[currentId] = value;
        }
    }

    public void CompleteStage()
    {
        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
        {
            CurrentStage++;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
        CurrentId = 0;
        CurrentStage = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CurrentId);
            stream.SendNext(CurrentStage);
        }
        else
        {
            CurrentId = (int)stream.ReceiveNext();
            CurrentStage = (int)stream.ReceiveNext();
        }
    }
}
