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
    private Animator m_CurrentEnnemyAnim;
    private SpriteRenderer m_CurrentRend;
    private Coroutine m_Coroutine;
    private bool m_isIdle = true;
    private int m_oldorderinlayer;
    private int m_TotalEnnemyCount = 0;
    private int m_AliveFriendlies = 0;
    private int m_AliveEnnemies = 0;
    private int m_CurrentEnnemyTurn = 1;
    private bool m_isEnnemyTurnSet = false;
    private bool m_StartEnnemyCombat = false;
    private int m_CurrentFriendlyAttacked;

    private bool m_WaitForNextInput = true;


    private List<int> m_PossibleEnnemyAttacks = new List<int>();

    private bool m_isEnnemyTurn = false;


    private void Start()
    {


        CombatManager.Instance.CombatController = this;
        CombatManager.Instance.InitEnnemyTeam();
        CombatManager.Instance.CombatSetup();

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
        m_TotalEnnemyCount = aEnnemyData.Count;
        m_AliveEnnemies = m_TotalEnnemyCount;
        m_AliveFriendlies = aFriendlyData.Count;
        for(int i = 0; i< 3; i++)
        {
            GameObject FGO = Instantiate(aFriendlyData[i].m_CharacterPrefab, m_FriendlyIdlePositions[i], Quaternion.identity);
            m_FriendlyList.Add(FGO);
        }

        for(int i = 0; i < aEnnemyData.Count; i++)
        {
            
            GameObject EGO = Instantiate(aEnnemyData[i].m_CharacterPrefab, m_EnnemyIdlePositions[i], Quaternion.identity);
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

        if(!m_IsFriendlyAttacking && !m_isEnnemyTurn)
        {
            if(m_WaitForNextInput)
            {
                HUDManager.Instance.combatUI.StartRound();
                m_WaitForNextInput = false;
            }
        }


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
                    m_Coroutine = StartCoroutine(WaitForAttack(m_TravelLerpDuration, true));
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
                m_isEnnemyTurn = true;

                ClearCurrentTurn();
            }
            
        }

        if(m_isEnnemyTurn)
        {

            if(m_AliveEnnemies > 0)
            {
                if(!m_isEnnemyTurnSet)
                {
                    Debug.Log("Ennemy Turn Checked");
                    m_CurrentEnnemyTurn = CheckEnnemyTurn();
                    m_isEnnemyTurnSet = true;
                }
                else
                {
                    if(m_StartEnnemyCombat)
                    {
                        Debug.Log("EnnemyCombatStarted");
                        if(m_CurrentEnnemyGO.transform.position != m_CurrentDestination && !m_isDoneAttacking)
                        {
                            m_CurrentEnnemyAnim.SetBool("isIdle", false);
                            m_CurrentEnnemyAnim.SetTrigger("Walk");
                            if(m_Coroutine == null)
                            {
                                m_Coroutine = StartCoroutine(MoveToNPC(m_CurrentEnnemyGO, m_CurentIdle, m_CurrentDestination, m_TravelLerpDuration));
                            }
                            Debug.Log("EnnemyWalkingToAttack");
                            
                        }
                        else if(m_CurrentEnnemyGO.transform.position == m_CurrentDestination && !m_isDoneAttacking)
                        {
                            EnnemyAttack();
                            if(m_Coroutine == null)
                            {
                                m_Coroutine = StartCoroutine(WaitForAttack(m_TravelLerpDuration, false));
                            }
                            m_isDoneAttacking = true;
                            Debug.Log("EnnemyAttacking");
                            
                        }
                        else if(m_isDoneAttacking && m_CurrentEnnemyGO.transform.position != m_CurentIdle)
                        {
                            m_CurrentEnnemyAnim.SetTrigger("Walk");
                            if(m_Coroutine == null)
                            {
                                m_Coroutine = StartCoroutine(MoveToNPC(m_CurrentEnnemyGO, m_CurrentDestination, m_CurentIdle, m_TravelLerpDuration));
                            }
                            Debug.Log("EnnemyGoingBack");

                        }
                        else if(m_isDoneAttacking && m_CurrentEnnemyGO.transform.position == m_CurentIdle)
                        {
                            m_CurrentRend.sortingOrder = m_oldorderinlayer;
                            m_CurrentRend.flipX = true;
                            m_CurrentEnnemyAnim.SetBool("isIdle", true);
                            m_CurrentEnnemyAnim.StopPlayback();
                            m_isEnnemyTurn = false;
                            m_StartEnnemyCombat = false;
                            m_isEnnemyTurnSet = false;

                            ClearCurrentTurn();
                            m_WaitForNextInput = true;
                        }
                    }
                    else
                    {
                        SetEnnemyCombat();
                    }
                }

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
        m_CurrentEnnemyAnim = null;
        m_CurrentRend = null;
        m_Coroutine = null;

        //m_StartEnnemyCombat = false;
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
        m_CurrentEnnemyAnim = m_CurrentEnnemyGO.GetComponent<Animator>();
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

    private void SetEnnemyCombat()
    {
        Debug.Log("Setting Ennemy Combat");
        m_StartEnnemyCombat = true;
        m_CurrentFriendlyAttacked = RandomizeEnnemyAttack();
        m_CurrentFriendlyStats = m_FriendlyList[m_CurrentFriendlyAttacked - 1].GetComponent<NPCController>();
        m_CurrentEnnemyStats = m_EnnemyList[m_CurrentEnnemyTurn -1].GetComponent<NPCController>();
        m_CurrentEnnemyGO = m_EnnemyList[m_CurrentEnnemyTurn -1];
        m_CurrentFriendlyGO = m_FriendlyList[m_CurrentFriendlyAttacked - 1];
        m_CurentIdle = m_EnnemyIdlePositions[m_CurrentEnnemyTurn - 1];
        m_CurrentDestination = m_EnnemyAttackingPositions[m_CurrentFriendlyAttacked -1];
        m_CurrentFriendlyAnim = m_CurrentFriendlyGO.GetComponent<Animator>();
        m_CurrentEnnemyAnim = m_CurrentEnnemyGO.GetComponent<Animator>();
        m_CurrentRend = m_CurrentEnnemyGO.GetComponent<SpriteRenderer>();

        m_oldorderinlayer = m_CurrentRend.sortingOrder;
        m_CurrentRend.sortingOrder = 12;

    }

    private int CheckEnnemyTurn()
    {
        if(m_AliveEnnemies == 3)
        {
            m_CurrentEnnemyTurn++;
            if(m_CurrentEnnemyTurn > 3)
            {
                m_CurrentEnnemyTurn = 1;
            }
            return m_CurrentEnnemyTurn;
        }
        else if(m_AliveEnnemies == 2)
        {
            m_CurrentEnnemyTurn++;
            if(m_CurrentEnnemyTurn > 2)
            {
                m_CurrentEnnemyTurn = 1;
            }
            return m_CurrentEnnemyTurn;
        }
        else if(m_AliveEnnemies == 1)
        {
            for(int i = 0; i < 3; i++)
            {
                NPCController tempEnnemy = m_EnnemyList[i].GetComponent<NPCController>();
                if(!tempEnnemy.isDead)
                {
                    m_CurrentEnnemyTurn = i + 1;
                    return m_CurrentEnnemyTurn;
                }
            }
        }
        /*else if(m_AliveEnnemies > 1)
        {
            int iter = m_CurrentEnnemyTurn - 1;
            Debug.Log("EnteringWhile");
            while(true)
            {
                Debug.Log("CreatingTempEnnemy");
                NPCController tempEnnemy = m_EnnemyList[iter].GetComponent<NPCController>();
                Debug.Log("TempEnnemyCreated");
                if(!tempEnnemy.isDead && iter != m_CurrentEnnemyTurn - 1)
                {
                    m_CurrentEnnemyTurn = iter + 1;
                    return m_CurrentEnnemyTurn;
                }
                
                iter++;
                if(iter >= m_AliveEnnemies)
                {
                    iter = 0;
                }
            }
        }*/

        Debug.LogError("THIS SHOULD NEVER HAPPEN");
        return 0;
    }

    private int RandomizeEnnemyAttack()
    {
        for(int i = 0; i < 3; i++)
        {
            NPCController tempfriendly = m_FriendlyList[i].GetComponent<NPCController>();
            if(!tempfriendly.isDead)
            {
                m_AliveFriendlies++;
                m_PossibleEnnemyAttacks.Add(i + 1);
            }
        }
        if(m_AliveFriendlies > 0)
        {
            int finalennemyattack;
            if(m_AliveFriendlies > 1)
            {
                int randennemyattack = (int)UnityEngine.Random.Range(0, m_PossibleEnnemyAttacks.Count);
                finalennemyattack = m_PossibleEnnemyAttacks[randennemyattack];
                return finalennemyattack;

            }
            else
            {
                finalennemyattack = m_PossibleEnnemyAttacks[0];
                return finalennemyattack;
            }
        }

        Debug.LogError("ALL FRIENDLIES ARE DEAD");
        return 0;

    }

    public IEnumerator WaitForAttack(float aSecs, bool aIsFriendly)
    {
        while(true)
        {
            yield return new WaitForSeconds(aSecs);
            StopAllCoroutines();
            m_Coroutine = null;
            if(aIsFriendly)
            {
                m_CurrentRend.flipX = true;
            }
            else
            {
                m_CurrentRend.flipX = false;
            }
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
       //Animator EAnim = m_CurrentEnnemyGO.GetComponent<Animator>();
       m_CurrentFriendlyAnim.SetTrigger("Attack");
    }

    private void EnnemyAttack()
    {
        int tDamage = m_CurrentEnnemyStats.Damage;
        m_CurrentFriendlyStats.CurrentHP -= tDamage;
        //m_CurrentFriendlyAnim.SetTrigger("TakeDamage");
        m_CurrentEnnemyAnim.SetTrigger("Attack");
    }
}
