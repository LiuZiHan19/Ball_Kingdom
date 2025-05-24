using UnityEditor;
using UnityEngine;

public class PrefabIdentify : MonoBehaviour
{
    public string Identify;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(Identify))
        {
            Identify = GUID.Generate().ToString();
        }
    }
}