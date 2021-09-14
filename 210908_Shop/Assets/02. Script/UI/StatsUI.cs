using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    PlayerInfo player;

    public Text HpText;
    public Text MpText;
    public Text HpRegenText;
    public Text MpRegenText;
    public Text StrText;
    public Text IntText;
    public Text NormalAtkText;
    public Text MagicAtkText;
    public Text NormalDefText;
    public Text MagicDefText;
    public Text CritChanceText;
    public Text CritDamageText;
    public Text AtkSpeedText;
    public Text CastSpeedText;
    public Text MoveSpeedText;
    public Text AtkHpRecoverText;
    public Text AtkLifeStealText;

    void Start()
    {
        player = FindObjectOfType<PlayerInfo>();
    }

    void Update()
    {
        RefreshStatsUIText();
    }

    public void RefreshStatsUIText()
    {
        HpText.text = player.curHp + " / " + player.finalMaxHp;
        MpText.text = player.curMp + " / " + player.finalMaxMp;
        HpRegenText.text = player.finalHpRegen + "";
        MpRegenText.text = player.finalMpRegen + "";
        StrText.text = player.finalStr + "";
        IntText.text = player.finalInt + "";
        NormalAtkText.text = player.finalNormalAtk + "";
        NormalDefText.text = player.finalNormalDef + "";
        MagicAtkText.text = player.finalMagicAtk + "";
        MagicDefText.text = player.finalMagicDef + "";
        CritChanceText.text = player.finalCriticalChance * 0.01f + " %";
        CritDamageText.text = player.finalCriticalDamageMuliplie * 100 + " %";
        //AtkSpeedText.text = player.;
        //CastSpeedText;
        //MoveSpeedText;
        AtkHpRecoverText.text = player.finalLifeSteal + "";
        AtkLifeStealText.text = player.finalLifeStealPercent + "";
    }
}