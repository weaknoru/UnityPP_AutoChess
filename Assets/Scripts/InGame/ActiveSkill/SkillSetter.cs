using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSetter : MonoBehaviour
{
    [Header("��ų ������Ʈ"), SerializeField] GameObject[] _skillObjs;
    /*
    public void SetSkill(BasicAttack attack, eCHARTYPE type)
    {
        ISkill target = null;
        foreach(var obj in _skillObjs)
        {
            ISkill skill = obj.GetComponent<ISkill>();
            if (skill.GetCharacterType() == type)
            {
                target = skill;
            }
        }
        attack.SetActiveSKill(target);
    }
    */
}
