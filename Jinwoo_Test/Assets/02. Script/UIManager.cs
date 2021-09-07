using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject hotKeyGuid;
    public GameObject hotKeyGuidTarget;
    public Image fadeImg;
    public ButtonCtrl buttonCtrl;

    public List<GameObject> wayPoints = new List<GameObject>();
    public List<GameObject> merchants = new List<GameObject>();

    [SerializeField]
    private Text explainTxt;
    [SerializeField]
    GameObject wayPointUI;
    [SerializeField]
    GameObject player;

    float recognitionRange;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        hotKeyGuid.SetActive(false);
    }

    private void Start()
    {
        wayPoints.AddRange(GameObject.FindGameObjectsWithTag("WAYPOINT"));
        merchants.AddRange(GameObject.FindGameObjectsWithTag("MERCHANT"));
        recognitionRange = 2.5f;
    }

    private void Update()
    {
        ShowExplainTxt();
        CheckDistance();
        ExitDistance();
    }

    public void ShowExplainTxt()
    {
        if (hotKeyGuid.activeSelf)
        {
            if (hotKeyGuidTarget.CompareTag("WAYPOINT"))
            {
                explainTxt.text = "작동시키기";
            }
            else if (hotKeyGuidTarget.CompareTag("MERCHANT")) //태그가 마을사람,상점이면 다르게 글자가 나오게
            {
                explainTxt.text = "대화하기";
            }
        }
    }

    private void CheckDistance()
    {
        foreach (GameObject _wayPoint in wayPoints) //웨이포인트 작용 키 활성화
        {
            if (Vector3.Distance(player.transform.position, _wayPoint.transform.position) <= recognitionRange)
            {
                if (!wayPointUI.activeSelf)
                {
                    hotKeyGuid.SetActive(true);
                    hotKeyGuidTarget = _wayPoint;
                    return;
                }
            }
            else
            {
                hotKeyGuid.SetActive(false);
            }
        }

        foreach (GameObject _merchant in merchants) //상점 or NPC 상호작용 키 활성화
        {
            if (Vector3.Distance(player.transform.position, _merchant.transform.position) <= recognitionRange)
            {
                hotKeyGuid.SetActive(true);
                hotKeyGuidTarget = _merchant;
                return;
            }
            else
            {
                hotKeyGuid.SetActive(false);

            }
        }
    }

    /// <summary>
    /// 거리가 멀어지면 UI꺼지게
    /// </summary>
    public void ExitDistance()
    {
        if (wayPointUI.activeSelf)
        {
            if (Vector3.Distance(player.transform.position, hotKeyGuidTarget.transform.position) > recognitionRange
                || hotKeyGuidTarget == null)
            {
                wayPointUI.SetActive(false);
            }
        }
    }

    /// <summary>
    /// fadein fadeout 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeCoroutine(float _darkTime,int _waynumber)
    {
        float fadeCount = 0;
        while(fadeCount < 1.0f)
        {
            fadeCount += 0.03f;
            yield return new WaitForSeconds(0.03f);
            fadeImg.color = new Color(0, 0, 0, fadeCount);
        }
        buttonCtrl.moveWaypoint(_waynumber);
        yield return new WaitForSeconds(_darkTime);
        while(fadeCount > 0)
        {
            fadeCount -= 0.03f;
            yield return new WaitForSeconds(0.03f);
            fadeImg.color = new Color(0, 0, 0, fadeCount);
        }
    }
}
