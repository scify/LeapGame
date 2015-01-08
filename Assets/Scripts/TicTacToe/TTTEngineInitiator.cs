﻿using UnityEngine;
using System.Collections.Generic;

public class TTTEngineInitiator : MonoBehaviour {

    public float offset_x;
    public float offset_y;

	void Start () {

        List<Actor> actors = new List<Actor>();
        actors.Add(new TTTActor("cursor", "Prefabs/TTT/Cursor", new Vector3(0, 0, 0), false, (WorldObject wo, GameEngine engine) => {
            if (wo is TTTStaticObject && engine is TTTGameEngine) {
                if ((wo as TTTStaticObject).prefab.Contains("TTT/O")) {
                    string audioClip = "Sounds/O";
                    //TODO: replace with call to auEngine
                    (engine as TTTGameEngine).state.environment.Add(new TTTSoundObject("Prefabs/TTT/AudioSource", audioClip, wo.position));
                } else if ((wo as TTTStaticObject).prefab.Contains("TTT/X")) {
                    string audioClip = "Sounds/X";
                    //TODO: replace with call to auEngine
                    (engine as TTTGameEngine).state.environment.Add(new TTTSoundObject("Prefabs/TTT/AudioSource", audioClip, wo.position));
                }
            }
        }));

        List<WorldObject> environment = new List<WorldObject>();
        environment.Add(new TTTStaticObject("Prefabs/TTT/Camera_Default", new Vector3(0, 10, 0), false));
        environment.Add(new TTTStaticObject("Prefabs/TTT/Light_Default", new Vector3(0, 10, 0), false));
        environment.Add(new TTTStaticObject("Prefabs/TTT/Line_Horizontal", new Vector3(0, 0, 2), false));
        environment.Add(new TTTStaticObject("Prefabs/TTT/Line_Horizontal", new Vector3(0, 0, -2), false));
        environment.Add(new TTTStaticObject("Prefabs/TTT/Line_Vertical", new Vector3(3.2f, 0, 0), false));
        environment.Add(new TTTStaticObject("Prefabs/TTT/Line_Vertical", new Vector3(-3.2f, 0, 0), false));

        List<Player> players = new List<Player>();
        players.Add(new Player("player0", "Nick"));
        players.Add(new TTTAIPlayer("player1", "AI"));

        TTTStateRenderer renderer = new TTTStateRenderer();
        AudioEngine auEngine = new AudioEngine();
        //TODO: initialize audio engine with default params?

        TTTRuleset rules = new TTTRuleset();
        rules.Add(new TTTRule("ALL", (TTTGameState state, GameEvent eve, TTTGameEngine engine) => {
            return !eve.initiator.StartsWith("player") || eve.initiator.Equals("player" + state.curPlayer);
        }));

        rules.Add(new TTTRule("action", (TTTGameState state, GameEvent eve, TTTGameEngine engine) => {
            if (state.timestamp >= 10 && eve.payload.Equals("enter")) {
                (state.result as TTTGameResult).status = TTTGameResult.GameStatus.Over;
                return false;
            }
            return true;
        }));

        rules.Add(new TTTRule("action", (TTTGameState state, GameEvent eve, TTTGameEngine engine) => {
            if (eve.payload.Equals("select")) {
                foreach (Actor actor in state.actors) {
                    foreach (WorldObject wo in state.environment) {
                        if (wo.position == actor.position) {
                            actor.interact(wo, engine);
                        }
                    }
                }
            }
            return true;
        }));

        rules.Add(new TTTRule("action", (TTTGameState state, GameEvent eve, TTTGameEngine engine) => {
            if (eve.payload.Equals("enter")) {
                foreach (Actor actor in state.actors) {
                    bool overlap = false;
                    foreach (WorldObject wo in state.environment) {
                        if (wo.position == actor.position && !(wo is TTTSoundObject)) {
                            overlap = true;
                        }
                    }
                    string audioClip;
                    if (overlap) {
                        audioClip = "Sounds/Error";
                        //TODO: replace with call to auEngine
                        engine.state.environment.Add(new TTTSoundObject("Prefabs/TTT/AudioSource", audioClip, actor.position));
                    } else {
                        int x = (int)(actor.position.x / offset_x + 1);
                        int y = (int)(actor.position.z / offset_y + 1);
                        state.board[x, y] = state.curPlayer;
                        string symbol = state.curPlayer == 0 ? "X" : "O";
                        engine.state.environment.Add(new TTTStaticObject("Prefabs/TTT/" + symbol, actor.position, false));
                        audioClip = "Sounds/" + symbol;
                        //TODO: replace with call to auEngine
                        engine.state.environment.Add(new TTTSoundObject("Prefabs/TTT/AudioSource",  audioClip, actor.position));
                        state.curPlayer = engine.players.Count - state.curPlayer - 1;
                        state.timestamp++;
                    }
                }
            }
            return true;
        }));

        rules.Add(new TTTRule("move", (TTTGameState state, GameEvent eve, TTTGameEngine engine) => {
            if (eve.payload.Length == 2) {
                int xy = int.Parse(eve.payload);
                int x = (xy % 10) - 1;
                int y = (xy / 10) - 1;
                foreach (Actor actor in state.actors) {
                    actor.position = new Vector3(offset_x * x, 0, offset_y * y);
                }
            }
            return true;
        }));

        rules.Add(new TTTRule("move", (TTTGameState state, GameEvent eve, TTTGameEngine engine) => {
            if (eve.payload.Length != 2) {
                int dx = 0;
                int dy = 0;
                switch (eve.payload) {
                    case "_up":
                        dy = -1;
                        break;
                    case "down":
                        dy = 1;
                        break;
                    case "left":
                        dx = -1;
                        break;
                    case "right":
                        dx = 1;
                        break;
                    default:
                        break;
                }
                foreach (Actor actor in state.actors) {
                    int x = (int)(actor.position.x / offset_x) + dx;
                    int y = (int)(actor.position.z / offset_y) + dy;
                    if (x < -1 || x > 1 || y < -1 || y > 1) {
                        string audioClip = "Sounds/Wall";
                        //TODO: replace with call to auEngine
                        engine.state.environment.Add(new TTTSoundObject("Prefabs/TTT/AudioSource", audioClip, actor.position));
                        return false;
                    }
                    actor.position = new Vector3(offset_x * x, 0, offset_y * y);
                }
            }
            return true;
        }));

        rules.Add(new TTTRule("action", (TTTGameState state, GameEvent eve, TTTGameEngine engine) => {
            if (eve.payload.Equals("enter")) {
                bool finished = false;
                for (int i = 0; i < 3; i++) {
                    if (state.board[i, 0] == state.board[i, 1] && state.board[i, 0] == state.board[i, 2] && state.board[i, 0] != -1) {
                        finished = true;
                    }
                }
                for (int i = 0; i < 3; i++) {
                    if (state.board[0, i] == state.board[1, i] && state.board[0, i] == state.board[2, i] && state.board[0, i] != -1) {
                        finished = true;
                    }
                }
                if (state.board[0, 0] == state.board[1, 1] && state.board[0, 0] == state.board[2, 2] && state.board[0, 0] != -1) {
                    finished = true;
                }
                if (state.board[0, 2] == state.board[1, 1] && state.board[0, 2] == state.board[2, 0] && state.board[0, 2] != -1) {
                    finished = true;
                }
                if (finished) {
                    (state.result as TTTGameResult).winner = state.curPlayer;
                    (state.result as TTTGameResult).status = TTTGameResult.GameStatus.Won;
                    state.timestamp = 10;
                    state.curPlayer = 0;
                    return false;
                }
                if (state.timestamp == 9) {
                    (state.result as TTTGameResult).status = TTTGameResult.GameStatus.Draw;
                    state.timestamp++;
                    state.curPlayer = 0;
                    return false;
                }
            }
            return true;
        }));

        gameObject.AddComponent<TTTGameEngine>();
        gameObject.AddComponent<TTTUserInterface>();
        gameObject.GetComponent<TTTGameEngine>().initialize(rules, actors, environment, players, renderer);
        gameObject.GetComponent<TTTUserInterface>().initialize(gameObject.GetComponent<TTTGameEngine>());
        gameObject.GetComponent<TTTGameEngine>().postEvent(new GameEvent("", "initialization", "unity"));
	}
}