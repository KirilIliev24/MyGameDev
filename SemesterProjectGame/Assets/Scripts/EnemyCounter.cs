using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    public TextMeshProUGUI enemyCounterText;
    public int enemiesLeft;

    // Start is called before the first frame update
    void OnEnable()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        enemiesLeft = enemies.Length;

        Debug.Log($"Enemy subcribed");
        EnemyScript.dieEvent += EnemyHasDied;
        SetEnemyCount(enemiesLeft);
    }

    void OnDisable()
    {
        Debug.Log($"Enemy unsubcribed from OnDisable");
        EnemyScript.dieEvent -= EnemyHasDied;
    }

    public void SetEnemyCount(int count)
    {
        enemyCounterText.text = $"Enemies left: {count}";
    }

    public void EnemyHasDied()
    {
        enemiesLeft--;
        Debug.Log($"Enemies left: {enemiesLeft}");
        SetEnemyCount(enemiesLeft);
        Debug.Log($"Enemy unsubcribed after death");
    }
}
