# Windows Photo Viewer Patcher
## Patch "Out of memory" exception when opening images containing an unknown color profile

Tested on: 
```
Windows 7 Enterprise Version 6.1.7601 Service Pack 1 Build 7601
Windows 10 Pro Version 1903 Build 18362.356
Windows 10 Version 22H2 Build 19045.2486
Windows 11 Pro 10.0.22000 build 22000
```

### Why?

Why not?
Windows Photo Viewer is EOL but i like it and a lot of people still use it. Besides, on a fresh install of Windows 7 you have nothing else to open images (except Paint).

### Before you start

The DLL we need to patch (ImagingEngine.dll) is usually located in ```"C:\Program Files\Windows Photo Viewer\"``` or ```"C:\Program Files (x86)\Windows Photo Viewer\"```.
I suggest patching both the x86 and x64 dll, most of the times Windows uses the x86 one even if you are in a x64 environment

Note that sometimes the antivirus may flag the .exe but it's a false positive and you can safely add it to the exclusions list. 

### How does it work?

When the image contains an unknown icc profile Windows Photo Viewer tries to perform color mapping by calling **CreateMultiProfileTransform** but fails, we can patch the check to ignore the invalid icc profile and move on.
What happens if we ignore the invalid profile? Does it use the default one? I really don't know, I only know it starts working.

The function flow looks like this:

```
7C1D9FD5                               | 50                       | push eax                                                    |
7C1D9FD6                               | FF15 50522D7C            | call dword ptr ds:[<&CreateMultiProfileTransform>]          |
7C1D9FDC                               | 85C0                     | test eax,eax                                                |
7C1D9FDE                               | 75 0B                    | jne imagingengine.7C1D9FEB                                  | Jump if ok
7C1D9FE0                               | 68 0D031586              | push 8615030D                                               |
7C1D9FE5                               | FF15 B4512D7C            | call dword ptr ds:[<&?Throw@Base@@YGXJ@Z>]                  | Prepare exception
```

Changing the check from JNE to JMP opens the image correctly. Ignoring exceptions maybe but do we care? I think not.

### Wanna make your own patch?

1) Open PhotoViewer. ```rundll32.exe [path to PhotoViewer.dll],ImageView_Fullscreen [path to image]```

    - [path to PhotoViewer.dll] is usually ```"C:\Program Files\Windows Photo Viewer\PhotoViewer.dll"``` or ```"C:\Program Files (x86)\Windows Photo Viewer\PhotoViewer.dll"```
    - **[path to image] must not be quoted!**

2) Attach your favourite debugger to **rundll32.exe**
3) Select the loaded module **ImagingEngine.dll**
4) Search for an intermodular call to **CreateMultiProfileTransform**
5) Change the **JNE** to **JMP**

### Alternative method (patch the single image instead of the software)

You can edit the image with an hex editor like HxD and corrupt the color profile indication.
Search for the string **ICC_PROFILE** and just change a random letter (for example make it ICC_aROFILE).
You can now normally open the image on every computer without needing the patch