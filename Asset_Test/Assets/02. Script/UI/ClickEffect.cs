using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    [SerializeField]
    Canvas clickEffectCanvas;
    [SerializeField]
    GameObject cameraArm;
    PlayerMovement playerMove;

    Ray ray;
    RaycastHit hit;

    void Awake()
    {
        playerMove = FindObjectOfType<PlayerMovement>();
    }

    private void Start()
    {
        clickEffectCanvas.enabled = false;
    }

    void Update()
    {
        if (clickEffectCanvas.enabled)
            clickEffectCanvas.transform.forward = cameraArm.transform.forward;
    }

    public IEnumerator ClickEffectCtrl(Vector3 _pos)
    {
        clickEffectCanvas.transform.position = _pos;

        clickEffectCanvas.enabled = true;

        yield return new WaitForSeconds(1f);

        clickEffectCanvas.enabled = false;
    }
}
