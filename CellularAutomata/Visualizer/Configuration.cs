namespace CellularAutomata.Visualizer;

public class Configuration
{
    public int W { get; set; }
    public int H{ get; set; }
    public int Scale{ get; set; }
    public uint Delay{ get; set; }  
    
    public Configuration(){}
    public Configuration(string configpath,Configuration defaultconfig)
    {
        if (File.Exists(configpath))
        {
            ReadConfig(configpath);
            return;
        }
        defaultconfig.WriteConfig(configpath);
        ReadConfig(configpath);
    }

    public void WriteConfig(string configpath)
    {
        string[] configlines =
        {
            $"width={W}",
            $"height={H}",
            $"scale={Scale}",
            $"delay={Delay}"
        };
        File.WriteAllLines(configpath, configlines);
    }

    private void ReadConfig(string configpath)
    {
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
                    W = value;
                    break;
                case "height":
                    H = value;
                    break;
                case "scale":
                    Scale = value;
                    break;
                case "delay":
                    Delay = (uint)value;
                    break;
            }
        }
    }
}