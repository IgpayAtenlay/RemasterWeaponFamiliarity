using Dawnsbury.Campaign.Encounters;
using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CombatActions;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemasterWeaponFamiliarity
{
#pragma warning disable CA1416 // Validate platform compatibility
    public class WeaponFamiliarityFeat : TrueFeat
    {
        public WeaponFamiliarityFeat(string name, string flavorText, Trait ancestry, Trait[]? otherWeapons = null)
            : base(
                  ModManager.RegisterFeatName("Remaster" + name + "WeaponFamiliarity", name + " Weapon Familiarity"),
                  1,
                  flavorText,
                  CreateRulesText(ancestry, otherWeapons),
                  [ancestry, Trait.Mod, RemasterWeaponFamiliarityLoader.Remaster]
            )
        {
            WithOnSheet(values =>
            {
                values.Proficiencies.AddProficiencyAdjustment(traits =>
                    HasOneTrait(traits, ancestry, otherWeapons)
                    && traits.Contains(Trait.Martial), Trait.Simple);
                values.Proficiencies.AddProficiencyAdjustment(traits =>
                    HasOneTrait(traits, ancestry, otherWeapons)
                    && traits.Contains(Trait.Advanced), Trait.Martial);
            });



            WithPermanentQEffect("Once you hit level 5, " + ancestry.ToString().ToLower() + " weapons trigger {tooltip:criteffect}critical specialization effects.{/}", qf =>
            {
                if (qf.Owner.Level >= 5)
                {
                    qf.YouHaveCriticalSpecialization = (effect, item, action, defender)
                    => HasOneTrait(action, ancestry, otherWeapons);
                }
            });
        }
        public WeaponFamiliarityFeat(string flavorText, Trait ancestry, Trait[]? otherWeapons = null) : this(ancestry.ToString(), flavorText, ancestry, otherWeapons)
        {
        }

        private static bool HasOneTrait(IEnumerable<Trait> traits, Trait ancestry, Trait[] otherWeapons)
        {
            if (traits.Contains(ancestry))
            {
                return true;
            }
            foreach (Trait t in otherWeapons)
            {
                if (traits.Contains(t))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool HasOneTrait(CombatAction action, Trait ancestry, Trait[] otherWeapons)
        {
            if (action.HasTrait(ancestry))
            {
                return true;
            }
            foreach (Trait t in otherWeapons)
            {
                if (action.HasTrait(t))
                {
                    return true;
                }
            }
            return false;
        }
        private static String CreateRulesText(Trait ancestry, Trait[] otherWeapons)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("You have familiarity with weapons with the ");
            sb.Append(ancestry.ToString().ToLower());
            sb.Append(" trait");
            if (otherWeapons != null && otherWeapons.Length != 0)
            {
                sb.Append(" plus ");
                for (int i = 0; i < otherWeapons.Length; i++)
                {
                    sb.Append(otherWeapons[i].ToString().ToLower());
                    if (i == sb.Length - 2)
                    {
                        if (sb.Length > 2)
                        {
                            sb.Append(',');
                        }
                        sb.Append(" and ");
                    }
                    else if (i < sb.Length - 2)
                    {
                        sb.Append(", ");
                    }
                }
            }

            sb.Append("—for the purposes of proficiency, you treat any of these that are martial weapons as simple weapons and any that are advanced weapons as martial weapons.\r\n\r\nAt 5th level, whenever you get a critical hit with one of these weapons, you get its {tooltip:criteffect}critical specialization effect.{/} effect.");

            return sb.ToString();
        }
    }
}
