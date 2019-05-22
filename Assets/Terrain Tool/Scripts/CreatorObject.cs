using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Nombre en popup", order = 1)]
public class SavesNodes : ScriptableObject
{
    public string prefabName;


    List<DrawnNode> datas;



    public int numberOfPrefabsToCreate;


    public Vector3[] spawnPoints;

}
