# Edge Remover for Windows

A small Windows Forms app that asks Windows Package Manager (winget) whether it can manage Microsoft Edge, then offers the standard interactive uninstall request.

## Build the Windows app

On a Windows PC with the .NET 8 SDK installed:

1. Download or clone this repository.
2. Open this folder in File Explorer.
3. Double-click `build-release.bat`.
4. Find `EdgeRemover.exe` in `bin\Release\net8.0-windows\win-x64\publish\`.

The build is self-contained for 64-bit Windows and does not require a separate .NET runtime on the target PC.

## Use

1. Run `EdgeRemover.exe`.
2. Choose **Check availability**.
3. If winget lists Microsoft Edge, choose **Uninstall Edge** and confirm.
4. Follow any Windows or winget prompts.

The app does not elevate itself, delete files, edit the registry, or use unofficial removal scripts. It invokes only:

```text
winget uninstall --id Microsoft.Edge --exact --interactive --accept-source-agreements
```

Windows controls whether Edge can be removed. Availability depends on your Windows version, region, and device policy. If winget cannot remove it, the app opens the Windows Installed apps settings link for you to inspect supported options. It does not attempt to bypass Windows restrictions.

## Limitations

- This repository contains the app source and build script. The `.exe` must be built on Windows with the .NET 8 SDK; this build environment does not have the Windows toolchain.
- The app is unsigned. Windows SmartScreen may show a warning for a locally built executable.
- Removing Edge may affect apps and Windows features that rely on Edge components or WebView. Consider that impact before confirming.
