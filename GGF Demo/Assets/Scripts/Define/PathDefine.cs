using System;
using System.Collections.Generic;
using Logic;

namespace Define
{
    public static class PathDefine
    {
        public static readonly Dictionary<Type, string> PlayerType2Path = new Dictionary<Type, string>()
        {
            {typeof(HeartPlayer), "Player/Heart/HeartPlayerPrefab"},
            {typeof(BodyPlayer), "Player/Body/BodyPlayerPrefab"},
        };
    }
}