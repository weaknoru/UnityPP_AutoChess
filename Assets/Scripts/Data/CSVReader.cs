using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    [Header("캐릭터 데이터"), SerializeField] TextAsset _charDataFile;

    //List<CChar> _charList = new List<CChar>();
    CharData _charData = new CharData();
    public CharData _CharData => _charData;
    private void Awake()
    {
        ReadCSV(_charDataFile, _charData);
    }
    void ReadCSV(TextAsset dataFile, CharData converter)
    {
        string[] lines = System.Text.Encoding.UTF8.GetString(dataFile.bytes).Split('\n');
        List<string> rows = new List<string>(lines);
        converter.ConvertToDictionary(rows);
        //_charList = _charData.GetData();
    }
}
