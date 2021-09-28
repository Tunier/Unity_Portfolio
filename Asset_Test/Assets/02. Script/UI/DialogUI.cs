using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public enum DialogButton
    {
        Left,
        Right,
    }

    public enum ButtonType
    {
        NextBack,
        AcceptRefuse,
        QuestClear,
        conversation,
    }

    public Text leftButtonText;
    public Text rightButtonText;

    public List<string> dialogTextList = new List<string>();
    public Text dialogText;

    public string questUIDCODE; // ��ũ��Ʈ �ɰ� ���� �ۼ�.

    int index = 0;
    public ButtonType buttonType = ButtonType.NextBack;

    PlayerInfo player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerInfo>();
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        index = 0;
    }

    private void Update()
    {
        if (gameObject.activeSelf && questUIDCODE != "")
        {
            if (QuestManager.Instance.QuestDic[questUIDCODE].State != 2)
            {
                if (dialogTextList.Count != 0)
                    ShowDialogText(index);

                if (index == dialogTextList.Count - 1)
                {
                    SetButtonTextAcceptRefuseType();
                }
            }
            else
            {
                dialogText.text = "����Ʈ�� �Ϸ��߱���!";
                switch (questUIDCODE)
                {
                    case "001":
                        dialogText.text += " ������ ��� 500, ����ġ 250�̾�";
                        break;
                }
                SetButtonTextQuestClearType();
            }
        }
    }

    public void SetButtonTextAcceptRefuseType()
    {
        SetButtonText(DialogButton.Left, "�����ϱ�");
        SetButtonText(DialogButton.Right, "�����ϱ�");
        buttonType = ButtonType.AcceptRefuse;
    }

    public void SetButtonTextNextBackType()
    {
        SetButtonText(DialogButton.Left, "��������");
        SetButtonText(DialogButton.Right, "��������");
        buttonType = ButtonType.NextBack;
    }

    public void SetButtonTextCoversationType()
    {
        SetButtonText(DialogButton.Left, "��");
        SetButtonText(DialogButton.Right, "����");
        buttonType = ButtonType.conversation;
    }

    public void SetButtonTextQuestClearType()
    {
        SetButtonText(DialogButton.Left, "����ޱ�");
        SetButtonText(DialogButton.Right, "��ȭ����");
        buttonType = ButtonType.QuestClear;
    }

    void SetButtonText(DialogButton _button, string _text)
    {
        switch (_button)
        {
            case DialogButton.Left:
                leftButtonText.text = _text;
                break;
            case DialogButton.Right:
                rightButtonText.text = _text;
                break;
            default:
                break;
        }
    }

    public void AddDialogText(string _text)
    {
        dialogTextList.Add(_text);
    }

    void ShowDialogText(int _index)
    {
        dialogText.text = dialogTextList[_index];
    }

    public void ClearTextList()
    {
        dialogTextList.Clear();
    }

    public void LeftButtonClick()
    {
        switch (buttonType)
        {
            case ButtonType.NextBack:
                if (index > 0)
                    index--;
                break;
            case ButtonType.AcceptRefuse:
                if (QuestManager.Instance.QuestDic[questUIDCODE].State == 0)
                {
                    QuestManager.Instance.GetQuest(questUIDCODE);
                    gameObject.SetActive(false);
                }
                else
                {
                    SetButtonTextCoversationType();
                    dialogText.text = "���̹� ����Ʈ�� �������̰ų� �Ϸ��߾�.";
                }
                questUIDCODE = "";
                ClearTextList();
                break;
            case ButtonType.conversation:
                gameObject.SetActive(false);
                questUIDCODE = "";
                ClearTextList();
                break;
            case ButtonType.QuestClear:
                QuestManager.Instance.QuestDic[questUIDCODE].State = 3;
                switch (questUIDCODE)
                {
                    case "001":
                        player.GetExp(250);
                        player.GetGold(500);
                        break;
                }
                gameObject.SetActive(false);
                questUIDCODE = "";
                ClearTextList();
                break;
        }
    }

    public void RightButtonClick()
    {
        switch (buttonType)
        {
            case ButtonType.NextBack:
                if (index < dialogTextList.Count - 1)
                    index++;
                break;
            case ButtonType.AcceptRefuse:
                gameObject.SetActive(false);
                questUIDCODE = "";
                ClearTextList();
                break;
            case ButtonType.conversation:
                gameObject.SetActive(false);
                questUIDCODE = "";
                ClearTextList();
                break;
            case ButtonType.QuestClear:
                gameObject.SetActive(false);
                questUIDCODE = "";
                ClearTextList();
                break;
        }
    }
}