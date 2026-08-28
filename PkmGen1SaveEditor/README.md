## Current features

- Open 32 KB Game Boy save files
- Validate the main save-data checksum
- Read the player name
- Read the rival name
- Read the player's money
- Read the eight Kanto badges
- Display the current play time
- Edit the player and rival names
- Edit the player's money
- Edit obtained badges
- Recalculate the checksum automatically
- Export changes to a new `.sav` file
- Preserve the original save file

## Supported games

| Game | Language | Status |
|---|---|---|
| Pokémon Red | English | Supported |
| Pokémon Blue | English | Supported |
| Pokémon Rouge | French | Planned |
| Pokémon Bleu | French | Planned |
| Pokémon Yellow | All versions | Not currently supported |

## Requirements

- Windows 10 or Windows 11
- .NET 10
- Visual Studio with the `.NET desktop development` workload

## Building the project

1. Clone the repository:

   ```bash
   git clone https://github.com/ThomasPeccavet/PkmGen1SaveEditor.git
Open PkmGen1SaveEditor.sln in Visual Studio.
Build the solution.
Start the application with F5.
Usage
Export a .sav file from an emulator, flash cartridge or cartridge-dumping device.
Open the save file in Pkm Gen 1 Save Editor.
Modify the desired values.
Select Save as to create a modified copy.
Import the modified save into the game or emulator.

Always keep a backup of the original save file.

Save-file safety

The application:

checks that the file has the expected size;
validates the existing checksum;
writes modifications to memory before exporting;
recalculates the main checksum;
suggests a separate _edited.sav output file.

The application is still under development. Use exported copies and retain your original save.

Roadmap
 Load and validate a save file
 Read trainer information
 Edit money and badges
 Recalculate the checksum
 Edit play time
 Add Pokémon party editing
 Add inventory editing
 Support French save files
 Add automated tests
 Publish the first Windows release
Project structure
PkmGen1SaveEditor/
├── MainForm.cs
├── MainForm.Designer.cs
├── Gen1SaveFile.cs
├── Program.cs
└── PkmGen1SaveEditor.csproj
Legal notice

Pokémon is a trademark of Nintendo, Game Freak and Creatures.

This project is an unofficial, fan-made utility. It is not affiliated with,
endorsed by or sponsored by Nintendo, Game Freak or Creatures.

No game ROMs, BIOS files or copyrighted game assets are included.

License

This project is available under the MIT License. See LICENSE for details.