namespace CellularAutomata.Automatas;
//WIP//
//NEED TO WORK ON CELL STATE 4 FOR LEFT ROTATION
class LangtonLoop : CellAutomata
{
    public new static string Description => "Langton's Loop";

    private static readonly string[][] Ruletable =
    {
        new[]
        {
            "000000", "000012", "000020", "000030", "000050", "000063", "000071", "000112", "000122", "000132",
            "000212", "000220", "000230", "000262", "000272", "000320", "000525", "000622", "000722", "001022",
            "001120", "002020", "002030", "002050", "002125", "002220", "002322", "005222", "012321", "012421",
            "012525", "012621", "012721", "012751", "014221", "014321", "014421", "014721", "016251", "017221",
            "017255", "017521", "017621", "017721", "025271"
        },
        new[]
        {
            "100011", "100061", "100077", "100111", "100121", "100211", "100244", "100277", "100511", "101011",
            "101111", "101244", "101277", "102026", "102121", "102211", "102244", "102263", "102277", "102327",
            "102424", "102626", "102644", "102677", "102710", "102727", "105427", "111121", "111221", "111244",
            "111251", "111261", "111277", "111522", "112121", "112221", "112244", "112251", "112277", "112321",
            "112424", "112621", "112727", "113221", "122244", "122277", "122434", "122547", "123244", "123277",
            "124255", "124267", "125275"
        },
        new[]
        {
            "200012", "200022", "200042", "200071", "200122", "200152", "200212", "200222", "200232", "200242",
            "200250", "200262", "200272", "200326", "200423", "200517", "200522", "200575", "200722", "201022",
            "201122", "201222", "201422", "201722", "202022", "202032", "202052", "202073", "202122", "202152",
            "202212", "202222", "202272", "202321", "202422", "202452", "202520", "202552", "202622", "202722",
            "203122", "203216", "203226", "203422", "204222", "205122", "205212", "205222", "205521", "205725",
            "206222", "206722", "207122", "207222", "207422", "207722", "211222", "211261", "212222", "212242",
            "212262", "212272", "214222", "215222", "216222", "217222", "222272", "222442", "222462", "222762", "222772"
        },
        new[] {"300013", "300022", "300041", "300076", "300123", "300421", "300622", "301021", "301220", "302511"},
        new[] {"401120", "401220", "401250", "402120", "402221", "402326", "402520", "403221"},
        new[]
        {
            "500022", "500215", "500225", "500232", "500272", "500520", "502022", "502122", "502152", "502220",
            "502244", "502722", "512122", "512220", "512422", "512722"
        },
        new[] {"600011", "600021", "602120", "612125", "612131", "612225"},
        new[] {"700077", "701120", "701220", "701250", "702120", "702221", "702251", "702321", "702525", "702720"}
    };
    
    private readonly CellType[][] _neighbourMatrix;
    private readonly int[][] _relativeNeighbours = {new[] {1, 0}, new[] {-1, 0}, new[] {0, 1}, new[] {0, -1}};

    public override byte[][] GetColors()
    {
        byte[][] colors = new byte[8][];
        colors[0] = new[] {(byte) 0, (byte) 0, (byte) 0}; //Background
        colors[1] = new[] {(byte) 0, (byte) 0, (byte) 255};
        colors[2] = new[] {(byte) 255, (byte) 0, (byte) 0};
        colors[3] = new[] {(byte) 0, (byte) 255, (byte) 0};
        colors[4] = new[] {(byte) 255, (byte) 255, (byte) 0};
        colors[5] = new[] {(byte) 255, (byte) 0, (byte) 255};
        colors[6] = new[] {(byte) 255, (byte) 255, (byte) 255};
        colors[7] = new[] {(byte) 0, (byte) 255, (byte) 255};
        return colors;
    }

    public LangtonLoop(int height, int width)
    {
        Height = height;
        Width = width;
        Matrix = new CellType[height * width];
        _neighbourMatrix = new CellType[height * width][];
        for (int i = 0; i < _neighbourMatrix.Length; i++)
            _neighbourMatrix[i] = new CellType[4];
    }

    private void CheckNeighbours()
    {
        //Iterate through all cells
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                //iterate through the relative neighbours array
                for (int neighbour = 0; neighbour < _relativeNeighbours.Length; neighbour++)
                {
                    int neighbCoordX = j + _relativeNeighbours[neighbour][0];
                    int neighbCoordY = i + _relativeNeighbours[neighbour][1];
                    if (neighbCoordX < 0 || neighbCoordY < 0 || neighbCoordX >= Width || neighbCoordY >= Height)
                        _neighbourMatrix[j + Width * i][neighbour] = CellType.Dead; //count as dead if out of range
                    else
                        _neighbourMatrix[j + Width * i][neighbour] = Matrix[neighbCoordX + Width * neighbCoordY];
                }
            }
        }
    }

    public override void Restart()
    {
        Array.Clear(Matrix);
        int y = 30;
        int x = 30;
        string[] initial =
        {
            "0222222220",
            "2170140142",
            "2022222202",
            "2720000212",
            "2120000212",
            "2020000212",
            "2720000212",
            "21222222122222",
            "20710710711111",
            "02222222222222"
        };
        for (int i = 0; i < initial.Length; i++)
        {
            for (int j = 0; j < initial[i].Length; j++)
            {
                CellType cell = (CellType) (initial[i][j] - '0');
                Matrix[x + j + Width * (y + i)] = cell;
            }
        }
    }

    private CellType NextState(CellType cell, CellType[] neighbours)
    {
        string[] cellRules = Ruletable[(int) cell];
        string[] rulesFilter = new string[cellRules.Length];
        for (int i = 0; i < cellRules.Length; i++)
        {
            rulesFilter[i] = cellRules[i].Substring(1, 4);
        }

        foreach (CellType neighbor in neighbours)
        {
            for (int i = 0; i < rulesFilter.Length; i++)
            {
                int charIndex = rulesFilter[i].IndexOf((char) (neighbor + '0'));
                if (charIndex != -1)
                    rulesFilter[i] = rulesFilter[i].Remove(charIndex, 1);
            }
        }

        for (int i = 0; i < rulesFilter.Length; i++)
        {
            if (rulesFilter[i] == string.Empty)
            {
                CellType next = (CellType) (cellRules[i][5] - '0');
                return next;
            }
        }

        //Should not get to this point
        return cell;
    }


    public override void Step()
    {
        CheckNeighbours();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                int index = j + Width * i;
                CellType cell = Matrix[index];
                CellType[] neighbours = _neighbourMatrix[index];
                Matrix[index] = NextState(cell, neighbours);
            }
        }
    }
}