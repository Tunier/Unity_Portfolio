using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Indicator_Test : MonoBehaviour
{
    [Header("원형 범위 스킬")]
    public GameObject circleIndicator;
    public Image circleIndicatorImage;
    [Header("직선형 범위 스킬")]
    public GameObject straightIndicator;
    public Image straightIndicatorImage;

    GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");

        circleIndicator.SetActive(false);
        straightIndicator.SetActive(false);
    }

    void Update()
    {
        transform.position = player.transform.position;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6);

        var hitPosDir = (hit.point - player.transform.position).normalized;
        float distance = Vector3.Distance(hit.point, player.transform.position);
        distance = Mathf.Min(distance, 20f);

        var newHitPos = player.transform.position + hitPosDir * distance;
        circleIndicatorImage.rectTransform.position = newHitPos + new Vector3(0, 0.2f, 0);

        straightIndicator.transform.localEulerAngles = new Vector3(0, 0, -player.transform.eulerAngles.y);
    }
}
