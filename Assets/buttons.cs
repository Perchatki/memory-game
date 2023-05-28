using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour
{
    [SerializeField] private AudioSource sounds, music;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private swap swap;
    [SerializeField] private flip flip;
    [SerializeField] private Image soundImage;
    [SerializeField] private Sprite soundOn, soundOff;
    public int soundEnabled = 0;
    private void Start()
    {
        soundEnabled = PlayerPrefs.GetInt("soundEnabled", 1);
        sounds.enabled = soundEnabled == 1;
        soundImage.sprite = soundEnabled == 1 ? soundOn : soundOff;
        music.enabled = soundEnabled == 1;
    }

    public void Volume()
    {
        soundEnabled = soundEnabled == 1 ? 0 : 1;
        sounds.enabled = soundEnabled == 1;
        soundImage.sprite = soundEnabled == 1 ? soundOn : soundOff;
        music.enabled = soundEnabled == 1;
    }
    public void Menu()
    {
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0) + int.Parse(text.text));
        swap.Save();
        SceneManager.LoadScene(0);
    }
    public void Play()
    {
        PlayerPrefs.SetInt("continue", 0);
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        PlayerPrefs.SetInt("continue", 1);
        SceneManager.LoadScene(1);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0) + int.Parse(text.text));
        swap.Save();
    }
}
