using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class flip : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    [SerializeField] public AnimationClip backwards, _flip;
    [SerializeField] public int winCount, winMax;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip open, win, lose, success;
    [SerializeField] private TMPro.TMP_Text currentScore, totalScore, winText;
    [SerializeField] private GameObject[] winMenu;
    [SerializeField] private swap swap;
    public id[] cards;
    private bool checking = false;
    void Start()
    {
        cards = new id[2];
    }

    // Update is called once per frame
    void Update()
    {
        totalScore.text = (winCount * 10 + (winCount >= 9 ? 50 : 0)).ToString();
        if (Input.touchCount > 0 && !checking)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if(Physics.Raycast(ray, out hit,100))
                {
                    hit.transform.GetChild(0).GetComponent<Animation>().Play();
                    hit.transform.GetChild(0).GetComponent<Animation>().clip = backwards;
                    if (cards[0] == null)
                    {
                        source.clip = open;
                        source.Play();
                        cards[0] = hit.transform.GetComponent<id>();
                        cards[0].GetComponent<BoxCollider>().enabled = false;
                    }
                    else
                    {
                        source.clip = open;
                        source.Play();
                        cards[1] = hit.transform.GetComponent<id>();
                        cards[1].GetComponent<BoxCollider>().enabled = false;
                        checking = true;
                        StartCoroutine(Check());
                    }
                }
            }
        }
    }

    IEnumerator Check()
    {
        if (cards[0].cardId == cards[1].cardId)
        {
            yield return new WaitForSeconds(.1f);
            source.clip = success;
            source.Play();
            winCount++;
            currentScore.text = (int.Parse(currentScore.text) + 10).ToString();
            cards[0].flipped = 1;
            cards[1].flipped = 1;
            swap.collidersEnabled[cards[0].cardNumber] = false;
            swap.collidersEnabled[cards[1].cardNumber] = false;
            totalScore.text = (winCount * 10).ToString();
            if (winCount >= winMax)
            {
                totalScore.text = "140";
                source.clip = win;
                source.Play();
                yield return new WaitForSeconds(.5f);
                currentScore.text = (int.Parse(currentScore.text) + 50).ToString();
                yield return new WaitForSeconds(.5f);
                winMenu[3].SetActive(true);
                winText.text = "140";
                while (winMenu[0].GetComponent<Image>().color.a < 1)
                {
                    yield return new WaitForEndOfFrame();
                    winMenu[0].GetComponent<Image>().color += new Color(0, 0, 0, 1f * Time.deltaTime);
                    winMenu[1].GetComponent<Image>().color += new Color(0, 0, 0, 1f * Time.deltaTime);
                    winMenu[2].GetComponent<TMPro.TMP_Text>().color += new Color(0, 0, 0, 1f * Time.deltaTime);
                    winMenu[3].GetComponent<Image>().color += new Color(0, 0, 0, 0.1f * Time.deltaTime);
                }
            }
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            Camera.main.GetComponent<Animation>().Play();
            source.clip = lose;
            source.Play();
            yield return new WaitForSeconds(.5f);
            if (cards[1] && cards[0])
            {
                cards[0].transform.GetChild(0).GetComponent<Animation>().Play();
                cards[1].transform.GetChild(0).GetComponent<Animation>().Play();
                cards[0].transform.GetChild(0).GetComponent<Animation>().clip = _flip;
                cards[1].transform.GetChild(0).GetComponent<Animation>().clip = _flip;
                yield return new WaitForSeconds(.3f);
                cards[0].GetComponent<Collider>().enabled = true;
                cards[1].GetComponent<Collider>().enabled = true;
            }
        }
        yield return new WaitForSeconds(.3f);
        cards[0] = null;
        cards[1] = null;
        checking = false;
    }
}
