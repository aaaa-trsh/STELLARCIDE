using System;
using System.Collections.Generic;
using System.IO;
using GenerationScripts;
using MapScripts;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

[Serializable]
public sealed class GenNode {
    public string Id;
    public string Type;              			// "Combat", "Survival", more(?)
    public int Width;
    public int Depth;
    public int Difficulty;
    public string Description;
    public int RedDwarfWeight;
    public int YellowDwarfWeight;
    public int RedGiantWeight;
    
    public GenNode(string id, string type, int width, int depth, int difficulty, string description)
    {
        Id = id;
        Type = type;
        Width = width;
        Depth = depth;
        Difficulty = difficulty;
        Description = description;
        DifficultyRandomization();
    }

    public void DifficultyRandomization()
    {
        RedDwarfWeight = YellowDwarfWeight = RedGiantWeight = 0;
        // rand
        // change numbers with rand based on stored difficulty value (check for nulls)
        switch (Difficulty)
        {
            case 0:
                RedDwarfWeight = 100;
                break;
            case 1:
                RedGiantWeight = 100;
                break;
            case 2:
                RedDwarfWeight = 60;
                RedGiantWeight = 40;
                break;
            case 3:
                RedDwarfWeight = 90;
                YellowDwarfWeight = 10;
                break;
            case 4:
                RedDwarfWeight = 40;
                RedGiantWeight = 40;
                YellowDwarfWeight = 20;
                break;
        }
    }
}

[Serializable]
public class Root
{
    public List<GenNode> nodes;
    public List<GenEdge> edges;

    public Root(List<GenNode> nodes, List<GenEdge> edges)
    {
        this.nodes = nodes;
        this.edges = edges;
    }
}


[Serializable]
public sealed class GenEdge {
    public string From;
    public string To;

    public GenEdge(string from, string to)
    {
        From = from;
        To = to;
    }
}

[Serializable]
public sealed class GenGraph {
    public Dictionary<string, GenNode> Nodes = new();
    public List<GenEdge> Edges = new();
    public Dictionary<string, List<GenEdge>> Out = new();
    
    public void AddNode(GenNode node)
    {
        Nodes.Add(node.Id, node);
        Out[node.Id] = new List<GenEdge>();
    }

    public void AddEdge(GenEdge edge)
    {
        Edges.Add(edge);
        Out[edge.From].Add(edge);
    }

    public GenNode GetNode(string id)
    {
        return Nodes[id];
    }
}

public class SelectorMapGenerator : MonoBehaviour
{
    
    public GenGraph graph;
    public Root currentRoot;
    public Description desc;
    [SerializeField] private int maxDepth = 8;
    [SerializeField] private int maxWidth = 4;
    [SerializeField] private int minWidth = 2;

    public TextAsset descFile;
    public string descPath = "SavedFiles/DescriptionMaster";
    public string fileToReadWrite = "";
    public string fileName = "PlayerMapPosition.txt";
    
    private void Start()
    {
        descFile = Resources.Load<TextAsset>(descPath);
        desc = JsonConvert.DeserializeObject<Description>(descFile.text);
        
        // root generation
        if (PlayerPrefs.GetInt("NewGame") == 1) ClearJson();
        currentRoot = ReadFromFile();
        Generate();
    }
    
