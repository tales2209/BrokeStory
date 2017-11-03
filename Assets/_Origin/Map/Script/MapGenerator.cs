using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    TILE_NONE = -1,
    TILE_SAND,
    TILE_GRASS,
   
    TILE_CLAFF,
   
    TILE_MAPLE,
    TILE_MAPLE_2,
    
   
    TILE_JUNGLE,
    TILE_GRASS_2,
    TILE_OLDRUIN,
    TILE_ICE,
    TILE_SNOW,
    TILE_MUD,
    TILE_MUD_2,



    


    TILE_MAX


}

public class MapGenerator : MonoBehaviour
{

    //public Texture T_sand;
    //public Texture T_grass;
    [SerializeField]
    public List<Tile> tileList;
    [SerializeField]
    public TileType BaseTileType = TileType.TILE_SAND;
    //[SerializeField]
    //Map currentMap;
    //public Map[] maps;

    [SerializeField]
    public float tileSize;

    [SerializeField]
    int TileStackSize = 0;
    ////Tile Prefabs
    //[SerializeField]
    //private Transform SandTile;
    //[SerializeField]
    //private Transform GrassTile;
    //[SerializeField]
    //private Transform WaterTile;
    //[SerializeField]
    //private Transform ClaffTile;
    //[SerializeField]
    //private Transform LavaTile;
    




    [SerializeField]
    public Coord mapSize;
    //Transform[,] tileMap;
    
        

    //// Holder(for오브젝트 정리)
    //public Transform mapHolder;
    //public Transform sandTileHolder;
    //public Transform grassTileHolder;
    //public Transform waterTileHolder;
    //public Transform claffTileHolder;
    //public Transform lavaTileHolder;

   public Dictionary<string, GameObject> DicTilePrefab;
   public Dictionary<string, GameObject> DicTileHolder;

    //public void Awake()
    //{
    //    if (DicTilePrefab != null)
    //        DicTilePrefab.Clear();
    //    if (DicTileHolder != null)
    //        DicTileHolder.Clear();
    //    if (tileList != null)
    //        tileList.Clear();

    //    //리스트 딕셔너리 할당
    //    DicTilePrefab = new Dictionary<string, GameObject>();
    //    DicTileHolder = new Dictionary<string, GameObject>();
    //    tileList = new List<Tile>();
    //    //TilePrefab Load
    //    for (int i = 0; i < (int)TileType.TILE_MAX; i++)
    //    {
    //        TileType e_Tiletype = (TileType)(i);
    //        string str_Tiletype = e_Tiletype.ToString();
    //        GameObject go = Resources.Load("Tiles/" + str_Tiletype) as GameObject;

    //        go.name = str_Tiletype;

    //        DicTilePrefab.Add(str_Tiletype, go);


    //    }






    //    GeneratorMap();
    //}




    public void DicLoad()
    {
        if (DicTilePrefab == null)
        {
            //DicTilePrefab.Clear();

            DicTilePrefab = new Dictionary<string, GameObject>();


            //TilePrefab Load
            for (int i = 0; i < (int)TileType.TILE_MAX; i++)
            {
                TileType e_Tiletype = (TileType)(i);
                string str_Tiletype = e_Tiletype.ToString();
                GameObject go = Resources.Load("Tiles/" + str_Tiletype) as GameObject;

                go.name = str_Tiletype;

                DicTilePrefab.Add(str_Tiletype, go);


            }

        }
        else
        {
            DicTilePrefab.Clear();



            //TilePrefab Load
            for (int i = 0; i < (int)TileType.TILE_MAX; i++)
            {
                TileType e_Tiletype = (TileType)(i);
                string str_Tiletype = e_Tiletype.ToString();
                GameObject go = Resources.Load("Tiles/" + str_Tiletype) as GameObject;

                go.name = str_Tiletype;

                DicTilePrefab.Add(str_Tiletype, go);

            }








        }
    }
    public GameObject GetTilePrefab(TileType tiletype)
    {
        if(DicTilePrefab == null)
        {
            DicLoad();
        }
        GameObject go = DicTilePrefab[tiletype.ToString()];
        return go;
        
    }
        



