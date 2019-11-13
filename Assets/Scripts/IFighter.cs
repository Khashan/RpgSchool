
using UnityEngine;

public interface IFighter
{
    void ReceiveDamage(int aDamage);
    void ReceiveHeal(int aHeal);
    GameObject GetFighterGameObject();
}
