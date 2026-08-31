<div align="center">

#  Pkm Gen 1 Save Editor

**A lightweight Windows save editor for Pokémon Red and Pokémon Blue.**

Open, validate, edit and export Generation I Game Boy save files through a simple Windows interface.
<img width="940" height="940" alt="PkmnGen1Save" src="https://github.com/ThomasPeccavet/PkmGen1SaveEditor/blob/master/PkmGen1SaveEditor/asset/PkmnGen1Save.png" />
<br>

<a href="https://github.com/ThomasPeccavet/PkmGen1SaveEditor">
  <img alt="GitHub repository" src="https://img.shields.io/badge/GitHub-Repository-181717?style=for-the-badge&logo=github&logoColor=white">
</a>
<a href="https://dotnet.microsoft.com/">
  <img alt=".NET 10" src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white">
</a>
<a href="https://learn.microsoft.com/dotnet/desktop/winforms/">
  <img alt="Windows Forms" src="https://img.shields.io/badge/UI-Windows_Forms-0078D4?style=for-the-badge&logo=windows11&logoColor=white">
</a>
<a href="./LICENSE">
  <img alt="MIT License" src="https://img.shields.io/badge/License-MIT-2EA44F?style=for-the-badge">
</a>
<a href="#roadmap">
  <img alt="Development status" src="https://img.shields.io/badge/Status-Alpha-F5A623?style=for-the-badge">
</a>

<br><br>

