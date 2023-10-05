using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StepInfo {
    public int new_state;
    public float reward;
    public bool terminated;
    public bool truncated;
    public string info;
}

public class Qtable : MonoBehaviour
{
    [SerializeField] private MapRender map;

    public float[,] qtable = new float[16, 4];

    private float[,] IEQtable = new float[16, 4];

    // params
    // private int n_training_episodes = 10000;
    private int n_training_episodes = 1000;
    private float learning_rate = 0.7f;
    private int n_eval_episodes = 100;
    private string env_id = "FrozenLake-v1";
    private int max_steps = 99;
    private float gamma = 0.95f;
    private int[] eval_seed = {};
    private float max_epsilon = 1.0f;
    private float min_epsilon = 0.05f;
    private float decay_rate = 0.0005f;

    private int currentState = 0;

    private void Awake() {
        initTable(qtable);
    }

    // Start is called before the first frame update
    void Start()
    {
        // showTable(qtable);
        // qtable = train(n_training_episodes, min_epsilon, max_epsilon, decay_rate, "env", max_steps, qtable);

        // StartCoroutine(trainWithIE(n_training_episodes, min_epsilon, max_epsilon, decay_rate, env_id, max_steps, IEQtable));
        
    }

    // Update is called once per frame
    void Update()
    {
        test();
    }

    IEnumerator trainWithIE (int n_training_episodes, float min_epsilon, float max_epsilon, float decay_rate, string env, int max_steps, float[,] qtable) {
        for (int episode = 0; episode < n_training_episodes; episode++) {
            var epsilon = min_epsilon + (max_epsilon - min_epsilon) * Mathf.Exp(-decay_rate * episode);
            int state = map.ResetMap(); // todo: 随机位置。0表示在左上角

            for (int step = 0; step < max_steps; step++) {
                int action = epsilon_greedy_policy(qtable, state, epsilon);

                StepInfo stepInfo = Step(action, state);

                // if (n_training_episodes < 9000) {
                //     yield return null;
                // } else {
                //     yield return new WaitForSeconds(0.2f);
                // }
                currentState = state;

                yield return new WaitForSeconds(2f);
                // yield return null;

                var new_state = stepInfo.new_state; //
                var reward = stepInfo.reward; //
                var terminated = stepInfo.terminated; //


                float [] qtable_state = new float[4];
                for (int action_index = 0; action_index < 4; action_index++) {
                    qtable_state[action_index] = qtable[state, action_index];
                }

                // QtableStateMaxAction(qtable_state) //
                qtable[state, action] = qtable[state, action] + learning_rate * (reward + gamma * QtableStateMaxAction(qtable_state) - qtable[state, action]);

                if (terminated) {
                    break;
                }

                state = new_state;
                currentState = state;
                // Debug.Log(state);
            }
            
            // showTable(IEQtable);
        }

        IEQtable = qtable;
        Debug.Log("训练完成");
        showTable(IEQtable);
    }

    private void initTable(float[,] table) {
        for (int i = 0; i < 16; i++) {
            for (int j = 0; j < 4; j++) {
                table[i, j] = 0;
            }
        }
    }

    private void showTable(float[,] table) {
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
    // action = right, down, left, up = 0, 1, 2, 3
    private int greedy_policy(float[,] table, int state) {
        int action = 0;
        float max = table[state, action];
        for (int action_index = 1; action_index < 4; action_index++) {
            if (table[state, action_index] > max) {
                max = table[state, action_index];
                action = action_index;
            }
        }
        return action;
    }

    private int epsilon_greedy_policy(float[,] table, int state, float epsilon) {
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

    private float[,] train (int n_training_episodes, float min_epsilon, float max_epsilon, float decay_rate, string env, int max_steps, float[,] qtable) {
        for (int episode = 0; episode < n_training_episodes; episode++) {
            var epsilon = min_epsilon + (max_epsilon - min_epsilon) * Mathf.Exp(-decay_rate * episode);
            int state = 0; // todo: 随机位置。0表示在左上角
            bool terminated = false;

            for (int step = 0; step < max_steps; step++) {
                int action = epsilon_greedy_policy(qtable, state, epsilon);

                StepInfo stepInfo = Step(action, state);
                var new_state = stepInfo.new_state;
                var reward = stepInfo.reward;
                terminated = stepInfo.terminated;

                float [] qtable_state = new float[4];
                for (int action_index = 0; action_index < 4; action_index++) {
                    qtable_state[action_index] = qtable[state, action_index];
                }

                qtable[state, action] = qtable[state, action] + learning_rate * (reward + gamma * QtableStateMaxAction(qtable_state) - qtable[state, action]);

                if (terminated) {
                    break;
                }

                state = new_state;
            }
            map.ResetMap();
        }

        return qtable;
    }

    private float QtableStateMaxAction(float[] qtable_state) {
        float max = qtable_state[0];
        for (int action_index = 1; action_index < 4; action_index++) {
            if (qtable_state[action_index] > max) {
                max = qtable_state[action_index];
            }
        }
        return max;
    }

    private StepInfo Step(int action, int currentState) {
        StepInfo info = MovePlayer(action, currentState);

        return info;
    }

    private StepInfo MovePlayer(int action, int currentState) {
        return map.SetPlayerPos(action, currentState);
    }

    private void test() {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            currentState = MovePlayer(0, currentState).new_state;
            Debug.Log("new state: " + currentState);
        } else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            currentState = MovePlayer(1, currentState).new_state;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            currentState = MovePlayer(2, currentState).new_state;
        } else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            currentState = MovePlayer(3, currentState).new_state;
        }

        // Debug.Log("currentState: " + currentState);
    }
}
