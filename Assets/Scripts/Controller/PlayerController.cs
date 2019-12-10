using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineT;

public class PlayerController : MonoBehaviour
{
    protected enum State
    {
        Idle,
        Run,
        Attack,
        COUNT
    }

    [SerializeField]
    private PlayerData m_Data;
    [SerializeField]
    private Rigidbody2D m_Rb;
    [SerializeField]
    private Animator m_Animator;
    [SerializeField]
    private float m_DistanceTravelled = 0;

    private const float ATTACK_TIME = 1f;
    private float m_AttackTimer;
    private float m_Speed;
    private float m_Hp;
    private float m_Timer = 2f;

    private float m_InitialHp = 0;

    private const float TIMER_TO_RETURN_TO_GAMPLAY = 1;
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
    

    protected StateMachine m_SM = new StateMachine();

    public void SetDistanceTravelled(int aDistance)
    {
        m_DistanceTravelled = aDistance;
    }

    private void Awake()
    {
        InitSM();
        GameManager.Instance.PlayerController(this);
        Vector3 lastPos = GameManager.Instance.GetLastPlayerPosition();
        transform.position = lastPos != Vector3.zero ? lastPos : transform.position; 
        m_InitialHp = m_Hp;
    }

    private void Start()
    {
        m_Rb = gameObject.GetComponent<Rigidbody2D>();
        m_Speed = m_Data.m_Speed;
        m_Hp = m_Data.m_Hp;
    }

    private void Update()
    {
        m_SM.UpdateSM();
        if(m_Hp <=0)
        {
            HUDManager.Instance.ActivateGameOverHUD(true);
            m_Timer -= Time.deltaTime;
            if(m_Timer <= 0)
            {
                LevelManager.Instance.ChangeLevel(LevelManager.Instance.LastScene,true,1);
                HUDManager.Instance.ActivateGameOverHUD(false);
                m_Hp = m_InitialHp;
                m_Timer = TIMER_TO_RETURN_TO_GAMPLAY;
            }
        }
    }

    private void FixedUpdate()
    {
        m_SM.FixedUpdateSM();
    }


    #region StateMachine
    protected virtual void InitSM()
    {
        m_SM.AddState((int)State.Idle);
        m_SM.OnEnter((int)State.Idle, OnIdleEnter);
        m_SM.OnUpdate((int)State.Idle, OnIdleUpdate);
        m_SM.OnFixedUpdate((int)State.Idle, OnIdleFixedUpdate);
        m_SM.OnExit((int)State.Idle, OnIdleExit);

        m_SM.AddState((int)State.Attack);
        m_SM.OnEnter((int)State.Attack, OnAttackEnter);
        m_SM.OnUpdate((int)State.Attack, OnAttackUpdate);
        
        m_SM.AddState((int)State.Run);
        m_SM.OnEnter((int)State.Run, OnRunEnter);
        m_SM.OnUpdate((int)State.Run, OnRunUpdate);
        m_SM.OnExit((int)State.Run, OnRunExit);

        m_SM.Init((int)State.Idle);
    }

    private void ChangeState(State aState)
    {
        m_SM.ChangeState((int)aState);
    }

    #endregion

    #region Idle State

    private void OnIdleEnter()
    {
        m_Animator.ResetTrigger("Walk");
        m_Animator.SetBool("isIdle", true);
    }

    protected virtual void OnIdleUpdate()
    {
        if(m_Velocity.x != 0 || m_Velocity.y != 0)
        {
            m_Animator.SetTrigger("Walk");
            m_Animator.SetBool("isIdle", false);
            ChangeState(State.Run);
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            ChangeState(State.Attack);
            return;
        }
                
        if(!m_IsInCombat)
        {
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

    private void OnIdleFixedUpdate()
    {
        m_Rb.velocity = m_Velocity;
    }

    private void OnIdleExit()
    {
        m_Animator.SetBool("isIdle", false);
    }

    #endregion

    #region Attack State

    private void OnAttackEnter()
    {
        m_AttackTimer = ATTACK_TIME;
        m_Animator.SetTrigger("Walk");
        m_Animator.SetTrigger("Attack");
    }

    private void OnAttackUpdate()
    {
        m_AttackTimer -= Time.deltaTime;
        if(m_AttackTimer <= 0f)
        {
            m_Animator.ResetTrigger("Attack");
            m_Animator.ResetTrigger("Attack");
            ChangeState(State.Idle);
        }
    }

    #endregion
  
    #region Run State
    private void OnRunEnter()
    {
        m_Animator.SetTrigger("Walk");
        m_Animator.SetBool("isIdle", false);
    }

    protected virtual void OnRunUpdate()
    {
        if(m_Velocity.x == 0 && m_Velocity.y == 0)
        {
            m_Animator.ResetTrigger("Walk");
            ChangeState(State.Idle);
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            ChangeState(State.Attack);
            return;
        }
                
        if(!m_IsInCombat)
        {
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

    private void OnRunFixedUpdate()
    {
        m_Rb.velocity = m_Velocity;
    }

    private void OnRunExit()
    {
        m_Animator.ResetTrigger("Walk");
    }

    #endregion
}


