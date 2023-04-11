using System.Linq;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Mods;
using BetterArmorPiercing;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Helpers;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppSystem.Collections.Generic;
using MelonLoader;

[assembly:
    MelonInfo(typeof(BetterArmorPiercingMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BetterArmorPiercing;

public class BetterArmorPiercingMod : BloonsTD6Mod
{
    private static readonly ModSettingInt ArmorPiercingDartsCost = new(3000)
    {
        displayName = "Armor Piercing Darts Cost",
        min = 0,
        icon = VanillaSprites.ArmorPiercingDartsUpgradeIcon
    };

    private static readonly ModSettingInt HeatTippedDartsBonus = new(1)
    {
        displayName = "Heat Tipped Darts Bonus Damage w/ Armor Piercing",
        min = 0,
        max = 10,
        slider = true,
        icon = VanillaSprites.HeatTippedDartUpgradeIcon
    };

    public override void OnNewGameModel(GameModel gameModel, List<ModModel> mods)
    {
        gameModel.GetUpgrade(UpgradeType.ArmorPiercingDarts).cost =
            CostHelper.CostForDifficulty(ArmorPiercingDartsCost, mods);

        foreach (var towerModel in gameModel.GetTowersWithBaseId(TowerType.MonkeySub)
                     .Where(model => model.appliedUpgrades.Contains(UpgradeType.ArmorPiercingDarts)))
        {
            var damageModel = towerModel.GetWeapon()!.projectile.GetDamageModel()!;
            damageModel.immuneBloonProperties = BloonProperties.None;
            if (towerModel.appliedUpgrades.Contains(UpgradeType.HeatTippedDarts))
            {
                damageModel.damage += HeatTippedDartsBonus;
            }
        }
    }
}