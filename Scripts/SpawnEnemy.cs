using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class SpawnEnemy : MonoBehaviour
{
    private Vector3 dir;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Enemy());
    }

    public void Setup(Vector3 dir)
    {
        this.dir = dir;
    }
    
    IEnumerator Enemy()
    {
        yield return new WaitForSeconds(3);
        //GameObject prefabObject = (GameObject)LoadPrefabFromFile("Bot");

        GameObject prefabObject = Resources.Load<GameObject>("Bot");

        GameObject pNewObject =
            (GameObject)Instantiate(prefabObject, dir, Quaternion.identity);

        pNewObject.name = "Bot";

        pNewObject.transform.SetParent(GameObject.Find("BotContainer").transform);
        
        Destroy(this.gameObject);
    }

/*    private UnityEngine.Object LoadPrefabFromFile(string filename)
    {
#if UNITY_EDITOR
        var loadedObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/" + filename + ".prefab");
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        return loadedObject;
#endif
    }*/
}
