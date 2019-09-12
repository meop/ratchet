# Ratchet

Ratchet is a wrapper for Microsoft Xdt lib.

Ratchet can apply XML XDT Transforms in a hierarchy to a base file.

It splits on dots in a filename, and also looks for direct base files in the transforms directory based on naming convention.
This is useful for switching between backend environment files that build upon each other.

Usable for:

- App.config (typically IntegrationTests)
- Web.config (typically Hosts)

Usage:

```
Ratchet.exe <sourceFilePath> <transformFilePath> [<targetFilePath>]
```

The last argument is optional.. if omitted, Ratchet will overwrite the source file directly instead of creating a new file.


## Simple transform example

```
Ratchet.exe
 "D:\some\project\App.config"
 "D:\some\project\App.tst-main.config"
 "D:\git\ratchet\Ratchet\bin\Debug\App.config"

Paths:
 Source: D:\some\project\App.config
 Transform: D:\some\project\App.tst-main.config
 Target: D:\git\ratchet\Ratchet\bin\Debug\App.config
Starting transform..
 Applied: D:\some\project\App.tst-main.config
 Saved: D:\git\ratchet\Ratchet\bin\Debug\App.config
..Done!
```


## Hierarchy transform example

```
Ratchet.exe
 "D:\some\project1\Web.config"
 "D:\some\project1\config\Web.Tst.config"
 "D:\git\ratchet\Ratchet\Web.config"

Paths:
 Source: D:\some\project1\Web.config
 Transform: D:\some\project1\config\Web.Tst.config
 Target: D:\git\ratchet\Ratchet\bin\Debug\Web.config
Starting transform..
 Applied: D:\some\project1\config\Web.TstMain.config
 Applied: D:\some\project1\config\Web.TstMain.DC.config
 Applied: D:\some\project1\config\Web.TstMain.DC.Green.config
 Applied: D:\some\project1\config\Web.Tst.config
 Saved: D:\git\ratchet\Ratchet\bin\Debug\Web.config
..Done!
```