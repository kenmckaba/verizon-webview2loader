# verizon-webview2loader
c# dotnet app demonstrates failing GetAvailableBrowserVersionString in path with foreign chars

CoreWebView2Environment.GetAvailableBrowserVersionString() will throw System.DllNotFoundException: Unable to load DLL 'WebView2Loader.dll' when attempting to load from a path with UNICODE characters.

When our app is running from a path with Japanese characters, e.g. お客様, CoreWebView2Environment.GetAvailableBrowserVersionString() throws:
System.DllNotFoundException: Unable to load DLL 'WebView2Loader.dll' or one of its dependencies: The specified module could not be found (0x8007007E)
   at Microsoft.Web.WebView2.Core.CoreWebView2Environment.LoadWebView2LoaderDll()
   at Microsoft.Web.WebView2.Core.CoreWebView2Environment.GetAvailableBrowserVersionString(String browserExecutableFolder)

Any method in CoreWebView2Environment that calls the CoreWebView2Environment.LoadWebView2LoaderDll() will throw the same exception if the path returned by CoreWebView2Environment.LoadSpecifyWebView2LoaderDll or the path set with CoreWebView2Environment.SetLoaderDllFolderPath containes UNICODE characters. 

When decompiling the 1.0.1343.22 version of Microsoft.Web.WebView2.Core it appears that the P/Invoke declaration for LoadLibrary does not specify the CharSet, which means the default CharSet.None is used according to the documentation.

// Decompiled from 1.0.1343.22
[DllImport("kernel32.dll", SetLastError = true)]
internal static extern IntPtr LoadLibrary(string dllToLoad);

// Issue is fixed when CharSet is set to Unicode
[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
internal static extern IntPtr LoadLibrary(string dllToLoad);

We found this similar issue https://github.com/MicrosoftEdge/WebView2Feedback/issues/1236
We’re using the latest Microsoft.Web.WebView2.Core & Microsoft.Web.WebView2.Wpf packages (1.0.1343.22) which should contain the fix mentioned in that issue.

Our app is using the DotNet 4.7 Framework.

I’ve attached a simple app to demonstrate the problem. It makes the call from either an all-latin path or from a path with Japanese chars.

Thanks!
