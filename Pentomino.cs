﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PentominoApp {

    public class Pentomino {
        private Board _board;

        // 問題を解く
        public bool Solve(int xSize, int ySize) {
            _board = new Board(xSize, ySize);
            return Solve(Piece.PieceList);
        }

        // 解を求める （再帰メソッド）
        private bool Solve(IEnumerable<Piece> pieceList) {
            // 最初のピースを取り出す
            var piece = pieceList.FirstOrDefault();              
            if (piece == null)
                // すべてのペースを使い切った(つまり成功)
                return true;
            // ピースを回転、反転させたものを取り出し試していく。。
            foreach (var curr in piece.AllSeries) {
                // すべての位置を順に取り出す、そこにcurrを置いていく
                foreach (var topleft in _board.AllPoints) { 
                    // 取り出した位置(左上)にピースを置いてみる
                    if (Put(topleft, curr)) {  
                        if (CountEmpty().Any(n => n % 5 != 0)) {
                            // 5で割り切れない空き領域があれば、そこにピースははめ込むことができない。
                            // 枝刈り処理 これ以上試しても仕方が無いので、次を試す。
                            Remove(topleft, curr);
                            continue;
                        }
                        var newlist = pieceList.Where(o => o.Char != curr.Char).ToList();
                        // 置けたら残りのピースで同じことを繰り返す
                        if (Solve(newlist) == true)
                            // 成功したら処理を終える
                            return true;
                        // 状態を戻して、次を試す
                        Remove(topleft, curr);       
                    }
                }
            }
            return false;
        }

        // ピースをBoardから取り除く
        private void Remove(Point topleft, Piece piece) {
            foreach (var pt in piece.Points) {
                var point = topleft.Add(pt);
                _board[point] = ' ';
            }
        }

        // ピースを指定位置に置く
        public bool Put(Point basePlace, Piece piece) {
            List<Point> save = new List<Point>();
            foreach (var pt in piece.Points) {
                var point = basePlace.Add(pt);
                
                if (_board.IsValidPoint(point) &&  _board[point] == ' ') {
                    _board[point] = piece.Char;
                    save.Add(point);
                } else {
                    // やっぱり置けなかったので、元に戻す。
                    foreach (var p in save)
                        _board[p] = ' ';
                    return false;
                }
            }
            return true;
        }

        // プリント
        public void Print() {
            _board.Print();
        }

        // 空いている領域の面積を列挙する。
        private IEnumerable<int> CountEmpty() {
            var bclone = _board.Clone();
            var pts = bclone.AllPoints
                          .Where(pt => bclone[pt] == ' ').ToArray();
            return pts.Select(pt => bclone.CountEmpty(pt))
                      .Where(cnt => cnt > 0);
        }
    }
}


