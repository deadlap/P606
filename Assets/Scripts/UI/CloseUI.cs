using UnityEngine;

public class CloseUI : MonoBehaviour
{
    public void Close()
    {
        if(PlayerInputEvent.isUIOpen)
        {
            PlayerInputEvent.OnCloseUI();
        }
    }
}
