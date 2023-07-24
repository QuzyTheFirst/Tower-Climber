using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class CostumesShopUI : MonoBehaviour
{
    private int _chosenCostume = 0;

    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI _coinsText;

    [Header("Buttons")]
    [SerializeField] private TextMeshProUGUI _buySelectButtonText;

    [Header("Shop")]
    [SerializeField] private Shop _shop;

    [Header("Localization")]
    [SerializeField] private LocalizedString _localStringPrice;

    private void OnEnable()
    {
        _localStringPrice.Arguments = new object[] { GameManager.Instance.ShopManager.SelectedCostume.Cost };
        _localStringPrice.StringChanged += ButButtonTextChanged;
    }

    private void OnDisable()
    {
        _localStringPrice.StringChanged -= ButButtonTextChanged;
    }

    private void Start()
    {
        Costume costume = GameManager.Instance.ShopManager.SelectedCostume;
        _chosenCostume = (int)costume.CostumeType;
        _shop.CostumeSwapper.ChangeCostume(costume);
        UpdateBuySelectButton();
    }

    public void UpdateUI(int coins)
    {
        _coinsText.text = coins.ToString();
    }

    public void Left()
    {
        _chosenCostume--;
        if (_chosenCostume < 0)
            _chosenCostume = 0;

        UpdateBuySelectButton();

        Costume costume = GameManager.Instance.ShopManager.GetCostume((ShopManager.CostumeType)_chosenCostume);
        _shop.CostumeSwapper.ChangeCostume(costume);
    }

    public void Right()
    {
        _chosenCostume++;
        if (_chosenCostume >= GameManager.Instance.ShopManager.CostumesCount)
            _chosenCostume = GameManager.Instance.ShopManager.CostumesCount - 1;

        UpdateBuySelectButton();

        Costume costume = GameManager.Instance.ShopManager.GetCostume((ShopManager.CostumeType)_chosenCostume);
        _shop.CostumeSwapper.ChangeCostume(costume);
    }

    public void BuySelect()
    {
        bool hasSucceeded = GameManager.Instance.ShopManager.BuySelectCostume((ShopManager.CostumeType)_chosenCostume);
        if (hasSucceeded)
        {
            UpdateBuySelectButton();
        }
    }

    public void UpdateBuySelectButton()
    {
        Costume costume = GameManager.Instance.ShopManager.GetCostume((ShopManager.CostumeType)_chosenCostume);
        if (costume.IsBought && costume == GameManager.Instance.ShopManager.SelectedCostume)
        {
            _localStringPrice.TableEntryReference = "ID_Costumes_Selected";
            //_buySelectButtonText.text = "Selected";
        }
        else if (costume.IsBought)
        {
            _localStringPrice.TableEntryReference = "ID_Costumes_Select";
            //_buySelectButtonText.text = "Select";
        }
        else
        {
            _localStringPrice.TableEntryReference = "ID_Costumes_Buy";
            _localStringPrice.Arguments[0] = costume.Cost;
        }
        _localStringPrice.RefreshString();
    }

    public void RestartCostumeUI()
    {
        _chosenCostume = 0;
        _shop.CostumeSwapper.ChangeCostume(GameManager.Instance.ShopManager.SelectedCostume);
        UpdateBuySelectButton();
    }

    private void ButButtonTextChanged(string value)
    {
        _buySelectButtonText.text = value;
    }

    public void ToMainMenu()
    {
        GameUIController.Instance.ToggleMainMenu(true);
        GameUIController.Instance.ToggleCostumesShop(false);

        GameManager.Instance.SwitchCameraToMainMenu();
    }
}
