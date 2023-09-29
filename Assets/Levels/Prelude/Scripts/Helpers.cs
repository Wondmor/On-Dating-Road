using UnityEngine;

public class Helpers
{ 
    static public GameObject FindChildByName(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name == name)
            {
                return child.gameObject;
            }

            GameObject result = FindChildByName(child, name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}
