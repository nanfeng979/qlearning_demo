using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int col;

    private int[,] map = {{2, 0, 0, 0}, {0, 3, 0, 3}, {0, 0, 0, 3}, {3, 0, 0, 1}};

    [SerializeField] private GameObject caodi;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject xianjin;

    private void Awake() {
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
                Instantiate(obj, new Vector3(j - col / 2, -1 * (i - row / 2), 0), Quaternion.identity, transform);
            }
        }
    }
    
    private void Start() {
        // for (int i = 0; i < row; i++) {
        //     for (int j = 0; j < col; j++) {
        //         switch (map[i, j]) {
        //             case 0:
        //                 Instantiate(caodi);
        //                 break;
        //             case 1:
        //                 Instantiate(goal);
        //                 break;
        //             case 2:
        //                 Instantiate(player);
        //                 break;
        //             case 3:
        //                 Instantiate(xianjin);
        //                 break;
        //             default:
        //                 break;
        //         }
        //     }
        // }
    }
}
