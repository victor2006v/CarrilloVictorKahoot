using UnityEngine;

public class Systems : MonoBehaviour{
    public static Systems Instance{ get; private set; }
    private void Awake() {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } 
        else Destroy(this.gameObject);
    }
}
