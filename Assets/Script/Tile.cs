using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{
    //타일 인덱스
    public int x;
    public int y;

    //스택 인덱스
    public int stackIndex = 0;

    //타일타입
    public TileType tileType;
    [SerializeField]
    public TileType wantedType;

    //현재 타일을 생성한 MapGenerator
    MapGenerator this_Creator = null;

    //내 위에 추가로 쌓기위해 생성한 타일들의 보관용 리스트와 얼마만큼 쌓을지 정하는 int값
    public List<Tile> list_MyStack;
    public int wantedStackCount = 0;
   
    

    private void Awake()
    {
        this_Creator = this.GetComponentInParent<MapGenerator>();
        list_MyStack = new List<Tile>();
        
    }
    public void TileInit(Vector3 tilePos, MapGenerator map, int _x , int _y,TileType eTileType = TileType.TILE_NONE, bool reCreate = false)
    {
        
        if(this_Creator == null)
        this_Creator = this.GetComponentInParent<MapGenerator>();

        
        
        if(list_MyStack == null)
        list_MyStack = new List<Tile>();

       
        //타일 인덱스
        x = _x;
        y = _y;

        if (map == null)
            map = this_Creator;

        //해당 타일의 타입
        if(eTileType != TileType.TILE_NONE)
        tileType = eTileType;

        
        this.gameObject.name = tileType.ToString() + "(" + x + "," + y + ")";


          if (tileType == TileType.TILE_NONE)
           Debug.Log("타일타입이 지정되지않았습니다");





        // if(!reCreate)
        //{
        //    if(map.DicTileHolder == null)
        //    {

        //    }
        //transform.parent = map.DicTileHolder[tileType.ToString()].transform;
        //}



        if (!reCreate)
        {
            transform.localPosition = tilePos;

            if (transform.childCount != 0)
                transform.localScale = Vector3.one * map.tileSize;
            else
                transform.localScale = Vector3.one;
        }

        if(map == null)
        {
            Debug.Log("MapGenerator가 null입니다.");
        }

        
        wantedType = tileType;


        ////material
        //Material tempMaterial = new Material(tileMeshRenderer.sharedMaterial);
        ////Material tempMaterial = Material(tileMeshRenderer.material);
        //float colorPercent = y / map.mapSize.y;
        //if(y /2 == 0)
        //tempMaterial.color = Color.Lerp(Color.white, Color.black, colorPercent);
        //tileMeshRenderer.sharedMaterial = tempMaterial;
        ////tileMeshRenderer.sharedMaterial.color = Color.Lerp(Color.white, Color.black, colorPercent);

    }


   
    public void tileUpdate()
    {
        this_Creator = this.GetComponentInParent<MapGenerator>();
        if (this_Creator == null)
        {
            Debug.Log("타일생성자가 없습니다");
        }


        //타일 재생성
        if(wantedType != tileType)
        {
            this_Creator.Recreate(wantedType, this);
            tileType = wantedType;
            
        }

        //스택
        if (wantedStackCount != 0)
            StackTile();


        //else
        //{
        //    TileInit(transform.localPosition, this_Creator, x, y);
        //}


    }

    public void DeleteStackParent()
    {
        Tile[] child;
        child = transform.GetComponentsInChildren<Tile>();
        foreach(Tile tile in child)
        {
            tile.transform.SetParent(this.transform.parent);
        }
        if (list_MyStack != null)
            list_MyStack.Clear();

        if(this_Creator == null)
        {
            
            this_Creator = GetComponentInParent<MapGenerator>();
            this_Creator.DestroyTile(this);
        }
    }


    public void ChangeScale(float size)
    {
        transform.localScale = Vector3.one * size;
        
        
        
        transform.position = new Vector3(x * size, stackIndex * size , y * size);

        

        

        

    }
    public void StackTile(int stackcount = 0, MapGenerator creator = null)
    {
        if (this_Creator == null)
            this_Creator = transform.GetComponentInParent<MapGenerator>();
        if(list_MyStack != null)
        {
            list_MyStack.Clear();
        }



        if (stackcount > 0)
            wantedStackCount = stackcount;
        if (creator != null)
            this_Creator = creator;
        //리스트 안에들은 타일들 삭제
        for(int i = 0; i < list_MyStack.Count; i++)
        {
            DestroyImmediate(list_MyStack[i].gameObject);
        }
        // 리스트도 초기화
        list_MyStack.Clear();

        if (this_Creator == null)
            Debug.Log("해당타일의 generator가 null입니다");
        Transform prefab = this_Creator.GetTilePrefab(tileType).transform;


        while (list_MyStack.Count != wantedStackCount)
        {
            if (list_MyStack.Count == 0)    //최초 스택생성
            {
                //스택생성
                Transform stackTile_Trans = Instantiate(prefab, this.transform) as Transform;

                //생성한 스택타일 정보입력
                stackTile_Trans.localScale = Vector3.one;
                stackTile_Trans.localPosition = new Vector3(0,list_MyStack.Count + 1, 0);
                stackTile_Trans.name = this.name +"STACK" + list_MyStack.Count.ToString();
                Tile stackTile = stackTile_Trans.GetComponent<Tile>();
                stackTile.tileType = this.tileType;
                stackTile.wantedType = stackTile.tileType;
                stackTile.x = x;
                stackTile.y = y;

                // 스택을 담아놓는 리스트에 스택추가
                list_MyStack.Add(stackTile);

                //Map Generator의 리스트에도 추가해줌
                this_Creator.tileList.Add(stackTile);

                // 스택 인덱스지정
                stackTile.stackIndex = list_MyStack.Count + stackIndex;
            }
            else
            {
                Tile lastTileInList = list_MyStack[list_MyStack.Count - 1];
                Vector3 lastTilepos = lastTileInList.transform.localPosition;

                //생성
                Transform stackTile_Trans = Instantiate(prefab, this.transform) as Transform;

                //정보입력
                stackTile_Trans.localScale = Vector3.one;
                stackTile_Trans.localPosition = new Vector3(0, list_MyStack.Count + 1 , 0);
                stackTile_Trans.name = this.name + "STACK" + list_MyStack.Count.ToString();
                Tile stackTile = stackTile_Trans.GetComponent<Tile>();
                stackTile.tileType = this.tileType;
                stackTile.wantedType = stackTile.tileType;
                stackTile.x = x;
                stackTile.y = y;

                
                //리스트에 추가
                list_MyStack.Add(stackTile);


                this_Creator.tileList.Add(stackTile);

                // 스택 인덱스지정
                stackTile.stackIndex = list_MyStack.Count + stackIndex;
            }
                




        }
        wantedStackCount = 0;
    }




}