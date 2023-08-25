using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageMaker))]
public class StageMakerEditor : Editor
{

    private string _fileName;
    private Stage _stage;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10f);
        if (false == Application.isPlaying)
            return;

        var stageMaker = serializedObject.targetObject.GetComponent<StageMaker>();
        _GetGateInspector(stageMaker);
        GUILayout.Space(10f);

        EditorGUILayout.BeginHorizontal();
        _fileName = EditorGUILayout.TextField("File Name", _fileName);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(2f);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Make Stage"))
            _MakeStage(stageMaker);
        if (GUILayout.Button("Save Json"))
            _SaveJson(stageMaker);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10f);

        if (null != _stage)
        {
            var json = JsonUtility.ToJson(_stage, true);
            EditorGUILayout.TextArea(json);
        }

    }

    private void _GetGateInspector(StageMaker stageMaker)
    {
        if (null == stageMaker.GateRoot)
            return;

        for (int ii = 0; ii < stageMaker.GatesInfo.Length; ++ii)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField("Gate", stageMaker.GatesInfo[ii].Gate, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            stageMaker.GatesInfo[ii].Multiplier = EditorGUILayout.IntField("Multiplier", stageMaker.GatesInfo[ii].Multiplier);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            stageMaker.GatesInfo[ii].GateType = (EGateTypes)EditorGUILayout.EnumPopup("GateType", stageMaker.GatesInfo[ii].GateType);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(2f);
        }
    }

    private void _MakeStage(StageMaker stageMaker)
    {
        if (null == _stage)
            _stage = new Stage();

        _stage.CastlePosition = stageMaker.Castle.transform.localPosition;
        _stage.CastleRotation = stageMaker.Castle.transform.localEulerAngles;

        _stage.Gates = new Stage.Gate[stageMaker.GatesInfo.Length];
        for (int ii = 0; ii < _stage.Gates.Length; ++ii)
        {
            _stage.Gates[ii] = new Stage.Gate();
            _stage.Gates[ii].Multiplier = stageMaker.GatesInfo[ii].Multiplier;
            _stage.Gates[ii].GateType = stageMaker.GatesInfo[ii].GateType.ToString();
            _stage.Gates[ii].GatePosition = stageMaker.GatesInfo[ii].Gate.transform.localPosition;
            _stage.Gates[ii].GateScale = stageMaker.GatesInfo[ii].Gate.transform.localScale;
        }

        var obstacles = stageMaker.ObstacleRoot.GetComponentsInChildren<Collider>();
        _stage.Obstacles = new Stage.Obstacle[obstacles.Length];
        for (int ii = 0; ii < _stage.Obstacles.Length; ++ii)
        {
            _stage.Obstacles[ii] = new Stage.Obstacle();
            _stage.Obstacles[ii].ObstacleName = obstacles[ii].name;
            _stage.Obstacles[ii].ObstaclePosition = obstacles[ii].transform.localPosition;
            _stage.Obstacles[ii].ObstacleRotation = obstacles[ii].transform.localEulerAngles;
            _stage.Obstacles[ii].ObstacleScale = obstacles[ii].transform.localScale;
        }
    }

    private void _SaveJson(StageMaker stageMaker)
    {
        _MakeStage(stageMaker);
        var json = JsonUtility.ToJson(_stage);

        var directoryPath = Application.dataPath + "/Resources/Stages/";
        var filePath = directoryPath + _fileName + ".json";
        if (File.Exists(filePath))
            File.Delete(filePath);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            var datas = Encoding.UTF8.GetBytes(json);
            fileStream.Write(datas, 0, datas.Length);
            Debug.Log("Create Stage Json");
        }
    }
}
