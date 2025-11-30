using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip electricButton;
    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    public void PlayElectricButton() {
        audioSource.PlayOneShot(electricButton);
    }
}
