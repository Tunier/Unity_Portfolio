using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTextUI : MonoBehaviour
{
    public string UIDCODE;
    public Text titleText;
    public Text descText;
    public Text stateText;

    public void SetTitleText(string _text)
    {
        titleText.text = _text;
    }

    public void SetDescText(string _text)
    {
        descText.text = _text;
    }

    private void Update()
    {
        switch (UIDCODE)
        {
            case "001":
                stateText.text = "��Ʋ���� " + QuestManager.Instance.quest1_Count + " / 10";
                break;
            default:
                stateText.text = "";
                break;
        }
    }
}
