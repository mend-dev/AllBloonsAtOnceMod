using MelonLoader;
using BTD_Mod_Helper;
using AllBloonsAtOnceMod;
using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Data.Gameplay;
using System.Collections.Generic;
using BTD_Mod_Helper.Api.ModOptions;

[assembly: MelonInfo(typeof(AllBloonsAtOnceMod.AllBloonsAtOnceMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace AllBloonsAtOnceMod;

public class AllBloonsAtOnceMod : BloonsTD6Mod {


    public static readonly ModSettingBool ZeroEndTime = new ModSettingBool(false);

    public override void OnApplicationStart() {
        ModHelper.Msg<AllBloonsAtOnceMod>("AllBloonsAtOnceMod loaded!");
    }

    [HarmonyPatch(typeof(InGame), nameof(InGame.StartMatch))]
    static class StartMatchPatch {
        [HarmonyPrefix]
        private static void Prefix(InGame __instance) {
            foreach (RoundModel rm in __instance.GetGameModel().roundSet.rounds) {
                List<BloonGroupModel> groups = new List<BloonGroupModel>();
                foreach (BloonGroupModel bgm in rm.groups) {
                    groups.Add(bgm);
                }
                rm.ClearBloonGroups();

                foreach (BloonGroupModel bgm in groups) {
                    if (ZeroEndTime) {
                        rm.AddBloonGroup(bgm.bloon, bgm.count, 0, 0);
                    } else {
                        rm.AddBloonGroup(bgm.bloon, bgm.count, 0, bgm.end - bgm.start);
                    }
                }
            }

        }
    }
}