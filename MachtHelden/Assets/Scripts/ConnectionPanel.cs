using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConnectionPanel : MonoBehaviour
{
    [SerializeField]
    PlayerNameInputField nameField;
    [SerializeField]
    GameObject connectButton;
    [SerializeField]
    TMP_Text infoText;
    static bool firstStart = true;
    // Start is called before the first frame update
    void Start()
    {
        if (firstStart)
        {
            Hide();
        }
        else
        {
            Show();
        }
        infoText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HeroChosen()
    {
        if (firstStart)
        {
            nameField.gameObject.SetActive(true);
        }
    }

    public void NameChosen()
    {
        if (firstStart)
        {
            connectButton.SetActive(true);
            firstStart = false;
        }
    }

    public void Message(string message)
    {
        infoText.text = message;
    }

    public void Show() {
        nameField.gameObject.SetActive(true);
        connectButton.gameObject.SetActive(true);
    }

    public void Hide()
    {
        nameField.gameObject.SetActive(false);
        connectButton.gameObject.SetActive(false);
    }
}
