using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestText : MonoBehaviour
{
    [SerializeField]
    TMP_Text questText;
    [SerializeField]
    float typeInterval;

    // Start is called before the first frame update
    void Start()
    {
        QuestManager.OnQuestUpdated += UpdateText;
    }

    public void UpdateText(Quest currentQuest)
    {
        string newText = currentQuest.objective + ": " + currentQuest.stage + " von " + currentQuest.maxStage;
        StopAllCoroutines();
        StartCoroutine(TypeText(newText, typeInterval));
    }
    IEnumerator TypeText(string text, float typeTime)
    {
        questText.text = "";
        yield return new WaitForSeconds(3);
        for (int i = 0; i < text.Length; i++)
        {
            questText.text = questText.text + text[i];
            yield return new WaitForSeconds(typeTime);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        QuestManager.OnQuestUpdated -= UpdateText;
    }
}
