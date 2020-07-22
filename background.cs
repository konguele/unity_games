using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
public class Background : MonoBehaviour
{
    public GameObject Player;
    public GameObject backContainerGO;
    public GameObject tunnelContainerGO;
    public float distance;
    public GameObject [ ] backContainerArray;
    public GameObject [ ] tunnelContainerArray;
    int backCounter = 0;
    int tunnelCounter = 0;
    int selectBack;
    int selectTunnel;
    public GameObject previousBack;
    public GameObject newBack;
    public float backSize;
    public Vector3 limitScreen;
    public bool outScreen;

    public MotorCarreteras motorCarreteras;

    public enum BackgroundType { None, Background, Tunnel }
    [Serializable]
    public struct BackgroundInfo
    {
        public BackgroundType backgroundType;
        public float distance;
    }

    private BackgroundType currentBackgroundType = BackgroundType.None;

    public List<BackgroundInfo> backgroundInfo = new List<BackgroundInfo>
    {
        new BackgroundInfo{ backgroundType = BackgroundType.None, distance = 0 },
        new BackgroundInfo{ backgroundType = BackgroundType.Tunnel, distance = 10 },
        new BackgroundInfo{ backgroundType = BackgroundType.Background, distance = 20 }
    };


    // Start is called before the first frame update
    void Start ( )
    {
        backContainerArray = GameObject.FindGameObjectsWithTag ( "Background" );
        tunnelContainerArray = GameObject.FindGameObjectsWithTag ( "Tunnel" );
        for ( var i = 0; i < backContainerArray.Length; i++ )
        {
            var item = backContainerArray [ i ].gameObject;
            item.transform.parent = backContainerGO.transform;
            item.name = "BackOFF_" + i;
            item.SetActive ( false );
        }
        for ( var i = 0; i < tunnelContainerArray.Length; i++ )
        {
            var item = tunnelContainerArray [ i ].gameObject;
            item.transform.parent = tunnelContainerGO.transform;
            item.name = "TunnelOFF_" + i;
            item.SetActive ( false );
        }
    }


    void CreateBackground ( BackgroundType backgroundType )
    {
        if ( previousBack != null ) Destroy ( previousBack );
        previousBack = newBack;
        switch ( backgroundType )
        {
            case BackgroundType.Background:
                newBack = Instantiate ( backContainerArray [ UnityEngine.Random.Range ( 0, backContainerArray.Length ) ] );
                break;
            case BackgroundType.Tunnel:
                newBack = Instantiate ( tunnelContainerArray [ UnityEngine.Random.Range ( 0, tunnelContainerArray.Length ) ] );
                break;
            default:
                Debug.Log ( "Incorrect BackgroundType." );
                return;
        }

        currentBackgroundType = backgroundType;
        newBack.name = $"Background{++backCounter}";
        newBack.transform.parent = gameObject.transform;

        for ( int i = 0; i < previousBack.transform.childCount; i++ )
            if ( previousBack.transform.GetChild ( i ).gameObject.GetComponent<Pieza> ( ) != null )
                backSize += previousBack.transform.GetChild ( i ).gameObject.GetComponent<SpriteRenderer> ( ).bounds.size.x;

        newBack.transform.position = new Vector3 ( previousBack.transform.position.x + backSize, previousBack.transform.position.y, 0 );
        newBack.SetActive ( true );
    }


    // Update is called once per frame
    void Update ( )
    {
        distance += Time.deltaTime * motorCarreteras.velocidad;
        Debug.Log ( distance );

        if ( distance > backgroundInfo [ backCounter ].distance && currentBackgroundType != backgroundInfo [ backCounter ].backgroundType )
            CreateBackground ( backgroundInfo [ ++backCounter ].backgroundType );
    }
}
