using CellularAutomata.Automatas;
using CellularAutomata.Visualizer.Scenes;
using CellularAutomata.Visualizer.Scenes.Automata;

namespace CellularAutomata
{
    class Constants
    {
        public static Type[] allautomatas =
            {typeof(LangtonAnt), typeof(LangtonLoop), typeof(Cgol), typeof(BriansBrain)};
        public const int Height = 1080/4;
        public const int Width = 1920/4;
        public const int Scale = 1;
        public const uint Delay = 5;
    }

    class Program
    {
        static void Main()
        {
            Dictionary<String, Type> allScenes = new();
            allScenes.Add("Start",typeof(SMainMenu));
            allScenes.Add("Automata",typeof(SAutomata));
            allScenes.Add("Settings",typeof(SSettings));
            var viz = new Visualizer.Visualizer(allScenes);
            viz.Init();
        }
    }
}