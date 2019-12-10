﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = PATH_NAME + "Comsumable")]
public class ConsumableItemData : SimonItemData
{
    public enum ConsumableType
    {
        HEALTH
    }

    [SerializeField]
    private ConsumableType m_Type;

    [SerializeField]
    private int m_EffectAmount;

    public override void Use(FigherData aFigher)
    {
        if(m_Type == ConsumableType.HEALTH)
        {
            aFigher.Heal(m_EffectAmount);
        }
    }
}
