using System.Collections;
using UnityEngine;

public class StartDuelDisplay : MonoBehaviour
{
    public Duel DuelMaster;

    public void BeginDuelEvent()
    {
        DuelMaster.BeginDisplay();
    }

    public void DisableTransition()
    {
        this.gameObject.SetActive(false);
    }
}
