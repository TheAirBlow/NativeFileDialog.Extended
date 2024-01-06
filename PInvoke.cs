using System.Runtime.InteropServices;

namespace NativeFileDialog.Extended; 

internal class PInvoke {
    public struct FilterU8 {
        public string Name;
        public string Spec;
    }
    
    public struct FilterN {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;
        
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Spec;
    }
    
    public enum Result {
        NFD_ERROR,
        NFD_OKAY,
        NFD_CANCEL
    };
    
    [DllImport("nfd")]
    public static extern Result NFD_Init();
    
    [DllImport("nfd")]
    public static extern Result NFD_Quit();
    
    [DllImport("nfd")]
    public static extern Result NFD_OpenDialogU8(out string outPath, 
        FilterU8[] filterList, int filterCount, string defaultPath);
    
    [DllImport("nfd")]
    public static extern Result NFD_OpenDialogN(
        [MarshalAs(UnmanagedType.LPWStr)] out string outPath, 
        FilterN[] filterList, int filterCount,
        [MarshalAs(UnmanagedType.LPWStr)] string defaultPath);
    
    [DllImport("nfd")]
    public static extern Result NFD_OpenDialogMultipleU8(out IntPtr outPaths, 
        FilterU8[] filterList, int filterCount, string defaultPath);
    
    [DllImport("nfd")]
    public static extern Result NFD_OpenDialogMultipleN(out IntPtr outPaths, 
        FilterN[] filterList, int filterCount, 
        [MarshalAs(UnmanagedType.LPWStr)] string defaultPath);
    
    [DllImport("nfd")]
    public static extern Result NFD_PathSet_GetCount(IntPtr pathSet, out int count);
    
    [DllImport("nfd")]
    public static extern Result NFD_PathSet_GetPathU8(IntPtr pathSet, int index, out string outPath);
    
    [DllImport("nfd")]
    public static extern Result NFD_PathSet_GetPathN(IntPtr pathSet, int index,
        [MarshalAs(UnmanagedType.LPWStr)] out string outPath);
    
    [DllImport("nfd")]
    public static extern Result NFD_SaveDialogU8(out string outPath, 
        FilterU8[] filterList, int filterCount, string defaultPath, string defaultName);
    
    [DllImport("nfd")]
    public static extern Result NFD_SaveDialogN(
        [MarshalAs(UnmanagedType.LPWStr)] out string outPath, 
        FilterN[] filterList, int filterCount,
        [MarshalAs(UnmanagedType.LPWStr)] string defaultPath,
        [MarshalAs(UnmanagedType.LPWStr)] string defaultName);
    
    [DllImport("nfd")]
    public static extern Result NFD_PickFolderU8(out string outPath, string defaultPath);
    
    [DllImport("nfd")]
    public static extern Result NFD_PickFolderN(
        [MarshalAs(UnmanagedType.LPWStr)] out string outPath,
        [MarshalAs(UnmanagedType.LPWStr)] string defaultPath);
    
    [DllImport("nfd")]
    public static extern string? NFD_GetError();
    
    [DllImport("nfd")]
    public static extern void NFD_ClearError();
}