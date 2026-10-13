using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MapScripts
{
    public class GraphUIRenderer : MonoBehaviour
    {
        [Header("Layout")]
        public float xSpacing = 180f;
        public float ySpacing = 90f;
        public float nodeSize = 40f;
        public Vector2 margin = new Vector2(100f, 100f);
        public Vector2 startPoint = new Vector2(-500f, 0f);

        [Header("Button Colors and Font")]
        public Color rootColor = new Color(0.5f, 0.75f, 1f);
        public List<Color> areaColor = new List<Color> { Color.green , Color.blue, Color.yellow, new Color(1, .5f, .13f), Color.red};
        public Color endColor = new Color(1f, 0.5f, 0.5f);
        public Color edgeColor = Color.black;
        public Font font = null;
        public int fontSize = 20;
    
        [Header("Prefabs")]
        public GameObject buttonPrefab;
        public Image edgePrefab;
        public GameObject playerMarkerPrefab;
        public GameObject ringPrefab;
        public GameObject missionPanelPrefab;

        // References
        private Dictionary<string, RectTransform> nodeRects = new();
        private GenGraph graphRef;
        private GenNode playerPosition;
        private GameObject ring;
        private MissionPanel missionPanel;

        private void Start()
        {
            ring = Instantiate(ringPrefab, gameObject.transform);
            ring.SetActive(false);
        }

        public void DrawGraph(GenGraph graph)
        {
            RectTransform canvasRect = GetComponent<RectTransform>();
            graphRef = graph;

            // Group nodes by depth
            Dictionary<int, List<GenNode>> nodesByDepth = new();
            foreach (var node in graph.Nodes.Values)
            {
                if (!nodesByDepth.TryGetValue(node.Depth, out var list))
                {
                    list = new List<GenNode>();
                    nodesByDepth[node.Depth] = list;
                }
                list.Add(node);
            }

            // Draw nodes
            foreach (var kvp in nodesByDepth)
            {
                int depth = kvp.Key;
                List<GenNode> nodes = kvp.Value;
            
                float x = margin.x + depth * xSpacing;
                float startY = canvasRect.sizeDelta.y/4 - (nodes.Count-1)*ySpacing/2;
            
                for (int i = 0; i < nodes.Count; i++)
                {
                    GenNode node = nodes[i];
                    
                    float y = startY + i * ySpacing;
                    Vector2 pos = new Vector2(x,y);

                    RectTransform rect = CreateNode(node, pos+startPoint, node.Difficulty);
                    nodeRects[node.Id] = rect;
                }
            }

            // Draw edges
            foreach (GenEdge edge in graph.Edges)
            {
                RectTransform from = nodeRects[edge.From];
                RectTransform to = nodeRects[edge.To];

                CreateEdge(from, to);
            }
        }

        /// <summary>
        /// Create Node creates an instance of the buttonPrefab at a position (that aligns with the algorithm above).
        /// This buttonPrefab will be passed all node information corresponding to it from the SelectorMapGenerator
        /// class. Difficulty changes node color
        /// </summary>
        /// <param name="node"></param>
        /// <param name="position"></param>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        RectTransform CreateNode(GenNode node, Vector2 position, int difficulty)
        {
            GameObject go = Instantiate(buttonPrefab, transform, false);

            // image 
            Image img = go.GetComponent<Image>();
            img.color = node.Type switch
            {
                "root" => rootColor,
                "end" => endColor,
                _ => areaColor[difficulty]
            };

            // determine position
            RectTransform rect = img.rectTransform;
            rect.sizeDelta = new Vector2(nodeSize, nodeSize);
            rect.anchoredPosition = position;
            
            // pass node information
            SelectorButton sb = go.GetComponent<SelectorButton>();

            sb.nodeId = node.Id;
            sb.mapGenerator = GetComponent<SelectorMapGenerator>();

            //print("made it");
            if (node.Id == PlayerPrefs.GetString("PlayerPosition"))
            {
                playerPosition = node;
                Instantiate(playerMarkerPrefab, go.transform);
                return rect;
            }

            // change player adjacency
            if (playerPosition != null)
            {
                foreach (GenEdge edge in graphRef.Out[playerPosition.Id])
                {
                    if (edge.To == node.Id)
                    {
                        sb.isPlayerAdjacent = true;
                    }
                }
            }

            return rect;
        }

        /// <summary>
        /// Create Edge creates a new UI element named "Edge" that will take two Rect Transforms
        /// in order to "draw" a line between them using itself
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        void CreateEdge(RectTransform from, RectTransform to)
        {
            GameObject go = new GameObject("Edge", typeof(Image));
            go.transform.SetParent(transform, false);

            Image img = go.GetComponent<Image>();
            img.color = edgeColor;

            RectTransform rect = img.rectTransform;

            Vector2 a = from.anchoredPosition;
            Vector2 b = to.anchoredPosition;
            Vector2 dir = b - a;

            float length = dir.magnitude;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            rect.sizeDelta = new Vector2(length - nodeSize, 3f);
            rect.anchoredPosition = a + dir * 0.5f;
            rect.rotation = Quaternion.Euler(0, 0, angle);
        }

        public void MissionChange(GameObject go, string id)
        {
            if (missionPanel == null)
            {
                GameObject temp = Instantiate(missionPanelPrefab, transform);
                missionPanel = temp.GetComponent<MissionPanel>();
                missionPanel.gameObject.SetActive(false);
            }
            
            // ring changes
            ring.transform.position = go.transform.position; 
            ring.SetActive(true);
            
            // mission panel changes
            GenNode selected = graphRef.Nodes[id];
            
            // grab difficulty and description
            missionPanel.description.text =  $"{selected.Type} MISSION\n\nMISSION REPORT\n\n{selected.Description}";
            missionPanel.scoreMult.text =  $"DIFFICULTY\nMULTIPLIER:\n{1 + selected.Difficulty * .5}X";
            missionPanel.currentNodeID = id;
            missionPanel.gameObject.SetActive(true);  
        }

        public void HideMissionPanel()
        {
            if (missionPanel != null)
            {
                missionPanel.gameObject.SetActive(false);
            }
        }
    }
}