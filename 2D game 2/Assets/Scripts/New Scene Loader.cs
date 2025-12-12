using UnityEngine;
using UnityEngine.SceneManagement;
public class NewSceneLoader : MonoBehaviour
{
    public void LoadNewScene(int _sceneIndex)
    { 
        SceneManager.LoadScene(_sceneIndex);
    }

}
