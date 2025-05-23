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
        System.Collections.Generic.List<WallDowner> downersToGet = new System.Collections.Generic.List<WallDowner>();
        for (int i = 0; i < transform.childCount; i++)
        {
            WallDowner wallDowner = transform.GetChild(i).GetComponent<WallDowner>();
            downersToGet.Add(wallDowner);
            if (!overwriteLowerHeight) continue;
            wallDowner.SetLowerHeight(loweredHeight);
        }
        wallDowners = downersToGet.ToArray();
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
