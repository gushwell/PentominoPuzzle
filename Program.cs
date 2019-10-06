using System;

namespace PentominoApp {
    class Program {
        static void Main(string[] args) {
            Pentomino solver = new Pentomino();
            Console.WriteLine(DateTime.Now);
            solver.Solve(20, 3);
            Console.WriteLine(DateTime.Now);
            solver.Print();
            Console.ReadLine();
        }
    }
}
