using UnityEngine;

public class UtilityGame : MonoBehaviour
{
    /// <summary>
    /// Find Transform in child tree
    /// </summary>
    public static Transform GetTransformChild(Transform _parent, string nameChild)
    {
        foreach (Transform _transform in _parent.GetComponentsInChildren<Transform>())
        {
            if (_transform.name == nameChild)
            {
                return _transform;
            }
        }
        return null;
    }
}