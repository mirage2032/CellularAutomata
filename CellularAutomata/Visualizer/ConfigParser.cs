namespace CellularAutomata.Visualizer;

public struct Configuration
{
    public int W;
    public int H;
    public int Scale;
    public int Delay;
}

public static class ConfigParser
{
    public static Configuration ReadConfig(string configpath)
    {
        Configuration config = new Configuration();
        string[] configlines = File.ReadAllLines(configpath);
        foreach (string configline in configlines)
        {
            string[] parsedline = configline.Replace(" ", "").Split("=");

            if (!int.TryParse(parsedline[1], out int value))
            {
                Console.WriteLine($"Cannot parse argument value of: {parsedline[0]}");
                throw new ArgumentException();
            }

            switch (parsedline[0])
            {
                case "width":
                    config.W = value;
                    break;
                case "height":
                    config.H = value;
                    break;
                case "scale":
                    config.Scale = value;
                    break;
                case "delay":
                    config.Delay = value;
                    break;
            }
        }

        return config;
    }

    public static void WriteConfig(string configpath, Configuration config)
    {
        string[] configlines =
        {
            $"width={config.W}",
            $"height={config.H}",
            $"scale={config.Scale}",
            $"delay={config.Delay}"
        };
        File.WriteAllLines(configpath, configlines);
    }

    public static Configuration GetConfig(string configpath, Configuration defaultconfig)
    {
        if (File.Exists(configpath))
        {
            return ReadConfig(configpath);
        }

        WriteConfig(configpath, defaultconfig);
        return defaultconfig;
    }
}