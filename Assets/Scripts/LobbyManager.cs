using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviour {

    #region Constants and Fields
    [SerializeField]
    Button _Button_new;
    [SerializeField]
    Button _Button_load;
    [SerializeField]
    Button _Button_quit;
    [SerializeField]
    GameObject _LoadPanel;
    [SerializeField]
    GameObject _MainButtonGroup;
    [SerializeField]
    /*
    AudioSource _Audio_Select;
    [SerializeField]
    AudioSource _Audio_Click;*/
    #endregion

    #region Public Properties
    #endregion

    #region Public Methods and Operators
    #endregion

    #region Event Handler Methods
    #endregion

    #region Methods

    public void OnLoadButtonClicked() {
        _LoadPanel.SetActive(true);
        _MainButtonGroup.SetActive(false);
    }
    public void OnQuitButtonClicked() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    public void CloseLoadPanel() {
        _LoadPanel.SetActive(false);
        _MainButtonGroup.SetActive(true);
    }

    #endregion

    #region Call by Unity
    void Awake() {
        
    }
    void Update() {
        
    }
#endregion
}