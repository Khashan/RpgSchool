using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField]
    private string m_SceneToGo;

    private void Start()
    {
        LevelManager.Instance.ChangeLevel(m_SceneToGo, false, 0);
    }

}
