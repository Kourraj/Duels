using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDuelDisplay : MonoBehaviour
{
    public Duel DuelMaster;

    public void BeginDuelDisplay()
    {
        DuelMaster.BeginDisplay();
        this.gameObject.SetActive(false);
    }
}
