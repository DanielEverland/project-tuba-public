using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Changes the current scene to a target scene
/// </summary>
public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private string sceneName = default;
    
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
