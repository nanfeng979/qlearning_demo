using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int col;

    private int[,] initMap = {{2, 0, 0, 0}, {0, 3, 0, 3}, {0, 0, 0, 3}, {3, 0, 0, 1}};
    private int[,] map = {{2, 0, 0, 0}, {0, 3, 0, 3}, {0, 0, 0, 3}, {3, 0, 0, 1}};
    private int[] initPlayerPos = {0, 0};
    private int[] playerPos = {0, 0}; 
    private GameObject[,] mapObjects = new GameObject[4, 4];
    private SpriteRenderer[] mapRenderers = new SpriteRenderer[4];

    [SerializeField] private GameObject caodi; // 0
    [SerializeField] private GameObject goal; // 1
    [SerializeField] private GameObject player; // 2
    [SerializeField] private GameObject xianjin; // 3

    private void Awake() {
        mapRenderers[0] = caodi.GetComponent<SpriteRenderer>();
        mapRenderers[1] = goal.GetComponent<SpriteRenderer>();
        mapRenderers[2] = player.GetComponent<SpriteRenderer>();
        mapRenderers[3] = xianjin.GetComponent<SpriteRenderer>();

        for (int i = 0; i < row; i++) {
            for (int j = 0; j < col; j++) {
                GameObject obj = null;
                switch (map[i, j]) {
                    case 0:
                        obj = caodi;
                        break;
                    case 1:
                        obj = goal;
                        break;
                    case 2:
                        obj = player;
                        break;
                    case 3:
                        obj = xianjin;
                        break;
                    default:
                        break;
                }
                mapObjects[i, j] = Instantiate(obj, new Vector3(j - col / 2, -1 * (i - row / 2), 0), Quaternion.identity, transform);
            }
        }
    }

    private void Update() {
        updateMayRender();
    }

    private void updateMayRender() {
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < col; j++) {
                SpriteRenderer sprite = mapRenderers[map[i, j]];
                mapObjects[i, j].GetComponent<SpriteRenderer>().sprite = sprite.sprite;
            }
        }
    }

    public void SetMap(int i, int j, int value) {
        map[i, j] = value;
        playerPos = initPlayerPos;
    }

    public int[,] GetMap() {
        return map;
    }

    private void ResetMap() {
        map = initMap;
    }

    public int[] GetPlayerPos() {
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < col; j++) {
                if (map[i, j] == 2) {
                    return new int[] { i, j };
                }
            }
        }
        return playerPos;
    }

    public void SetPlayerPos(int[] newPos) {
        int[] playerOldPos = playerPos;
        playerPos = newPos;
        map[playerPos[0], playerPos[1]] = 2;
        map[playerOldPos[0], playerOldPos[1]] = 0;
    }
}
