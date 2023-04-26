using System;
using System.Linq;
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

    public bool CanEdit {
        get { return nations.Count > 1 && nations.All(i => i.Value.code != null); }
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

    public bool UpdateCode(int id, Nation? code) {
        if(code == null || !nations.Any(i => i.Value.code == code)) {
            nations[id].code = code;
            return true;
        }

        return false;
    }

    public Aggressiveness? GetNationAggresiveness(int nationStateId) {
        return nations[nationStateId].aggressiveness;
    }

    public Nation GetNation(int nationStateId) {
        return nations[nationStateId].code ?? throw new ArgumentNullException();;
    }

    public List<Tuple<Nation, Diplomacy?>> GetDiplomacy(int nationStateId) {
        var result = new List<Tuple<Nation, Diplomacy?>>();

        var nationState = nations[nationStateId];

        foreach(var keyValue in nations) {
            if(keyValue.Key == nationStateId) {
                continue;
            }

            var code = keyValue.Value.code ?? throw new ArgumentNullException();

            Diplomacy? diplomacy = null;

            if(nationState.diplomacy.ContainsKey(code)) {
                diplomacy = nationState.diplomacy[code];
            }

            result.Add(new Tuple<Nation, Diplomacy?>(code, diplomacy));
        }

        return result;
    }
}
