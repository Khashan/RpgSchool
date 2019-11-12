using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private List<GameObject> m_FriendlyList = new List<GameObject>();
    [SerializeField]
    private List<Vector2> m_FriendlyIdlePositions = new List<Vector2>();
    private List<Vector2> m_FriendlyAttackingPositions = new List<Vector2>();


    private List<GameObject> m_EnnemyList = new List<GameObject>();
    [SerializeField]
    private List<Vector2> m_EnnemyIdlePositions = new List<Vector2>();
    private List<Vector2> m_EnnemyAttackingPositions = new List<Vector2>();

    [SerializeField]
    private float m_TravelLerpDuration = 2.0f;



    private bool m_IsFriendlyTurn = true;
    private bool m_IsFriendlyAttacking = false;
    private NPCController m_CurrentFriendlyStats;
    private NPCController m_CurrentEnnemyStats;
    private GameObject m_CurrentFriendlyGO;
    private GameObject m_CurrentEnnemyGO;
    private Vector3 m_CurrentDestination;
    private Vector3 m_CurentIdle;
    private bool m_isDoneAttacking = false;
    private Animator m_CurrentFriendlyAnim;
    private SpriteRenderer m_CurrentRend;
    private Coroutine m_Coroutine;
    private bool m_isIdle = true;
    private int m_oldorderinlayer;

    private bool m_isEnnemyTurn = false;


    private void Start()
    {


        CombatManager.Instance.CombatController = this;
        CombatManager.Instance.InitEnnemyTeam();
        CombatManager.Instance.CombatSetup();

        //CombatManager.Instance.CombatSetup(m_FriendlyList, m_EnnemyList);

        //m_FriendlyIdlePositions[0] = new Vector2(m_FriendlyList[0].transform.position.x, m_FriendlyList[0].transform.position.y);
        //m_FriendlyIdlePositions[1] = new Vector2(m_FriendlyList[1].transform.position.x, m_FriendlyList[1].transform.position.y);
        //m_FriendlyIdlePositions[2] = new Vector2(m_FriendlyList[2].transform.position.x, m_FriendlyList[2].transform.position.y);

       // m_EnnemyIdlePositions[0] = new Vector2(m_EnnemyList[0].transform.position.x, m_EnnemyList[0].transform.position.y);
       // m_EnnemyIdlePositions[1] = new Vector2(m_EnnemyList[1].transform.position.x, m_EnnemyList[1].transform.position.y);
       // m_EnnemyIdlePositions[2] = new Vector2(m_EnnemyList[2].transform.position.x, m_EnnemyList[2].transform.position.y);

        m_EnnemyAttackingPositions.Add(new Vector2(m_FriendlyIdlePositions[0].x + 2, m_FriendlyIdlePositions[0].y));
        m_EnnemyAttackingPositions.Add(new Vector2(m_FriendlyIdlePositions[1].x + 2, m_FriendlyIdlePositions[1].y));
        m_EnnemyAttackingPositions.Add(new Vector2(m_FriendlyIdlePositions[2].x + 2, m_FriendlyIdlePositions[2].y));

        m_FriendlyAttackingPositions.Add(new Vector2(m_EnnemyIdlePositions[0].x - 2, m_EnnemyIdlePositions[0].y));
        m_FriendlyAttackingPositions.Add(new Vector2(m_EnnemyIdlePositions[1].x - 2, m_EnnemyIdlePositions[1].y));
        m_FriendlyAttackingPositions.Add(new Vector2(m_EnnemyIdlePositions[2].x - 2, m_EnnemyIdlePositions[2].y));
    }

    private void CombatOver()
    {
        m_EnnemyList.Clear();
        m_FriendlyList.Clear();
    }

    public void SetupCombat(List<CharacterData> aFriendlyData, List<CharacterData> aEnnemyData)
    {
        for(int i = 0; i< 3; i++)
        {
            GameObject FGO = Instantiate(aFriendlyData[i].m_CharacterPrefab, m_FriendlyIdlePositions[i], Quaternion.identity);
            GameObject EGO = Instantiate(aEnnemyData[i].m_CharacterPrefab, m_EnnemyIdlePositions[i], Quaternion.identity);
            m_FriendlyList.Add(FGO);
            m_EnnemyList.Add(EGO);

            SpriteRenderer tRender = m_EnnemyList[i].GetComponent<SpriteRenderer>();
            if(tRender != null)
            {
                tRender.flipX = true;
            }
        }
    }


    private void Update()
    {

        if(m_IsFriendlyAttacking)
        {
            if(m_CurrentFriendlyGO.transform.position != m_CurrentDestination && !m_isDoneAttacking)
            {
                m_CurrentFriendlyAnim.SetBool("isIdle", false);
                m_CurrentFriendlyAnim.SetTrigger("Walk");
                if(m_Coroutine == null)
                {
                    m_Coroutine = StartCoroutine(MoveToNPC(m_CurrentFriendlyGO, m_CurentIdle, m_CurrentDestination, m_TravelLerpDuration));
                }
                
            }
            else if(m_CurrentFriendlyGO.transform.position == m_CurrentDestination && !m_isDoneAttacking)
            {
                Attack();
                if(m_Coroutine == null)
                {
                    m_Coroutine = StartCoroutine(WaitForAttack(m_TravelLerpDuration));
                }
                m_isDoneAttacking = true;
                
            }
            else if(m_isDoneAttacking && m_CurrentFriendlyGO.transform.position != m_CurentIdle)
            {
                m_CurrentFriendlyAnim.SetTrigger("Walk");
                if(m_Coroutine == null)
                {
                    m_Coroutine = StartCoroutine(MoveToNPC(m_CurrentFriendlyGO, m_CurrentDestination, m_CurentIdle, m_TravelLerpDuration));
                }
                Debug.Log("not in begin pos");

            }
            else if(m_isDoneAttacking && m_CurrentFriendlyGO.transform.position == m_CurentIdle)
            {
                m_CurrentRend.sortingOrder = m_oldorderinlayer;
                m_CurrentRend.flipX = false;
                m_CurrentFriendlyAnim.SetBool("isIdle", true);
                m_CurrentFriendlyAnim.StopPlayback();
                m_IsFriendlyAttacking = false;

                ClearCurrentTurn();
            }
            
        }
    }

    private void ClearCurrentTurn()
    {
        m_isDoneAttacking = false;
        m_CurrentFriendlyStats = null;
        m_CurrentEnnemyStats = null;
        m_CurrentFriendlyGO = null;
        m_CurrentEnnemyGO = null;
        m_CurrentFriendlyAnim = null;
    }

    public void FriendlyAttack(int aAttackingPosition, int aAttackedPosition)
    {
        Debug.Log("Setup attack : " + aAttackingPosition + "  Attacks   " + aAttackedPosition);
        m_CurrentFriendlyStats = m_FriendlyList[aAttackingPosition - 1].GetComponent<NPCController>();
        m_CurrentEnnemyStats = m_EnnemyList[aAttackedPosition - 1].GetComponent<NPCController>();
        m_CurrentEnnemyGO = m_EnnemyList[aAttackedPosition -1];
        m_CurrentFriendlyGO = m_FriendlyList[aAttackingPosition - 1];
        m_CurentIdle = m_FriendlyIdlePositions[aAttackingPosition - 1];
        m_CurrentDestination = m_FriendlyAttackingPositions[aAttackedPosition -1];
        m_CurrentFriendlyAnim = m_CurrentFriendlyGO.GetComponent<Animator>();
        m_CurrentRend = m_CurrentFriendlyGO.GetComponent<SpriteRenderer>();
        if(m_CurrentFriendlyStats != null && m_CurrentEnnemyStats != null && m_CurrentEnnemyGO != null && m_CurrentFriendlyGO != null)
        {
            m_IsFriendlyAttacking = true;
        }
        m_oldorderinlayer = m_CurrentRend.sortingOrder;
        m_CurrentRend.sortingOrder = 12;
        //Debug.Log("ORIGIN = " + m_CurentIdle + "      DESTINATION: " + m_CurrentDestination);
        if(!m_IsFriendlyAttacking)
        {
            Debug.LogError("STATS WERE NOT SET CORRECTLY");
        }
    }

    public IEnumerator WaitForAttack(float aSecs)
    {
        while(true)
        {
            yield return new WaitForSeconds(aSecs);
            StopAllCoroutines();
            m_Coroutine = null;
            m_CurrentRend.flipX = true;
        }
        //m_Coroutine = null;
    }

    public IEnumerator MoveToNPC(GameObject aMovingNPC, Vector3 aInitial, Vector3 aDestination, float aTime)
    {
        float elapsed = 0f;
        while(elapsed < aTime)
        {
            aMovingNPC.transform.position = Vector3.Lerp(aInitial, aDestination, (elapsed / aTime));
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        aMovingNPC.transform.position = aDestination;
        m_Coroutine = null;
    }

    private void Attack()
    {
       int tDamage = m_CurrentFriendlyStats.Damage;
       m_CurrentEnnemyStats.CurrentHP -= tDamage;
       Animator EAnim = m_CurrentEnnemyGO.GetComponent<Animator>();
       m_CurrentFriendlyAnim.SetTrigger("Attack");
    }
}
