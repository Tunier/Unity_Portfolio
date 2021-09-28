using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public string UIDCODE;
    public string Title;
    public string Desc;
    public int State; //(0:미수락, 1:수행중, 2:조건완료, 3:퀘스트완료)
}

public class QuestManager : MonoSingletone<QuestManager>
{
    public GameObject questPanel;
    public GameObject go_QuestText;

    public Dictionary<string, Quest> QuestDic = new Dictionary<string, Quest>(); // 퀘스트UID, 퀘스트

    Quest quest1 = new Quest();

    public int quest1_Count = 0;

    private void Awake()
    {
        quest1.UIDCODE = "001";
        quest1.Title = "돼지고기 구하기";
        quest1.Desc = "리틀보어 10마리 잡기.";

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
