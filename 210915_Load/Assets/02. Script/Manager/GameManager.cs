using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingletone<GameManager>
{
    public GameObject wayPointUI;
    public GameObject inventoryUI;
    public GameObject statusUI;
    public GameObject skilltreeUI;
    public GameObject PauseCanvas;

    public GameObject dieText;

    PlayerInfo player;
    Inventory inventory;

    public bool isPause = false;

    //public Texture2D[] cursorImg;


    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerInfo>();
        inventory = FindObjectOfType<Inventory>();

        isPause = false;

        //Vector2 mousePos = new Vector2(-2f, 0);
    }
    private void Start()
    {
        //#if UNITY_EDITOR

        //#else
        //        Cursor.SetCursor(cursorImg[0], mousePos, CursorMode.ForceSoftware);
        //#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (wayPointUI.activeSelf || inventoryUI.activeSelf || statusUI.activeSelf || skilltreeUI.activeSelf)
            {
                wayPointUI.SetActive(false);
                inventoryUI.SetActive(false);
                skilltreeUI.SetActive(false);
                statusUI.SetActive(false);
            }
            else if (!wayPointUI.activeSelf && !inventoryUI.activeSelf && !statusUI.activeSelf && !skilltreeUI.activeSelf)
                isPause = !isPause;
        }

        if (isPause)
        {
            PauseCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            PauseCanvas.SetActive(false);
            Time.timeScale = 1;
        }

        //if (player.state == PlayerCtrl.State.DIE)
        //{
        //    dieText.SetActive(true);
        //    if (Input.GetKeyDown(KeyCode.R))
        //    {
        //        SceneManager.LoadScene("01. Stage");
        //    }
        //}
        //else
        //{
        //    dieText.SetActive(false);
        //}

        //#if UNITY_EDITOR

        //#else
        //        if (shop.isBuying)
        //        {
        //            Cursor.SetCursor(GameManager.instance.cursorImg[1], Vector2.zero, CursorMode.ForceSoftware);
        //        }
        //        else if (shop.isSelling)
        //        {
        //            Cursor.SetCursor(GameManager.instance.cursorImg[2], Vector2.zero, CursorMode.ForceSoftware);
        //        }
        //        else
        //        {
        //            Cursor.SetCursor(GameManager.instance.cursorImg[0], Vector2.zero, CursorMode.ForceSoftware);
        //        }
        //#endif
    }

    public void OnPauseClick()
    {
        isPause = !isPause;
    }

    public void OnStatusButtonClick()
    {
        statusUI.SetActive(!statusUI.activeSelf);
    }

    public void OnInventoryButtonClick()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public void OnSkillTreeButtonClick()
    {
        skilltreeUI.SetActive(!skilltreeUI.activeSelf);
    }

    public void OnClickSaveButton()
    {
        player.SavePlayerInfo();
        inventory.SaveInven();
    }
    public void OnExitButtonClick()
    {
        player.SavePlayerInfo();
        inventory.SaveInven();
        // ���۾� �ε��ϴ� ��� �߰��ʿ�.
    }
}