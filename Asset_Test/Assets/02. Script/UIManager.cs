using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingletone<UIManager>
{
    public GameObject go_DamageText;

    public PlayerInfo player;

    public Canvas BackCanvas;

    public void ShowDamageText(float _Damage, bool _critical = false)
    {
        var go_damageText = Instantiate(go_DamageText, BackCanvas.transform);

        DamageTextUI DamageText = go_damageText.GetComponent<DamageTextUI>();

        DamageText.mob = player.targetMonster;

        DamageText.SetDamageText(_Damage);

        if (_critical)
            DamageText.SetTextColor(Color.red);
    }
}
