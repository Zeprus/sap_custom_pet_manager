using Spacewood.Core.Models.Abilities;
using Spacewood.Unity;

namespace Zeprus.Sap {
    public class CustomAbilityCollection {
        private AbilityEnum mEnum;
        private AbilityAsset asset;
        private AbilityCollection abilityCollection;

        internal CustomAbilityCollection(AbilityEnum mEnum, AbilityCollection abilityCollection, AbilityAsset asset) {
            this.mEnum = mEnum;
            this.asset = asset;
            this.abilityCollection = abilityCollection;
        }

        public AbilityEnum GetEnum() { return this.mEnum; }
        public AbilityAsset GetAsset() { return this.asset; }
        public AbilityCollection GetAbilityCollection() { return this.abilityCollection; }
    }
}