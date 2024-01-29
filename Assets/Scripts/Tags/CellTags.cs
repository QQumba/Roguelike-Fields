using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tags
{
    public static class CellTags
    {
        public const string Hero = "hero";
        public const string Enemy = "enemy";
        public const string Damageable = "damageable";
        public const string Pickable = "pickable";
        public const string HasHealth = "hasHealth";

        public static List<string> GetTags()
        {
            var fields = typeof(CellTags).GetFields(BindingFlags.Static | BindingFlags.Public);
            var tags = fields.Select(x => x.GetRawConstantValue() as string).ToList();
            return tags;
        }
    }
}