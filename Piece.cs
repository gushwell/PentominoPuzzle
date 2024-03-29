﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PentominoApp {

    public class Piece {
        public IList<Point> Points { get; private set; }

        public IList<Piece> AllSeries { get; private set; }

        public int YSize { get; set; }
        public int XSize { get; set; }

        public char Char { get; private set; }

        private Piece() {

        }

        public Piece(char[,] piece) {
            YSize = piece.GetUpperBound(1) + 1;
            XSize = piece.GetUpperBound(0) + 1;
            Points = GetAllPoints().Where(pt => piece[pt.X, pt.Y] != ' ')
                                        .OrderBy(pt => pt.Value).ToList();
            var first = Points[0];
            Char = piece[first.X, first.Y];
            AllSeries = this.AllCandidates().Distinct(_pieceComparer).ToList();
        }

        // 右に90度回転
        public Piece R90() {
            return new Piece {
                Char = this.Char,
                Points = Points.Select(pt => new Point { X = pt.Y, Y = this.XSize - pt.X - 1 })
                                         .OrderBy(pt => pt.Value).ToList(),
                XSize = this.YSize,
                YSize = this.XSize,
            };
        }

        // 左右に反転
        public Piece Mirror() {
            return new Piece {
                Char = this.Char,
                Points = Points.Select(pt => new Point { X = pt.X, Y = this.YSize - pt.Y - 1 })
                                         .OrderBy(pt => pt.Value).ToList(),
                XSize = this.XSize,
                YSize = this.YSize,
            };
        }

        // ピースの全ての位置を列挙する
        private IEnumerable<Point> GetAllPoints() {
            for (int xx = 0; xx < this.XSize; xx++) {
                for (int yy = 0; yy < this.YSize; yy++) {
                    yield return new Point { X = xx, Y = yy };
                }
            }
        }

        // 回転、反転の８つのピースを得る （左右対称なら４つ）
        private IEnumerable<Piece> AllCandidates() {
            yield return this;
            Piece r1 = this;
            for (int j = 0; j < 3; j++) {
                r1 = r1.R90();
                yield return r1;
            }
            var mirror = this.Mirror();
            if (!_pieceComparer.Equals(this, mirror)) {
                yield return mirror;
                for (int j = 0; j < 3; j++) {
                    mirror = mirror.R90();
                    yield return mirror;
                }
            }
        }

        private static PieceComparer _pieceComparer = new PieceComparer();

        // 利用するピースデータ
        public static  IList<Piece> PieceList = new List<Piece> {
            new Piece(
                new char[,] {
                    { ' ','X',' ' },
                    { 'X','X','X' },
                    { ' ','X',' ' },
            }),
            new Piece(
                new char[,] {
                    { ' ','F','F' },
                    { 'F','F',' ' },
                    { ' ','F',' ' },
            }),
            new Piece(
                new char[,] {
                    { 'I','I','I','I','I' },
                }),
            new Piece(
                new char[,] {
                    { 'L',' ' },
                    { 'L',' ' },
                    { 'L',' ' },
                    { 'L','L' },
                }),
            new Piece(
                new char[,] {
                    { 'N',' ' },
                    { 'N',' ' },
                    { 'N','N' },
                    { ' ','N' },
            }),
            new Piece(
                new char[,] {
                    { 'P','P' },
                    { 'P','P' },
                    { 'P',' ' },
            }),
            new Piece(
                new char[,] {
                    { 'T','T','T' },
                    { ' ','T',' ' },
                    { ' ','T',' ' },
            }),
            new Piece(
                new char[,] {
                    { 'U',' ','U' },
                    { 'U','U','U' },
            }),
            new Piece(
                new char[,] {
                    { 'V',' ',' ' },
                    { 'V',' ',' ' },
                    { 'V','V','V' },
            }),
            new Piece(
                new char[,] {
                    { 'W',' ',' ' },
                    { 'W','W',' ' },
                    { ' ','W','W' },
            }),
            new Piece(
                new char[,] {
                    { ' ','Y' },
                    { 'Y','Y' },
                    { ' ','Y' },
                    { ' ','Y' },
            }),
            new Piece(
                new char[,] {
                    { 'Z','Z',' ' },
                    { ' ','Z',' ' },
                    { ' ','Z','Z' },
            }),
        };
    }
}
