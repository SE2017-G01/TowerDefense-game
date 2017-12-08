using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : Singleton<Game> {
    public float MapWidth;//地图宽
    public float MapHeight;//地图高
    public float TileWidth;//格子宽
    public float TileHeight;//格子高
    public Point StartPoint;
    public Point EndPoint;


    private void Init()
    {
        
    }

	void Start () {
	    //确保Game对象一直存在
	    DontDestroyOnLoad(this.gameObject);
	    SceneManager.LoadScene("1.Main");
    }
}
