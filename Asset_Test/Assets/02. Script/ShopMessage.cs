using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMessage : MonoBehaviour
{
    [SerializeField]
    PlayerInfo player;
    [SerializeField]
    Inventory inven;
    [SerializeField]
    Slot slot;
    [SerializeField]
    Shop_Test shop;
    [SerializeField]
    Text messageTxt;
    [SerializeField]
    GameObject messageBackground;
    [SerializeField]
    GameObject quantityMessage;
    [SerializeField]
    Text quantityTxt;

    public InputField inputField;

    Item item;
    int selectNum;
    int count = 0;
    int lastCount;

    private void Start()
    {

    }
    private void Update()
    {
        if (item != null)
            CheckCount(item);
    }

    public void OnClilckYesButton()
    {
        switch (selectNum)
        {
            case 0:
                SellItem(item);
                messageBackground.SetActive(false);
                break;
            case 1:
                //ĳ���Ϳ��� �Ǹ��Ҷ�
                //�Ǹ��Լ� �־��ֱ�
                messageBackground.SetActive(false);
                break;
            case 2:
                messageBackground.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void OnClickQuantityYes()
    {
        switch (selectNum)
        {
            case 0:
                SellItem(item, lastCount);
                quantityMessage.SetActive(false);
                break;
            case 1:
                quantityMessage.SetActive(false);
                break;
            case 2:
                quantityMessage.SetActive(false);
                break;
        }
    }
    public void OnClickNoButton()
    {
        messageBackground.SetActive(false);
        quantityMessage.SetActive(false);
    }

    public void ShowMessageTxt(Item _item ,int _num)
    {
        messageBackground.SetActive(true);
        
        item = _item;
        selectNum = _num;
        switch (_num)
        {
            case 0:
                messageTxt.text = "���� �Ͻðڽ��ϱ�?";
                break;
            case 1:
                messageTxt.text = "�Ǹ� �Ͻðڽ��ϱ�?";
                break;
            case 2:
                messageTxt.text = "��尡 �����մϴ�.";
                break;
            default:
                break;
        }
    }

    public void ShowQuantityTxt(Item _item, int _num)
    {
        quantityMessage.SetActive(true);
        item = _item;
        selectNum = _num;
        
        switch (_num)
        {
            case 0:
                quantityTxt.text = "���� ����";
                break;
            case 1:
                quantityTxt.text = "�Ǹ� ����";
                break;
            case 2:
                break;
        }
    }
    public void SellItem(Item _item,int _count =1)
    {
        if (player.stats.Gold >= _item.BuyCost)
        {
            player.stats.Gold -= _item.BuyCost*_count;
            inven.GetItem(_item,_count);
        }
        else
        {
            ShowMessageTxt(item, 2);
        }
    }
    
    public void CheckCount(Item _item)
    {
        switch (selectNum)
        {
            case 0:
                count = player.stats.Gold / _item.BuyCost;

                if (inputField.text != "")
                {
                    if (count < int.Parse(inputField.text))
                    {
                        inputField.text = count.ToString();
                        lastCount = count;
                    }
                    else
                    {
                        lastCount = int.Parse(inputField.text);
                    }
                }
                break;
            case 1:
                count = slot.itemCount;
                if(inputField.text != "")
                {
                    if(count < int.Parse(inputField.text))
                    {
                        inputField.text = count.ToString();
                    }
                    else
                    {

                    }
                }
                break;
        }
        
    }
    
    public void BuyItem(Item _item)
    {
        //�������忡�� ������ �����Ҷ�
    }
}
