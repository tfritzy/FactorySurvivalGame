using System.Collections.Generic;
using System.Drawing;
using Core;
using JetBrains.Annotations;
using UnityEngine.UIElements;

public class SmeltSection : VisualElement
{
    private Label temperatureLabel;
    private VisualElement temperatureGauge;
    private Building building;
    private SmeltingRecipe? renderedRecipe;

    public SmeltSection(Building building)
    {
        this.building = building;
        Setup();
    }

    private void Setup()
    {
        this.renderedRecipe = building.Smelt?.RecipeBeingSmelted;
        this.Clear();

        if (building.Smelt == null || building.Smelt.RecipeBeingSmelted == null)
        {
            return;
        }

        var label = new Label("Smelting");
        label.style.color = UIManager.ColorTheme.PrimaryText;
        label.style.fontSize = 14;
        label.pickingMode = PickingMode.Ignore;
        this.Add(label);

        float tempGaugeWidth = 150;
        VisualElement tempGaugeContainer = new VisualElement();
        tempGaugeContainer.style.backgroundColor = UIManager.ColorTheme.PanelForegroundColor;
        tempGaugeContainer.style.width = tempGaugeWidth;
        tempGaugeContainer.style.height = 10;
        tempGaugeContainer.SetAllBorderRadius(5);
        temperatureGauge = new VisualElement();
        temperatureGauge.style.height = 10;
        temperatureGauge.style.backgroundColor = UIManager.ColorTheme.MainAccent;
        temperatureGauge.SetAllBorderRadius(5);
        float smeltProgress =
            building.Smelt.SmeltingItemTemperature_C /
            building.Smelt.RecipeBeingSmelted.HighestMeltingPoint_C * Smelt.TempRatioRequiredToSmelt;
        temperatureGauge.style.width = smeltProgress * tempGaugeWidth;
        tempGaugeContainer.Add(temperatureGauge);

        float meltingPoint = building.Smelt.RecipeBeingSmelted.HighestMeltingPoint_C;
        VisualElement meltingPointTick = new VisualElement();
        meltingPointTick.style.width = 1;
        meltingPointTick.style.height = 15;
        meltingPointTick.style.position = Position.Absolute;
        meltingPointTick.style.left = meltingPoint / tempGaugeWidth;
        meltingPointTick.style.top = 0;
        tempGaugeContainer.Add(meltingPointTick);
        this.Add(tempGaugeContainer);

        temperatureLabel = new Label();
        temperatureLabel.style.color = UIManager.ColorTheme.MainAccent;
        this.Add(temperatureLabel);
    }

    public void Update()
    {
        if (renderedRecipe != building.Smelt?.RecipeBeingSmelted)
        {
            Setup();
        }

        if (building.Smelt != null && temperatureLabel != null)
        {
            temperatureLabel.text = ((int)building.Smelt.SmeltingItemTemperature_C) + " C";
        }

        if (building.Smelt != null && temperatureGauge != null && building.Smelt.RecipeBeingSmelted != null)
        {
            float smeltProgress =
                building.Smelt.SmeltingItemTemperature_C /
                building.Smelt.RecipeBeingSmelted.HighestMeltingPoint_C * Smelt.TempRatioRequiredToSmelt;
            temperatureGauge.style.width = smeltProgress * 100;
        }
    }
}