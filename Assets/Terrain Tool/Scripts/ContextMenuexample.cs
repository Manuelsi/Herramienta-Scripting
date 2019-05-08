using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

public class ContextMenuexample : MonoBehaviour
{
    #region NO REQUIERE IMPORTAR UNITY EDITOR

    public string _myName;

    //añade un context menu especifico a la VARIABLE. pide el texto a mostrar,
    //seguido del nombre de la funcion a ejecutar
    [ContextMenuItem("RandomHP", "Randomize")]
    public int myHp;

    //agrega este elemento al menu contextual del COMPONENTE, para ejecutar esta funcion
    [ContextMenu("Reset name value")]
    private void ResetName()
    {
        _myName = "";
    }

    private void Randomize()
    {
        myHp = Random.Range(0, 100);
    }
    #endregion

    #region REQUIERE UNITY_EDITOR

    [MenuItem("CONTEXT/Transform/ResetAll")]
    private static void MyTransformOption(MenuCommand menuCommand)
    {
        var tr = menuCommand.context as Transform; //consigo referencia al transform clikeado
        tr.position = Vector3.one;
        tr.rotation = Quaternion.identity;
        tr.localScale = Vector3.one;
    }
    #endregion
}
