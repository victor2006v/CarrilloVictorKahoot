using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] protected SceneField sceneToLoad;
    public void ChangeScene() {
        SceneManager.LoadScene(sceneToLoad);
    }
}
