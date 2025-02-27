using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ISkill
{
    void ExecuteSkill();
    //void SetParticle(ParticleSystem[] particles);
    void SetTileState(eTILESTATE tileType);
    //eCHARTYPE GetCharacterType();
    void SetBuff(int level,float apIncrease);
    void SetTargetObj(GameObject targetObj);
    void SetCharInfo(CharInfo charInfo);
}
public class SkillAttack : MonoBehaviour
{
    /*
    [Header("ÆÄÆ¼Å¬"), SerializeField] ParticleSystem[] _mainParticles;
    ISkill _curSkill;

    public void SetSkill(ISkill skill)
    {
        _curSkill = skill;
        //_curSkill.SetParticle(_mainParticles);
    }

    public void SkillInteract()
    {
        if (_curSkill != null)
        {
            _curSkill.ExecuteSkill();
            //PlayParticle();
        }
        else
        {
            Debug.Log("No skill assigned!");
        }
    }
    
    void PlayParticle()
    {
        if(_mainParticles != null)
        {
            _mainParticles.Play(true);

        }
        
    }
    
    */
}