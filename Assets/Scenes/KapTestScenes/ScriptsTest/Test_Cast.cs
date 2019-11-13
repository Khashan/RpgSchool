using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Cast : MonoBehaviour, IFighter
{
    public BaseAbility m_Ability;

    private void Start()
    {
        m_Ability.CastAbilityTo(new IFighter[] { this });
    }

    public GameObject GetFighterGameObject()
    {
        return gameObject;
    }

    public void ReceiveDamage(int aDamage)
    {
        Debug.Log("Damage received: " + aDamage);
    }

    public void ReceiveHeal(int aHeal)
    {
        Debug.Log("Heal received: " + aHeal);
    }
}
