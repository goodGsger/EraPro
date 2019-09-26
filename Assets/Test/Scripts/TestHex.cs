using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Game;

public class TestHex : MonoBehaviour
{
    private List<HexObject> hexObjects;
    private List<RectObject> rectObjects;

    private void Awake()
    {
        App.inst.Init();
        App.inst.InitDefaultManagers();
        App.logManager.enabled = true;
        App.logManager.isPrint = true;

        InitHex();
        InitRect();
        InitSpine();
        TestSkill();
    }

    private void InitHex()
    {
        int cols = 20;
        int rows = 20;
        int size = 5;
        int type = OffsetCoord.POINTY;
        int offset = OffsetCoord.ODD;
        HexAStar astar = new HexAStar();
        HexGrid grid = new HexGrid();
        astar.grid = grid;
        hexObjects = new List<HexObject>();

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                OffsetCoord offsetCoord = new OffsetCoord(x, y);
                Hex hex = type == OffsetCoord.FLAT ? OffsetCoord.GetCubeFromQOffsetCoord(offset, offsetCoord) : OffsetCoord.GetCubeFromROffsetCoord(offset, offsetCoord);
                Vector2 point = type == OffsetCoord.FLAT ? OffsetCoord.GetQPixelFromOffsetCoord(offset, offsetCoord, size) : OffsetCoord.GetRPixelFromOffsetCoord(offset, offsetCoord, size);
                HexObject hexObject = new HexObject();
                hexObject.offsetCoord = offsetCoord;
                hexObject.hex = hex;
                hexObject.point = point;
                hexObject.transform.SetParent(GameObject.Find("HexMap").transform, false);
                hexObject.transform.rotation = Quaternion.Euler(90f, 0, 0);
                hexObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                hexObject.transform.localPosition = new Vector3(point.x, 0, point.y);
                hexObject.renderer.color = new Color(hexObject.renderer.color.r, hexObject.renderer.color.g, hexObject.renderer.color.b, 0.2f);
                //hexObject.transform.localPosition = new Vector3(point.x, point.y);
                hexObjects.Add(hexObject);
                bool moveable = Random.Range(0f, 1f) > 0.25f ? true : false;
                grid.AddNode(hex, moveable);
            }
        }

        while (true)
        {
            int index = MathUtil.RandomRangeInt(0, hexObjects.Count - 1);
            HexObject hexObject = hexObjects[index];
            HexNode node = grid.GetNode(hexObject.hex);
            if (node.moveable)
            {
                grid.SetStartNode(node);
                break;
            }
        }

        {
            HexObject hexObject = hexObjects[MathUtil.RandomRangeInt(0, hexObjects.Count - 1)];
            HexNode node = grid.GetNode(hexObject.hex);
            grid.SetEndNode(node);
        }

        List<HexNode> path = astar.Find(grid.startNode.hex, grid.endNode.hex);
        if (path != null)
        {
            Debug.Log("开始寻路：");
            foreach (HexNode node in path)
            {
                Debug.Log(node.hex);
            }
        }
        else
        {
            Debug.Log("路径为空：");
        }
    }

    private void InitRect()
    {
        int cols = 20;
        int rows = 20;
        int size = 10;
        AStar astar = new AStar();
        Framework.Grid grid = new Framework.Grid(GridType.DIRECTION_4);
        astar.grid = grid;
        grid.InitGrid(rows, cols);
        byte[][] gridData = new byte[rows][];
        rectObjects = new List<RectObject>();

        for (int x = 0; x < cols; x++)
        {
            gridData[x] = new byte[cols];
            for (int y = 0; y < rows; y++)
            {
                gridData[x][y] = (byte)(Random.Range(0f, 1f) > 0.25f ? 0 : 1);
                RectObject rectObject = new RectObject();
                rectObject.x = x;
                rectObject.y = y;
                rectObject.transform.SetParent(GameObject.Find("RectMap").transform, false);
                rectObject.transform.rotation = Quaternion.Euler(90f, 0, 0);
                rectObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                rectObject.transform.localPosition = new Vector3(x * size, 0, y * size);
                rectObject.renderer.color = new Color(rectObject.renderer.color.r, rectObject.renderer.color.g, rectObject.renderer.color.b, 0.2f);
                rectObjects.Add(rectObject);
            }
        }
        grid.InitGridData(gridData);

        while (true)
        {
            int index = MathUtil.RandomRangeInt(0, rectObjects.Count - 1);
            RectObject rectObject = rectObjects[index];
            Node node = grid.GetNode(rectObject.x, rectObject.y);
            if (node.moveable)
            {
                grid.SetStartNode(node.x, node.y);
                break;
            }
        }

        {
            RectObject rectObject = rectObjects[MathUtil.RandomRangeInt(0, rectObjects.Count - 1)];
            grid.SetEndNode(rectObject.x, rectObject.y);
        }

        List<Node> path = astar.Find(grid.startNode.x, grid.startNode.y, grid.endNode.x, grid.endNode.y);
        if (path != null)
        {
            Debug.Log("开始寻路：");
            foreach (Node node in path)
            {
                Debug.Log(node.x + "," + node.y);
            }
        }
        else
        {
            Debug.Log("路径为空：");
        }
    }

    private void InitSpine()
    {
        SpineManager.inst.CreateSpineDataFromPath("hero-pro", "Assets/Test/Resources/ab/hero-pro.ab");

        CreateSpineRender(new Vector3(39, 0, 112), Quaternion.Euler(0, 0, 0));
        CreateSpineRender(new Vector3(13, 0, 82), Quaternion.Euler(0, 0, 0));
        CreateSpineRender(new Vector3(35, 0, 60), Quaternion.Euler(0, 0, 0));

        CreateSpineRender(new Vector3(95, 0, 120), Quaternion.Euler(0, 180, 0));
        CreateSpineRender(new Vector3(112, 0, 90), Quaternion.Euler(0, 180, 0));
    }

    private SpineRenderer CreateSpineRender(Vector3 position, Quaternion rotation)
    {
        SpineRenderer sr = SpineManager.inst.CreateSpineRenderer("hero-pro", true);
        sr.transform.SetParent(GameObject.Find("Spine").transform, false);
        sr.skeletonAnimation.state.SetAnimation(0, "idle", true);
        sr.skeletonAnimation.skeletonDataAsset.atlasAssets[0].PrimaryMaterial.shader = ShaderManager.inst.GetShader("Spine/Skeleton Fill");
        sr.transform.localPosition = position;
        sr.transform.localRotation = rotation;
        sr.transform.localScale = new Vector3(3, 3, 3);

        return sr;
    }

    private void TestSkill()
    {
        SA_PlayEffect sa1 = new SA_PlayEffect();
        sa1.totalTime = 0;
        sa1.startTime = 0.5f;
        SA_Camera sa2 = new SA_Camera();
        sa2.needUpdate = true;
        sa2.totalTime = 3;
        sa2.startTime = 0f;
        SA_PlayEffect sa3 = new SA_PlayEffect();
        sa3.totalTime = 0;
        sa3.startTime = 2;
        SA_Camera sa4 = new SA_Camera();
        sa4.needUpdate = true;
        sa4.totalTime = 2f;
        sa4.startTime = 3f;

        SkillConfig skillConfig = new SkillConfig();
        skillConfig.totalTime = 5f;
        skillConfig.AddAction(sa1);
        skillConfig.AddAction(sa2);
        skillConfig.AddAction(sa3);
        skillConfig.AddAction(sa4);

        SkillEntity skillEntity = new SkillEntity();
        skillEntity.config = skillConfig;

        SkillManager.inst.AddSkill(skillEntity);
        skillEntity.Start();
    }

    private void Update()
    {
        SkillManager.inst.Update(Time.deltaTime);
    }
}

public class HexObject
{
    public Hex hex;
    public OffsetCoord offsetCoord;
    public Vector2 point;

    public GameObject gameObject;
    public Transform transform;
    public SpriteRenderer renderer;
    public Material material;
    public MaterialPropertyBlock mpb;

    public HexObject()
    {
        gameObject = new GameObject("HexObject");
        transform = gameObject.transform;
        renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("hex/尖图");
        material = renderer.sharedMaterial;
        mpb = new MaterialPropertyBlock();
    }
}

public class RectObject
{
    public int x;
    public int y;

    public GameObject gameObject;
    public Transform transform;
    public SpriteRenderer renderer;
    public Material material;
    public MaterialPropertyBlock mpb;

    public RectObject()
    {
        gameObject = new GameObject("RectObject");
        transform = gameObject.transform;
        renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("hex/正方形");
        material = renderer.sharedMaterial;
        mpb = new MaterialPropertyBlock();
    }
}