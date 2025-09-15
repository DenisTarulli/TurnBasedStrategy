using System;
using UnityEngine;

public class MyCodeStyle : MonoBehaviour
{
    // Constants: UpperCase SnakeCase
    public const int CONSTANT_FIELD = 10;

    // Properties: PascalCase
    public static MyCodeStyle Instance { get; private set; }

    // Events: PascalCase
    public event EventHandler OnSomethingHappenned;

    // Fields: camelCase
    private float memberVariable;

    // Function names: PascalCase
    private void Awake()
    {
        Instance = this;

        DoSomething(10f);
    }

    // Functions params: camelCase
    private void DoSomething(float time)
    {
        // Do something...
        memberVariable = time + Time.deltaTime;
        if (memberVariable > 0)
        {
            // Do something else...
            OnSomethingHappenned?.Invoke(this, EventArgs.Empty);
        }
    }
}

