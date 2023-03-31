using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform Map;
    public Transform newMap;
    public GameManager GameManager;

    public void StartNewRound()
    {
        if (GameManager.stage >= 11 && !GameManager.stage11MapSwitched)
        {
            MoveToNewMap();
            ApplyOffsetToActiveChildren();
            GameManager.stage11MapSwitched = true;
        }
    }

    private void MoveToNewMap()
    {
        Map.position = newMap.position;
    }

    public void ApplyOffsetToActiveChildren()
{
    ChildMapAdjuster[] childAdjusters = GetComponentsInChildren<ChildMapAdjuster>(true);
    foreach (ChildMapAdjuster adjuster in childAdjusters)
    {
        if (adjuster.gameObject.transform.parent.gameObject.activeInHierarchy)
        {
            adjuster.ApplyOffset();
        }
    }
}
}