using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int col;

    private int[,] initMap = {
                              {2, 0, 0, 0}, 
                              {3, 3, 0, 3}, 
                              {3, 0, 0, 3}, 
                              {3, 0, 0, 1}
                                          };
    private int[,] map;
    private int[] initPlayerPos = {0, 0};
    private int[] playerPos; 
    private GameObject[,] mapObjects = new GameObject[4, 4];
    private SpriteRenderer[] mapRenderers = new SpriteRenderer[4];

    [SerializeField] private GameObject caodi; // 0
    [SerializeField] private GameObject goal; // 1
    [SerializeField] private GameObject player; // 2
    [SerializeField] private GameObject xianjin; // 3

    private void Awake() {
        map = initMap;
        playerPos = initPlayerPos;
        
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

    private void showMap() {
        string s = "";
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < col; j++) {
                s += map[i, j] + " ";
            }
            s += "\n";
        }
        Debug.Log(s);
    }

    public void SetMap(int i, int j, int value) {
        map[i, j] = value;
    }

    public int[,] GetMap() {
        return map;
    }

    private void ResetMap() {
        map = initMap;
        playerPos = initPlayerPos;
    }

    public int[] GetPlayerPos() {
        return playerPos;
    }

    public void SetPlayerPos(int[] playerNewPos) {
        SetMap(playerPos[0], playerPos[1], 0);
        SetMap(playerNewPos[0], playerNewPos[1], 2);
        playerPos = playerNewPos;
    }

    public void SetPlayerPos(int action) {
        int[] playerNewPos = new int[2];
        for (int i = 0; i < 2; i++) {
            playerNewPos[i] = playerPos[i];
        }

        if (action == 0) {
            if (playerNewPos[1] + 1 >= col) {
                return;
            }
            playerNewPos[1]++;
        } else if (action == 1) {
            if (playerNewPos[0] + 1 >= row) {
                return;
            }
            playerNewPos[0]++;            
        } else if (action == 2) {
            if (playerNewPos[1] - 1 < 0) {
                return;
            }
            playerNewPos[1]--;
        } else if (action == 3) {
            if (playerNewPos[0] - 1 < 0) {
                return;
            }
            playerNewPos[0]--;
        }

        SetMap(playerPos[0], playerPos[1], 0);
        SetMap(playerNewPos[0], playerNewPos[1], 2);
        
        playerPos = playerNewPos;
    }
}
