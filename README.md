> [!WARNING]
> This project contains malicious code and it's not recommended to execute it on the real hardware!
> 
> Run this project only inside the safe environment!
> 
> We are not responsible for any damages!

# Smart-Shield

<img src="https://github.com/MalwareStudio/Smart-Shield/blob/main/Screenshots/UI.png" alt="" width="700"/>

## What is Smart Shield 🤔?
Smart Shield, as the name suggests, is a form of antivirus software; nonetheless, it does not come with a legit anti-malware engine, which makes this antivirus fake (in other words, a [rogue antivirus](https://en.wikipedia.org/wiki/Rogue_security_software)). 

Executing this rogue antivirus forces the user to use it without any option to quit or uninstall.

The core idea is based on the infamous [Navashield](https://crappysoftware.miraheze.org/wiki/NavaShield) and [Anvi](https://www.enigmasoftware.com/anvi-removal/) antivirus. Some animations, functionality, and UI were inspired by those two antiviruses.

It's worth mentioning that this rogue was created mainly for my content on YouTube and also for a challenge. The challenge for me was to develop a malicious application on the WPF framework (which I didn't understand well) and make it destructive, difficult to remove, annoying, and most importantly, interesting. So I came up with developing my own rogue antivirus.

This rogue should be executed inside a safe environment, such as a [virtual computer](https://en.wikipedia.org/wiki/Virtual_machine). There are many brands that you can choose from; the well-known and easy-to-use ones are [Oracle VirtualBox](https://www.virtualbox.org/) (free) and [VMware](https://www.vmware.com/) (paid).

## How to compile it 🔧?
This project uses custom DLL files. In order to compile it properly in Visual studio, please follow steps below.
All used DLL files are located in **"Custom DLL's"** folder. Please download all DLL files from that folder, if you want to compile the project.

###Steps
Open **Visual Studio** ⇾ In the **"Solution Explorer"** right click on project name **(RogueAntivirusPatched or Rogue_Installer)** ⇾ Click on the option called **"Add"** ⇾ Then hit the option called **"Reference.."** ⇾ Go to **"Browse"** ⇾ From there click on the button **"Browse.."** and select the **missing DLL** ⇾ Make sure the DLL is **enabled** ✅

## Guide for noobs 📄
Because not all of you are developers, I decided to make a quick guide about how to run it properly.

First of all, you need to get the installer (setup) of this rogue. 

Please do not try to launch **"smart_shield.exe"** alone. The rogue must be installed along with external resources and settings!

You can find the installer here → "Smart-Shield\Rogue_Installer\Rogue_Installer\bin\Release" then search for **"smart_shieldx64_setup.exe"** and there you have it!

Download the **"smart_shieldx64_setup.exe"**, drop it wherever you want to, and run it. 

> [!NOTE]
> You don't need to get other files from this repository.
>
> The rogue has everything inside. When you run it, it extracts its resources and changes settings on your system automatically.

## How it works ⚙️?
Once the user allows the rogue to install, it sets the rogue as a startup application. This is done by multiple methods, which are the registry, the startup folder, and lastly, the task scheduler. To make it even more confusing, the rogue creates multiple copies of itself with random names, and those copies are stored inside random directories. Rogue changes the file attributes of these files to hidden. The only way to see these files is to enable the "Show hidden files, folders, and drives" option and uncheck the option "Hide protected operating system files (Recommended)" inside "File Explorer Options."

System applications Registry Editor, Task Manager, and Task Scheduler will be blocked and injected via [the Image File Execution Option method (IFEO)](https://learn.microsoft.com/en-us/previous-versions/windows/desktop/xperf/image-file-execution-options). Any attempt to run these applications runs the rogue instead.

It does not necessarily mean that this rogue is unkillable. The command "[taskkill](https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/taskkill)" is still available; however, trying to kill the rogue causes [Blue Screen Of Death (BSOD)](https://en.wikipedia.org/wiki/Blue_screen_of_death) thanks to the one function in WinAPI called "[NtSetInformationProcess](http://undocumented.ntinternals.net/index.html?page=UserMode%2FUndocumented%20Functions%2FNT%20Objects%2FProcess%2FNtSetInformationProcess.html)". 
The only way to terminate this rogue without triggering BSOD is by restarting the computer. It doesn't cause BSOD upon restarting because it fires an event called "[SystemEvents_SessionEnding](https://learn.microsoft.com/en-us/dotnet/api/microsoft.win32.systemevents.sessionending?view=windowsdesktop-9.0)" in which the rogue sets itself as a non-critical process.

<img src="https://github.com/MalwareStudio/Smart-Shield/blob/main/Screenshots/BSOD10.jpg" alt="" width="600"/>

Getting rid of files that carry the name "Smart Shield.exe" will not prevent the rogue from executing itself because these files are generated by source files (a source file is basically a file dropper whose purpose is to launch "Smart Shield.exe" and after that terminate itself so it can hide from process managers such as Task Manager and Process Hacker).

The rogue also has a license system. The default version is Trial. This particular version is limited to only 7 days. When the trial version ends, it launches payloads that come with generated PCM audio, GDI effects, random inputs, and the destruction of critical system files.
To make this license system possible, Rogue stores data about datetime inside its base key in the registry, "HKEY_LOCAL_MACHINE\SOFTWARE\Smart Shield." The base key of this rogue is also used by other features such as Antivirus Center, Junk Cleaner, and Registry Optimizer.
Rogue does not only store data into the registry but also into its base directory, "C:\Windows\Smart Shield." This directory contains resources and text files. Text files are generated and used by previously mentioned features.

<img src="https://github.com/MalwareStudio/Smart-Shield/blob/main/Screenshots/registry.png" alt="" width="600"/>
<img src="https://github.com/MalwareStudio/Smart-Shield/blob/main/Screenshots/based_folder.png" alt="" width="600"/>

When the user updates to the Pro Version, most annoying features, which include "Advertisement" and "Notifications," are disabled. The rogue also enables the "Quit" option inside the tray icon. Although it actually terminates the rogue, the critical flag remains untouched, resulting in an instant BSOD.

<img src="https://github.com/MalwareStudio/Smart-Shield/blob/main/Screenshots/advertisement.png" alt="" width="600"/>

The keylogger was built via extracted WinAPI functions. Unlike standard keystroke detection, which provides the C# NetFramework 4.5, the low-level functions from the API allow the rogue to detect all keyboard inputs, especially outside the application. This is called a Global Keylogger, and it is used vastly by many keyloggers. Every keystroke that the user presses is stored into a char array. If those combined characters match with hardcoded forbidden keywords (Avast, virus, Defender, Kaspersky, etc.), then the rogue blocks all inputs, and after a few seconds, it terminates itself, which results in BSOD.

## Credits
**Founder and main developer - CYBER SOLDIER aka Clutter**
* Worked on the entire project
* Also bug fixer and tester

**Second developer - Exlon**
* Made Keylogger, participated in making animations and some payloads

## Follow Us 👉
CYBER SOLDIER's youtube channel: https://www.youtube.com/@ClutterTech

Exlon's youtube channel: https://www.youtube.com/@exlon
