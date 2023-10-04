using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int col;

    private int[,] map = {{2, 0, 0, 0}, {0, 3, 0, 3}, {0, 0, 0, 3}, {3, 0, 0, 1}};
    private GameObject[,] mapObjects = new GameObject[4, 4];
    private SpriteRenderer[] mapRenderers = new SpriteRenderer[4];

    [SerializeField] private GameObject caodi;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject xianjin;

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
}
