using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonManager : MonoBehaviour
{
    private WorkerLogic workerLogic;
    private CoinCounter coinCounter;
    private UIDocument mainDoc;
    private VisualElement chefButton;
    private VisualElement grillUpgrader;
    private VisualElement storageUpgrader;
    private VisualElement sodaMachineUpgrader;
    private Label valueChef;
    private Label grillValue;
    private Label storageValue;
    private Label sodaMachineValue;
    public Sprite unpressedButton;
    public Sprite pressedButton;
    private int[] ChefSpeedPrice = {25, 50, 75, 125, 200, 450};
    
    private int[] upgraderPrices = {20, 40, 60, 80, 120, 160};

    void Start()
    {
        workerLogic = GameObject.FindGameObjectWithTag("ChefCat").GetComponent<WorkerLogic>();
        coinCounter = GetComponent<CoinCounter>();
        mainDoc = GetComponent<UIDocument>();

        chefButton = mainDoc.rootVisualElement.Q("PlusButtonChef");
        grillUpgrader = mainDoc.rootVisualElement.Q("GrillUpgrader");
        storageUpgrader = mainDoc.rootVisualElement.Q("StorageUpgrader");
        sodaMachineUpgrader = mainDoc.rootVisualElement.Q("SodaMachineUpgrader");

        valueChef = mainDoc.rootVisualElement.Q<Label>("ValueChef");
        grillValue = mainDoc.rootVisualElement.Q<Label>("GrillValue");
        storageValue = mainDoc.rootVisualElement.Q<Label>("StorageValue");
        sodaMachineValue = mainDoc.rootVisualElement.Q<Label>("SodaMachineValue");

        changeChefSpeedValue();
        changeGrillUpCost();
        changeStorageUpCost();
        changeSodaMachineUpCost();
        chefButton.RegisterCallback<ClickEvent>(OnChefButtonClick);
        chefButton.RegisterCallback<PointerDownEvent>(PointerDownSpriteChange);
        chefButton.RegisterCallback<PointerUpEvent>(PointerUpSpriteChange);

        grillUpgrader.RegisterCallback<ClickEvent>(OnGrillUpgraderClick);
        storageUpgrader.RegisterCallback<ClickEvent>(OnStorageUpgraderClick);
        sodaMachineUpgrader.RegisterCallback<ClickEvent>(OnSodaMachineUpgraderClick);
    }

    private void changeChefSpeedValue()
    {
        valueChef.text = ChefSpeedPrice[workerLogic.getAgentSpeedLevel()].ToString();
    }
    private void changeGrillUpCost()
    {
        grillValue.text = upgraderPrices[workerLogic.getGrillIterLevel()].ToString();
    }
    private void changeStorageUpCost()
    {
        storageValue.text = upgraderPrices[workerLogic.getStorageIterLevel()].ToString();
    }
    private void changeSodaMachineUpCost()
    {
        sodaMachineValue.text = upgraderPrices[workerLogic.getSodaMachineIterLevel()].ToString();
    }
    private void PointerUpSpriteChange(PointerUpEvent evt)
    {
        chefButton.style.backgroundImage = new StyleBackground(unpressedButton);

    }
    private void PointerDownSpriteChange(PointerDownEvent evt)
    {
        chefButton.style.backgroundImage = new StyleBackground(pressedButton);
    }

    void OnChefButtonClick(ClickEvent clk)
    {
        if (
            coinCounter.doTransaction(ChefSpeedPrice[workerLogic.getAgentSpeedLevel()])
            )
        {
            workerLogic.IncreaseChefSpeed();
            changeChefSpeedValue();
            Debug.Log("chef speed increased");
        }
        else 
        {
            // create a responsive feedback
            Debug.Log("not enough coins stranger");
        }
    }

    void OnGrillUpgraderClick(ClickEvent clk)
    {
        if (coinCounter.doTransaction(upgraderPrices[workerLogic.getGrillIterLevel()]))
        {
            workerLogic.IncreaseGrillIterLevel();
            changeGrillUpCost();
            Debug.Log("grill iter level increased");
        }
        else 
        {
            // create a responsive feedback
            Debug.Log("not enough coins for grill upgrade stranger");
        }
    }
    void OnStorageUpgraderClick(ClickEvent clk)
    {
        if (coinCounter.doTransaction(upgraderPrices[workerLogic.getStorageIterLevel()]))
        {
            workerLogic.IncreaseStorageIterLevel();
            changeStorageUpCost();
            Debug.Log("storage iter level increased");
        }
        else 
        {
            // create a responsive feedback
            Debug.Log("not enough coins for storage upgrade stranger");
        }
    }
    void OnSodaMachineUpgraderClick(ClickEvent clk)
    {
        if (coinCounter.doTransaction(upgraderPrices[workerLogic.getSodaMachineIterLevel()]))
        {
            workerLogic.IncreaseSodaMachineIterLevel();
            changeSodaMachineUpCost();
            Debug.Log("soda machine iter level increased");
        }
        else 
        {
            // create a responsive feedback
            Debug.Log("not enough coins for soda machine upgrade stranger");
        }
    }
}
