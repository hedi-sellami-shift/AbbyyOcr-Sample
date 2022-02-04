using System;
using System.Runtime.InteropServices;
using FREngine;

public class ManualLoadAbbyy : IDisposable
{
    public ManualLoadAbbyy()
    {
        var dllPath = "C:\\Program Files\\ABBYY SDK\\12\\FineReader Engine\\Bin64\\FREngine.dll";
        // Load the FREngine.dll library
        dllHandle = LoadLibraryEx(dllPath, IntPtr.Zero, LOAD_WITH_ALTERED_SEARCH_PATH);

        try
        {
            if (dllHandle == IntPtr.Zero)
            {
                throw new Exception("Can't load " + dllPath);
            }

            IntPtr initializeEnginePtr = GetProcAddress(dllHandle, "InitializeEngine");
            if (initializeEnginePtr == IntPtr.Zero)
            {
                throw new Exception("Can't find InitializeEngine function");
            }

            IntPtr deinitializeEnginePtr = GetProcAddress(dllHandle, "DeinitializeEngine");
            if (deinitializeEnginePtr == IntPtr.Zero)
            {
                throw new Exception("Can't find DeinitializeEngine function");
            }

            IntPtr dllCanUnloadNowPtr = GetProcAddress(dllHandle, "DllCanUnloadNow");
            if (dllCanUnloadNowPtr == IntPtr.Zero)
            {
                throw new Exception("Can't find DllCanUnloadNow function");
            }

            // Convert pointers to delegates
            _initializeEngineFunc = (InitializeEngineFunc) Marshal.GetDelegateForFunctionPointer(
                initializeEnginePtr, typeof(InitializeEngineFunc));
            deinitializeEngine = (DeinitializeEngine) Marshal.GetDelegateForFunctionPointer(
                deinitializeEnginePtr, typeof(DeinitializeEngine));
            dllCanUnloadNow = (DllCanUnloadNow) Marshal.GetDelegateForFunctionPointer(
                dllCanUnloadNowPtr, typeof(DllCanUnloadNow));

            // Call the InitializeEngine function
            int hresult = _initializeEngineFunc(Program.CustomProjectId, "", Program.LicensePassword, "", "", false,
                ref engine);
            Marshal.ThrowExceptionForHR(hresult);

            engine.LoadPredefinedProfile("Default");
        }
        catch (Exception)
        {
            ExplicitlyUnload();
            throw;
        }
    }

    // Kernel32.dll functions
    [DllImport("kernel32.dll")]
    private static extern IntPtr LoadLibraryEx(string dllToLoad, IntPtr reserved, uint flags);

    private const uint LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008;

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

    [DllImport("kernel32.dll")]
    private static extern bool FreeLibrary(IntPtr hModule);

    // FREngine.dll functions
    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private delegate int InitializeEngineFunc(
        string customerProjectId, string licensePath, string licensePassword,
        string dataFolder, string tempFolder, bool isSharedCPUCoresMode, ref FREngine.IEngine engine);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate int DeinitializeEngine();

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate int DllCanUnloadNow();

    // private variables
    private FREngine.IEngine engine = null;
    public FREngine.IEngine Engine => engine;

    // Handle to FREngine.dll
    private IntPtr dllHandle = IntPtr.Zero;

    private InitializeEngineFunc _initializeEngineFunc = null;
    private DeinitializeEngine deinitializeEngine = null;
    private DllCanUnloadNow dllCanUnloadNow = null;

    public void ExplicitlyUnload()
    {
        engine = null;
        int hresult = deinitializeEngine();

        // Deleting all objects before FreeLibrary call
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        hresult = dllCanUnloadNow();
        if (hresult == 0)
        {
            FreeLibrary(dllHandle);
        }

        dllHandle = IntPtr.Zero;
        _initializeEngineFunc = null;
        deinitializeEngine = null;
        dllCanUnloadNow = null;

        // thowing exception after cleaning up
        Marshal.ThrowExceptionForHR(hresult);
    }

    public void Dispose()
    {
        ExplicitlyUnload();
    }
}