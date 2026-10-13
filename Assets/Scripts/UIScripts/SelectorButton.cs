using System;
using MapScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SelectorButton : MonoBehaviour
{
    public string nodeId;
    public bool isPlayerAdjacent = false;
    public SelectorMapGenerator mapGenerator;
    public GraphUIRenderer graphUIRenderer;

    private void Start()
    {
        graphUIRenderer =  FindAnyObjectByType<GraphUIRenderer>();
    }

    public void OnClick()
    {
        if (!isPlayerAdjacent) return;

        graphUIRenderer.MissionChange(gameObject, nodeId);
        SpawnerManager.Instance.initialRedDwarfWeight = mapGenerator.graph.Nodes[nodeId].RedDwarfWeight;
        SpawnerManager.Instance.initialRedGiantWeight = mapGenerator.graph.Nodes[nodeId].RedGiantWeight;
        SpawnerManager.Instance.initialYellowDwarfWeight = mapGenerator.graph.Nodes[nodeId].YellowDwarfWeight;
        SpawnerManager.Instance.roundType = mapGenerator.graph.Nodes[nodeId].Type == "Survival" ? RoundType.SURVIVAL : RoundType.EXTERMINATE;
        Debug.Log(SpawnerManager.Instance.roundType);
    }

    // public void Selected()
    // {
    //     // TODO needs code here that changes to the selected node's map scene.
    //     mapGenerator.ChangePlayerLocation(nodeId);
    //     GameManager.Instance.difficultySum += mapGenerator.graph.Nodes[nodeId].Difficulty + 1;
    //     SceneManager.LoadScene("Scenes/MapTestScene");
    // }
}
