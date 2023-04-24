using System.Collections;
using System.Collections.Generic;

public class NationState {
    private  Nation? _code = null;
    public Nation? code {
        get { return _code; }
        set { _code = value; }
    }


    private Aggressiveness? _aggressiveness = null;
    public Aggressiveness? aggressiveness {
        get { return _aggressiveness; }
        set { _aggressiveness = value; }
    }

    private readonly Dictionary<Nation, Diplomacy> _diplomacy = new Dictionary<Nation, Diplomacy>();
    public Dictionary<Nation, Diplomacy> diplomacy {
        get { return _diplomacy; }
    }
}
