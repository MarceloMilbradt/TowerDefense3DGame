using UnityEngine;

[CreateAssetMenu(fileName ="WaveModifier", menuName ="ScriptableObjects/WaveModifier")]
public class WaveModifier : ScriptableObject
{
    public Enemy enemyType;
    public int enemyCount;
}
