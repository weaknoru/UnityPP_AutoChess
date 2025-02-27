using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCHAR_COLUMN
{
    ID,
    Char_Name,
    Char_Type,
    Char_Cost,
    Pref_Name,
    Level,
    MaxHp,
    Att,
    AttSpeed,
    MaxMp,
    StartMp,
    Def,
    MR,
    Range, 
    Synergy_0, 
    Synergy_1, 
    Synergy_2
}
public interface ICSVDataConverter<T>
{
    void ConvertToDictionary(List<string> rows);
    List<T> GetData();
}
public class CharData
{
    Dictionary<eCHARTYPE,CChar> _charDatas = new Dictionary<eCHARTYPE, CChar>();
    public Dictionary<eCHARTYPE, CChar> _CharDatas => _charDatas;
    public void ConvertToDictionary(List<string> rows)
    {
        rows.RemoveAt(0); // 첫 줄(컬럼 헤더) 제거
        rows.RemoveAt(rows.Count - 1);  // 마지막 공백 행 제거
        Dictionary<eCHARTYPE, CChar> charDictionary = new Dictionary<eCHARTYPE, CChar>();
        foreach (string row in rows)
        {
            string[] values = row.Split(',');
            int id = int.Parse(values[(int)eCHAR_COLUMN.ID]);

            float maxHp = float.Parse(values[(int)eCHAR_COLUMN.MaxHp]);
            float att = float.Parse(values[(int)eCHAR_COLUMN.Att]);
            float attSpeed = float.Parse(values[(int)eCHAR_COLUMN.AttSpeed]);
            float maxMp = float.Parse(values[(int)eCHAR_COLUMN.MaxMp]);
            float startMp = float.Parse(values[(int)eCHAR_COLUMN.StartMp]);
            float def = float.Parse(values[(int)eCHAR_COLUMN.Def]);
            float mr = float.Parse(values[(int)eCHAR_COLUMN.MR]);
            int range = int.Parse(values[(int)eCHAR_COLUMN.Range]);

            int level = int.Parse(values[(int)eCHAR_COLUMN.Level]);
            CStat stat = new CStat(maxHp, att, attSpeed, maxMp, startMp, def, mr, range);

            eCHARTYPE type = (eCHARTYPE)System.Enum.Parse(typeof(eCHARTYPE), values[(int)eCHAR_COLUMN.Char_Type]);

            if (!charDictionary.ContainsKey(type))
            {
                string name = values[(int)eCHAR_COLUMN.Char_Name];
                int cost = int.Parse(values[(int)eCHAR_COLUMN.Char_Cost]);
                string prefName = values[(int)eCHAR_COLUMN.Pref_Name];
                

                eSYNERGY synergy_0 = string.IsNullOrEmpty(values[(int)eCHAR_COLUMN.Synergy_0]) ? 0 : (eSYNERGY)System.Enum.Parse(typeof(eSYNERGY), values[(int)eCHAR_COLUMN.Synergy_0]);
                eSYNERGY synergy_1 = string.IsNullOrEmpty(values[(int)eCHAR_COLUMN.Synergy_1]) ? 0 : (eSYNERGY)System.Enum.Parse(typeof(eSYNERGY), values[(int)eCHAR_COLUMN.Synergy_1]);
                eSYNERGY synergy_2 = string.IsNullOrEmpty(values[(int)eCHAR_COLUMN.Synergy_2]) ? 0 : (eSYNERGY)System.Enum.Parse(typeof(eSYNERGY), values[(int)eCHAR_COLUMN.Synergy_2]);
                //eSYNERGY synergy_2 = (values[(int)eCHAR_COLUMN.Synergy_2] == "\r") ? 0 : (eSYNERGY)System.Enum.Parse(typeof(eSYNERGY), values[(int)eCHAR_COLUMN.Synergy_2]);
                List<eSYNERGY> synergies = new List<eSYNERGY>();
                if(synergy_0 != 0)
                {
                    synergies.Add(synergy_0);
                }
                if(synergy_1 != 0)
                {
                    synergies.Add(synergy_1);
                }
                if (synergy_2 != 0)
                {
                    synergies.Add(synergy_2);
                }
                CChar cchar = new CChar(name, type, cost, prefName, stat, synergies);
                charDictionary.Add(type, cchar);
                charDictionary[type].SetStatMap(level, stat);
                //charDictionary[id] = skill;
            }
            else
            {
                charDictionary[type].SetStatMap(level, stat);
            }
            

            

            
        }
        _charDatas = charDictionary;
    }
    public string GetPrefNameByType(eCHARTYPE type)
    {
        string prefName = _charDatas[type]._prefName;
        /*
        foreach(CChar cchar in _charDatas)
        {
            if(cchar._type == type)
            {
                prefName = cchar._prefName;
                break;
            }
        }
        */
        return prefName;
    }
    public int GetLevelByType(eCHARTYPE type)
    {
        int level = _charDatas[type]._level;
        /*
        foreach(CChar cchar in _charDatas)
        {
            if(cchar._type == type)
            {
                level = cchar._level;
                break;
            }
        }
        */
        return level;
    }
    /*
    public List<CChar> GetData()
    {
        return _charDatas;
    }
    */
}