    void Generate()
    {
        // SKIP IF ALREADY CREATED!!!!! Check through write to JSON?
        if (currentRoot.edges.Count == 0)
        {
            Random rand = new Random();
            DescriptionFormatter df = new DescriptionFormatter();
            GenNode root = new GenNode("0", "root", 0, 0, 0, df.ReturnRandomDescription(desc, "origin"));
            graph.AddNode(root);
            currentRoot.nodes.Add(root);
            int nodeId = 1;
            int difficulty = 0;

            // create "columns" of nodes with depth, randomly creating 1 to max_width number of nodes
            int columnWidth = minWidth;
            for (int depth = 1; depth < maxDepth - 1; depth++)
            {
                for (int width = 0; width < columnWidth; width++)
                {
                    difficulty = rand.Next(0, 5);
                    string roomType = rand.Next(0, 2) == 0 ? "Survival" : "Exterminate";
                    currentRoot.nodes.Add(new GenNode($"{nodeId}", roomType, width, depth, difficulty, df.ReturnRandomDescription(desc, "origin")));
                    graph.AddNode(new GenNode($"{nodeId}", roomType, width, depth, difficulty, df.ReturnRandomDescription(desc, "origin")));
                    nodeId++;
                }

                if (columnWidth == minWidth)
                {
                    columnWidth++;
                }
                else if (columnWidth == maxWidth)
                {
                    columnWidth--;
                }
                else
                {
                    columnWidth += rand.Next(2) > 0 ? 1 : -1;
                }
            }

            difficulty = 4;
            //create end node (id: ?, type: end, width: ?, depth: max_depth-1)
            graph.AddNode(new GenNode($"{nodeId}", "end", 0, maxDepth - 1, difficulty, df.ReturnRandomDescription(desc, "origin")));
            currentRoot.nodes.Add(new GenNode($"{nodeId}", "end", 0, maxDepth - 1, difficulty, df.ReturnRandomDescription(desc, "origin")));

            // re-organize nodes based on depth
            Dictionary<int, List<GenNode>> nodesByDepth = new Dictionary<int, List<GenNode>>();
            foreach (GenNode node in graph.Nodes.Values)
            {
                if (!nodesByDepth.TryGetValue(node.Depth, out var list))
                {
                    list = new List<GenNode>();
                    nodesByDepth[node.Depth] = list;
                }

                list.Add(node);
            }

            // add edges to nodes based on depth
            for (int depth = 0; depth < maxDepth - 2; depth++)
            {
                if (!nodesByDepth.TryGetValue(depth, out var parents))
                    continue;
                if (!nodesByDepth.TryGetValue(depth + 1, out var children))
                {
                    continue;
                }

                // root node case
                if (depth == 0)
                {
                    foreach (GenNode child in children)
                    {
                        graph.AddEdge(new GenEdge(parents[0].Id, child.Id));
                            currentRoot.edges.Add(new GenEdge(parents[0].Id, child.Id));
                    }

                    continue;
                }

                // Link to width-adjacent children
                if (parents.Count < children.Count)
                {
                    foreach (GenNode parent in parents)
                    {
                        foreach (GenNode child in children)
                        {
                            if (child.Width == parent.Width || child.Width == parent.Width + 1)
                            {
                                graph.AddEdge(new GenEdge(parent.Id, child.Id));
                                currentRoot.edges.Add(new GenEdge(parent.Id, child.Id));
                            }
                        }
                    }
                }
                else if (depth != maxDepth - 2)
                {
                    foreach (GenNode parent in parents)
                    {
                        foreach (GenNode child in children)
                        {
                            if (child.Width == parent.Width || child.Width == parent.Width - 1)
                            {
                                graph.AddEdge(new GenEdge(parent.Id, child.Id));
                                currentRoot.edges.Add(new GenEdge(parent.Id, child.Id));
                            }
                        }
                    }
                }
            }

            // end node case
            GenNode endNode = graph.GetNode($"{nodeId}");
            foreach (GenNode parent in nodesByDepth[maxDepth - 2])
            {
                graph.AddEdge(new GenEdge(parent.Id, endNode.Id));
                currentRoot.edges.Add(new GenEdge(parent.Id, endNode.Id));
            }
        }
        else
        {
            // data of the JSON: Root -> List of Nodes and Edges
            // Step 1: grab nodes

            foreach (GenNode node in currentRoot.nodes)
            {
                graph.AddNode(node);
            }

            // Step 2: grab edges
            foreach (GenEdge edge in currentRoot.edges)
            {
                graph.AddEdge(edge);    
            }
        }
        WriteToFile(currentRoot);
        GetComponent<GraphUIRenderer>().DrawGraph(graph);
    }

    public void ChangePlayerLocation(string nodeId)
    {
        PlayerPrefs.SetString("PlayerPosition", nodeId);
        PlayerPrefs.Save();
        // modify the root and re-write json
        foreach (GenNode node in currentRoot.nodes)
        {
            if (node.Id == PlayerPrefs.GetString("PlayerPosition") && int.Parse(node.Id) == currentRoot.nodes.Count-1)
            {
                // clearing the json effectively wipes the map and lets the next instance create it anew
                ClearJson();
                return;
            }
        }
        WriteToFile(currentRoot);
    }

    public Root ReadFromFile()
    {
        if (!PlayerPrefs.HasKey("PlayerPosition"))
            PlayerPrefs.SetString("PlayerPosition", "0");
        PlayerPrefs.Save();

        fileToReadWrite = DataHandler.loadData(fileName);
        
        if (fileToReadWrite == null)
        {
            return new Root(new List<GenNode>(), new List<GenEdge>());
        }
        
        // break apart json into node and edges text
        string json = fileToReadWrite;
        
        currentRoot = JsonConvert.DeserializeObject<Root>(json);

        // debug prints
        /*foreach (GenNode node in currentRoot.nodes)
            print(node.Id);
        foreach(GenEdge edge in currentRoot.edges)
            print(edge.From + "->" + edge.To);
        */
        
        // reached end of tree, so start with a new tree.
        if(currentRoot.nodes[currentRoot.nodes.Count - 1].Id == PlayerPrefs.GetString("PlayerPosition"))
            return new Root(new List<GenNode>(), new List<GenEdge>());
        
        print("found existing graph");
        return currentRoot;
    }

    public void WriteToFile(Root root)
    {
        string save = JsonConvert.SerializeObject(root);
        //Debug.Log(save);
        DataHandler.saveData(save, fileName);
    }

    public void ClearJson()
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, fileName);
        print(tempPath);
        if (File.Exists(tempPath))
        {
            DataHandler.deleteData(fileName);
        }
        PlayerPrefs.SetInt("NewGame", 0);
        
        PlayerPrefs.SetString("PlayerPosition", "0");
    }
}