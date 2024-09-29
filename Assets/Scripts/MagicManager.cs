using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectData
{
    public GameObject effectPrefab;
    public float holdButtonTime;

    public EffectData(GameObject effectPrefab, float holdButtonTime)
    {
        this.effectPrefab = effectPrefab;
        this.holdButtonTime = holdButtonTime;
    }
}

public class MagicManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject tiwazPrefab; // t
    [SerializeField] GameObject ansuzPrefab; // a
    [SerializeField] GameObject sowiloPrefab; // s
    [SerializeField] GameObject perthroPrefab; // p
    [SerializeField] GameObject naudizPrefab;  // n
    [SerializeField] GameObject isazPrefab; // i

    Dictionary<string, EffectData> magicEffects = new Dictionary<string, EffectData>();
    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        magicEffects.Add("t", new EffectData(tiwazPrefab, 0.5f));
        magicEffects.Add("a", new EffectData(ansuzPrefab, 1));
        magicEffects.Add("s", new EffectData(sowiloPrefab, 1));
        magicEffects.Add("p", new EffectData(perthroPrefab, 1));
        magicEffects.Add("n", new EffectData(naudizPrefab, 1));
        magicEffects.Add("i", new EffectData(isazPrefab, 1));
    }

    private void OnEnable()
    {
        DraggableLetter.onSelect += OnLetterSelect;
        DraggableLetter.onRelease += CancelLetterHold;
    }

    private void OnDisable()
    {
        DraggableLetter.onSelect -= OnLetterSelect;
        DraggableLetter.onRelease -= CancelLetterHold;
    }

    public void SpawnEffect(string letter)
    {
        // Spawn the correct effect based on the letter

        if (magicEffects.ContainsKey(letter))
        {
            GameObject effect = Instantiate(magicEffects[letter].effectPrefab, player.position, Quaternion.identity);

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

    public IEnumerator HoldButtonRoutine(DraggableLetter draggableLetter)
    {
        EffectData effectData = magicEffects[draggableLetter.value];
        Debug.Log("start coroutine");
        yield return new WaitForSeconds(effectData.holdButtonTime);
        Debug.Log("reach end of coroutine");

        if (draggableLetter.state == ButtonState.Pressed)
        {
            SpawnEffect(draggableLetter.value);
        }
    }

    void CancelLetterHold(DraggableLetter draggableLetter)
    {
        //StopCoroutine(HoldButtonRoutine(draggableLetter));
        Debug.Log("cancel hold");
        StopAllCoroutines();
    }

    void OnLetterSelect(DraggableLetter draggableLetter)
    {
        if (draggableLetter.state == ButtonState.Idle)
            StartCoroutine(HoldButtonRoutine(draggableLetter));
    }


    // move towaz towards mouse quickly


    // perthro: randomize the position of the effect

    // naudiz: slow down time

    // isaz: freeze enemies

    // ansuz: show names of all objects in the scene
}
