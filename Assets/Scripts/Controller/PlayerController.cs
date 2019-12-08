using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_Data;
    private float m_Speed;
    private float m_Hp;

    private float m_PercentageCompletion;

    private bool m_IsMoving = false;

    private Vector2 m_InitialPos;
    private Vector2 m_WantedPos;
    private bool m_IsInCombat = false;
    private Vector3 m_Movement = new Vector3();

    public void SetBool(bool aBool)
    {
        m_IsInCombat = aBool;
    }
    public bool GetBool()
    {
        return m_IsInCombat;
    }
    
    [SerializeField]
    private int m_DistanceTravelled = 0;

    public void SetDistanceTravelled(int aDistance)
    {
        m_DistanceTravelled = aDistance;
    }

    private void Awake()
    {
        GameManager.Instance.PlayerController(this);

        Vector3 lastPos = GameManager.Instance.GetLastPlayerPosition();
        transform.position = lastPos != Vector3.zero ? lastPos : transform.position; 
    }

    private void Start()
    {
        m_Speed = m_Data.m_Speed;
        m_Hp = m_Data.m_Hp;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            GameManager.Instance.StartBoss();
        }

        if(!m_IsInCombat)
        {
            CollectPotion();
            float InputsX = Input.GetAxisRaw("Horizontal");
            float InputsY = Input.GetAxisRaw("Vertical");
            if(!m_IsMoving)
            {
                if (InputsX != 0)
                {
                    m_IsMoving = true;
                    m_PercentageCompletion = 0f;
                    m_InitialPos = transform.position;
                    m_Movement.x = InputsX;
                    m_WantedPos = transform.position + m_Movement;
                }

                else if(InputsY != 0)
                {
                    m_IsMoving = true;
                    m_PercentageCompletion = 0f;
                    m_InitialPos = transform.position;
                    m_Movement.y = InputsY;
                    m_WantedPos = transform.position + m_Movement;
                }
            }

            if (m_IsMoving)
            {
                m_PercentageCompletion += Time.fixedDeltaTime * m_Speed;
                m_PercentageCompletion = Mathf.Clamp(m_PercentageCompletion, 0f, 1f);

                transform.position = Vector3.Lerp(m_InitialPos, m_WantedPos, m_PercentageCompletion);

                if (m_PercentageCompletion >= 1)
                {
                    m_DistanceTravelled++;
                    GameManager.Instance.GetPlayerDistance(m_DistanceTravelled);
                    m_Movement = Vector3.zero;
                    m_IsMoving = false;
                }
            }
        }
    }

    private void CollectPotion()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //Collect Potion put it on inventory
        }
    }
    private void OnCollisionEnter2D(Collision2D aCol)
    {
        if(aCol.gameObject.layer == 10)
        {
            m_WantedPos = m_InitialPos;
        }
    }
}
