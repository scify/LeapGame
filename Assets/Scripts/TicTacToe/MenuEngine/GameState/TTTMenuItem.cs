﻿
/**
 * Copyright 2016 , SciFY NPO - http://www.scify.org
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using UnityEngine;
using System.Collections;

public class TTTMenuItem : StaticObject, IUnityRenderable {

    public string prefab;
    public bool selected;
    public string message;
    public string target;
    public string audioMessageCode;
    public AudioClip audioMessage;

    public TTTMenuItem(string prefab, string message, string target, string audioMessageCode, AudioClip audioMessage, Vector3 position, bool hidden, bool selected = false) : base(position, hidden) {
        this.prefab = prefab;
        this.message = message;
        this.target = target;
        this.audioMessageCode = audioMessageCode;
        this.audioMessage = audioMessage;
        this.selected = selected;
	}

    public string getPrefab() {
        return prefab;
    }

}