[Features](#features) •
[Compatibility](#compatibility) •
[Installation](#installation) •
[Usage](#usage) •
[Roadmap](#roadmap) •
[Contributing](#contributing)

</div>

---

<a id="overview"></a>

##  Overview

Pkm Gen 1 Save Editor is an unofficial, fan-made Windows application for editing save files from the English versions of **Pokémon Red** and **Pokémon Blue**.

The application reads raw Game Boy `.sav` files, validates their data, exposes editable trainer information and automatically recalculates the game's checksum when exporting changes.

The original save file is never overwritten by default.

> [!WARNING]
> This project is still in active development. Always keep a backup of your original save file before making modifications.

<a id="features"></a>

##  Features

### Save-file management

- Open standard 32 KiB Game Boy `.sav` files
- Verify the expected save-file size
- Validate the main save-data checksum
- Detect incompatible or corrupted saves
- Export changes to a separate `_edited.sav` file
- Preserve the original save file

### Trainer information

- Read and edit the player name
- Read and edit the rival name
- Read and edit the player's money
- Display the current play time
- Read and edit the eight Kanto badges

### Data integrity

- Encode text using the Generation I character table
- Decode binary-coded decimal money values
- Read and update individual badge bits
- Recalculate the main checksum automatically
- Reject unsupported characters and invalid values

<a id="screenshot"></a>

## Screenshot

A screenshot of the redesigned interface will be added before the first public release.

<!--
Place the image at docs/screenshots/main-window.png, then remove this comment:

![Pkm Gen 1 Save Editor main window](docs/screenshots/main-window.png)
-->

<a id="compatibility"></a>

## Compatibility

| Game | Language | Status |
|---|---|:---:|
| Pokémon Red | English | ✅ Supported |
| Pokémon Blue | English | ✅ Supported |
| Pokémon Rouge | French | 🛠️ Planned |
| Pokémon Bleu | French | 🛠️ Planned |
| Pokémon Yellow | English | ❌ Not supported |
| Pokémon Yellow | French | ❌ Not supported |

The current version expects a raw **32,768-byte** save file without an emulator-specific header or footer.

Game ROMs, BIOS files and console keys are neither required nor included.

<a id="requirements"></a>

## Requirements

### To run from source

- Windows 10 or Windows 11
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/)
- Visual Studio workload: **.NET desktop development**

### To use a future release

The standalone Windows release will not require Visual Studio.

Prebuilt executables will be provided through the repository's
[Releases page](https://github.com/ThomasPeccavet/PkmGen1SaveEditor/releases)
when version `0.1.0` is ready.

<a id="installation"></a>

## Installation

The project is currently available from source.

### Using Visual Studio

1. Clone the repository:

   ```bash
   git clone https://github.com/ThomasPeccavet/PkmGen1SaveEditor.git
   ```

2. Open `PkmGen1SaveEditor.sln` in Visual Studio.
3. Wait for dependency restoration to finish.
4. Select the `Debug` or `Release` configuration.
5. Build the solution.
6. Press `F5` to start the application.

You can also download the repository as a ZIP file from GitHub and open the solution manually.

<a id="usage"></a>

## Usage

1. Export a `.sav` file from:
   - an emulator;
   - a flash cartridge;
   - or a cartridge-dumping device.
2. Start Pkm Gen 1 Save Editor.
3. Select **Open a save file**.
4. Choose a compatible Pokémon Red or Blue save.
5. Modify the desired trainer information.
6. Select **Save as**.
7. Save the result as a new `.sav` file.
8. Import the edited save into your emulator or cartridge tool.

### Recommended workflow

```text
Original save
      │
      ├── Keep as a permanent backup
      │
      └── Open in Pkm Gen 1 Save Editor
                    │
                    └── Export as *_edited.sav
```

Never test new editing features on your only copy of a save file.

<a id="technical-details"></a>

## ⚙️ Technical details

The editor currently handles the following Generation I structures:

| Data | Representation |
|---|---|
| Player name | Generation I character encoding |
| Rival name | Generation I character encoding |
| Money | Three-byte binary-coded decimal |
| Badges | One byte containing eight bit flags |
| Play time | Separate hour, minute and second values |
| Integrity | Complemented eight-bit checksum |

The save-file parsing and editing logic is kept inside `Gen1SaveFile`, separate from the Windows Forms interface.

This separation will make it easier to add automated tests, additional game versions and alternative interfaces later.

<a id="project-structure"></a>

## Project structure

```text
PkmGen1SaveEditor/
├── PkmGen1SaveEditor.sln
├── README.md
├── LICENSE
├── .gitignore
│
└── PkmGen1SaveEditor/
    ├── Gen1SaveFile.cs
    ├── MainForm.cs
    ├── MainForm.Designer.cs
    ├── MainForm.resx
    ├── Program.cs
    └── PkmGen1SaveEditor.csproj
```

<a id="roadmap"></a>

## Roadmap

### Version 0.1.0

- [x] Create the Windows Forms project
- [x] Open raw Game Boy save files
- [x] Validate the file size
- [x] Validate the main checksum
- [x] Read trainer information
- [x] Edit player and rival names
- [x] Edit money
- [x] Edit obtained badges
- [x] Export an edited save file
- [ ] Complete the redesigned interface
- [ ] Add an application icon
- [ ] Add automated save-file tests
- [ ] Publish the first Windows release

### Future versions

- [ ] Edit play time
- [ ] Edit the player's Pokémon party
- [ ] Edit Pokémon species, levels and moves
- [ ] Edit the inventory
- [ ] Support French save files
- [ ] Support Pokémon Yellow
- [ ] Add drag-and-drop save loading
- [ ] Add automatic backup management

<a id="known-limitations"></a>

## Known limitations

- Only English Pokémon Red and Blue saves are currently supported.
- Emulator-specific save headers and footers are not supported.
- Pokémon party and inventory editing are not yet implemented.
- The application has not yet been tested with every emulator or cartridge-dumping device.
- Only characters supported by the implemented Generation I table can be written.

<a id="contributing"></a>

## Contributing

Feedback, bug reports and contributions are welcome.

Before submitting an issue:

1. Confirm that the save comes from English Pokémon Red or Blue.
2. Confirm that its size is exactly 32,768 bytes.
3. Keep a backup of the original file.
4. Do not publicly upload personal save files unless necessary.

You can report a problem through
[GitHub Issues](https://github.com/ThomasPeccavet/PkmGen1SaveEditor/issues).

When contributing code:

1. Create a dedicated branch.
2. Keep save-file logic separate from the interface.
3. Test changes using copies of save files.
4. Describe the change clearly in the pull request.

<a id="acknowledgements"></a>

## Acknowledgements

Save-file research and implementation were assisted by the
[pret/pokered](https://github.com/pret/pokered) disassembly project.

Thanks to the Pokémon reverse-engineering and preservation communities for documenting the original games.

<a id="legal-notice"></a>

## Legal notice

Pokémon and all related names are trademarks of Nintendo, Game Freak and Creatures.

Pkm Gen 1 Save Editor is an unofficial, fan-made project. It is not affiliated with, endorsed by or sponsored by Nintendo, Game Freak or Creatures.

This repository does not contain or distribute:

- game ROMs;
- BIOS files;
- encryption keys;
- copyrighted sprites;
- proprietary game assets.

Users are responsible for obtaining and exporting their own save files legally.

<a id="license"></a>

## License

This project is distributed under the
[MIT License](LICENSE).

Copyright © 2026
[Thomas Peccavet](https://github.com/ThomasPeccavet).

---

<div align="center">

Made with C# and .NET.

<a href="#top">Back to top</a>

</div>