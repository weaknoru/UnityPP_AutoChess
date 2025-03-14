using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelector : MonoBehaviour
{
    //List<CharInfo> _collectedCharList = new List<CharInfo>();
    Dictionary<eCHARTYPE, int> _collectedCharList = new Dictionary<eCHARTYPE, int>();
    //List<eCHARTYPE> _collectedCharList = new List<eCHARTYPE>();


    [Header("플레이어 대기열"), SerializeField] List<Tile> _playerQueues;

    [SerializeField] CSVReader _csvReader;

    List<GameObject> _selectedGo = new List<GameObject>();

    private void Start()
    {
        TestAwakeSetting();
        SetUnitOnQueue();
    }
    
    void TestAwakeSetting()
    {
        StageManager.Instance.SetPlayerUnitCount(1);

        _collectedCharList.Clear();
        //AddCollectedChar(eCHARTYPE.Warwick, 1);
        //AddCollectedChar(eCHARTYPE.Gnar, 1);
        //AddCollectedChar(eCHARTYPE.Varus, 1);

        CharManager.Instance.CountCollectedChar(eCHARTYPE.Warwick);
        CharManager.Instance.CountCollectedChar(eCHARTYPE.Nidalee);
        CharManager.Instance.CountCollectedChar(eCHARTYPE.Nidalee);
        CharManager.Instance.CountCollectedChar(eCHARTYPE.Nidalee);
        eCHARTYPE[] collectedCharsArr = CharManager.Instance.GetCollectedChars();
        /*
        for(int i = 0; i < collectedCharsArr.Length; i++)
        {
            int level = CharManager.Instance.GetLevel(collectedCharsArr[i]);
            AddCollectedChar(collectedCharsArr[i], level);
        }
        */
        foreach(eCHARTYPE collectedChar in collectedCharsArr)
        {
            int level = CharManager.Instance.GetLevel(collectedChar);
            AddCollectedChar(collectedChar, level);
        }
    }
    public void SetUnitOnQueue()
    {
        List<eCHARTYPE> charList = new List<eCHARTYPE>(_collectedCharList.Keys);
        List<int> levelList = new List<int>(_collectedCharList.Values);
        for(int i = 0; i < _collectedCharList.Count; i++)
        {
            GameObject findPref = CharManager.Instance.FindGoByType(charList[i]);
            GameObject go = Instantiate(findPref);

            CharInfo charInfo = go.GetComponent<CharInfo>();
            charInfo.UpdateLevel(levelList[i]);

            CStat stat = _csvReader._CharData._CharDatas[charList[i]].GetStat(levelList[i]);
            charInfo.SetStat(stat);

            charInfo.SetMovable(true, _playerQueues[i]);
            _playerQueues[i].SetTileState(eTILESTATE.Player);

            charInfo.SetState(eTILESTATE.Player);

            charInfo.SetHealth(true);

            _playerQueues[i].PutCharacter(go);


            _selectedGo.Add(go);
            CharManager.Instance.AddPlayerTrses(go.transform);
        }
        StageManager.Instance.SetSelectedGo(_selectedGo);
    }

    public void AddCollectedChar(eCHARTYPE charType, int level)
    {
        _collectedCharList.Add(charType, level);
        
    }
    
    /*
    void UpgradeLevel(eCHARTYPE charType)
    {
        if(_collectedCharList.ContainsKey(charType))
        {
            if (_collectedCharList[charType] < 3)
            {
                _collectedCharList[charType]++;
            }
        }
    }
    */
}
