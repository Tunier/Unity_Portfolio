using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNpc : Npc
{
    DialogUI dialogUI;

    [TextArea]
    public string dialog1;
    [TextArea]
    public string dialog2;
    [TextArea]
    public string dialog3;

    public List<string> dialogs = new List<string>();

    Vector3 firstRot;

    void Awake()
    {
        firstRot = transform.eulerAngles;
        player = GameObject.Find("Player");
        dialogUI = FindObjectOfType<DialogUI>();

        dialogs.Add(dialog1);
        dialogs.Add(dialog2);
        dialogs.Add(dialog3);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= UIManager.Instance.recognitionRange + 3f)
        {
            Vector3 rot = player.transform.position - transform.position;
            rot.y = transform.position.y;
            rot = rot.normalized;
            transform.rotation = Quaternion.LookRotation(rot);

            if (Vector3.Distance(transform.position, player.transform.position) <= UIManager.Instance.recognitionRange && !UIManager.Instance.hotKeyGuid.activeSelf)
            {
                UIManager.Instance.hotKeyGuid.SetActive(true);
                UIManager.Instance.hotKeyGuidTarget = gameObject;
            }
            else if (Vector3.Distance(transform.position, player.transform.position) > UIManager.Instance.recognitionRange && UIManager.Instance.hotKeyGuidTarget == gameObject)
            {
                UIManager.Instance.hotKeyGuid.SetActive(false);
                dialogUI.gameObject.SetActive(false);
                dialogUI.ClearTextList();
            }
        }
        else
        {
            transform.eulerAngles = firstRot;
        }

        if (dialogUI.gameObject.activeSelf)
        {
            UIManager.Instance.hotKeyGuid.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.G))
            if (UIManager.Instance.hotKeyGuidTarget == gameObject && UIManager.Instance.hotKeyGuid.activeSelf)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (dialogs[i] != "")
                        dialogUI.AddDialogText(dialogs[i]);
                }

                dialogUI.questUIDCODE = questUIDCODE;
                dialogUI.SetButtonTextNextBackType();
                dialogUI.gameObject.SetActive(true);
            }
    }
}
