using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMover : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_Data;
    [SerializeField]
    private GameObject m_Instructions;
    [SerializeField]
    private GameObject m_Prefab;

    private float m_Speed;
    private float m_Hp;


    private float m_PercentageCompletion;

    private bool m_IsMoving = false;

    private Vector2 m_InitialPos;
    private Vector2 m_WantedPos;
    private bool m_IsInCombat = false;

    private Vector3 m_Right = new Vector3(1,0,0);
    private Vector3 m_Left = new Vector3(-1,0,0);
    private Vector3 m_Up = new Vector3(0,1,0);
    private Vector3 m_Down = new Vector3(1,-1,0);



    public void SetBool(bool aBool)
    {
        m_IsInCombat = aBool;
    }
    public bool GetBool()
    {
        return m_IsInCombat;
    }
    
    private void Start()
    {
        m_Speed = m_Data.m_Speed;
        m_Hp = m_Data.m_Hp;
    }

    private void Update()
    {
        if(!m_IsInCombat)
        {
            if(!m_IsMoving)
            {
                System.Random rand = new System.Random();
                int tRandom1 = rand.Next(-4, 4);
                System.Random tRand = new System.Random();
                int tRandom2 = rand.Next(-4, 4);

                if (tRandom1 == 1)
                {
                    MoveNPC(m_Right);
                }
                else if(tRandom1 == -1)
                {
                    MoveNPC(m_Left);
                }

                if (tRandom2 == 1)
                {
                    MoveNPC(m_Up);
                }
                else if(tRandom2 == -1)
                {
                    MoveNPC(m_Down);
                }
            }

            if (m_IsMoving)
            {
                m_PercentageCompletion += Time.fixedDeltaTime * m_Speed;
                m_PercentageCompletion = Mathf.Clamp(m_PercentageCompletion, 0f, 1f);

                transform.position = Vector3.Lerp(m_InitialPos, m_WantedPos, m_PercentageCompletion);

                if (m_PercentageCompletion >= 1)
                {
                    m_IsMoving = false;
                }
            }
        }
    }

    private void MoveNPC(Vector3 aOrientation)
    {
        m_IsMoving = true;
        m_PercentageCompletion = 0f;
        m_InitialPos = transform.position;
        m_WantedPos = transform.position + aOrientation;
    }

    private void OnTriggerEnter2D(Collider2D aTrig)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        m_IsInCombat = true;
        PlayerController player = aTrig.GetComponent<PlayerController>();
        if(player != null)
        {
            PoolManager.Instance.UseObjectFromPool(m_Prefab, transform.position, transform.rotation);
            m_Instructions.SetActive(true);
        }

        HUDManager.Instance.MainUI.SetShopWindowVisibility(true);

    }

    private void OnTriggerExit2D(Collider2D aTrig)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        m_IsInCombat = false;
        PlayerController player = aTrig.GetComponent<PlayerController>();
        if(player != null)
        {
            m_Instructions.SetActive(false);
        }

        HUDManager.Instance.MainUI.SetShopWindowVisibility(false);
    }
}
