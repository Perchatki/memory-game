using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class swap : MonoBehaviour
{
    [SerializeField] private Material[] sprites;
    [SerializeField] private Transform[] cards;
    [SerializeField] private flip flip;
    [SerializeField] private buttons buttons;
    List<int> exclude;
    public Vector3[] rotations;
    public bool[] collidersEnabled;
    public int[] cardIds;
    public int[] flipped;
    void Start()
    {
        if (PlayerPrefs.GetInt("continue") == 0 || PlayerPrefs.GetString("save","").Length < 3)
        {
            Swap();
        }
        else
        {
            Load();
        }
    }

    // Update is called once per frame
    public void Swap()
    {
        for (int ii = 0; ii < 18; ii++)
        {
            cards[ii].GetChild(0).GetComponent<Animation>().Stop();
            cards[ii].transform.eulerAngles = rotations[1];
            flip.winCount = 0;
            cards[ii].GetComponent<id>().flipped = 0;
        }
        exclude = new List<int>();
        flip.winCount = 0;
        if (flip.cards[0] != null)
        {
            flip.cards[0] = null;
        }
        if (flip.cards[1] != null)
        {
            flip.cards[1] = null;
        }

        foreach (Transform i in cards)
        {
            i.GetComponentInChildren<Animation>().clip = flip.backwards;
            i.GetComponentInChildren<Animation>().Play();
            i.GetComponent<Collider>().enabled = false;
            StartCoroutine(EnableColliders());
            i.GetChild(0).GetComponent<Animation>().clip = flip._flip;
        }

        while (exclude.Count < cards.Length)
        {
            int card1 = UnityEngine.Random.Range(0, cards.Length);
            int card2 = UnityEngine.Random.Range(0, cards.Length);
            if (!exclude.Contains(card1) && !exclude.Contains(card2) && card2 != card1)
            {
                int spriteId = UnityEngine.Random.Range(0, sprites.Length);
                cards[card1].GetComponent<id>().cardId = spriteId;
                cards[card2].GetComponent<id>().cardId = spriteId;
                cards[card1].GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = sprites[spriteId];
                cards[card2].GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = sprites[spriteId];
                exclude.Add(card1); exclude.Add(card2);
            }
        }
    }

    public void Load()
    {
        cardIds = JsonUtility.FromJson<SaveValues>(PlayerPrefs.GetString("save", "")).ids;
        flipped = JsonUtility.FromJson<SaveValues>(PlayerPrefs.GetString("save", "")).flipped;
        for (int ii = 0; ii < 18; ii++)
        {
            cards[ii].GetComponent<id>().cardId = cardIds[ii];
            cards[ii].GetComponent<id>().flipped = flipped[ii];
            cards[ii].GetComponent<BoxCollider>().enabled = flipped[ii] == 0;
            cards[ii].GetChild(0).eulerAngles = rotations[flipped[ii]];
            cards[ii].GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = sprites[cardIds[ii]];
        }
        int flippedAmount = 0;
        foreach (int i in flipped)
        {
            flippedAmount += i;
        }
        flip.winCount = flippedAmount / 2;
        if (flip.winCount >=9)
        {
            Swap();
        }
    }

    public void Save()
    {
        cardIds = new int[18];
        int ii = 0;
        foreach (Transform i in cards)
        {
            cardIds[i.GetComponent<id>().cardNumber] = i.GetComponent<id>().cardId;
            cards[ii].GetComponent<BoxCollider>().enabled = flipped[ii] == 0;
            flipped[ii] = i.GetComponent<id>().flipped;
            ii++;
        }
        SaveValues save = new SaveValues();
        save.ids = cardIds;
        save.flipped = flipped;
        PlayerPrefs.SetString("save", JsonUtility.ToJson(save));
        PlayerPrefs.SetInt("soundEnabled", buttons.soundEnabled);
    }
    IEnumerator EnableColliders()
    {
        yield return new WaitForSeconds(1f);
        foreach (Transform i in cards)
        {
            i.GetComponent<Collider>().enabled = true;
        }
    }
}


[System.Serializable]
struct SaveValues
{
    public int[] ids;
    public int[] flipped;
}

