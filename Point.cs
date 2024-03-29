﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PentominoApp {
    public struct Point {
        public int X { get; set; }
        public int Y { get; set; }

        // 上下左右の位置を列挙
        public IEnumerable<Point> GetAroundPoints() {
            yield return new Point { X = this.X, Y = this.Y + 1 };
            yield return new Point { X = this.X + 1, Y = this.Y };
            yield return new Point { X = this.X - 1, Y = this.Y };
            yield return new Point { X = this.X, Y = this.Y - 1 };
        }

        // BoardにPieceを置く際に利用する。
        public Point Add(Point pt) {
            return new Point {
                X = X + pt.X,
                Y = Y + pt.Y
            };
        }

        public override int GetHashCode() {
            return Value;
        }

        public int Value {
            get { return X + Y * 100; }
        }
    }

}
