using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommonMonster : CommonEntity
{
    #region 字段

    protected float hp = 10;
    protected float speed = 1;
    protected int money = 10;
    protected Vector3 currentLocation;
    protected Point nextPoint;
    protected List<Point> path;
    protected int state = MResources.MonsterStateForward;

    #endregion

    #region Unity回调函数

    // Use this for initialization
    void Start()
    {
        path = new List<Point>();
        for (var x = 1; x <= 9; x++)
            path.Add(new Point(x,2));
        nextPoint = path[0];
        currentLocation = this.GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.state)
        {
            case MResources.MonsterStateForward:
                Forward();
                break;
            case MResources.MonsterStatePaused:
                Paused();
                break;
            case MResources.MonsterStateDying:
                Dying();
                break;
        }
    }

    #endregion

    #region 状态机

    protected void Forward()
    {
        //FindPath(path, Point currentLocation);//更新路径

        //更新nextPoint
        var nextLocation = Point.Point2Vector3(nextPoint);
        if (Vector3.Distance(currentLocation, nextLocation) < 0.3f)
        {
            if (path[0] == null) return;
            nextPoint = path[0];
            path.RemoveAt(0);
            nextLocation = Point.Point2Vector3(nextPoint);
        }
        //前进
        currentLocation = Vector3.MoveTowards(currentLocation, nextLocation, speed * Time.deltaTime);
        this.GetComponent<Transform>().SetPositionAndRotation(currentLocation, new Quaternion(0, 0, 0, 0));
    }

    protected void Paused()
    {

    }

    protected void Dying()
    {

    }

    #endregion

    #region 辅助方法

    private void FindPath(List<Point> path, Point currentLocation)
    {

    }

    #endregion

}


