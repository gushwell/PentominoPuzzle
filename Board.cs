using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PentominoApp {
    public class Board {
        private char[,] _box;
        int _xSize;
        int _ySize;

        // コンストラクタ
        private Board(Board board) {
            this._xSize = board._xSize;
            this._ySize = board._ySize;
            this._box = board._box.Clone() as char[,];
        }

        // コンストラクタ
        public Board(int xmax, int ymax) {
            _box = new char[xmax + 2, ymax + 2];
            _xSize = xmax + 2;
            _ySize = ymax + 2;

            foreach (var pt in GetPointsIncludeFrame()) {
                if (IsValidPoint(pt))
                    this[pt] = ' ';
                else
                    this[pt] = '*';
            }
        }

        // 枠も含めてすべての位置を列挙する
        private IEnumerable<Point> GetPointsIncludeFrame() {
            for (int yy = 0; yy < this._ySize; yy++) {
                for (int xx = 0; xx < this._xSize; xx++) {
                    yield return new Point { X = xx, Y = yy };
                }
            }
        }

        private List<Point> _AllValidPoint = null;

        // 枠を除いた位置を列挙する
        public IEnumerable<Point> AllPoints {
            get {
                if (_AllValidPoint == null) {
                    var list = new List<Point>();
                    for (int yy = 1; yy < this._ySize - 1; yy++) {
                        for (int xx = 1; xx < this._xSize - 1; xx++) {
                            list.Add(new Point { X = xx, Y = yy });
                        }
                    }
                    _AllValidPoint = list.OrderBy(pt => pt.X + pt.Y).ToList();
                }
                return _AllValidPoint;
            }
        }      


        // インデクサ
        public char this[Point pt] {
            get { return _box[pt.X, pt.Y]; }
            set { _box[pt.X, pt.Y] = value; }
        }

        // Boardの内容をプリントする
        public void Print() {
            var ystr = new string(Enumerable.Repeat('-', _xSize-2).ToArray());
            Console.WriteLine($"+{ystr}+");
            for (int y = 1; y < _ySize - 1; y++) {
                Console.Write("|");
                for (int x = 1; x < _xSize - 1; x++) {
                    Console.Write(_box[x, y]);
                }
                Console.WriteLine("|");
            }
            Console.WriteLine($"+{ystr}+");
        }

        // 複製をつくる
        public Board Clone() {
            return new Board(this);
        }

        // 有効な位置か （枠ならばfalse)
        internal bool IsValidPoint(Point point) {
            return ((1 <= point.X && point.X < _xSize - 1) &&
                    (1 <= point.Y && point.Y < _ySize - 1));
        }

        // Boardの内容を変更してしまうので注意。元の状態に戻すのは呼び出す側の責務とする。
        // Pointのの位置に注目した時に、いくつの空白があるかをカウントする。pointも含める。
        // 再帰的に処理をしている。
        internal int CountEmpty(Point point) {
            if (this[point] != ' ')
                return 0;
            this[point] = '#';
            int count = 1;
            foreach (var pt in point.GetAroundPoints()) {
                if (this[pt] == ' ') {
                    count += CountEmpty(pt);
                }
            }
            return count;
        }
    }
}
