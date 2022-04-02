using System.Collections.Generic;
using System;
using UnityEngine;
using HarmonyLib;
using Spacewood.Core.Enums;
using Spacewood.Unity;
using Spacewood.Core.Models.Abilities;

namespace Zeprus.Sap {
    public class CustomPetManager : MonoBehaviour {
        
        private static BepInEx.Logging.ManualLogSource log;
        private static Dictionary<MinionEnum, MinionAsset> CustomMinionAssetDictionary = new Dictionary<MinionEnum, MinionAsset>();
        private static Dictionary<MinionEnum, CustomPet> CustomPetDictionary = new Dictionary<MinionEnum, CustomPet>();
        private static Dictionary<AbilityEnum, AbilityAsset> CustomAbilityAssetDictionary = new Dictionary<AbilityEnum, AbilityAsset>();
        private static Dictionary<AbilityEnum, CustomAbilityCollection> CustomAbilityCollectionDictionary = new Dictionary<AbilityEnum, CustomAbilityCollection>();
        
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
            MinionConstants.Minions.Add(customPet.GetEnum(), customPet.GetTemplate());
            CustomMinionAssetDictionary.Add(customPet.GetEnum(), customPet.GetAsset());
            log.LogInfo("Created custom pet '" + customPet.GetTemplate().Name + "' with ID " + customPet.GetEnum());
        }

        public static CustomPet getCustomPet(MinionEnum minionEnum) {
            return CustomPetDictionary[minionEnum];
        }
        #endregion

        #region CustomAbility

        public static CustomAbilityCollection CreateCustomAbilityCollection() {
            AbilityEnum abilityEnum = createAbilityEnum();
            AbilityCollection abilityCollection = createAbilityCollection(abilityEnum);
            abilityCollection.AddAbility(createAbility(abilityEnum, 1));
            abilityCollection.AddAbility(createAbility(abilityEnum, 2));
            abilityCollection.AddAbility(createAbility(abilityEnum, 3));
            AbilityAsset abilityAsset = createAbilityAsset(abilityEnum);
            CustomAbilityCollection customAbilityCollection = new CustomAbilityCollection(abilityEnum, abilityCollection, abilityAsset);
            registerCustomAbilityCollection(customAbilityCollection);
            return customAbilityCollection;
        }

        private static AbilityEnum createAbilityEnum() {
            return (AbilityEnum) (Enum.GetNames(typeof(AbilityEnum)).Length + CustomAbilityCollectionDictionary.Count + 1);
        }

        private static AbilityCollection createAbilityCollection(AbilityEnum abilityEnum) {
            AbilityCollection abilityCollection = new AbilityCollection(abilityEnum);
            return abilityCollection;
        }


        private static Ability createAbility(AbilityEnum abilityEnum, int level) {
            Ability ability = new Ability(abilityEnum, level);
            return ability;
        }

        private static AbilityAsset createAbilityAsset(AbilityEnum abilityEnum) {
            AbilityAsset abilityAsset = ScriptableObject.CreateInstance<AbilityAsset>();
            abilityAsset.Enum = abilityEnum;
            return abilityAsset;
        }

        private static void registerCustomAbilityCollection(CustomAbilityCollection customAbilityCollection) {
            CustomAbilityCollectionDictionary.Add(customAbilityCollection.GetEnum(), customAbilityCollection);
            CustomAbilityAssetDictionary.Add(customAbilityCollection.GetEnum(), customAbilityCollection.GetAsset());
            // AbilityConstants.abilityCollections.Add(customAbilityCollection.GetEnum(), customAbilityCollection.GetAbilityCollection());
        }

        public static CustomAbilityCollection getCustomAbilityCollection(AbilityEnum abilityEnum) {
            return CustomAbilityCollectionDictionary[abilityEnum];
        }
        #endregion

        #region hooks
        [HarmonyPatch]
        class CustomPetManagerHooks {
            [HarmonyPrefix]
            [HarmonyPatch(typeof(MinionLibrary), "Get")]
            public static bool prefixMinionLibraryGet(MinionEnum value, ref MinionAsset __result) {
                // check if requested Assets are for a custom pet
                if(CustomMinionAssetDictionary.ContainsKey(value)) {
                    // set return to our created MinionAsset
                    __result = CustomMinionAssetDictionary[value];
                    // prevent executing the regular MinionLibrary.Get function to avoid errors
                    return false;
                } else {
                    return true;
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(AbilityLibrary), "Get")]
            public static bool prefixAbilityLibraryGet(AbilityEnum value, ref AbilityAsset __result) {
                if(CustomAbilityAssetDictionary.ContainsKey(value)) {
                    __result = CustomAbilityAssetDictionary[value];
                    return false;
                }
                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(AbilityConstants), "GetAbility")]
            public static bool prefixAbilityConstantsGetAbility(AbilityEnum @enum, int level, ref Ability __result) {
                if(CustomAbilityCollectionDictionary.ContainsKey(@enum)) {
                    __result = CustomAbilityCollectionDictionary[@enum].GetAbilityCollection().GetAbility(level);
                    return false;
                } else {
                    return true;
                }
            }
        }
        #endregion
    }
}