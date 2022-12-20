using BigDLL4221.Enum;
using BigDLL4221.Models;
using BigDLL4221.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Reflection;


namespace Apho100Yujin
{
    public class Apho100Yujin
    {
        public static string PackageId = "Apho100Yujin";
        public static string Path;
        public static string CategoryName = "100% Yujin";
    }
    public class Apho100YujinInit : ModInitializer
    {
        public override void OnInitializeMod()
        {
            OnInitParameters();
            ArtUtil.GetArtWorks(new DirectoryInfo(Apho100Yujin.Path + "/ArtWork"));
            ArtUtil.PreLoadBufIcons();
            ArtUtil.GetSpeedDieArtWorks(new DirectoryInfo(Apho100Yujin.Path + "/CustomDiceArtWork"));
            CardUtil.ChangeCardItem(ItemXmlDataList.instance, Apho100Yujin.PackageId);
            KeypageUtil.ChangeKeypageItem(BookXmlList.Instance, Apho100Yujin.PackageId);
            PassiveUtil.ChangePassiveItem(Apho100Yujin.PackageId);
            LocalizeUtil.AddGlobalLocalize(Apho100Yujin.PackageId);
            LocalizeUtil.RemoveError();
            CardUtil.InitKeywordsList(new List<Assembly> { Assembly.GetExecutingAssembly() });
            ArtUtil.InitCustomEffects(new List<Assembly> { Assembly.GetExecutingAssembly() });
            CustomMapHandler.ModResources.CacheInit.InitCustomMapFiles(Assembly.GetExecutingAssembly());
        }
        private static void OnInitParameters()
        {
            ModParameters.PackageIds.Add(Apho100Yujin.PackageId);
            Apho100Yujin.Path = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            ModParameters.Path.Add(Apho100Yujin.PackageId, Apho100Yujin.Path);
            OnInitRewards();
            OnInitCategories();
            OnInitKeypages();
            OnInitSprites();
            OnInitPassives();
        }
        private static void OnInitRewards()
        {
            ModParameters.StartUpRewardOptions.Add(new RewardOptions(keypages: new List<LorId>
            {
                    new LorId(Apho100Yujin.PackageId, 240001),
                }
            ));
        }
        private static Color shired = new Color(0.988f, 0.016f, 0.02f);
        private static void OnInitCategories()
        {
            ModParameters.CategoryOptions.Add(Apho100Yujin.PackageId, new List<CategoryOptions>
            {
                new CategoryOptions(Apho100Yujin.PackageId, additionalValue: "_1", categoryBooksId:new List<int>{240001}, categoryName: Apho100Yujin.CategoryName,customIconSpriteId: "Apho100Yujin_Icon", bookDataColor: new CategoryColorOptions(Color.white, shired), credenzaType: CredenzaEnum.NoCredenza, chapter: 7),
            });
        }
        
        private static void OnInitKeypages()
        {
            ModParameters.KeypageOptions.Add(Apho100Yujin.PackageId, new List<KeypageOptions>
            {
                new KeypageOptions(240001, everyoneCanEquip: true, keypageColorOptions: new KeypageColorOptions(Color.white, shired), customDiceColorOptions: new CustomDiceColorOptions("Apho100YujinDice", shired)),
            });
        }
        private static void OnInitSprites()
        {
            ModParameters.SpriteOptions.Add(Apho100Yujin.PackageId, new List<SpriteOptions>
            {
                new SpriteOptions(SpriteEnum.Custom, 240001, "Apho100Yujin_Sprite"),
            });
        }
        private static void OnInitPassives()
        {
            ModParameters.PassiveOptions.Add(Apho100Yujin.PackageId, new List<PassiveOptions>
            {
                new PassiveOptions(240301, true, passiveColorOptions: new PassiveColorOptions(shired, Color.white)),
            });
        }
    }
    public class PassiveAbility_Apho100Yujin_Overbreathing : PassiveAbilityBase
    {
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (curCard.card.GetOriginCost() >= 4)
            {
                this.owner.cardSlotDetail.RecoverPlayPoint(2);
            }
        }
    }
    public class PassiveAbility_Apho100Yujin_EyeOfDeath : PassiveAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            BattleCardTotalResult battleCardResultLog = this.owner.battleCardResultLog;
            if (battleCardResultLog != null)
            {
                battleCardResultLog.SetPassiveAbility(this);
            }
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                power = 4
            });
        }
    }
    public class PassiveAbility_Apho100Yujin_Kizuna : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            this._stack = 0;
        }
        public override void OnRoundStart()
        {
            if (this._stack > 0)
            {
                this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this._stack, this.owner);
                this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, this._stack, this.owner);
            }
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit.faction == this.owner.faction && this._stack < 2)
            {
                this._stack++;
            }
        }
        private int _stack;
    }


}
