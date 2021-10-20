using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerInfo))]
public class PlayerInfoEditor : Editor
{
    PlayerInfo playerInfo;

    bool playerStatFoldout = true;

    void OnEnable()
    {
        playerInfo = target as PlayerInfo;

        if (EditorPrefs.HasKey("playerStatFoldout"))
            playerStatFoldout = EditorPrefs.GetBool("playerStatFoldout");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        EditorGUILayout.Space();

        if (playerStatFoldout = EditorGUILayout.Foldout(playerStatFoldout, "플레이어 정보"))
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.enabled = false;
            EditorGUILayout.FloatField("최대 Hp", playerInfo.finalMaxHp);
            GUI.enabled = true;
            playerInfo.curHp = EditorGUILayout.FloatField("현재 Hp", playerInfo.curHp);
            GUI.enabled = false;
            EditorGUILayout.FloatField("Hp 리젠/s", playerInfo.finalHpRegen);
            EditorGUILayout.FloatField("최대 Mp", playerInfo.finalMaxHp);
            GUI.enabled = true;
            playerInfo.curMp = EditorGUILayout.FloatField("현재 Mp", playerInfo.curMp);
            GUI.enabled = false;
            EditorGUILayout.FloatField("Mp 리젠/s", playerInfo.finalMpRegen);
            EditorGUILayout.FloatField("물리 공격력", playerInfo.finalNormalAtk);
            EditorGUILayout.FloatField("마법 공격력", playerInfo.finalMagicAtk);
            EditorGUILayout.FloatField("물리 방어력", playerInfo.finalNormalDef);
            EditorGUILayout.FloatField("마법 방어력", playerInfo.finalMagicDef);
            EditorGUILayout.FloatField("힘", playerInfo.finalStr);
            EditorGUILayout.FloatField("지능", playerInfo.finalInt);
            GUI.enabled = true;
            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }

    void OnDisable()
    {
        EditorPrefs.SetBool("playerStatFoldout", playerStatFoldout);
    }
}
