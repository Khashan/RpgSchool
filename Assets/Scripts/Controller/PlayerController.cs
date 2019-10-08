using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerData m_Data;
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_Hp;

    private float m_PercentageCompletion;

    private bool m_IsMoving = false;

    private Vector2 m_InitialPos;
    private Vector2 m_WantedPos;

    [SerializeField]
    private int m_DistanceTravelled = 0;

    public void SetDistanceTravelled(int aDistance)
    {
        m_DistanceTravelled = aDistance;
    }

    private void Awake()
    {
        GameManager.Instance.PlayerController(this);
    }
    private void Start()
    {
        m_Speed = m_Data.m_Speed;
        m_Hp = m_Data.m_Hp;
    }

    private void Update()
    {
        
        float InputsX = Input.GetAxisRaw("Horizontal");
        float InputsY = Input.GetAxisRaw("Vertical");
        if(!m_IsMoving)
        {
            if (InputsX != 0)
            {
                m_IsMoving = true;
                m_PercentageCompletion = 0f;
                m_InitialPos = transform.position;
                m_WantedPos = transform.position + new Vector3(InputsX,0,0);
            }

            else if(InputsY != 0)
            {
                m_IsMoving = true;
                m_PercentageCompletion = 0f;
                m_InitialPos = transform.position;
                m_WantedPos = transform.position + new Vector3(0,InputsY,0);
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
                m_IsMoving = false;
            }
        }
    }


}
