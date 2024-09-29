using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject tiwazPrefab; // t
    [SerializeField] GameObject ansuzPrefab; // a
    [SerializeField] GameObject sowiloPrefab; // s
    [SerializeField] GameObject perthroPrefab; // p
    [SerializeField] GameObject naudizPrefab;  // n
    [SerializeField] GameObject isazPrefab; // i

    Dictionary<string, GameObject> magicEffects = new Dictionary<string, GameObject>();
    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        magicEffects.Add("t", tiwazPrefab);
        magicEffects.Add("a", ansuzPrefab);
        magicEffects.Add("s", sowiloPrefab);
        magicEffects.Add("p", perthroPrefab);
        magicEffects.Add("n", naudizPrefab);
        magicEffects.Add("i", isazPrefab);
    }

    public void SpawnEffect(string letter)
    {
        // Spawn the correct effect based on the letter

        if (magicEffects.ContainsKey(letter))
        {
            GameObject effect = Instantiate(magicEffects[letter], player.position, Quaternion.identity);

            if (letter == "t")
            {
                if (gameManager.enemies.Count != 0)
                {
                    FaceClosestEnemy(effect);
                }
            }
        }

        void FaceClosestEnemy(GameObject effect)
        {
            Vector3 closestEnemyPos = GetClosestEnemy().transform.position;
            Vector3 dir = closestEnemyPos - player.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            effect.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

        GameObject GetClosestEnemy()
        {
            GameObject closestEnemy = null;
            float closestDistance = float.MaxValue;

            foreach (var enemy in gameManager.enemies)
            {
                float distance = Vector3.Distance(player.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }
    }


    // move towaz towards mouse quickly


    // perthro: randomize the position of the effect

    // naudiz: slow down time

    // isaz: freeze enemies

    // ansuz: show names of all objects in the scene
}
