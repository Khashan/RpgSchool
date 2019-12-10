using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Anderson.CustomWindows
{
    [System.Serializable]
    public struct WindowSettings
    {
        [Header("Canvas")]
        public bool m_ShowOnAwake;
        public bool m_IsMovable;
        public bool m_BlockRaycast;

        [Header("Logic")]
        [Tooltip("The Unity's Update callback will keep running event if the window is not focused.")]
        public bool m_AlwaysUpdating;

        [Header("Transition")]
        public WindowTransition m_WindowTransition;
    }

    public enum WindowTransition
    {
        None,
        Fading,
        Zooming,
    }

    public abstract class CustomWindow : MonoBehaviour, IWindowLogic, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private const int OFFSCREEN_OFFSET = 5;       //Used to make sure the window is still clickable if the player drag it out of the screen;

        [SerializeField]
        private WindowSettings m_WindowSettings;

        [Header("UI")]
        [SerializeField]
        private CanvasGroup m_WindowCanvasGroup;
        [SerializeField]
        private Button m_CloseWindow;
        [SerializeField]
        private Collider2D m_DraggableZone;
        [SerializeField]
        private GameObject m_EventSystemFocusOnOpen;

        private bool m_IsDragging;
        private bool m_IsOpen;

        private Vector2 m_OffsetDragging;

        private void Awake()
        {
            if (m_WindowSettings.m_ShowOnAwake)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        private void Update()
        {
            if (m_WindowSettings.m_AlwaysUpdating || m_IsOpen)
            {
                OnUpdate();
            }
        }

        protected abstract void OnUpdate();

        private void Start()
        {
            m_CloseWindow?.onClick.AddListener(Close);
        }

        public virtual void Close()
        {
            if(!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            CancelTransition();

            m_IsOpen = false;
            StartCoroutine(WaitForTransition());
        }

        public virtual void Open()
        {
            if(!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            CancelTransition();

            m_IsOpen = true;
            StartCoroutine(WaitForTransition());
            Focus();
        }

        public virtual void Focus()
        {
            transform.SetAsLastSibling();

            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(m_EventSystemFocusOnOpen);
            }
        }

        public bool IsOpen()
        {
            return m_IsOpen;
        }

        private void CancelTransition()
        {
            StopAllCoroutines();

            switch (m_WindowSettings.m_WindowTransition)
            {
                case WindowTransition.Zooming:
                    {
                        m_WindowCanvasGroup.transform.localScale = m_IsOpen ? Vector3.one : Vector3.zero;
                        break;
                    }

                case WindowTransition.None:
                case WindowTransition.Fading:
                    {
                        m_WindowCanvasGroup.alpha = m_IsOpen ? 1 : 0;
                        break;
                    }
            }

            m_WindowCanvasGroup.blocksRaycasts = m_IsOpen ? m_WindowSettings.m_BlockRaycast : false;
        }

        private IEnumerator WaitForTransition()
        {
            switch (m_WindowSettings.m_WindowTransition)
            {
                case WindowTransition.None:
                    {
                        m_WindowCanvasGroup.alpha = m_IsOpen ? 1 : 0;
                        break;
                    }

                case WindowTransition.Fading:
                    {
                        bool effectDone = false;

                        while (!effectDone)
                        {
                            FadingEffect(m_IsOpen);

                            effectDone = (m_IsOpen && m_WindowCanvasGroup.alpha == 1) || (!m_IsOpen && m_WindowCanvasGroup.alpha == 0);
                            yield return new WaitForEndOfFrame();
                        }
                        break;
                    }

                case WindowTransition.Zooming:
                    {
                        m_DraggableZone.enabled = false; //Safety since a collider will stop working at 0 of scaling
                        m_WindowCanvasGroup.alpha = 1;
                        bool effectDone = false;

                        while (!effectDone)
                        {
                            ZoomingEffect(m_IsOpen);

                            effectDone = (m_IsOpen && transform.localScale.x == 1) || (!m_IsOpen && transform.localScale.x == 0);
                            yield return new WaitForEndOfFrame();
                        }

                        m_DraggableZone.enabled = m_IsOpen ? true : false;
                        break;
                    }
            }

            m_WindowCanvasGroup.blocksRaycasts = m_IsOpen ? m_WindowSettings.m_BlockRaycast : false;

            if (!m_IsOpen)
            {
                gameObject.SetActive(false);
                m_IsDragging = false;
            }

            yield return null;
        }

        private void FadingEffect(bool a_IsOpening)
        {
            if (a_IsOpening)
            {
                m_WindowCanvasGroup.alpha += 0.1f;
            }
            else
            {
                m_WindowCanvasGroup.alpha -= 0.1f;
            }
        }

        private void ZoomingEffect(bool a_IsOpening)
        {
            Vector3 scaling = transform.localScale;

            if (a_IsOpening)
            {
                scaling.x += 0.1f;
                scaling.y += 0.1f;
                scaling.z += 0.1f;

                if (scaling.x > 1)
                {
                    scaling = Vector3.one;
                }
            }
            else
            {
                scaling.x -= 0.1f;
                scaling.y -= 0.1f;
                scaling.z -= 0.1f;

                if (scaling.x < 0)
                {
                    scaling = Vector3.zero;
                }
            }

            transform.localScale = scaling;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (m_WindowSettings.m_IsMovable)
            {
                if (m_DraggableZone.bounds.Contains(eventData.position))
                {
                    m_OffsetDragging = eventData.position - (Vector2)transform.position;
                    m_IsDragging = true;
                }
            }
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (m_IsDragging)
            {
                Vector2 mousePosition = eventData.position;

                if (mousePosition.x > Screen.width)
                {
                    mousePosition.x = Screen.width - OFFSCREEN_OFFSET;
                }

                if (mousePosition.x < 0)
                {
                    mousePosition.x = 0 + OFFSCREEN_OFFSET;
                }

                if (mousePosition.y > Screen.height)
                {
                    mousePosition.y = Screen.height - OFFSCREEN_OFFSET;
                }

                if (mousePosition.y < 0)
                {
                    mousePosition.y = 0 + OFFSCREEN_OFFSET;
                }

                transform.position = mousePosition - m_OffsetDragging;

            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (m_IsDragging)
            {
                Focus();
                m_IsDragging = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Focus();
        }
    }
}
