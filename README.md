# SMF package
Collection of unity extensions that speed-ups working with an engine

<img src="https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white" /> <img src="https://img.shields.io/github/license/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/v/release/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/forks/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/last-commit/SzymonMis/smf-package.svg" /> <img src="https://img.shields.io/github/followers/SzymonMis.svg?style=social&label=Follow&maxAge=2592000" />

## Instalation

### Using Unity Package Manager UI

Copy link from github:

![Instalation-1](Instalation-1.png)

Open Unity Package Manager Window and select "Add package from git URL" option:

![Instalation-2](Instalation-2.png)

Paste link and then click add button

![Instalation-3](Instalation-3.png)

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

