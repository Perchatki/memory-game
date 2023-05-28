using UnityEngine;

public class score : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMPro.TMP_Text>().text = PlayerPrefs.GetInt("score", 0).ToString();
    }
}
