using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMapAdjuster : MonoBehaviour
{
    public Vector3 localOffset;

    public void ApplyOffset()
    {
        GameObject enemyRespawnZoneGroup = GameObject.Find("Enemy Respawn Zone Group");
        List<GameObject> respawnZones = new List<GameObject>();

        if (enemyRespawnZoneGroup != null)
        {
            for (int i = 0; i < enemyRespawnZoneGroup.transform.childCount; i++)
            {
                GameObject child = enemyRespawnZoneGroup.transform.GetChild(i).gameObject;
                respawnZones.Add(child);
                child.SetActive(true);
            }
        }

        transform.localPosition += localOffset;

        if (enemyRespawnZoneGroup != null)
        {
            foreach (GameObject respawnZone in respawnZones)
            {
                respawnZone.SetActive(false);
            }
        }
    }
}