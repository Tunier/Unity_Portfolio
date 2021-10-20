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

        if (playerStatFoldout = EditorGUILayout.Foldout(playerStatFoldout, "�÷��̾� ����"))
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.enabled = false;
            EditorGUILayout.FloatField("�ִ� Hp", playerInfo.finalMaxHp);
            GUI.enabled = true;
            playerInfo.curHp = EditorGUILayout.FloatField("���� Hp", playerInfo.curHp);
            GUI.enabled = false;
            EditorGUILayout.FloatField("Hp ����/s", playerInfo.finalHpRegen);
            EditorGUILayout.FloatField("�ִ� Mp", playerInfo.finalMaxHp);
            GUI.enabled = true;
            playerInfo.curMp = EditorGUILayout.FloatField("���� Mp", playerInfo.curMp);
            GUI.enabled = false;
            EditorGUILayout.FloatField("Mp ����/s", playerInfo.finalMpRegen);
            EditorGUILayout.FloatField("���� ���ݷ�", playerInfo.finalNormalAtk);
            EditorGUILayout.FloatField("���� ���ݷ�", playerInfo.finalMagicAtk);
            EditorGUILayout.FloatField("���� ����", playerInfo.finalNormalDef);
            EditorGUILayout.FloatField("���� ����", playerInfo.finalMagicDef);
            EditorGUILayout.FloatField("��", playerInfo.finalStr);
            EditorGUILayout.FloatField("����", playerInfo.finalInt);
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
