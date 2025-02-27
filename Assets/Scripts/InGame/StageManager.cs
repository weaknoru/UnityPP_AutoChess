using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    static StageManager _instance;

    public static StageManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<StageManager>();
            return _instance;
        }
    }

    [Header("게임시작 버튼"), SerializeField] Button _gameStartButton;

    int _playerCount = 0;
    int _enemyCount = 0;
    private void Awake()
    {
        _gameStartButton.onClick.AddListener(() => BTN_StartGame());
    }
    public void SetPlayerUnitCount(int count)
    {
        _playerCount = count;
    }
    public void SetEnemyUnitCount(int count)
    {
        _enemyCount = count;
    }
    public void MinusCount(bool isPlayer)
    {
        if(isPlayer)
        {
            _playerCount--;
        }
        else
        {
            _enemyCount--;
        }
        SetStageResult();
    }
    public void SetStageResult()
    {
        if(_playerCount < 1)
        {
            Debug.Log("게임 패배");
            Time.timeScale = 0f;
        }
        else if(_enemyCount < 1)
        {
            Debug.Log("게임 승리");
            Time.timeScale = 0f;
        }
        
    }
    
    void BTN_StartGame()
    {

        CharManager.Instance.UpdatePath();

    }

}
