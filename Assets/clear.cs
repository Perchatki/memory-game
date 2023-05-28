using UnityEngine;

public class clear : MonoBehaviour
{
    [SerializeField] bool delete;
    void Start()
    {
        if (delete)
            PlayerPrefs.DeleteAll();
    }
}
