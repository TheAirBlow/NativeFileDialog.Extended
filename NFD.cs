using System.Runtime.InteropServices;

namespace NativeFileDialog.Extended; 

/// <summary>
/// Native file dialog extended wrapper
/// </summary>
public static class NFD {
    /// <summary>
    /// NFD method result
    /// </summary>
    private enum Result {
        /// <summary>
        /// Programmatic error
        /// </summary>
        NFD_ERROR, 
        
        /// <summary>
        /// User pressed okay, or successful return
        /// </summary>
        NFD_OKAY,
        
        /// <summary>
        /// User pressed cancel
        /// </summary>
        NFD_CANCEL
    };

    /// <summary>
    /// NFD filter item
    /// </summary>
    public struct Filter {
        /// <summary>
        /// User-friendly name
        /// </summary>
        public string Name;
        
        /// <summary>
        /// Specified extensions
        /// </summary>
        public string Spec;

        /// <summary>
        /// Constructs a new NFD filter item
        /// </summary>
        /// <param name="name">User-friendly name</param>
        /// <param name="spec">Specified extensions</param>
        public Filter(string name, string spec) {
            Name = name; Spec = spec;
        }
    }
    
    [DllImport("nfd")]
    private static extern Result NFD_Init();
    
    [DllImport("nfd")]
    private static extern Result NFD_Quit();
    
    [DllImport("nfd")]
    private static extern Result NFD_OpenDialogU8(out string outPath, 
        Filter[] filterList, int filterCount, string defaultPath);
    
    [DllImport("nfd")]
    private static extern Result NFD_OpenDialogMultipleU8(out IntPtr outPaths, 
        Filter[] filterList, int filterCount, string defaultPath);
    
    [DllImport("nfd")]
    private static extern Result NFD_PathSet_GetCount(IntPtr pathSet, out int count);
    
    [DllImport("nfd")]
    private static extern Result NFD_PathSet_GetPathU8(IntPtr pathSet, int index, out string outPath);
    
    [DllImport("nfd")]
    private static extern Result NFD_SaveDialogU8(out string outPath, 
        Filter[] filterList, int filterCount, string defaultPath, string defaultName);
    
    [DllImport("nfd")]
    private static extern Result NFD_PickFolderU8(out string outPath, string defaultPath);
    
    [DllImport("nfd")]
    private static extern string? NFD_GetError();
    
    [DllImport("nfd")]
    private static extern void NFD_ClearError();

    /// <summary>
    /// Returns current error message
    /// </summary>
    /// <returns>Error</returns>
    private static string? GetError() {
        var error = NFD_GetError();
        NFD_ClearError(); return error;
    }

    /// <summary>
    /// Opens a file picker dialog with extesion filters
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <param name="filterList">Filter List</param>
    /// <returns>Path on success, null on failure</returns>
    public static string? OpenDialog(string defaultPath, Filter[] filterList) {
        NFD_Init(); string? value = null;
        switch (NFD_OpenDialogU8(out var path, 
                    filterList, filterList.Length, defaultPath)) {
            case Result.NFD_ERROR:
                throw new Exception(GetError());
            case Result.NFD_OKAY:
                value = path;
                break;
            case Result.NFD_CANCEL:
                break;
        }
        
        NFD_Quit();
        return value;
    }

    /// <summary>
    /// Opens a file picker dialog
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <returns>Path on success, null on failure</returns>
    public static string? OpenDialog(string defaultPath)
        => OpenDialog(defaultPath, Array.Empty<Filter>());
    
    /// <summary>
    /// Opens a file picker dialog for multiple files with extension filters
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <param name="filterList">Filter List</param>
    /// <returns>Path on success, null on failure</returns>
    public static string[]? OpenDialogMultiple(string defaultPath, Filter[] filterList) {
        NFD_Init(); string[]? value = null;
        switch (NFD_OpenDialogMultipleU8(out var ptr, filterList,
                    filterList.Length, defaultPath)) {
            case Result.NFD_ERROR:
                throw new Exception(GetError());
            case Result.NFD_OKAY:
                NFD_PathSet_GetCount(ptr, out var count);
                value = new string[count];
                for (var i = 0; i < count; i++) {
                    NFD_PathSet_GetPathU8(ptr, i, out var path);
                    value[i] = path;
                }
                break;
            case Result.NFD_CANCEL:
                break;
        }
        
        NFD_Quit();
        return value;
    }
    
    /// <summary>
    /// Opens a file picker dialog for multiple files
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <returns>Path on success, null on failure</returns>
    public static string[]? OpenDialogMultiple(string defaultPath)
        => OpenDialogMultiple(defaultPath, Array.Empty<Filter>());

    /// <summary>
    /// Opens a file save dialog with extension filters
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <param name="defaultName">Default Name</param>
    /// <param name="filterList">Filter List</param>
    /// <returns>Path on success, null on failure</returns>
    public static string? SaveDialog(string defaultPath, string defaultName, Filter[] filterList) {
        NFD_Init(); string? value = null;
        switch (NFD_SaveDialogU8(out var path, filterList,
                    filterList.Length, defaultPath, defaultName)) {
            case Result.NFD_ERROR:
                throw new Exception(GetError());
            case Result.NFD_OKAY:
                value = path;
                break;
            case Result.NFD_CANCEL:
                break;
        }
        
        NFD_Quit();
        return value;
    }

    /// <summary>
    /// Opens a file save dialog
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <param name="defaultName">Default Name</param>=
    /// <returns>Path on success, null on failure</returns>
    public static string? SaveDialog(string defaultPath, string defaultName)
        => SaveDialog(defaultPath, defaultName, Array.Empty<Filter>());
    
    /// <summary>
    /// Opens a folder picker dialog
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <returns>Path on success, null on failure</returns>
    public static string? PickFolder(string defaultPath) {
        NFD_Init(); string? value = null;
        switch (NFD_PickFolderU8(out var path, defaultPath)) {
            case Result.NFD_ERROR:
                throw new Exception(GetError());
            case Result.NFD_OKAY:
                value = path;
                break;
            case Result.NFD_CANCEL:
                break;
        }
        
        NFD_Quit();
        return value;
    }
}