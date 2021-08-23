using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_StatusUI_Ctrl : MonoBehaviour
{
    PlayerInfo playerInfo;

    [SerializeField]
    Image hpBar;
    [SerializeField]
    Text hpText;

    [SerializeField]
    Image mpBar;
    [SerializeField]
    Text mpText;

    [SerializeField]
    Image expBar;
    [SerializeField]
    Text expText;

    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();

        hpBar.fillAmount = playerInfo.curHp / playerInfo.finalMaxHp;
        mpBar.fillAmount = playerInfo.curMp / playerInfo.finalMaxMp;
        expBar.fillAmount = playerInfo.stats.CurExp / playerInfo.stats.MaxExp;
    }

    void Update()
    {
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, playerInfo.curHp / playerInfo.finalMaxHp, Time.deltaTime * 3.5f);
        hpText.text = Mathf.FloorToInt(playerInfo.curHp) + " / " + Mathf.FloorToInt(playerInfo.finalMaxHp);

        mpBar.fillAmount = Mathf.Lerp(mpBar.fillAmount, playerInfo.curMp / playerInfo.finalMaxMp, Time.deltaTime * 3.5f);
        mpText.text = Mathf.FloorToInt(playerInfo.curMp) + " / " + Mathf.FloorToInt(playerInfo.finalMaxMp);

        expBar.fillAmount = Mathf.Lerp(expBar.fillAmount, playerInfo.stats.CurExp / playerInfo.stats.MaxExp, Time.deltaTime * 3.5f);
        expText.text = Mathf.FloorToInt(playerInfo.stats.CurExp) + " / " + Mathf.FloorToInt(playerInfo.stats.MaxExp);
    }
}
