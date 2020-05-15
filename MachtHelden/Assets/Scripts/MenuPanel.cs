using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuPanel : MonoBehaviour
{
    [SerializeField]
    GameObject panel;
    bool state = false;

    public void ChangeActive()
    {
        state = !state;
        panel.SetActive(state);
    }
}
