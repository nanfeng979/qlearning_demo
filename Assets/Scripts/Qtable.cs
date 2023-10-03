using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qtable : MonoBehaviour
{
    public int[,] qtable = new int[16, 4];

    private void Awake() {
        initTable(qtable);
    }

    // Start is called before the first frame update
    void Start()
    {
        showTable(qtable);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initTable(int[,] table) {
        for (int i = 0; i < 16; i++) {
            for (int j = 0; j < 4; j++) {
                table[i, j] = 0;
            }
        }
    }

    private void showTable(int[,] table) {
        string s = "";
        for (int i = 0; i < 16; i++) {
            for (int j = 0; j < 4; j++) {
                s += table[i, j] + " ";
            }
            s += "\n";
        }
        Debug.Log(s);
    }
}
