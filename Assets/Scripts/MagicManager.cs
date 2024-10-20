using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EffectData
{
    public GameObject effectPrefab;
    public float holdButtonTime;
    public bool isFacingClosestEnemy;
    public int spawnCount;
    public string letter;

    public EffectData(GameObject effectPrefab, float holdButtonTime, bool isFacingClosestEnemy, int spawnCount, string letter)
    {
        this.effectPrefab = effectPrefab;
        this.holdButtonTime = holdButtonTime;
        this.isFacingClosestEnemy = isFacingClosestEnemy;
        this.spawnCount = spawnCount;
        this.letter = letter;
    }
}

public class MagicManager : MonoBehaviour
{
    public static event Action<string> onEffectSpawn;
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
        magicEffects.Add("t", new EffectData(tiwazPrefab, 0.8f, true, 1, "t"));
        magicEffects.Add("a", new EffectData(ansuzPrefab, 0.4f, false, 1, "a"));
        magicEffects.Add("s", new EffectData(sowiloPrefab, 0.6f, false, 1, "s"));
        magicEffects.Add("p", new EffectData(perthroPrefab, 0.3f, true, 3, "p"));
        magicEffects.Add("n", new EffectData(naudizPrefab, 0.3f, false, 1, "n"));
        magicEffects.Add("i", new EffectData(isazPrefab, 0.7f, true, 1, "i"));
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
            EffectData data = magicEffects[letter];

            for (int i = 0; i < data.spawnCount; i++)
            {
                SpawnEffectSingle(data);
            }
        }
    }

    void SpawnEffectSingle(EffectData data)
    {
        onEffectSpawn?.Invoke(data.letter);

        GameObject effect = Instantiate(data.effectPrefab, player.position, Quaternion.identity);

        if (data.isFacingClosestEnemy)
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

    public IEnumerator HoldButtonRoutine(DraggableLetter draggableLetter)
    {
        EffectData effectData = magicEffects[draggableLetter.value];
        yield return new WaitForSeconds(effectData.holdButtonTime);

        if (draggableLetter.state == ButtonState.Pressed)
        {
            SpawnEffect(draggableLetter.value);
        }
    }

    void CancelLetterHold(DraggableLetter draggableLetter)
    {
        StopAllCoroutines();
    }

    void OnLetterSelect(DraggableLetter draggableLetter)
    {
        if (draggableLetter.state == ButtonState.Idle)
            StartCoroutine(HoldButtonRoutine(draggableLetter));
    }
}
