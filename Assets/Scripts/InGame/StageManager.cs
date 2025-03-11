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

    [Header("���ӽ��� ��ư"), SerializeField] Button _gameStartButton;

    int _playerCount = 0;
    int _enemyCount = 0;

    List<GameObject> _selectedGo = new List<GameObject>();
    private void Awake()
    {
        _gameStartButton.onClick.AddListener(() => BTN_StartGame());
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            GiveShieldPlayer(500f, 5f);
        }
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
            Debug.Log("���� �й�");
            Time.timeScale = 0f;
        }
        else if(_enemyCount < 1)
        {
            Debug.Log("���� �¸�");
            Time.timeScale = 0f;
        }
        
    }
    
    void BTN_StartGame()
    {

        CharManager.Instance.UpdatePath();
        GiveShieldPlayer(200f, 20f);

    }
    void GiveShieldPlayer(float value, float dur)
    {
        for (int i = 0; i < _selectedGo.Count; i++)
        {
            CharHealth health = _selectedGo[i].GetComponent<CharHealth>();
            health.SetShield(value, dur);
        }
    }
    public void SetSelectedGo(List<GameObject> selectedGo)
    {
        _selectedGo = selectedGo;
    }

}
