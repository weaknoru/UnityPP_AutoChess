using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCtrl : MonoBehaviour {
    #region Constants and Fields
    public GameObject _map;
    #endregion

    #region Public Properties
    #endregion

    #region Public Methods and Operators
    #endregion

    #region Event Handler Methods
    #endregion

    #region Methods
    public void CloseMap() {
        _map.SetActive(false);
    }
    #endregion

    #region Call by Unity
    void Awake() {
        
    }
    void Update() {
        
    }
    #endregion
}