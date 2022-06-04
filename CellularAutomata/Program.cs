using SDL2;
using CellularAutomata.Automatas;

namespace CellularAutomata
{
    static class Constants
    {
        public const uint Height = 1080;
        public const uint Width = 1920;
        public const uint Zoom = 1;
        public const uint Delay = 50;
    }

    class Program
    {
        private static void ReadInt(string message, out uint output)
        {
            while (true)
            {
                Console.Write(message);
                string? userinput = Console.ReadLine();

                if (userinput == string.Empty)
                    continue;

                if (!uint.TryParse(userinput, out output))
                {
                    Console.WriteLine("Not a number");
                    continue;
                }

                break;
            }
        }

        private static (Type, uint, uint, uint, uint) GetStartingParameters()
        {
            uint automataindex, width, height, scale, delay;
            Type[] allautomatas = {typeof(LangtonAnt), typeof(LangtonLoop), typeof(Cgol)};
            for (int i = 0; i < allautomatas.Length; i++)
            {
                Console.WriteLine($"{i}. " + allautomatas[i].GetProperty("Description")!.GetValue(null));
            }

            while (true)
            {
                Console.Write("Enter a number: ");
                string? userinput = Console.ReadLine();
                if (userinput == string.Empty)
                    continue;
                if (!uint.TryParse(userinput, out automataindex))
                {
                    Console.WriteLine("Not a number");
                    continue;
                }

                if (automataindex >= allautomatas.Length)
                {
                    Console.WriteLine("Number not in range");
                    continue;
                }

                break;
            }

            Console.WriteLine();
            Console.WriteLine(
                $"Use default Height({Constants.Height}),Width({Constants.Width})" +
                $",Zoom({Constants.Zoom}) and Delay({Constants.Delay}) ? (Y/N)");
            while (true)
            {
                Console.Write(">> ");
                string? userinput = Console.ReadLine();
                if (userinput == string.Empty)
                    continue;
                if (char.ToLower(userinput[0]) == 'y')
                    return (allautomatas[automataindex], Constants.Height, Constants.Width, Constants.Zoom,
                        Constants.Delay);
                if (char.ToLower(userinput[0]) == 'n')
                    break;
                Console.WriteLine("Invalid command");
            }

            ReadInt("Height: ", out height);
            ReadInt("Width: ", out width);
            ReadInt("Zoom: ", out scale);
            ReadInt("Delay: ", out delay);
            return (allautomatas[automataindex], height, width, scale, delay);
        }

        static void Main(string[] args)
        {
            (Type automata, uint height, uint width, uint scale, uint delay) = GetStartingParameters();
            CellAutomata chosenautomata =
                (CellAutomata) Activator.CreateInstance(automata, height / scale, width / scale)!;
            CaVis visualizer = new CaVis(chosenautomata, scale, chosenautomata.GetColors());
            visualizer.Start();
            while (!visualizer.Quit)
            {
                visualizer.Render();
                visualizer.HandleInput();
                SDL.SDL_Delay(delay);
                visualizer.Step();
            }
        }
    }
}