using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void loadScene(string a_Scene)
    {
        LevelManager.Instance.ChangeLevel(a_Scene, true, 0);
    }
}
