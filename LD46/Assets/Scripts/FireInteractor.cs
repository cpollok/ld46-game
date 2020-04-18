using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireNotFoundException : Exception {
    public FireNotFoundException(string fireName) : base("Couldn't find GameObject containing fire of type: " + fireName) { }
}

public class FireInteractor<T> : MonoBehaviour where T : Fire {
    protected T fire;

	protected virtual void Awake() {
        fire = GameObject.FindObjectOfType<T>();

        if (!fire) {
            throw new FireNotFoundException(typeof(T).Name);
        }
    }
}
