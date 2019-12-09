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
    private int m_CurrentEnnemyAttacked;
    private bool m_IsFirstTurn = true;
    private bool m_IsBossFight = false;
    private CharacterData m_BossData;
    private string m_ChosenBossSpell;

    private bool m_WaitForNextInput = true;


    private List<int> m_PossibleEnnemyAttacks = new List<int>();

    private bool m_isEnnemyTurn = false;


    private void Start()
    {


        CombatManager.Instance.CombatController = this;
        if(!CombatManager.Instance.m_isBoss)
        {
            CombatManager.Instance.InitEnnemyTeam();
            CombatManager.Instance.CombatSetup();
        }
        else
        {
            m_IsBossFight = true;
            CombatManager.Instance.BossSetup();
        }

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

        if(!m_IsFriendlyAttacking && !m_isEnnemyTurn && !m_IsFirstTurn)
        {
            if(m_WaitForNextInput)
            {
                HUDManager.Instance.combatUI.NextRound();
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

        if(m_isEnnemyTurn && !m_IsBossFight)
        {

            if(m_AliveEnnemies > 0)
            {
                if(!m_isEnnemyTurnSet)
                {
                    m_CurrentEnnemyTurn = CheckEnnemyTurn();
                    m_isEnnemyTurnSet = true;
                }
                else
                {
                    if(m_StartEnnemyCombat)
                    {
                        if(m_CurrentEnnemyGO.transform.position != m_CurrentDestination && !m_isDoneAttacking)
                        {
                            m_CurrentEnnemyAnim.SetBool("isIdle", false);
                            m_CurrentEnnemyAnim.SetTrigger("Walk");
                            if(m_Coroutine == null)
                            {
                                m_Coroutine = StartCoroutine(MoveToNPC(m_CurrentEnnemyGO, m_CurentIdle, m_CurrentDestination, m_TravelLerpDuration));
                            }
                            
                        }
                        else if(m_CurrentEnnemyGO.transform.position == m_CurrentDestination && !m_isDoneAttacking)
                        {
                            EnnemyAttack();
                            if(m_Coroutine == null)
                            {
                                m_Coroutine = StartCoroutine(WaitForAttack(m_TravelLerpDuration, false));
                            }
                            m_isDoneAttacking = true;
                            
                        }
                        else if(m_isDoneAttacking && m_CurrentEnnemyGO.transform.position != m_CurentIdle)
                        {
                            m_CurrentEnnemyAnim.SetTrigger("Walk");
                            if(m_Coroutine == null)
                            {
                                m_Coroutine = StartCoroutine(MoveToNPC(m_CurrentEnnemyGO, m_CurrentDestination, m_CurentIdle, m_TravelLerpDuration));
                            }

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

        else if(m_isEnnemyTurn && m_IsBossFight)
        {
            Debug.Log("m_isennemyturnset = " + m_isEnnemyTurnSet);
            if(!m_isEnnemyTurnSet)
            {
                NPCController tBossController = m_EnnemyList[0].GetComponent<NPCController>();
                int remaininghp = tBossController.CurrentHP;
                Debug.Log(remaininghp + " boss hp");
                if(remaininghp < 120)
                {
                    int tHealRand = Random.Range(0,2);
                    Debug.Log(tHealRand + " RANDOM HEAL IF 1");
                    if(tHealRand == 1)
                    {
                        SetupBossAttack("Heal");
                    }
                    else
                    {
                        if(m_AliveFriendlies == 3)
                        {
                            int tAoERand = Random.Range(0,2);
                            if(tAoERand == 1)
                            {
                                SetupBossAttack("IceBurst");
                            }
                            else
                            {
                                SetupBossAttack("Lazer");
                            }
                        }
                        else
                        {
                            SetupBossAttack("Lazer");
                        }
                    }
                }
                else
                {
                    if(m_AliveFriendlies == 3)
                    {
                        int tAoERand = Random.Range(0,2);
                        Debug.Log(tAoERand + " RANDOM AOE IF 1");
                        if(tAoERand == 1)
                        {
                            SetupBossAttack("IceBurst");
                        }
                        else
                        {
                            SetupBossAttack("Lazer");
                        }
                    }
                    else
                    {
                        SetupBossAttack("Lazer");
                    }  
                }
                m_isEnnemyTurnSet = true;
            }
            else if(m_StartEnnemyCombat)
            {
                if(m_CurrentEnnemyGO.transform.position != m_CurrentDestination && !m_isDoneAttacking)
                {
                    m_CurrentEnnemyAnim.SetBool("isIdle", false);
                    m_CurrentEnnemyAnim.SetTrigger("Walk");
                    if(m_Coroutine == null)
                    {
                        m_Coroutine = StartCoroutine(MoveToNPC(m_CurrentEnnemyGO, m_CurentIdle, m_CurrentDestination, m_TravelLerpDuration));
                    }
                }
                else if(m_CurrentEnnemyGO.transform.position == m_CurrentDestination && !m_isDoneAttacking)
                {
                    BossAttack();
                    if(m_Coroutine == null)
                    {
                        m_Coroutine = StartCoroutine(WaitForAttack(m_TravelLerpDuration, false));
                    }
                    m_isDoneAttacking = true;
                }
                else if(m_isDoneAttacking && m_CurrentEnnemyGO.transform.position != m_CurentIdle)
                {
                    m_CurrentEnnemyAnim.SetTrigger("Walk");
                    m_CurrentEnnemyAnim.SetBool("isIdle", false);
                    if(m_Coroutine == null)
                    {
                        m_Coroutine = StartCoroutine(MoveToNPC(m_CurrentEnnemyGO, m_CurrentDestination, m_CurentIdle, m_TravelLerpDuration));
                    }

                }
                else if(m_isDoneAttacking && m_CurrentEnnemyGO.transform.position == m_CurentIdle)
                {
                    Debug.Log("End of boss turn");
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
        m_IsFirstTurn = false;
        m_CurrentEnnemyAttacked = aAttackedPosition + 3;
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
        if(!m_IsFriendlyAttacking)
        {
            Debug.LogError("STATS WERE NOT SET CORRECTLY");
        }
    }

    private void SetEnnemyCombat()
    {
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

    public List<int> GetAliveEnnemies()
    {
        List<int> tAliveEnnemyList = new List<int>();
        for(int i = 0; i < m_EnnemyList.Count; i++)
        {
            NPCController tEnnemyC = m_EnnemyList[i].GetComponent<NPCController>();
            if(tEnnemyC != null)
            {
                if(!tEnnemyC.isDead)
                {
                    tAliveEnnemyList.Add(i);
                }
            }
        }
        return tAliveEnnemyList;
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
            if(m_CurrentEnnemyTurn > 3)
            {
                m_CurrentEnnemyTurn = 1;
            }
            NPCController tempEnnemy = m_EnnemyList[m_CurrentEnnemyTurn - 1].GetComponent<NPCController>();
            if(tempEnnemy.isDead)
            {
                m_CurrentEnnemyTurn++;
            }
            if(m_CurrentEnnemyTurn > 3)
            {
                m_CurrentEnnemyTurn = 1;
            }
            return m_CurrentEnnemyTurn;
        }
        else if(m_AliveEnnemies == 1)
        {
            for(int i = 0; i < m_EnnemyList.Count; i++)
            {
                NPCController tempEnnemy = m_EnnemyList[i].GetComponent<NPCController>();
                if(!tempEnnemy.isDead)
                {
                    m_CurrentEnnemyTurn = i + 1;
                    return m_CurrentEnnemyTurn;
                }
            }
        }
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
                m_PossibleEnnemyAttacks.Clear();
                return finalennemyattack;

            }
            else
            {
                finalennemyattack = m_PossibleEnnemyAttacks[0];
                m_PossibleEnnemyAttacks.Clear();
                return finalennemyattack;
            }
        }

        m_PossibleEnnemyAttacks.Clear();
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
       CombatManager.Instance.ChangeLifeValue(m_CurrentEnnemyStats, m_CurrentEnnemyAttacked);
       if(m_CurrentEnnemyStats.isDead == true)
        {
            m_CurrentEnnemyAnim.SetBool("isDead", true);
            m_AliveEnnemies--;
            if(m_AliveEnnemies == 0)
            {
                string LastScene = LevelManager.Instance.LastScene;
                LevelManager.Instance.ChangeLevel(LastScene, true, 1);
            }
        }
       //Animator EAnim = m_CurrentEnnemyGO.GetComponent<Animator>();
       m_CurrentFriendlyAnim.SetTrigger("Attack");
    }

    private void EnnemyAttack()
    {
        int tDamage = m_CurrentEnnemyStats.Damage;
        m_CurrentFriendlyStats.CurrentHP -= tDamage;
        CombatManager.Instance.ChangeLifeValue(m_CurrentFriendlyStats, m_CurrentFriendlyAttacked);
        if(m_CurrentFriendlyStats.isDead == true)
        {
            m_CurrentFriendlyAnim.SetBool("isDead", true);
            m_AliveFriendlies--;
            if(m_AliveFriendlies == 0)
            {
                string LastScene = LevelManager.Instance.LastScene;
                LevelManager.Instance.ChangeLevel("MainMenu", true, 3);
            }
        }
        //m_CurrentFriendlyAnim.SetTrigger("TakeDamage");
        m_CurrentEnnemyAnim.SetTrigger("Attack");
    }

    private void BossAttack()
    {
        switch(m_ChosenBossSpell)
        {
            case "IceBurst":
            {
                SpellData tChosenSpell = m_CurrentEnnemyStats.GetSpellData(m_ChosenBossSpell);
                int tDamage = tChosenSpell.m_Damage;
                for(int i = 0; i < m_AliveFriendlies; i++)
                {
                    Instantiate(tChosenSpell.m_SpellPrefab, m_FriendlyIdlePositions[i], Quaternion.identity);
                    m_CurrentFriendlyStats = m_FriendlyList[i].GetComponent<NPCController>();
                    m_CurrentFriendlyAnim = m_FriendlyList[i].GetComponent<Animator>();
                    m_CurrentFriendlyStats.CurrentHP -= tDamage;
                    m_CurrentFriendlyAttacked = i;
                    CombatManager.Instance.ChangeLifeValue(m_CurrentFriendlyStats, m_CurrentFriendlyAttacked + 1);
                    if(m_CurrentFriendlyStats.isDead == true)
                    {
                        m_CurrentFriendlyAnim.SetBool("isDead", true);
                        m_AliveFriendlies--;
                        if(m_AliveFriendlies == 0)
                        {
                            string LastScene = LevelManager.Instance.LastScene;
                            LevelManager.Instance.ChangeLevel("MainMenu", true, 3);
                        }
                    }
                    m_CurrentEnnemyAnim.SetTrigger("Attack");
                    
                }
                break;
            }
            case "Heal":
            {
                SpellData tChosenSpell = m_CurrentEnnemyStats.GetSpellData(m_ChosenBossSpell);
                int tHealAmmount = tChosenSpell.m_Damage;
                m_CurrentEnnemyStats.CurrentHP += tHealAmmount;
                m_CurrentEnnemyAnim.SetBool("isIdle", true);
                CombatManager.Instance.ChangeLifeValue(m_CurrentEnnemyStats, m_CurrentEnnemyAttacked);
                Vector2 tDest = m_CurrentEnnemyGO.transform.position;
                tDest.x -= 1;
                Instantiate(tChosenSpell.m_SpellPrefab, tDest, Quaternion.identity);
                break;
            }
            case "Lazer":
            {
                SpellData tChosenSpell = m_CurrentEnnemyStats.GetSpellData(m_ChosenBossSpell);
                int tDamage = tChosenSpell.m_Damage;
                m_CurrentFriendlyStats.CurrentHP -= tDamage;
                CombatManager.Instance.ChangeLifeValue(m_CurrentFriendlyStats, m_CurrentFriendlyAttacked);
                if(m_CurrentFriendlyStats.isDead == true)
                {
                    m_CurrentFriendlyAnim.SetBool("isDead", true);
                    m_AliveFriendlies--;
                    if(m_AliveFriendlies == 0)
                    {
                        string LastScene = LevelManager.Instance.LastScene;
                        LevelManager.Instance.ChangeLevel("MainMenu", true, 3);
                    }
                }
                Vector2 tDest = m_CurrentEnnemyGO.transform.position + new Vector3(0,-0.5f);
                tDest.x -= 5;
                Instantiate(tChosenSpell.m_SpellPrefab, tDest, Quaternion.identity);
                m_CurrentEnnemyAnim.SetTrigger("Attack");
                break;
            }
        }
    }

    private void SetupBossAttack(string aSpell)
    {
        Animator tBossAnim = m_EnnemyList[0].GetComponent<Animator>();
        m_CurrentEnnemyAnim = tBossAnim;
        m_CurrentEnnemyGO = m_EnnemyList[0];
        m_ChosenBossSpell = aSpell;
        m_StartEnnemyCombat = true;
        m_CurentIdle = m_EnnemyIdlePositions[1] + new Vector2(0,2);
        m_CurrentEnnemyStats = m_EnnemyList[0].GetComponent<NPCController>();
        m_CurrentRend = m_EnnemyList[0].GetComponent<SpriteRenderer>();
        
        switch(aSpell)
        {
            case "IceBurst":
            {
                Vector2 tDest = m_FriendlyIdlePositions[1];
                tDest.x += 4;
                tDest.y += 1;
                m_CurrentDestination = tDest;
                break;
            }
            case "Lazer":
            {
                m_CurrentFriendlyAttacked = RandomizeEnnemyAttack();
                m_CurrentFriendlyStats = m_FriendlyList[m_CurrentFriendlyAttacked - 1].GetComponent<NPCController>();
                m_CurrentFriendlyAnim = m_FriendlyList[m_CurrentFriendlyAttacked -1].GetComponent<Animator>();
                Vector2 tDest = m_FriendlyIdlePositions[m_CurrentFriendlyAttacked - 1] + new Vector2(0, 2);
                tDest.x += 4;
                m_CurrentDestination = tDest;
                break;
            }
            case "Heal":
            {
                Vector2 tDest = m_EnnemyIdlePositions[1] + new Vector2(0,2);
                tDest.x -= 3;
                m_CurrentDestination = tDest;
                break;
            }
        }
    }

    public void SetupBoss(List<CharacterData> aFriendlyList, CharacterData aBossData)
    {
        m_TotalEnnemyCount = 1;
        m_AliveEnnemies = 1;
        m_AliveFriendlies = 3;

        for(int i = 0; i< 3; i++)
        {
            GameObject FGO = Instantiate(aFriendlyList[i].m_CharacterPrefab, m_FriendlyIdlePositions[i], Quaternion.identity);
            m_FriendlyList.Add(FGO);
        }

        GameObject tBoss = Instantiate(aBossData.m_CharacterPrefab, m_EnnemyIdlePositions[1] + new Vector2(0,2), Quaternion.identity);
        m_EnnemyList.Add(tBoss);
        m_BossData = aBossData;
        m_CurrentEnnemyStats = tBoss.GetComponent<NPCController>();
        m_CurrentRend = tBoss.GetComponent<SpriteRenderer>();

    }
}
