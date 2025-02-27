using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour {
    #region Constants and Fields
    public GameObject[] CharacterPanels;
    int _selected;
    #endregion

    #region Public Properties
    #endregion

    #region Public Methods and Operators
    #endregion

    #region Event Handler Methods
    #endregion

    #region Methods
    //캐릭터 선택 이동 버튼
    public void GoRight() {
        if(_selected != CharacterPanels.Length-1) {
            CharacterPanels[_selected].SetActive(false);
            _selected++;
            CharacterPanels[_selected].SetActive(true);
        }
        else {
            CharacterPanels[_selected].SetActive(false);
            _selected=0;
            CharacterPanels[_selected].SetActive(true);
        }
    }
    public void GoLeft() {
        if (_selected != 0) {
            CharacterPanels[_selected].SetActive(false);
            _selected--;
            CharacterPanels[_selected].SetActive(true);
        }
        else {
            CharacterPanels[_selected].SetActive(false);
            _selected = CharacterPanels.Length - 1;
            CharacterPanels[_selected].SetActive(true);
        }
    }
    //게임 시작 버튼
    public void StartGame() {
        LoadSceneManager.instance.GotoGameScene(false);
    }
    #endregion

    #region Call by Unity
    void Awake() {
        _selected = 0;
    }
    void Update() {
        
    }
    #endregion
}