using UnityEngine;

[System.Serializable]
public class TransformData
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale = Vector3.one;

    public TransformData(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
        scale = transform.localScale;
    }
}
