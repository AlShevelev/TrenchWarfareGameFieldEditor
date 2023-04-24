using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NationsState : MonoBehaviour {
   private readonly Dictionary<int, NationState> nations = new Dictionary<int, NationState>();

    private int nextId = 0;

    public const int MaxNations = 8;

    public bool CanAdd {
        get { return nations.Count < MaxNations; }
    }

    public int AddNation() {
        if(nations.Count >= MaxNations) {
            return -1;
        }

        var state = new NationState();
        var id = nextId++;

        nations.Add(id, state);

        return id;
    }

    public void DeleteNation(int id) {
        nations.Remove(id);
    }

    public void UpdateCode(int id, int code) {
        if(code == 0) {
            return;
        }

        nations[id].code = (Nation)code;
    }
}
