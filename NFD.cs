using System.Runtime.InteropServices;

namespace NativeFileDialog.Extended; 

/// <summary>
/// Native file dialog extended wrapper
/// </summary>
public static class NFD {
    /// <summary>
    /// Returns current error message
    /// </summary>
    /// <returns>Error</returns>
    private static string? GetError() {
        var error = PInvoke.NFD_GetError();
        PInvoke.NFD_ClearError(); return error;
    }

    /// <summary>
    /// Opens a file picker dialog with extesion filters
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <param name="filterList">Filter List</param>
    /// <returns>Path to file, can be empty</returns>
    public static string OpenDialog(string defaultPath, Dictionary<string, string> filterList)
        => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? OpenDialogN(defaultPath, filterList)
            : OpenDialogU8(defaultPath, filterList);

    /// <summary>
    /// Opens a file picker dialog
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <returns>Path to file, can be empty</returns>
    public static string OpenDialog(string defaultPath)
        => OpenDialog(defaultPath, new Dictionary<string, string>());
    
    /// <summary>
    /// Opens a file picker dialog for multiple files with extension filters
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <param name="filterList">Filter List</param>
    /// <returns>Paths to files, can be empty</returns>
    public static string[] OpenDialogMultiple(string defaultPath, Dictionary<string, string> filterList)
        => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? OpenDialogMultipleN(defaultPath, filterList)
            : OpenDialogMultipleU8(defaultPath, filterList);
    
    /// <summary>
    /// Opens a file picker dialog for multiple files
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <returns>Paths to files, can be empty</returns>
    public static string[] OpenDialogMultiple(string defaultPath)
        => OpenDialogMultiple(defaultPath, new Dictionary<string, string>());

    /// <summary>
    /// Opens a file save dialog with extension filters
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <param name="defaultName">Default Name</param>
    /// <param name="filterList">Filter List</param>
    /// <returns>Path to file, can be empty</returns>
    public static string SaveDialog(string defaultPath, string defaultName, Dictionary<string, string> filterList)
        => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? SaveDialogN(defaultPath, defaultName, filterList)
            : SaveDialogU8(defaultPath, defaultName, filterList);

    /// <summary>
    /// Opens a file save dialog
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <param name="defaultName">Default Name</param>=
    /// <returns>Path to file, can be empty</returns>
    public static string SaveDialog(string defaultPath, string defaultName)
        => SaveDialog(defaultPath, defaultName, new Dictionary<string, string>());
    
    /// <summary>
    /// Opens a folder picker dialog
    /// </summary>
    /// <param name="defaultPath">Default Path</param>
    /// <returns>Path to folder, can be empty</returns>
    public static string PickFolder(string defaultPath)
        => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? PickFolderN(defaultPath) : PickFolderU8(defaultPath);
    
    // Internal N and U8 implementations
    private static string PickFolderN(string defaultPath) {
        PInvoke.NFD_Init();
        PInvoke.NFD_PickFolderU8(out var path,
            defaultPath).ThrowOnError();
        PInvoke.NFD_Quit(); return path;
    }
    
    private static string PickFolderU8(string defaultPath) {
        PInvoke.NFD_Init();
        PInvoke.NFD_PickFolderU8(out var path,
            defaultPath).ThrowOnError();
        PInvoke.NFD_Quit(); return path;
    }
    
    private static string SaveDialogN(string defaultPath, string defaultName, Dictionary<string, string> filterList) {
        PInvoke.NFD_Init();
        PInvoke.NFD_SaveDialogN(out var path, filterList.ToFilterListN(),
            filterList.Count, defaultPath, defaultName).ThrowOnError();
        PInvoke.NFD_Quit(); return path;
    }
    
    private static string SaveDialogU8(string defaultPath, string defaultName, Dictionary<string, string> filterList) {
        PInvoke.NFD_Init();
        PInvoke.NFD_SaveDialogU8(out var path, filterList.ToFilterListU8(),
            filterList.Count, defaultPath, defaultName).ThrowOnError();
        PInvoke.NFD_Quit(); return path;
    }
    
    private static string[] OpenDialogMultipleN(string defaultPath, Dictionary<string, string> filterList) {
        PInvoke.NFD_Init();
        PInvoke.NFD_OpenDialogMultipleN(out var ptr, filterList.ToFilterListN(),
            filterList.Count, defaultPath).ThrowOnError();
        PInvoke.NFD_PathSet_GetCount(ptr, out var count);
        var array = new string[count];
        for (var i = 0; i < count; i++) {
            PInvoke.NFD_PathSet_GetPathN(ptr, i, out var path);
            array[i] = path;
        }
        
        PInvoke.NFD_Quit();
        return array;
    }
    
    private static string[] OpenDialogMultipleU8(string defaultPath, Dictionary<string, string> filterList) {
        PInvoke.NFD_Init();
        PInvoke.NFD_OpenDialogMultipleU8(out var ptr, filterList.ToFilterListU8(),
            filterList.Count, defaultPath).ThrowOnError();
        PInvoke.NFD_PathSet_GetCount(ptr, out var count);
        var array = new string[count];
        for (var i = 0; i < count; i++) {
            PInvoke.NFD_PathSet_GetPathU8(ptr, i, out var path);
            array[i] = path;
        }
        
        PInvoke.NFD_Quit();
        return array;
    }
    
    private static string OpenDialogN(string defaultPath, Dictionary<string, string> filterList) {
        PInvoke.NFD_Init();
        PInvoke.NFD_OpenDialogN(out var path, filterList.ToFilterListN(),
            filterList.Count, defaultPath).ThrowOnError();
        PInvoke.NFD_Quit(); return path;
    }
    
    private static string OpenDialogU8(string defaultPath, Dictionary<string, string> filterList) {
        PInvoke.NFD_Init();
        PInvoke.NFD_OpenDialogU8(out var path, filterList.ToFilterListU8(),
            filterList.Count, defaultPath).ThrowOnError();
        PInvoke.NFD_Quit(); return path;
    }

    // Throws an exception on error
    private static void ThrowOnError(this PInvoke.Result result) {
        if (result == PInvoke.Result.NFD_ERROR)
            throw new Exception(GetError());
    }
    
    // Converts dictionary to filter list
    private static PInvoke.FilterU8[] ToFilterListU8(this Dictionary<string, string> dict) {
        var list = dict.ToList();
        var filters = new PInvoke.FilterU8[dict.Count];
        for (var i = 0; i < filters.Length; i++)
            filters[i] = new PInvoke.FilterU8 {
                Name = list[i].Key,
                Spec = list[i].Value
            };
        return filters;
    }
    
    private static PInvoke.FilterN[] ToFilterListN(this Dictionary<string, string> dict) {
        var list = dict.ToList();
        var filters = new PInvoke.FilterN[dict.Count];
        for (var i = 0; i < filters.Length; i++)
            filters[i] = new PInvoke.FilterN {
                Name = list[i].Key,
                Spec = list[i].Value
            };
        return filters;
    }
}