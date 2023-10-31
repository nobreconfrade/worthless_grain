using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonManager : MonoBehaviour
{
    private WorkerLogic workerLogic;
    private UIDocument mainDoc;
    private VisualElement chefButton;
    private bool buttonPressed;
    public Sprite unpressedButton;
    public Sprite pressedButton;
    void Start()
    {
        workerLogic = GameObject.FindGameObjectWithTag("ChefCat").GetComponent<WorkerLogic>();
        mainDoc = GetComponent<UIDocument>();
        chefButton = mainDoc.rootVisualElement.Q("PlusButtonChef");
        chefButton.RegisterCallback<ClickEvent>(OnChefButtonClick);
        chefButton.RegisterCallback<PointerDownEvent>(PointerDownSpriteChange);
        chefButton.RegisterCallback<PointerUpEvent>(PointerUpSpriteChange);
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
        workerLogic.IncreaseChefSpeed();
    }
}
