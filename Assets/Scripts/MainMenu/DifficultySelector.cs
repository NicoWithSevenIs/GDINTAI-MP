using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelector : ScrollablePanel
{
    private void Start()
    {
        MenuManager.instance.onDifficultySelect += setPage;
    }

}