    public void GeneratorMap()
    {
    
        if(DicTilePrefab != null)
        DicTilePrefab.Clear();
        if(DicTileHolder != null)
        DicTileHolder.Clear();
        if(tileList != null)
        tileList.Clear();

        //리스트 딕셔너리 할당
        if(DicTilePrefab == null)
        DicTilePrefab = new Dictionary<string, GameObject>();
        if(DicTileHolder == null)
        DicTileHolder = new Dictionary<string, GameObject>();
        if(tileList == null)
        tileList = new List<Tile>();
       
        
        
        
        
        //TilePrefab Load
    for(int i = 0; i < (int)TileType.TILE_MAX; i++)
    {
            TileType e_Tiletype = (TileType)(i);
            string str_Tiletype  = e_Tiletype.ToString();
            GameObject go = Resources.Load("Tiles/" + str_Tiletype) as GameObject;
            
            go.name = str_Tiletype;

            DicTilePrefab.Add(str_Tiletype, go);
            
            
    }
        ////타일들 담을 상위 오브젝트 생성
        //string holderName = "Generated Map";
        //if (transform.Find(holderName))
        //{
        //    DestroyImmediate(transform.Find(holderName).gameObject);
        //}
        //mapHolder = new GameObject(holderName).transform;
        //mapHolder.parent = this.transform;
        //mapHolder.localPosition = Vector3.zero;


        //Holder 생성

        if (transform.Find("TileHolder"))
        {
            DestroyImmediate(transform.Find("TileHolder").gameObject);
        }
        GameObject tileHolder = new GameObject("TileHolder");
        tileHolder.transform.parent = this.transform;
        tileHolder.transform.localPosition = Vector3.zero;
        tileHolder.transform.localScale = Vector3.one;
        //for (int i = 0; i < (int)TileType.TILE_MAX; i++)
        //{
        //    TileType e_Tiletype = (TileType)(i);
        //    string str_Tiletype = e_Tiletype.ToString();

        //    string tileholderName = str_Tiletype + " Holder";

        //    if(  transform.Find (tileholderName) )
        //    {
        //        DestroyImmediate(transform.Find(tileholderName).gameObject);
        //    }
        //    GameObject go = new GameObject(tileholderName);

        //    go.transform.parent = this.transform;
        //    go.transform.localPosition = Vector3.zero;
        //    go.transform.localScale = Vector3.one;

        //    DicTileHolder.Add(str_Tiletype, go);
        //}




        //타일 생성
        for (int x = 0; x < mapSize.x; x++) 
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                //BaseTile 생성
                Vector3 tilePosition = CoordToPosition(x, y);

                
                GameObject TilePrefab = DicTilePrefab[BaseTileType.ToString()];
                Transform Trans_newTile = Instantiate(TilePrefab.transform, Vector3.zero, Quaternion.identity) as Transform;
                Tile newTile = Trans_newTile.GetComponent<Tile>(); // 생성된 타일의 타일 컴포넌트 가지고옴
                tileList.Add(newTile);
                newTile.TileInit(tilePosition, this, x, y, BaseTileType); // 타일 데이터 입력
                newTile.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
                newTile.transform.parent = tileHolder.transform;
                if(TileStackSize > 0)
                {
                    newTile.StackTile(TileStackSize,this);

                }



            }

        }
    }

    public void tilelistReset()
    {
        tileList.Clear();

        Tile[] tile = GetComponentsInChildren<Tile>();

        for(int i = 0; i < tile.Length; i++)
        {
            tileList.Add(tile[i]);
        }
    }

    public void reloadTile()
    {

        List<Tile> ParentTile = new List<Tile>();

        if (tileList == null || tileList.Count == 0)
        {
            Debug.LogError("타일리스트가 없거나 비어있음");
        }
        
        for (int i = 0; i < tileList.Count; i++)
        {
            if (tileList[i] == null)
            {
                Debug.Log("타일리스트에 null인 타일이 있습니다.");
            }

            if (tileList[i].list_MyStack.Count != 0)
            {

                //Recreate(tileList[i].tileType, tileList[i]);
            }
            else
            {
                ParentTile.Add(tileList[i]);
            }
            
        }
        for(int i = 0; i < ParentTile.Count; i++)
        {
            Recreate(ParentTile[i].tileType, ParentTile[i]);
        }

        for(int i = 0; i < tileList.Count; i++)
        {
            Recreate(tileList[i].tileType, tileList[i]);
        }

    }

    public void TileSizeChange()
    {
        Tile[] tile = transform.GetComponentsInChildren<Tile>();

        for(int i = 0; i < tile.Length; i++)
        {
            tile[i].ChangeScale(tileSize);
        }
    }
    //public void Update()
    //{
    //    for (int i = 0; i < tileList.Count; i++)
    //    {
    //        tileList[i].tileUpdate();
    //    }
    //}

        //타일 삭제
    public void DestroyTile(Tile target)
    {
        tileList.Remove(target);
        DestroyImmediate(target.gameObject);
    }


    //원하는 타입의 타일로 교체
    public void Recreate(TileType wantedType, Tile target)
    {
        if (DicTilePrefab == null)
        {
            DicLoad();
        }

        //타일타입 NONE이면 삭제만 진행
        if (wantedType == TileType.TILE_NONE)
        {
            //리스트에서 기존 타일 제거
            int tileIndex = tileList.IndexOf(target);   //리스트안에서 현재타일의 인덱스
            tileList.RemoveAt(tileIndex);
            DestroyTile(target);
            
        }

        if (wantedType != TileType.TILE_NONE)
        {
            //리스트에서 기존 타일 제거
            int tileIndex = tileList.IndexOf(target);   //리스트안에서 현재타일의 인덱스
            tileList.RemoveAt(tileIndex);

            //타일 인덱스 저장
            int _x = target.x;
            int _y = target.y;
            int stackIndex = target.stackIndex;

            // 기존 위치 저장
            Vector3 createPos = target.transform.position;
            Vector3 orgScale = target.transform.localScale;
            Transform targetParent = target.transform.parent;
            
            if(target.transform.childCount > 0)
            {

                int childcount = target.transform.childCount;
                Transform child = null;

                if (target.transform.childCount == 1)
                {
                    child = target.transform.GetChild(0);
                    child.parent = targetParent;
                }
                else
                {
                    for (int i = childcount - 1; i >=  0; i--)
                    {
                        if(target.transform.GetChild(i) != null)
                        {
                         child = target.transform.GetChild(i);

                        }
                        child.parent = targetParent;
                    }

                }

            }
            
            // 게임오브젝트삭제
            DestroyImmediate(target.gameObject);


            GameObject prefab = DicTilePrefab[wantedType.ToString()];
            Transform recreateTile = Instantiate(prefab.transform, createPos, Quaternion.identity) as Transform;
            
            Tile createTile = recreateTile.GetComponent<Tile>();
            createTile.transform.parent = targetParent;
            createTile.transform.localScale = orgScale;
            createTile.TileInit(createPos, this, _x, _y, wantedType, true);
            //스택인덱스값 넘겨주기
            createTile.stackIndex = stackIndex;

            // 지우고 새로 생성한 블록 리스트에 원래 인덱스 자리에 넣음
            tileList.Insert(tileIndex, createTile);

        }
    }





    Vector3 CoordToPosition(int x, int y) //2차원좌표 -> 3차원
        {
        return new Vector3(
            -mapSize.x / 2f + x, 0, -mapSize.y / 2f + y) * tileSize;
        }
}


// [System.Serializable]
//public class Map
// {
//     public Coord mapSize;
//     public List<Tile> tileList; //생성된 타일 저장하는 리스트
//     public Coord mapCenter
//     {
//         get
//         {
//             return new Coord(mapSize.x / 2, mapSize.y / 2);
//         }
//     }
// }




[System.Serializable]
    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y) //생성자
        {
            x = _x;
            y = _y;
        }
        

    }










