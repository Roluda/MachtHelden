using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HeroSelecter : MonoBehaviour
{
    [SerializeField]
    RectTransform[] targetPosition;
    [SerializeField]
    ConnectionPanel connectonPanel;
    [SerializeField]
    RectTransform[] targetPositions;
    [SerializeField]
    Image[] heroes;
    [SerializeField]
    Image heroPrefab;
    [SerializeField]
    float transitionSpeed;
    int Index
    {
        get
        {
            if (PlayerPrefs.HasKey("HeroSelectIndex"))
            {
                return PlayerPrefs.GetInt("HeroSelectIndex");
            }
            else
            {
                PlayerPrefs.SetInt("HeroSelectIndex", 0);
                return Index;
            }
        }
        set
        {
            if (value >= HeroPool.Instance.heroes.Length)
            {
                value = 0;
            }else if (value < 0)
            {
                value = HeroPool.Instance.heroes.Length-1;
            }
            PlayerPrefs.SetInt("HeroSelectIndex", value);
        }
    }

    private Vector3 firstPress;
    private Vector3 lastPress;
    private float dragDistance;
    bool inTransition = false;

    // Start is called before the first frame update
    void Start()
    {
        dragDistance = Screen.height * 15 / 100;
        Setup();
        SetHero();
    }

    // Update is called once per frame
    void Update()
    {
        if (inTransition)
        {
            return;
        }
        GetTouch();
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NextHero();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PreviousHero();
        }
    }

    public void GetTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                firstPress = touch.position;
                lastPress = touch.position;
            }else if(touch.phase == TouchPhase.Moved)
            {
                lastPress = touch.position;
            }else if(touch.phase == TouchPhase.Ended)
            {
                lastPress = touch.position;
                if(Mathf.Abs(lastPress.x-firstPress.x)>dragDistance || Mathf.Abs(lastPress.y - firstPress.y) > dragDistance)
                {
                    if (Mathf.Abs(lastPress.x - firstPress.x) > Mathf.Abs(lastPress.y - firstPress.y))
                    {
                        if (lastPress.x > firstPress.x)
                        {
                            PreviousHero();
                        }
                        else
                        {
                            NextHero();
                        }
                    }
                    else
                    {
                        if (lastPress.y > firstPress.y)
                        {
                            Debug.Log("VerticalSwipe");
                        }
                        else
                        {
                            Debug.Log("VerticalSwipe");
                        }
                    }
                }
            }
        }
    }

    public void Setup()
    {
        heroes[0].sprite = HeroPool.Instance.heroes[GetIndexNeighbour(Index - 1)].pic;
        heroes[1].sprite = HeroPool.Instance.heroes[Index].pic;
        heroes[2].sprite = HeroPool.Instance.heroes[GetIndexNeighbour(Index + 1)].pic;
    }

    public void NextHero()
    {
        connectonPanel.HeroChosen();
        Index++;
        DestroyImmediate(heroes[0].gameObject);
        StartCoroutine(MoveToPosition(heroes[2].rectTransform, targetPositions[1], transitionSpeed));
        StartCoroutine(MoveToPosition(heroes[1].rectTransform, targetPositions[0], transitionSpeed));
        heroes[0] = heroes[1];
        heroes[1] = heroes[2];
        heroes[2] = Instantiate(heroPrefab, targetPositions[2].position, Quaternion.identity, transform);
        heroes[2].sprite = heroes[2].sprite = HeroPool.Instance.heroes[GetIndexNeighbour(Index + 1)].pic;
        SetHero();
    }

    public void PreviousHero()
    {
        connectonPanel.HeroChosen();
        Index--;
        DestroyImmediate(heroes[2].gameObject);
        StartCoroutine(MoveToPosition(heroes[0].rectTransform, targetPositions[1], transitionSpeed));
        StartCoroutine(MoveToPosition(heroes[1].rectTransform, targetPositions[2], transitionSpeed));
        heroes[2] = heroes[1];
        heroes[1] = heroes[0];
        heroes[0] = Instantiate(heroPrefab, targetPositions[0].position, Quaternion.identity, transform);
        heroes[0].sprite= HeroPool.Instance.heroes[GetIndexNeighbour(Index - 1)].pic;
        SetHero();
    }

    IEnumerator MoveToPosition(RectTransform image, RectTransform target, float transTime)
    {
        float time = 0;
        Vector3 Direction = target.position - image.position;
        inTransition = true;
        while (time < transTime)
        {
            time += Time.deltaTime;
            image.Translate((Direction * Time.deltaTime) / transTime);
            yield return null;
        }
        inTransition = false;
    }

    public void SetHero()
    {
        ExitGames.Client.Photon.Hashtable current = PhotonNetwork.LocalPlayer.CustomProperties;
        current["Hero"] = Index;
        PhotonNetwork.LocalPlayer.SetCustomProperties(current);
    }

    int GetIndexNeighbour(int request)
    {
        if (request > Index && request >= HeroPool.Instance.heroes.Length)
        {
            return 0;
        }else if(request <Index&& request < 0)
        {
            return HeroPool.Instance.heroes.Length - 1;
        }else
        {
            return request;
        }
    }
}
