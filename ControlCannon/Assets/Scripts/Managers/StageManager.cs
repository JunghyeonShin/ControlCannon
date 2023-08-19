using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class StageManager
{
    private List<Stage> _stagesList;

    private const string STAGE_STORAGE_PATH = "/Resources/Stages";
    private const string ANY_JSON_FILE = "*.json";
    private const string JSON_FILE = ".json";

    public void Init()
    {
        _stagesList = new List<Stage>();

        var directoryPath = Application.dataPath + STAGE_STORAGE_PATH;
        if (Directory.Exists(directoryPath))
        {
            var directoryInfo = new DirectoryInfo(directoryPath);
            var filesCount = directoryInfo.GetFiles(ANY_JSON_FILE).Length;
            for (int ii = 0; ii < filesCount; ++ii)
            {
                var filePath = new StringBuilder();
                if (ii < 10)
                    filePath.Append(directoryPath + "/Stage_0" + ii + JSON_FILE);
                else
                    filePath.Append(directoryPath + "/Stage_" + ii + JSON_FILE);
                using (var fileStream = new FileStream(filePath.ToString(), FileMode.Open))
                {
                    var data = new byte[fileStream.Length];
                    fileStream.Read(data, 0, data.Length);
                    var json = Encoding.UTF8.GetString(data);
                    var stage = JsonUtility.FromJson<Stage>(json);
                    _stagesList.Add(stage);
                }
            }
        }
    }
}
