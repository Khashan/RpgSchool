using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_Data;
    [SerializeField]
    private Rigidbody2D m_Rb;
    [SerializeField]
    private Animator m_Animator;

    private float m_Speed;
    private float m_Hp;


    private bool m_IsInCombat = false;
    private Vector2 m_Velocity;

    public void SetBool(bool aBool)
    {
        m_IsInCombat = aBool;
    }
    public bool GetBool()
    {
        return m_IsInCombat;
    }
    
    [SerializeField]
    private float m_DistanceTravelled = 0;

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
        m_Rb = gameObject.GetComponent<Rigidbody2D>();
        m_Speed = m_Data.m_Speed;
        m_Hp = m_Data.m_Hp;
    }

    private void Update()
    {
        if(m_Velocity.x == 0 && m_Velocity.y == 0)
        {
            m_Animator.ResetTrigger("Walk");
            m_Animator.SetBool("isIdle", true);
        }
        else
        {
            m_Animator.SetTrigger("Walk");
            m_Animator.SetBool("isIdle", false);
        }
        
        if(Input.GetKeyDown(KeyCode.K))
        {
            GameManager.Instance.StartBoss();
        }

        if(!m_IsInCombat)
        {
            CollectPotion();

            m_Velocity = transform.right * Input.GetAxisRaw("Horizontal");
            m_Velocity += (Vector2)transform.up * Input.GetAxisRaw("Vertical");
            m_Velocity *= m_Speed;

            if(m_Velocity != Vector2.zero)
            {
                m_DistanceTravelled += Time.deltaTime;
                GameManager.Instance.GetPlayerDistance(m_DistanceTravelled);
            }
        }
    }

    private void FixedUpdate()
    {
        m_Rb.velocity = m_Velocity;
    }

    private void CollectPotion()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //Collect Potion put it on inventory
        }
    }
}
