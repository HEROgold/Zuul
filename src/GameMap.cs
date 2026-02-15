public class GameMap
{
    public static Room WinRoom { get; private set; }
    public static Room MapRoom { get; private set; } = new("Map Room", "in the campus map room");
    public static Room NurgleRoom { get; private set; }
    public static Room StartRoom { get; private set; }
    public static string CampusMap { get; private set; }

    static GameMap()
    {
        CreateRooms();
    }

    private static void CreateRooms()
    {
        Room outside = new("Outside", "outside the main entrance of the university");
        Room theatre = new("Theatre", "in a lecture theatre");
        Room pub = new("Pub", "in the campus pub");
        Room lab = new("Lab", "in a computing lab");
        Room office = new("Office", "in the computing admin office");
        Room kitchen = new("Kitchen", "in the pub's kitchen");
        Room cellar = new("Cellar", "in the pub's cellar");
        Room backyard = new("Backyard", "in the backyard");
        Room infirmary = new("Infirmary", "in the university infirmary");

        outside.SetExit(Direction.East, ExitProvider.Normal(theatre));
        outside.SetExit(Direction.South, ExitProvider.Normal(lab));
        outside.SetExit(Direction.West, ExitProvider.Normal(pub));

        theatre.SetExit(Direction.West, ExitProvider.Normal(outside));
        theatre.SetExit(Direction.South, ExitProvider.Normal(backyard));
        theatre.AddLock(); // Win room requires hydraulics to unlock

        pub.SetExit(Direction.East, ExitProvider.Normal(outside));
        pub.SetExit(Direction.South, ExitProvider.Normal(kitchen));

        lab.SetExit(Direction.North, ExitProvider.Normal(outside));
        lab.SetExit(Direction.East, ExitProvider.Normal(office));
        lab.SetExit(Direction.South, ExitProvider.Healing(infirmary, 5, 15));

        office.SetExit(Direction.West, ExitProvider.Normal(lab));
        office.SetExit(Direction.East, ExitProvider.Normal(MapRoom));

        MapRoom.SetExit(Direction.West, ExitProvider.Normal(office));

        kitchen.SetExit(Direction.Down, ExitProvider.Hazardous(cellar, 1, 10));
        kitchen.SetExit(Direction.North, ExitProvider.Normal(pub));
        kitchen.SetExit(Direction.East, ExitProvider.Normal(backyard));
        kitchen.AddEnemy(EnemyProvider.MountOfFlesh);

        cellar.SetExit(Direction.Up, ExitProvider.Hazardous(kitchen, 1, 8));

        backyard.SetExit(Direction.West, ExitProvider.Hazardous(kitchen, 1, 5));
        backyard.SetExit(Direction.North, ExitProvider.Normal(theatre));

        infirmary.SetExit(Direction.North, ExitProvider.Normal(lab));

        outside.Chest.Put(ItemProvider.Bandage);
        pub.Chest.Put(ItemProvider.Medkit);
        cellar.Chest.Put(ItemProvider.Key);
        backyard.Chest.Put(ItemProvider.MetalRod);
        lab.Chest.Put(ItemProvider.Piston);
        kitchen.Chest.Put(ItemProvider.DuctTape);

        // Set special rooms
        WinRoom = theatre;
        NurgleRoom = cellar;
        StartRoom = outside;

        CampusMap = BuildMapDescription(
            [outside, theatre, pub, lab, office, MapRoom, kitchen, cellar, backyard, infirmary]);
    }

    private static string BuildMapDescription(IEnumerable<Room> rooms)
    {
        var roomLines = rooms
            .OrderBy(r => r.Name)
            .Select(r =>
            {
                var exitInfos = r.GetExitInfos().ToList();
                if (exitInfos.Count == 0) return $"{r.Name}: no exits";

                var exitsText = exitInfos
                    .OrderBy(info => info.direction.ToDirectionString())
                    .Select(info => $"{info.direction.ToDirectionString()} -> {info.destination.Name}");

                return $"{r.Name}: {string.Join(", ", exitsText)}";
            });

        return "Campus Map:\n" + string.Join("\n", roomLines);
    }
}
