using UnityEngine;
public enum Type
{
    BUTTON
}
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    void Awake()
    {
        instance = this;
    }
        
    public void Save(string id,bool state,Type type)
    {   
        if(type == Type.BUTTON)
        {
            PlayerPrefs.SetInt(id, state ? 1 :0);        
        }
    }
}
