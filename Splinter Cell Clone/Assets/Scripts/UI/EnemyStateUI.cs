using TMPro;
using UnityEngine;

public class EnemyStateText : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] TextMeshProUGUI stateText;

    void Update()
    {
        stateText.text = enemy.GetCurrentState().ToString();
    }
}
