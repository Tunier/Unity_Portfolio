using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public string UIDCODE;
    public string Title;
    public string Desc;
    public int State; //(0:�̼���, 1:������, 2:���ǿϷ�, 3:����Ʈ�Ϸ�)
}

public class QuestManager : MonoSingletone<QuestManager>
{
    public GameObject questPanel;
    public GameObject go_QuestText;

    public Dictionary<string, Quest> QuestDic = new Dictionary<string, Quest>(); // ����ƮUID, ����Ʈ

    Quest quest1 = new Quest();

    public int quest1_Count = 0;

    private void Awake()
    {
        quest1.UIDCODE = "001";
        quest1.Title = "������� ���ϱ�";
        quest1.Desc = "��Ʋ���� 10���� ���.";

        QuestDic.Add(quest1.UIDCODE, quest1);
    }

    private void Start()
    {
        ShowQuestInPanel(QuestDic["001"]);
        QuestDic["001"].State = 1;
    }

    public void ShowQuestInPanel(Quest _quest)
    {
        var obj = Instantiate(go_QuestText);
        obj.name = _quest.Title + " QuestText";

        obj.transform.SetParent(questPanel.transform);

        var questText = obj.GetComponent<QuestTextUI>();

        questText.UIDCODE = _quest.UIDCODE;
        questText.SetTitleText(_quest.Title);
        questText.SetDescText(_quest.Desc);
    }
}
