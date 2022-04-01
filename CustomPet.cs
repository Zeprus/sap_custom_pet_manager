using Spacewood.Core.Enums;
using Spacewood.Unity;

namespace Zeprus.Sap {
    public class CustomPet {
        private MinionEnum mEnum;
        private MinionTemplate template;
        private MinionAsset asset;

        internal CustomPet(MinionEnum mEnum, MinionTemplate template, MinionAsset asset) {
            this.mEnum = mEnum;
            this.template = template;
            this.asset = asset;
        }

        public MinionEnum GetEnum() { return this.mEnum; }
        public MinionTemplate GetTemplate() { return this.template; }
        public MinionAsset GetAsset() { return this.asset; }
    }
}