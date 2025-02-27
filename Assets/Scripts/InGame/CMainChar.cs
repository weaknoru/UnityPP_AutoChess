using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eMAINCHAR
{
    Vacuity,
    Wild
}
public class CMainChar
{
    eMAINCHAR _charType;
    public eMAINCHAR _CharType => _charType;

    public void SetCharType(eMAINCHAR charType)
    {
        _charType = charType;
    }
    
}
