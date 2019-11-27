[Setup]
AppName=Chroma Control
AppVersion=1.0.0.0
WizardStyle=modern
DefaultDirName={autopf32}\ChromaControl
DefaultGroupName=Chroma Control
UninstallDisplayName=Chroma Control
UninstallDisplayIcon={app}\icon.ico
Compression=lzma2
SolidCompression=yes
OutputDir=..\bin\Setup\
OutputBaseFilename=ChromaControlSetup
PrivilegesRequired=admin
CloseApplications=force
AppPublisher=Razer Inc.

[Types]
Name: "custom"; Description: "Select components to install"; Flags: iscustom

[Components]
Name: "aura"; Description: "ASUS AURA Support"; Types: custom
Name: "cue"; Description: "Corsair ICUE Support"; Types: custom

[Files]
Source: "..\bin\Release\netcoreapp3.0\win-x86\publish\**"; DestDir: "{app}"; Flags: recursesubdirs ignoreversion
Source: "icon.ico"; DestDir: "{app}"; Flags: ignoreversion

[Run]
Filename: "{sys}\sc.exe"; Parameters: "create ChromaControlAura start=auto binpath=""{app}\ChromaControl.Aura.exe"""; StatusMsg: "Installing Chroma Control AURA Service..."; Flags: runhidden waituntilterminated; Components: aura
Filename: "{sys}\sc.exe"; Parameters: "sc config ChromaControlAura depend=asComSvc/LightingService"; StatusMsg: "Configuring Chroma Control AURA Service..."; Flags: runhidden waituntilterminated; Components: aura
Filename: "{sys}\sc.exe"; Parameters: "start ChromaControlAura"; StatusMsg: "Starting Chroma Control AURA Service..."; Flags: runhidden waituntilterminated; Components: aura
Filename: "{sys}\sc.exe"; Parameters: "create ChromaControlCue start=auto binpath=""{app}\ChromaControl.Cue.exe"""; StatusMsg: "Installing Chroma Control CUE Service..."; Flags: runhidden waituntilterminated; Components: cue
Filename: "{sys}\sc.exe"; Parameters: "start ChromaControlCue"; StatusMsg: "Starting Chroma Control CUE Service..."; Flags: runhidden waituntilterminated; Components: cue

[UninstallRun]
Filename: "{sys}\sc.exe"; Parameters: "stop ChromaControlAura"; StatusMsg: "Stopping Chroma Control AURA Service..."; Flags: runhidden waituntilterminated; Components: aura
Filename: "{sys}\sc.exe"; Parameters: "delete ChromaControlAura"; StatusMsg: "Uninstalling Chroma Control AURA Service..."; Flags: runhidden waituntilterminated; Components: aura
Filename: "{sys}\sc.exe"; Parameters: "stop ChromaControlCue"; StatusMsg: "Stopping Chroma Control CUE Service..."; Flags: runhidden waituntilterminated; Components: cue
Filename: "{sys}\sc.exe"; Parameters: "delete ChromaControlCue"; StatusMsg: "Uninstalling Chroma Control CUE Service..."; Flags: runhidden waituntilterminated; Components: cue
