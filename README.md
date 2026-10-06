# Windows Photo Viewer Patcher
[![GitHub license](https://img.shields.io/github/license/PaaaulZ/PhotoViewerOutOfMemoryNoMore?style=flat-square)](https://github.com/PaaaulZ/PhotoViewerOutOfMemoryNoMore/blob/main/LICENSE)
[![GitHub release](https://img.shields.io/github/v/release/PaaaulZ/PhotoViewerOutOfMemoryNoMore?style=flat-square)](https://github.com/PaaaulZ/PhotoViewerOutOfMemoryNoMore/releases)
[![GitHub stars](https://img.shields.io/github/stars/PaaaulZ/PhotoViewerOutOfMemoryNoMore?style=flat-square)](https://github.com/PaaaulZ/PhotoViewerOutOfMemoryNoMore/stargazers)

Fix Windows Photo Viewer "Out of memory" errors caused by invalid or unknown ICC color profiles.

This patcher modifies ImagingEngine.dll so Windows Photo Viewer can open images that would otherwise fail with an "Out of memory" error.

<img width="624" height="190" alt="image" src="https://github.com/user-attachments/assets/b2295bb9-efcb-423b-8422-e9b0d9d25853" />

### The problem

Some images, especially those produced by certain phones, contain ICC color
profiles that Windows Photo Viewer cannot handle correctly.

Instead of opening the image, Photo Viewer displays:

> Out of memory

This patch makes Photo Viewer ignore the failing ICC profile check and continue
loading the image.

### Why?

Windows Photo Viewer is EOL but i like it and a lot of people still use it. Besides, on a fresh install of Windows 7 you have nothing else to open images (except Paint).

### Requirements

- Administrator privileges
- .NET Framework 4.5

### Usage

1) Download the latest release.
2) Run `PhotoViewerPatcher.exe`.
3) Ensure Auto Patch is checked
4) Click **Patch**

### Troubleshooting

* If "Auto Patch" fails you can uncheck it and manually browse for the correct path.
* If you get an access denied error manually take ownership of the folder and give yourself full read and write permissions

### Before you start

Some antivirus software may flag the patcher because it modifies a Windows DLL, only download the patcher from the official GitHub Releases page.

Do not disable your antivirus globally. If your antivirus blocks the executable verify the release and eventually add it to your exclusions list

### Manual patch instructions

If you disabled "Auto Patch" know that the DLL we need to patch (ImagingEngine.dll) is usually located in ```"C:\Program Files\Windows Photo Viewer\"``` or ```"C:\Program Files (x86)\Windows Photo Viewer\"```.

I suggest patching both the x86 and x64 dll, most of the times Windows uses the x86 one even if you are in a x64 environment.

If you don't know what you're doing just leave "Auto Patch" on, it will patch both automatically

Tested on: 
```
Windows Vista Home Premium
Windows 7 Enterprise Version 6.1.7601 Service Pack 1 Build 7601
Windows 10 Pro Version 1903 Build 18362.356
Windows 10 Version 22H2 Build 19045.2486
Windows 11 Pro 10.0.22000 build 22000
```

Other Windows versions might work even if not tested

### How does it work?

When the image contains an unknown icc profile Windows Photo Viewer tries to perform color mapping by calling **CreateMultiProfileTransform** but fails, we can patch the check to ignore the invalid icc profile and move on.
What happens when the invalid profile is ignored? Does Windows fall back to the default color profile? I don't know. I only know that Photo Viewer successfully
opens the image afterwards.

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
