using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int col;

    private int[,] initMap = {
                              {2, 0, 0, 0}, 
                              {0, 3, 0, 3}, 
                              {0, 0, 0, 3}, 
                              {3, 0, 0, 1}
                                          };
    private int[,] map = new int[4, 4];
    private GameObject[,] mapObjects = new GameObject[4, 4];
    private SpriteRenderer[] mapRenderers = new SpriteRenderer[4];

    private int currentState;
    private int currentReward;
    private bool isTerminated;

    [SerializeField] private GameObject caodi; // 0
    [SerializeField] private GameObject goal; // 1
    [SerializeField] private GameObject player; // 2
    [SerializeField] private GameObject xianjin; // 3

    private void Awake() {
        ResetMap();
        
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
        // updateMayRender();
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

    public StepInfo ResetMap() {
        // Debug.Log("reset");

        for (int i = 0; i < row; i++) {
            for (int j = 0; j < col; j++) {
                map[i, j] = initMap[i, j];
            }
        }

        currentState = 0; // todo: 随机
        currentReward = 0;
        isTerminated = false;

        StepInfo stepInfo = new StepInfo(currentState, currentReward, isTerminated, false, null);

        return stepInfo;
    }

    public StepInfo SetPlayerPos(int action, int currentState) {
        bool yueJie = false;

        if (yueJie = isYueJie(action, currentState)) {
            // Debug.Log("yuejie");
        } else {
            currentState = MapUpdatePlayer(action, currentState);
            // Debug.Log("no yuejie");
        }
        
        StepInfo info = new StepInfo();

        info.new_state = currentState;
        info.reward = currentReward;
        info.terminated = isTerminated;

        info.truncated = false;
        info.info = null;

        return info;
    }

    private bool isYueJie(int action, int state) {
        int[] pos = StateToPos(state);
        int x = pos[0];
        int y = pos[1];

        switch (action) {
            case 0:
                if (y + 1 >= row) {
                    return true;
                }
                break;
            case 1:
                if (x + 1 >= col) {
                    return true;
                }
                break;
            case 2:
                if (y - 1 < 0) {
                    return true;
                }
                break;
            case 3:
                if (x - 1 < 0) {
                    return true;
                }
                break;
        }
        return false;
    }

    private int MapUpdatePlayer(int action, int state) {
        int[] pos = StateToPos(state);
        int x = pos[0];
        int y = pos[1];

        map[x, y] = 0;
        switch (action) {
            case 0:
                y += 1;
                break;
            case 1:
                x += 1;
                break;
            case 2:
                y -= 1;
                break;
            case 3:
                x -= 1;
                break;
        }

        UpdatePlayer(x, y);

        pos[0] = x;
        pos[1] = y;

        // Debug.Log("current pos: x: " + pos[0] + " y: " + pos[1]);
        // Debug.Log("current state: " + PosToState(pos));
        return PosToState(pos);
    }

    private int[] StateToPos(int state) {
        int x = state / col;
        int y = state % col;
        return new int[] { x, y };
    }

    private int PosToState(int[] pos) {
        return pos[0] * col + pos[1];
    }

    private void UpdatePlayer(int x, int y) {
        switch (map[x, y]) {
            case 0:
                // Debug.Log("caodi");
                currentReward = 0;
                isTerminated = false;
                break;
            case 1:
                // Debug.Log("goal");
                currentReward = 1;
                isTerminated = true;
                break;
            case 2:
                // Debug.Log("player");
                currentReward = 0;
                isTerminated = false;
                break;
            case 3:
                // Debug.Log("xianjin");
                currentReward = 0;
                isTerminated = true;
                break;
        }

        map[x, y] = 2;
    }
}
