# SMF package
Collection of unity extensions that speed-ups working with an engine

<img src="https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white" /> <img src="https://img.shields.io/github/license/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/v/release/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/forks/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/last-commit/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/followers/SzymonMis.svg?style=social&label=Follow&maxAge=2592000" />

## Description


## Content List
 - [Runtime](https://github.com/SzymonMis/smf-package)
	 - [Core](https://github.com/SzymonMis/smf-package)
		 - [Attributes](https://github.com/SzymonMis/smf-package)
          - [ReadOnly](https://github.com/SzymonMis/smf-package)
         - [Misclleanous](https://github.com/SzymonMis/smf-package)
            - [Unparent](https://github.com/SzymonMis/smf-package)
         - [Update Manager](https://github.com/SzymonMis/smf-package)
     - [Extensions](https://github.com/SzymonMis/smf-package)
         - [Custom Colliders](https://github.com/SzymonMis/smf-package)         
            - [Cylinder Collider](https://github.com/SzymonMis/smf-package)
            - [Stairs Collider](https://github.com/SzymonMis/smf-package)
            - [Arc Collider](https://github.com/SzymonMis/smf-package)
            - [Arc Stairs Collider](https://github.com/SzymonMis/smf-package)
         - [Design Patterns](https://github.com/SzymonMis/smf-package)
            - [Singleton](https://github.com/SzymonMis/smf-package)         
         - [Types Extensions](https://github.com/SzymonMis/smf-package)
            - [List Extension](https://github.com/SzymonMis/smf-package)
            - [String Extension](https://github.com/SzymonMis/smf-package)
 - [Editor](https://github.com/SzymonMis/smf-package)
     - [Core](https://github.com/SzymonMis/smf-package)
         - [Drawers](https://github.com/SzymonMis/smf-package)
             - [ReadOnly](https://github.com/SzymonMis/smf-package)
     - [Extensions](https://github.com/SzymonMis/smf-package)
         - [Colliders](https://github.com/SzymonMis/smf-package)
     - [Tools](https://github.com/SzymonMis/smf-package)
         - [Group Game Objects By Parameter](https://github.com/SzymonMis/smf-package)
         - [Remove Missing Scripts](https://github.com/SzymonMis/smf-package)

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

