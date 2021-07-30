using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Indicator_Test : MonoBehaviour
{
    public Image abilityIndicatorImage;

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6);

        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, 20f);

        var newHitPos = transform.position + hitPosDir * distance;
        abilityIndicatorImage.rectTransform.position = newHitPos + new Vector3(0, 0.2f, 0);
    }
}
