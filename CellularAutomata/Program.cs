using CellularAutomata.Automatas;

namespace CellularAutomata
{
    class Constants
    {
        public static Type[] allautomatas =
            {typeof(LangtonAnt), typeof(LangtonLoop), typeof(Cgol), typeof(BriansBrain)};
        public const int Height = 1080;
        public const int Width = 1920;
        public const int Scale = 1;
        public const int Delay = 5;
    }

    class Program
    {
        static void Main()
        {
            var viz = new Visualizer.Visualizer();
            viz.Init();
        }
    }
}