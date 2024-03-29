﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PentominoApp {
    // 2つのピースを比較するクラス
    public class PieceComparer : IEqualityComparer<Piece> {
        public bool Equals(Piece a, Piece b) {
            return a.Points.SequenceEqual(b.Points);
        }

        public int GetHashCode(Piece obj) {
            return obj.Points.Aggregate(0, (hc, pt) => hc ^ pt.GetHashCode());
        }
    }
}
