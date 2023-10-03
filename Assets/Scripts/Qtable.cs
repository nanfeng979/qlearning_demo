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

    // state count = raw * col = 4 * 4
    private int greedy_policy(int[,] table, int state) {
        int action = table[state, 0];
        for (int i = 1; i < 4; i++) {
            if (table[state, i] > action) {
                action = table[state, i];
            }
        }
        return action;
    }

    private int epsilon_greedy_policy(int[,] table, int state, float epsilon) {
        float random_num = Random.Range(0f, 1f);
        if (random_num > epsilon) {
            int action = greedy_policy(table, state);
            return action;
        }
        else {
            int action = Random.Range(0, 4);
            return action;
        }
    }
}
