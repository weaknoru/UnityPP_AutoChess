using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSetter : MonoBehaviour
{
    private void Awake()
    {
        
    }

    public void DebugStageSetting()
    {
        HexBoardMaker.Instance.SetEnemyUnit(eCHARTYPE.RekSai, 2,new Vector2(5, 5));
        //HexBoardMaker.Instance.SetEnemyUnit(eCHARTYPE.Kassadin, 1, new Vector2(2, 7));
        //HexBoardMaker.Instance.SetEnemyUnit(eCHARTYPE.Khazix, 1, new Vector2(3, 7));

        StageManager.Instance.SetEnemyUnitCount(1);
    }
}
