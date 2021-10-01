using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcCanvas : MonoBehaviour
{
    GameObject cameraArm;
    Npc npc;

    [SerializeField]
    Image upperImage;
    [SerializeField]
    Text nameText;

    void Awake()
    {
        cameraArm = GameObject.Find("CameraArm");
        npc = GetComponentInParent<Npc>();

        nameText.text = npc.npcName;
    }

    void Update()
    {
        Vector3 Rot = new Vector3(0, cameraArm.transform.eulerAngles.y, 0);
        transform.eulerAngles = Rot;

        if (npc.questUIDCODE != "")
        {
            if (QuestManager.Instance.QuestDic[npc.questUIDCODE].State == 0)
            {
                upperImage.gameObject.SetActive(true);
                upperImage.sprite = Resources.Load<Sprite>("UI/57");
            }
            else if (QuestManager.Instance.QuestDic[npc.questUIDCODE].State == 2)
            {
                upperImage.gameObject.SetActive(true);
                upperImage.sprite = Resources.Load<Sprite>("UI/64");
            }
        }
        else
        {
            upperImage.gameObject.SetActive(false);
        }
    }
}
