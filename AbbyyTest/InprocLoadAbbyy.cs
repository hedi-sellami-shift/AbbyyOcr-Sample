﻿using System;
using FREngine;

public class InprocLoadAbbyy : IDisposable
{
    private IEngineLoader _loader = new InprocLoader();
    public IEngine Engine { get; }

    public InprocLoadAbbyy()
    {
        Engine = _loader.InitializeEngine("", "", "");
    }

    public void Dispose()
    {
        _loader.ExplicitlyUnload();
    }
}