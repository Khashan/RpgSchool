using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField]
    private List<CharacterData> m_FriendlyList = new List<CharacterData>();
    [SerializeField]
    private List<Vector2> m_FriendlyIdlePositions = new List<Vector2>();
    private List<Vector2> m_FriendlyAttackingPositions = new List<Vector2>();


    [SerializeField]
    private List<CharacterData> m_EnnemyList = new List<CharacterData>();
    [SerializeField]
    private List<Vector2> m_EnnemyIdlePositions = new List<Vector2>();
    private List<Vector2> m_EnnemyAttackingPositions = new List<Vector2>();

    [SerializeField]
    private float m_TravelLerpDuration = 1.5f;



    private bool m_IsFriendlyTurn = true;


    private void Start()
    {
        CombatManager.Instance.InitEnnemyTeam();
        CombatManager.Instance.CombatSetup(m_FriendlyList, m_EnnemyList);
        m_EnnemyAttackingPositions[0] = new Vector2(m_FriendlyIdlePositions[0].x + 2, m_FriendlyIdlePositions[0].y);
        m_EnnemyAttackingPositions[1] = new Vector2(m_FriendlyIdlePositions[1].x + 2, m_FriendlyIdlePositions[1].y);
        m_EnnemyAttackingPositions[2] = new Vector2(m_FriendlyIdlePositions[2].x + 2, m_FriendlyIdlePositions[2].y);

        m_FriendlyAttackingPositions[0] = new Vector2(m_EnnemyIdlePositions[0].x - 2, m_EnnemyIdlePositions[0].y);
        m_FriendlyAttackingPositions[1] = new Vector2(m_EnnemyIdlePositions[1].x - 2, m_EnnemyIdlePositions[1].y);
        m_FriendlyAttackingPositions[2] = new Vector2(m_EnnemyIdlePositions[2].x - 2, m_EnnemyIdlePositions[2].y);
    }

    private void CombatOver()
    {
        m_EnnemyList.Clear();
        m_FriendlyList.Clear();
    }


    private void Update()
    {

    }

    private void Attack(CharacterData aAttackingCharacter, CharacterData aAttackedCharacter)
    {
        //float currentlerp = Vector2.Lerp()
    }
}
