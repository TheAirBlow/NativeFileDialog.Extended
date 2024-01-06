# NativeFileDialog.Extended
Simple to use C# wrapper for [nativefiledialog-extended](https://github.com/btzy/nativefiledialog-extended)

## Usage
```csharp
using NativeFileDialog.Extended;

NFD.OpenDialog(".");
NFD.OpenDialog(".",
    new Dictionary<string, string> {
        ["Text File"] = "txt"
    });

NFD.OpenDialogMultiple(".");
NFD.OpenDialogMultiple(".",
    new Dictionary<string, string> {
        ["Text File"] = "txt"
    });

NFD.SaveDialog(".", "NFD.txt");
NFD.SaveDialog(".", "NFD.cs",
    new Dictionary<string, string> {
        ["Text File"] = "txt"
    });

NFD.PickFolder(".");
```
