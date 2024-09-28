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
            Destroy(effect, 2f);


        }
    }

    // TODO: rotate the tiwaz effect to face the mouse
    // move towaz towards mouse quickly


    // perthro: randomize the position of the effect

    // naudiz: slow down time

    // isaz: freeze enemies

    // ansuz: show names of all objects in the scene
}
