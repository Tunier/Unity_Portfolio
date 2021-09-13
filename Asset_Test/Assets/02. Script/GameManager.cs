using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoSingletone<GameManager>
{
    public GameObject wayPointUI;
    public GameObject inventoryUI;

    public GameObject dieText;
    PlayerInfo player;

    public bool isPause = false;

    //public Texture2D[] cursorImg;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerInfo>();


        isPause = false;

        Vector2 mousePos = new Vector2(-2f, 0);

//#if UNITY_EDITOR

//#else
//        Cursor.SetCursor(cursorImg[0], mousePos, CursorMode.ForceSoftware);
//#endif
    }

    private void Update()
    {
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

    public void OnStatusBottonClick()
    {
        //statusUI.SetActive(!statusUI.activeSelf);
    }

    public void OnInventoryBottonClick()
    {
        //inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    void QuickSlotUseItem(int i)
    {
        //if (quickSlots[i].item != null)
        //{
        //    if (quickSlots[i].item.itemType == Item.ItemType.Equipment)
        //    {
        //        quickSlots[i].EquipItem(quickSlots[0].item);
        //        return;
        //    }

        //    database.UseItem(quickSlots[i].item);

        //    if (quickSlots[i].item.itemType == Item.ItemType.Used)
        //        quickSlots[i].SetSlotCount(-1);
        //}
    }
}
