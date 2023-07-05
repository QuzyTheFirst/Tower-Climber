using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CostumesShopUI : MonoBehaviour
{
    private int _chosenCostume = 0;

    [SerializeField] private GameUIController _gameUIController;
    [SerializeField] private TextMeshProUGUI _buySelectButtonText;

    [SerializeField] private Shop _shop;

    private void Start()
    {
        Costume costume = GameManager.Instance.ShopManager.GetCostume(0);
        _shop.CostumeSwapper.ChangeCostume(costume.transform);
        UpdateBuySelectButton();
    }

    public void Left()
    {
        _chosenCostume--;
        if (_chosenCostume < 0)
            _chosenCostume = 0;

        UpdateBuySelectButton();

        Costume costume = GameManager.Instance.ShopManager.GetCostume((ShopManager.CostumeType)_chosenCostume);
        _shop.CostumeSwapper.ChangeCostume(costume.transform);
    }

    public void Right()
    {
        _chosenCostume++;
        if (_chosenCostume >= GameManager.Instance.ShopManager.CostumesCount)
            _chosenCostume = GameManager.Instance.ShopManager.CostumesCount - 1;

        UpdateBuySelectButton();

        Costume costume = GameManager.Instance.ShopManager.GetCostume((ShopManager.CostumeType)_chosenCostume);
        _shop.CostumeSwapper.ChangeCostume(costume.transform);
    }

    public void MainMenu()
    {
        
    }

    public void BuySelect()
    {
        bool hasSucceeded = GameManager.Instance.ShopManager.BuySelectCostume((ShopManager.CostumeType)_chosenCostume);
        if (hasSucceeded)
        {
            UpdateBuySelectButton();
        }
    }

    private void UpdateBuySelectButton()
    {
        Costume costume = GameManager.Instance.ShopManager.GetCostume((ShopManager.CostumeType)_chosenCostume);
        if (costume.IsBought && costume == GameManager.Instance.ShopManager.SelectedCostume)
        {
            _buySelectButtonText.text = "Selected";
        }
        else if (costume.IsBought)
        {
            _buySelectButtonText.text = "Select";
        }
        else
        {
            _buySelectButtonText.text = $"Buy: {costume.Cost}";
        }
    }
}
