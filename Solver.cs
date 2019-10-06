//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Hakooki {
//    // 2つのピースを比較するクラス
//    public class PieceComparer : IEqualityComparer<int[,]> {
//        public bool Equals(int[,] x, int[,] y) {
//            if (x.GetUpperBound(0) != y.GetUpperBound(0))
//                return false;
//            if (x.GetUpperBound(1) != y.GetUpperBound(1))
//                return false;
//            foreach (var pt in x.GetAllPoints()) {
//                if (x[pt.X, pt.Y] != y[pt.X, pt.Y])
//                    return false;
//            }
//            return true;
//        }

//        public int GetHashCode(int[,] obj) {
//            int code = 0;
//            foreach (var pt in obj.GetAllPoints()) {
//                code += obj[pt.X, pt.Y].GetHashCode();
//            }
//            return code;
//        }
//    }

//    // 座標
//    public struct Point {
//        public int X { get; set; }
//        public int Y { get; set; }
//    }

//    // ピース関連のユーティリティクラス
//    public static class PieceUty {
//        // 一つのピースから、回転、反転させたものを求める（重複は除外する）
//        public static IEnumerable<int[,]> Series(this int[,] piece) {
//            return AllCandidates(piece).Distinct(new PieceComparer());
//        }

//        // 2次元配列のすべての座標を列挙する
//        public static IEnumerable<Point> GetAllPoints(this int[,] array) {
//            for (int xx = 0; xx <= array.GetUpperBound(0); xx++) {
//                for (int yy = 0; yy <= array.GetUpperBound(1); yy++) {
//                    yield return new Point { X = xx, Y = yy };
//                }
//            }
//        }

//        // ピースを右に90度回転
//        private static int[,] R90(int[,] piece) {
//            int[,] npiece = new int[piece.GetUpperBound(1)+1, piece.GetUpperBound(0)+1];
//            foreach (var pt in piece.GetAllPoints()) {
//                npiece[pt.Y, piece.GetUpperBound(0) - pt.X] = piece[pt.X, pt.Y];
//            }
//            return npiece;
//        }

//        // ピースを反転
//        private static int[,] Mirror(int[,] piece) {
//            int[,] npiece = new int[piece.GetUpperBound(0) + 1, piece.GetUpperBound(1) + 1];
//            foreach (var pt in piece.GetAllPoints()) {
//                npiece[pt.X, piece.GetUpperBound(1) - pt.Y] = piece[pt.X, pt.Y];
//            }
//            return npiece;
//        }

//        // 一つのピースを回転、反転させて、８つのパターンのピースを列挙する。
//        private static IEnumerable<int[,]> AllCandidates(int[,] piece) {
//            for (int i = 0; i < 2; i++, piece = Mirror(piece)) {
//                yield return piece;
//                int[,] r1 = piece;
//                for (int j = 0; j < 3; j++ ) {
//                    r1 = R90(r1);
//                    yield return r1;
//                }
//            }
//        }
//    }

//    public class Pentomino {
//        private static int _xSize = 4;
//        private static int _ySize = 4;
//        public int[,] box = new int[_xSize, _ySize];

//        // 解を求める （再帰メソッド）
//        public bool Solve(int xSize, int ySize, IEnumerable<int[,]> pieceList) {
//            _xSize = xSize;
//            _ySize = ySize;
//            return Solve(pieceList);
//        }

//        // 解を求める （再帰メソッド）
//        private bool Solve(IEnumerable<int[,]> pieceList) {
//            if (IsFin())
//                return true;
//            if (pieceList.Count() == 0)
//                return false;
//            var piece = pieceList.First();                  // 最初のピースを取り出す
//            foreach (var p in piece.Series()) {             // ピースを回転、反転させたものを取り出す
//                foreach (var pt in box.GetAllPoints()) {    // 取り出したピースをすべての位置で試す
//                    int[,] clone = box.Clone() as int[,];   // 箱をコピーし、保管
//                    if (Put(p, pt.X, pt.Y)) {               // ピースを置いてみる
//                        if (Solve(pieceList.Skip(1)))       // 置けたら、残りのピースで同じことを繰り返す
//                            return true;                    //  成功したら処理を終える
//                        box = clone;                        // 保管していた箱をboxに戻す。
//                    }
//                }
//            }
//            return false;
//        }

//        // ピースを （ｘ、ｙ）の位置に置く
//        public bool Put(int[,] piece, int x, int y) {
//            int[,] clone = box.Clone() as int[,];
//            foreach (var pt in piece.GetAllPoints()) {
//                int tx = x + pt.X;
//                int ty = y + pt.Y;
//                if (tx < _xSize && ty < _ySize) {
//                    if (clone[tx, ty] == 0 || piece[pt.X, pt.Y] == 0) {
//                        clone[tx, ty] += piece[pt.X, pt.Y];
//                    } else {
//                        return false;
//                    }
//                } else {
//                    return false;
//                }
//            }
//            box = clone;
//            return true;
//        }

//        // 完成したか調べる （箱がすべて埋まっていれば完成）
//        public bool IsFin() {
//            return box.GetAllPoints().All(pt => box[pt.X, pt.Y] != 0);
//        }

//        // 箱の中身をプリントする
//        public void Print() {
//            for (int x = 0; x < _xSize; x++) {
//                for (int y = 0; y < _ySize; y++) {
//                    Console.Write(box[x, y]);
//                }
//                Console.WriteLine();
//            }
//            Console.WriteLine();
//        }
//    }
//}

///* この手のプログラムでは、ピースデータをどのように表現するかで、プログラムは大きく変わってきます。
// * ここでは、ひとつのピースをint型の２次元配列として表現しています。
// * 0 の部分は、ピースが無いことを示しています。また、ピースがあるところには、ピースの番号を入れています。
// * 
// * 例えば、Ｌ 字型のピースでは、
//     new int[3, 2] {
//        { 4,0 },
//        { 4,0 },
//        { 4,4 },
//     };
// * のように保持します。4 は、ピースの番号で、ピースごとに別の番号を振っています。
// * このプログラムは、コンソールアプリとして作成しているので、配列の内容をそのまま
// * 出力すれば、ピースの形がわかるようにするためです。
// * そのため、WindowsFormsや Silverlightでグラフィカルに表示する場合は、ピース毎に
// * 数字を変える必要はありません。
// * 
// * ピースのデータ構造が決まれば、あとは、総当たりで、ピースを箱にはめていき
// * すべてのピースが箱に収まるまで試行を繰り返すことになります。
// * もちろん、ピースが箱に入らなくなった時点で試行を中止し、別の置き方を試します。
// * この部分は、他のパズルと同様に、深さ優先の探索で再帰的に処理しています。
// * Solveメソッドが探索をしているメソッドです。 * 
// * ソースコードにはコメントを書いておきましたので、詳しくはそちらを見てください。
// * 
// * なお、一つのピースは、回転／反転させることで、最大で８つの置き方が存在します。
// * 回転／反転後に、重ね合わせて同じ形になる場合は、それを除外するようにしています。
// * 
// * 
//*/

