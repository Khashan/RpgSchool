
using UnityEngine;

public interface IFighter
{
    void ReceiveDamage(int aDamage);
    void ReceiveHeal(int aHeal);
    void AddArmor(int aArmor);
    void Cleanse();
    GameObject GetFighterGameObject();
}
