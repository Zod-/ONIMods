using Harmony;
using JetBrains.Annotations;

namespace ZodBain.SolidFilterPokeshellFix
{
    [HarmonyPatch(typeof(TagNameComparer), nameof(TagNameComparer.Compare))]
    public class TagNameComparerComparePatch
    {
        private static Tag _babbyCrabShell = new Tag("BabyCrabShell");
        private static Tag _crabShell = new Tag("CrabShell");
        public static bool FixActive;

        [UsedImplicitly]
        [HarmonyPostfix]
        // ReSharper disable once InconsistentNaming
        public static void Postfix(Tag x, Tag y, ref int __result)
        {
            if (!FixActive)
            {
                return;
            }

            //The real comparer uses the actual names which in both cases is "Pokeshell Molt"
            if (x.Name == _crabShell.Name && y.Name == _babbyCrabShell.Name)
            {
                __result = -1;
            }
            else if (y.Name == _crabShell.Name && x.Name == _babbyCrabShell.Name)
            {
                __result = 1;
            }
        }
    }


    [HarmonyPatch(typeof(FilterSideScreen), "Configure")]
    public class FilterSideScreenConfigurePatch
    {
        [UsedImplicitly]
        [HarmonyPrefix]
        public static void Prefix()
        {
            // Just to make sure we don't break anything else only enable this during the one method.
            TagNameComparerComparePatch.FixActive = true;
        }

        [UsedImplicitly]
        [HarmonyPostfix]
        public static void Postfix()
        {
            TagNameComparerComparePatch.FixActive = false;
        }
    }
}