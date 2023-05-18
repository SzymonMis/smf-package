# SMF package
Collection of unity extensions that speed-ups working with an engine

<img src="https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white" /> <img src="https://img.shields.io/github/license/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/v/release/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/forks/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/last-commit/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/followers/SzymonMis.svg?style=social&label=Follow&maxAge=2592000" />

## Description


## Content List
 - [Runtime](https://github.com/SzymonMis/smf-package)
     - [Core](https://github.com/SzymonMis/smf-package)		 
         - [Audio](https://github.com/SzymonMis/smf-package) 
            - [Audio Fader](https://github.com/SzymonMis/smf-package)         
         - [Localization](https://github.com/SzymonMis/smf-package)
            - [Localization Selector](https://github.com/SzymonMis/smf-package)
            - [Localize TMPFont Event](https://github.com/SzymonMis/smf-package)
         - [Misclleanous](https://github.com/SzymonMis/smf-package)
            - [Close Game](https://github.com/SzymonMis/smf-package)
            - [Load Scene](https://github.com/SzymonMis/smf-package)
            - [Open Link](https://github.com/SzymonMis/smf-package)
            - [Unparent](https://github.com/SzymonMis/smf-package)
         - [UI Framework](https://github.com/SzymonMis/smf-package)            
            - [Interactables](https://github.com/SzymonMis/smf-package)
               - [IInteractable](https://github.com/SzymonMis/smf-package)
               - [UI Close Window](https://github.com/SzymonMis/smf-package)
               - [UI Color Changer](https://github.com/SzymonMis/smf-package)
               - [UI Events](https://github.com/SzymonMis/smf-package)
               - [UI Interaction Handler](https://github.com/SzymonMis/smf-package)
               - [UI Open Window](https://github.com/SzymonMis/smf-package)
               - [UI Scaler](https://github.com/SzymonMis/smf-package)
               - [UI Sounds](https://github.com/SzymonMis/smf-package)
               - [UI Transition](https://github.com/SzymonMis/smf-package)
            - [Windows](https://github.com/SzymonMis/smf-package)
               - [UI Window Manager](https://github.com/SzymonMis/smf-package)
               - [UI Window](https://github.com/SzymonMis/smf-package)
               - [UI Sub Window](https://github.com/SzymonMis/smf-package)
               - [UI Main Window](https://github.com/SzymonMis/smf-package)
         - [Update Manager](https://github.com/SzymonMis/smf-package)
     - [Extensions](https://github.com/SzymonMis/smf-package)
         - [Custom Colliders](https://github.com/SzymonMis/smf-package)         
            - [Cylinder Collider](https://github.com/SzymonMis/smf-package)
            - [UICloseWindow](https://github.com/SzymonMis/smf-package)
            - [Arc Collider](https://github.com/SzymonMis/smf-package)
            - [Arc Stairs Collider](https://github.com/SzymonMis/smf-package)
         - [Design Patterns](https://github.com/SzymonMis/smf-package)
            - [Generic Singleton](https://github.com/SzymonMis/smf-package)         
            - [Object Pooling](https://github.com/SzymonMis/smf-package)         
         - [Types Extensions](https://github.com/SzymonMis/smf-package)
            - [List Extension](https://github.com/SzymonMis/smf-package)
            - [String Extension](https://github.com/SzymonMis/smf-package)
     - [Tools](https://github.com/SzymonMis/smf-package)
         - [Box Selection](https://github.com/SzymonMis/smf-package)
 - [Editor](https://github.com/SzymonMis/smf-package)
     - [Core](https://github.com/SzymonMis/smf-package)
         - [Attributes](https://github.com/SzymonMis/smf-package)
         - [Drawers](https://github.com/SzymonMis/smf-package)
             - [Read Only Drawer](https://github.com/SzymonMis/smf-package)
             - [Tag String Drawer](https://github.com/SzymonMis/smf-package)
         - [Json Formatter](https://github.com/SzymonMis/smf-package)
     - [Extensions](https://github.com/SzymonMis/smf-package)
         - [Colliders](https://github.com/SzymonMis/smf-package)
         - [World Transform Extension](https://github.com/SzymonMis/smf-package)
     - [Tools](https://github.com/SzymonMis/smf-package)
         - [Code Line Counter](https://github.com/SzymonMis/smf-package)
         - [Color Picker](https://github.com/SzymonMis/smf-package)
         - [Copyright Injector](https://github.com/SzymonMis/smf-package)
         - [Group Game Objects By Parameter](https://github.com/SzymonMis/smf-package)                  
         - [Json Serialization Tester](https://github.com/SzymonMis/smf-package)
         - [Remove Missing Scripts](https://github.com/SzymonMis/smf-package)
         - [Resource Checker](https://github.com/SzymonMis/smf-package)
         - [Scriptable Object Creator](https://github.com/SzymonMis/smf-package)
         - [Textures To HDRP Maskmap](https://github.com/SzymonMis/smf-package)
         - [Value Tracker](https://github.com/SzymonMis/smf-package)

## Installation 

### Using Unity Package Manager UI

Copy link from github:

![Installation-1](Installation-1.png)

Open Unity Package Manager Window and select "Add package from git URL" option:

![Installation-2](Installation-2.png)

Paste link and then click add button

![Installation-3](Installation-3.png)

<br><br>

### Using Unity Package Manager Manifest

Find the manifest.json file in the Packages folder of your project and edit it to look like this:

```
{
  "dependencies": {
    "com.szymon-mis.smf": "https://github.com/SzymonMis/smf-package.git",
    ...
  },
}
```

