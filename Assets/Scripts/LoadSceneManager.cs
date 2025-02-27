using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadSceneManager : MonoBehaviour {
    #region Constants and Fields

    [Header("�ε� �г�"), SerializeField]
    GameObject _loadingPanel;
    [Header("�ε� ��"), SerializeField]
    Scrollbar _loadingBar;
    [Header("�ı� �ȵǴ� ĵ����"), SerializeField]
    GameObject _staticCanvas;

    public bool _isLoad;

    #endregion

    #region Public Properties
    public static LoadSceneManager instance = null;

    public static LoadSceneManager Instance {
        get {
            if (null == instance) {
                return null;
            }
            return instance;
        }
    }
    #endregion

    #region Public Methods and Operators
    #endregion

    #region Event Handler Methods
    #endregion

    #region Methods
    public void GotoGameScene(bool isLoad) {
        Time.timeScale = 1;
        StartCoroutine(LoadScene("Game"));
        _loadingPanel.SetActive(true);
        _isLoad = isLoad;
    }

    public void GotoSelectScene(bool isLoad) {
        Time.timeScale = 1;
        StartCoroutine(LoadScene("Select"));
        _loadingPanel.SetActive(true);
        _isLoad = isLoad;
    }

    public void GotoLobbyScene(bool isLoad) {
        Time.timeScale = 1;
        StartCoroutine(LoadScene("Lobby"));
        _loadingPanel.SetActive(true);
        _isLoad = isLoad;
    }

    IEnumerator LoadScene(string sceneName) {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone) {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f) {
                _loadingBar.size = Mathf.Lerp(_loadingBar.size, op.progress, timer);
                if (_loadingBar.size >= op.progress) {
                    timer = 0f;
                }
            }
            else {
                _loadingBar.size = Mathf.Lerp(_loadingBar.size, 1f, timer);
                if (_loadingBar.size == 1.0f) {
                    op.allowSceneActivation = true;
                    _loadingPanel.SetActive(false);
                    yield break;
                }
            }
        }
    }
    #endregion

    #region Call by Unity
    void Awake() {
        //�̱��� �۾�
        if (null == instance) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    private void Start() {
        DontDestroyOnLoad(_staticCanvas);
    }
    void Update() {
        
    }
    #endregion
}