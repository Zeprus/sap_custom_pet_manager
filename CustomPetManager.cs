using System.Collections.Generic;
using System;
using UnityEngine;
using HarmonyLib;
using Spacewood.Core.Enums;
using Spacewood.Unity;

namespace Zeprus.Sap {
    public class CustomPetManager : MonoBehaviour {
        
        private static BepInEx.Logging.ManualLogSource log;

        private static Dictionary<MinionEnum, MinionAsset> CustomMinionAssetDictionary = new Dictionary<MinionEnum, MinionAsset>();
        private static Dictionary<MinionEnum, CustomPet> CustomPetDictionary = new Dictionary<MinionEnum, CustomPet>();
        
        public CustomPetManager(IntPtr ptr) : base(ptr) {
            log = BepInExLoader.log;
        }

        #region CustomPet
        public static CustomPet CreateCustomPet(string name) {
            MinionEnum minionEnum = createMinionEnum();
            MinionTemplate minionTemplate = createMinionTemplate(minionEnum, name);
            MinionAsset minionAsset = createMinionAsset(minionEnum, name);
            CustomPet customPet = new CustomPet(minionEnum, minionTemplate, minionAsset);
            registerPet(customPet);
            return customPet;
        }

        private static MinionEnum createMinionEnum() {
            return (MinionEnum) (Enum.GetNames(typeof(MinionEnum)).Length + CustomPetDictionary.Count + 1);
        }

        private static MinionTemplate createMinionTemplate(MinionEnum minionEnum, string name) {
            MinionTemplate minionTemplate = new MinionTemplate(minionEnum);
            minionTemplate.Enum = minionEnum;
            minionTemplate.Name = name;
            minionTemplate.SetStats(1, 1);
            minionTemplate.Rewardless = false;
            minionTemplate.Elite = false;
            return minionTemplate;
        }

        private static MinionAsset createMinionAsset(MinionEnum minionEnum, string name) {
            MinionAsset minionAsset = ScriptableObject.CreateInstance<MinionAsset>();
            minionAsset.Enum = minionEnum;
            minionAsset.Name = name;
            return minionAsset;
        }

        private static void registerPet(CustomPet customPet) {
            log.LogInfo("Registering pet: " + customPet + " " + customPet.GetEnum());
            log.LogInfo("Template: " + customPet.GetTemplate());
            log.LogInfo("Asset: " + customPet.GetAsset());
            MinionConstants.Minions.Add(customPet.GetEnum(), customPet.GetTemplate());
            log.LogInfo("Minions.add passed");
            log.LogInfo("Adding <" + customPet.GetEnum() + ", " + customPet.GetAsset() + "> to Dictionary");
            CustomMinionAssetDictionary.Add(customPet.GetEnum(), customPet.GetAsset());
            log.LogInfo("Found key " + customPet.GetEnum() + " " + CustomMinionAssetDictionary.ContainsKey(customPet.GetEnum()));
            log.LogInfo("Found key " + 184 + " " + CustomMinionAssetDictionary.ContainsKey((MinionEnum) 184));

            log.LogInfo("Created custom pet " + customPet.GetTemplate().Name + " with ID " + customPet.GetEnum());
        }
        #endregion

        #region hooks
        [HarmonyPatch]
        class CustomPetManagerHooks {
            [HarmonyPrefix]
            [HarmonyPatch(typeof(MinionLibrary), "Get")]
            public static bool prefixGet(MinionEnum value, ref MinionAsset __result) {
                // check if requested Assets are for a custom pet
                if(CustomMinionAssetDictionary.ContainsKey(value)) {
                    // set return to our created MinionAsset
                    __result = CustomMinionAssetDictionary[value];
                    // prevent executing the regular MinionLibrary.Get function to avoid errors
                    return false;
                } else {
                    log.LogInfo("Didn't find key " + value);
                    return true;
                }
            }
        }
        #endregion
    }
}