using UnityEditor;
using UnityEngine;

public class SelectAllTagsWizard : ScriptableWizard
{
    public string tagName;
    [MenuItem("/My Tools/Select By Tag")]
    public static void CreateWizard()
    {
        DisplayWizard<SelectAllTagsWizard>("Select By Tag Name", "Select");
    }

    public void OnWizardCreate()
    {
        GameObject[] findByTag = GameObject.FindGameObjectsWithTag(tagName);
        Selection.objects = findByTag;
    }

    public void OnWizardUpdate()
    {
    }
}