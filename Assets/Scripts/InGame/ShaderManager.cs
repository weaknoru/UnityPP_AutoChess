using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eDRAGSTATE
{
    Defalut,
    Drag,
    Selected
}
public class ShaderManager : MonoBehaviour
{
    static ShaderManager _instance;

    public static ShaderManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<ShaderManager>();
            return _instance;
        }
    }

    List<Transform> _tileTrses = new List<Transform>();
    [Header("평상 시 머터리얼"), SerializeField]
    Material[] _defaultMats;
    [Header("드래그 시 머터리얼"), SerializeField]
    Material[] _dragMats;
    [Header("선택 시 머터리얼"), SerializeField]
    Material[] _selectedMats;

    public void AddTileTrs(Transform tileTrs)
    {
        _tileTrses.Add(tileTrs);
    }
    public void SetTileMatarial(Renderer renderer, eDRAGSTATE state)
    {
        Material[] mats = renderer.materials;
        switch (state)
        {
            case eDRAGSTATE.Defalut:
                mats[0] = _defaultMats[0];
                break;
            case eDRAGSTATE.Drag:
                mats[0] = _dragMats[0];
                break;
            case eDRAGSTATE.Selected:
                mats[0] = _selectedMats[0];
                break;
        }

        renderer.materials = mats;
    }
    public void SetLineTileMatarial(eDRAGSTATE state)
    {
        switch(state)
        {
            case eDRAGSTATE.Defalut:
                for (int i = 0; i < _tileTrses.Count; i++)
                {
                    Renderer renderer = _tileTrses[i].GetComponentInChildren<Renderer>();
                    Material[] mats = renderer.materials;
                    mats[1] = _defaultMats[1];
                    renderer.materials = mats;
                    //renderer.materials = _defaultMats;
                }
                break;
            case eDRAGSTATE.Drag:
                for(int i = 0; i < _tileTrses.Count; i++)
                {
                    Renderer renderer = _tileTrses[i].GetComponentInChildren<Renderer>();
                    Material[] mats = renderer.materials;
                    mats[1] = _dragMats[1];
                    renderer.materials = mats;
                    //renderer.materials = _dragMats;
                }
                break;
        }
    }
}
