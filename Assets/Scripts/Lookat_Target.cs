using UnityEngine;

public class Lookat_Target : MonoBehaviour
{

    public bool isTargetable = true;

    public Transform parentTransform()
    {
        if (isTargetable)
        {
            return GetComponentInParent<Transform>();
        }
        else
        {
            return null;
        }

    }

}
