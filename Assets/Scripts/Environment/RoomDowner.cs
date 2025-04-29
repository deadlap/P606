using RoomFocusing;

public class RoomDowner : WallDowner
{
    private WallDowner[] wallDowners;

    private void Awake()
    {
        // Avoid this being duplicated in a peculiar way
        isCopy = true;
    }

    private void Start()
    {
        wallDowners = GetComponentsInChildren<WallDowner>();
    }

    public override void PlayerEntered()
    {
        foreach (WallDowner wallDowner in wallDowners)
        {
            wallDowner.PlayerEntered();
        }
        //Debug.Log($"There are {playersInside} players inside {name}");
    }
    public override void PlayerExited()
    {
        foreach (WallDowner wallDowner in wallDowners)
        {
            wallDowner.PlayerExited();
        }
        //Debug.Log($"There are {playersInside} players inside {name}");
    }
}
