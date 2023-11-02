# Microsoft Edge Rewriter

APP STATUS: Beta

## What is this?
This is a continuation of the now dead project EdgeDeflector

## How does this work?
This program is put in place of the original msedge executable (which is renamed to original-msedge.exe). If it detects that edge was launched by a protocol, it rewrites it (using the original EdgeDeflector code) and launches the standard browser. Otherwise, it launches edge.

## How do I use this?
For now, an installer is not provided (this is very beta). Just go to C:\Program Files (x86)\Microsoft\Edge\Application and rename msedge.exe to original-msedge.exe, build USING THE PUBLISH TEMPLATE this solution and put the generated exe in the folder with the name msedge.exe

## It stopped working, why?
Micro$oft probably forced your PC to reinstall edge. Follow again the installation instructions and make sure to use the updated edge as orginal-msedge.exe

## What works? What doesn't?
Pretty much everything that worked with edgedeflector should work, but I'm not too sure with Windows Copilot. If it does not work, I'l try my best to support it.

## Other notes
This project uses code from edgedeflector. Check out the original project if you want. https://github.com/da2x/EdgeDeflector