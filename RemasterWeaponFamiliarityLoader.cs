using Dawnsbury;
using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CharacterBuilder.FeatsDb;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Modding;

namespace RemasterWeaponFamiliarity
{
#pragma warning disable CA1416 // Validate platform compatibility
    public class RemasterWeaponFamiliarityLoader
    {
        public static Trait Remaster;

        [DawnsburyDaysModMainMethodAttribute]
        public static void LoadMod()
        {

            Remaster = ModManager.RegisterTrait("Remaster");

            IEnumerable<Feat> weaponFamiliarityFeats = CreateWeaponFamiliarityFeats();

            AllFeats.All.RemoveAll(feat => MatchingName(feat.FeatName, [
                "Elven", "Dwarven", "Gnome", "Goblin", "Orc"
                ]));

            AddFeats(weaponFamiliarityFeats);
        }

        private static void AddFeats(IEnumerable<Feat> feats)
        {
            foreach (var feat in feats)
            {
                ModManager.AddFeat(feat);
            }
        }

        private static IEnumerable<Feat> CreateWeaponFamiliarityFeats()
        {
            yield return new WeaponFamiliarityFeat(
                "Elven",
                "You favor bows and other elegant weapons.",
                Trait.Elf,
                [Trait.Longbow, Trait.CompositeLongbow, Trait.Rapier, Trait.Shortbow, Trait.CompositeShortbow]);
            yield return new WeaponFamiliarityFeat(
                "Dwarven",
                "Your kin have instilled in you an affinity for hard-hitting weapons, and you prefer these to more elegant arms.",
                Trait.Dwarf,
                [Trait.BattleAxe, Trait.Pick, Trait.Warhammer]);
            yield return new WeaponFamiliarityFeat(
                "You favor unusual weapons tied to your people, such as blades with curved and peculiar shapes.",
                Trait.Gnome,
                [Trait.Glaive, Trait.Kukri]);
            yield return new WeaponFamiliarityFeat(
                "Others might look upon them with disdain, but you know that the weapons of your people are as effective as they are sharp.",
                Trait.Goblin);
            yield return new WeaponFamiliarityFeat(
                "In combat, you favor the brutal weapons that are traditional for your orc ancestors.",
                Trait.Orc,
                [Trait.Falchion, Trait.Greataxe]);
        }
        private static bool MatchingName(FeatName name, string[] feats)
        {
            foreach (String feat in feats)
            {
                if (name.ToString().Contains(feat + "Weapon")) return true;
            }

            return false;
        }
    }
}