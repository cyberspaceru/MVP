using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP.Connectors;
using MVP.CSGO.GameObjects;
using MVP.GameConcepts;

namespace MVP.CSGO
{
    public static class Signatures
    {
        public const string EntityListPattern = "42 18 3B C7";
        public const string ViewMatrixPattern = "0F 10 05 ? ? ? ? 8D 85 ? ? ? ? B9";
        public const string GlowObjectPattern = "8D 8F ? ? ? ? A1 ? ? ? ? C7 04 02 ? ? ? ? 89 35 ? ? ? ? 8B 51";
        public const long GlowObjectOffset = 0x2BD38BB0;
    }
}
