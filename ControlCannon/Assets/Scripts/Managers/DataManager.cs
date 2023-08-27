using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager
{
    private GameData _gameData;
    private string _saveDataPath;

    public int ClearStageIndex
    {
        get
        {
            return _gameData.ClearStageIndex;
        }
        set
        {
            _gameData.ClearStageIndex = value;
            _SaveGameData();
        }
    }

    private const string SAVE_DATA = "/SaveData.json";

    public void Init()
    {
        _saveDataPath = Application.persistentDataPath + SAVE_DATA;
        if (_LoadGameData())
            return;

        _gameData = new GameData();
        ClearStageIndex = 0;
        _SaveGameData();
    }

    private bool _LoadGameData()
    {
        if (false == File.Exists(_saveDataPath))
            return false;

        var rawData = File.ReadAllText(_saveDataPath);
        _gameData = JsonUtility.FromJson<GameData>(rawData);
        if (null == _gameData)
            return false;

        return true;
    }

    private void _SaveGameData()
    {
        var json = JsonUtility.ToJson(_gameData);
        File.WriteAllText(_saveDataPath, json);
    }
}
