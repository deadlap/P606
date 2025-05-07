using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class YesThisIsBugFix : MonoBehaviour
{
    private uint theBugToBeFixed = 0;

    public void DoTheFixing()
    {
        theBugToBeFixed++;
        if (theBugToBeFixed == 10)
        {
            GetComponent<AudioSource>().Play();
            theBugToBeFixed -= 10;
        }
    }
}
