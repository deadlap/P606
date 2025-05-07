using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class YesThisIsBugFix : MonoBehaviour
{
    private int theBugToBeFixed = 0;

    public void DoTheFixing()
    {
        theBugToBeFixed++;
        if (theBugToBeFixed == 10)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
