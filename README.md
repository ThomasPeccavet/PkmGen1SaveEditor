<div align="center">

<img src="PkmGen1SaveEditor/asset/PkmnGen1Save.png" width="260" alt="Logo Pkm Gen 1 Save Editor">

# Pkm Gen 1 Save Editor

**Un éditeur Windows moderne pour les sauvegardes de Pokémon Rouge et Bleu.**

[![Build](https://github.com/ThomasPeccavet/PkmGen1SaveEditor/actions/workflows/build.yml/badge.svg)](https://github.com/ThomasPeccavet/PkmGen1SaveEditor/actions/workflows/build.yml)
![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/UI-Windows%20Forms-0078D4?logo=windows11&logoColor=white)
[![Licence MIT](https://img.shields.io/badge/Licence-MIT-2EA44F)](LICENSE.txt)
![Statut](https://img.shields.io/badge/Statut-Alpha-F5A623)

[Fonctionnalités](#fonctionnalités) ·
[Compatibilité](#compatibilité) ·
[Installation](#installation) ·
[Utilisation](#utilisation) ·
[Feuille de route](#feuille-de-route)

</div>

---

## Présentation

Pkm Gen 1 Save Editor est une application non officielle permettant d’ouvrir,
de consulter et de modifier une sauvegarde Game Boy de **Pokémon Rouge ou
Pokémon Bleu** depuis une interface Windows Forms.

L’éditeur manipule directement les structures binaires de la première
génération : informations du dresseur, équipe, statistiques des Pokémon et
douze boîtes PC. Les checksums concernés sont recalculés après chaque
modification afin de préserver l’intégrité de la sauvegarde.

L’interface reprend l’esthétique de Pokémon Rouge/Bleu : palette Game Boy,
cadres pixelisés et composants graphiques personnalisés.

> [!WARNING]
> Le projet est encore en version alpha. Conservez toujours une copie intacte
> de votre sauvegarde originale avant d’effectuer une modification.

## Fonctionnalités

### Gestion des sauvegardes

- ouverture des fichiers Game Boy bruts de 32 Kio (`.sav`) ;
- vérification de la taille et du checksum principal ;
- détection des sauvegardes incompatibles ou corrompues ;
- export vers un nouveau fichier `_edited.sav` ;
- conservation du fichier original par défaut.

### Dresseur

- lecture et modification du nom du joueur ;
- lecture et modification du nom du rival ;
- lecture et modification de l’argent, de 0 à 999 999 ₽ ;
- lecture du temps de jeu ;
- lecture et modification des huit badges de Kanto.

### Équipe Pokémon

- affichage du surnom, de l’espèce, des types, du niveau, des PV, du statut et
  des attaques ;
- fiche détaillée avec modification des statistiques et de l’expérience ;
- ajout cohérent d’un Pokémon à partir de son espèce et de son niveau ;
- remplacement, suppression et duplication ;
- réorganisation de l’ordre de l’équipe ;
- soin collectif des PV, statuts et PP ;
- sprites Rouge/Bleu chargés à la demande et mis en cache ;
- protection contre une équipe vide ou supérieure à six membres.

### Boîtes PC

- lecture des 12 boîtes et de leur capacité de 20 Pokémon ;
- ajout et suppression de Pokémon stockés ;
- dépôt depuis l’équipe et retrait vers l’équipe ;
- déplacement d’un Pokémon entre deux boîtes ;
- recherche par surnom, espèce, type ou attaque ;
- synchronisation de la boîte active et des banques de stockage.

### Intégrité des données

- encodage et décodage du texte de la génération I ;
- lecture et écriture de l’argent en BCD ;
- lecture et écriture des valeurs 16 et 24 bits en big-endian ;
- recalcul du checksum principal ;
- recalcul des checksums des deux banques PC et de chaque boîte ;
- validation des limites avant toute écriture binaire.

## Aperçu de l’interface

L’application comporte maintenant un thème commun à toutes les fenêtres :

- palette vert crème inspirée de l’écran Game Boy ;
- `PokemonGroupBox` dessinées à partir de tuiles 8 × 8 ;
- boutons, champs et tableaux harmonisés ;
- fiches d’ajout, de déplacement et de modification cohérentes avec la fenêtre
  principale.

Une capture complète sera ajoutée avant la première version distribuable.

## Compatibilité

| Jeu | Région/langue | État |
|---|---|:---:|
| Pokémon Rouge / Bleu | Français | ✅ Pris en charge |
| Pokémon Red / Blue | Anglais | 🧪 Structure prise en charge, tests à compléter |
| Pokémon Jaune / Yellow | Toutes | ❌ Non pris en charge |
| Formats avec en-tête ou pied d’émulateur | Toutes | ❌ Non pris en charge |

Le fichier doit faire exactement **32 768 octets**. La sélection de langue
présente dans l’interface est encore informative : la détection automatique de
la version et de la langue reste à développer.

Les ROM, BIOS et clés de console ne sont ni nécessaires ni fournis.

## Prérequis

- Windows 10 ou Windows 11 ;
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) ;
- [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/)
  avec la charge de travail **Développement Desktop .NET**.

## Installation

Le projet est actuellement distribué sous forme de code source.

```bash
git clone https://github.com/ThomasPeccavet/PkmGen1SaveEditor.git
cd PkmGen1SaveEditor
```

Ouvrez ensuite `PkmGen1SaveEditor.slnx` dans Visual Studio, attendez la
restauration des dépendances, puis lancez le projet avec `F5`.

Pour compiler en ligne de commande :

```bash
dotnet build PkmGen1SaveEditor.slnx --configuration Release
```

Les futures versions prêtes à l’emploi seront publiées dans les
[Releases GitHub](https://github.com/ThomasPeccavet/PkmGen1SaveEditor/releases).

## Utilisation

1. Exportez le fichier `.sav` depuis votre émulateur, flashcart ou lecteur de
   cartouche.
2. Lancez l’application et cliquez sur **Ouvrir une sauvegarde**.
3. Modifiez les informations du dresseur si nécessaire.
4. Ouvrez **Voir l’équipe** pour gérer l’équipe et les boîtes PC.
5. Cliquez sur **Enregistrer sous…** dans la fenêtre principale.
6. Importez le fichier `_edited.sav` dans votre émulateur ou votre matériel.

> [!TIP]
> Travaillez toujours sur une copie. Ne remplacez votre sauvegarde originale
> qu’après avoir vérifié le résultat dans le jeu.

### Sprites

Les sprites ne sont pas inclus dans le dépôt. Ils sont téléchargés à la demande
depuis le dépôt public de [PokeAPI/sprites](https://github.com/PokeAPI/sprites),
puis conservés en mémoire pendant la session. L’édition reste fonctionnelle sans
connexion Internet ; seul l’aperçu du sprite sera absent.

## Structure du projet

```text
PkmGen1SaveEditor/
├── .github/workflows/build.yml       # Compilation Windows automatique
├── PkmGen1SaveEditor.slnx
├── README.md
├── LICENSE.txt
└── PkmGen1SaveEditor/
    ├── Gen1SaveFile.cs               # Dresseur, équipe et checksum principal
    ├── Gen1SaveFile.Storage.cs       # Équipe avancée et boîtes PC
    ├── Gen1Pokemon.cs                # Modèle d’un Pokémon
    ├── Gen1SpeciesCatalog.cs         # Noms et identifiants internes
    ├── Gen1SpeciesData.cs            # Stats, types et croissance
    ├── Gen1MoveCatalog.cs            # Attaques de la génération I
    ├── PokemonSpriteService.cs       # Chargement et cache des sprites
    ├── PokemonGroupBox.cs            # Cadre pixelisé personnalisé
    ├── PokemonTheme.cs               # Thème partagé par les fenêtres
    ├── MainForm.cs                    # Fenêtre principale
    ├── TeamForm.cs                    # Équipe et stockage PC
    ├── PokemonDetailsForm.cs          # Statistiques détaillées
    ├── AddPokemonForm.cs              # Création et remplacement
    └── BoxSelectionForm.cs            # Choix d’une boîte PC
```

## Détails techniques

| Donnée | Représentation |
|---|---|
| Taille de sauvegarde | 32 Kio / 32 768 octets |
| Noms | Table de caractères Pokémon génération I |
| Argent | 3 octets BCD |
| Badges | 8 indicateurs binaires dans un octet |
| Expérience | entier 24 bits big-endian |
| Équipe | 1 à 6 structures de 44 octets |
| Boîte PC | 12 boîtes de 20 Pokémon |
| Intégrité | checksums principal, banques PC et boîtes |

La logique binaire est séparée de l’interface afin de faciliter les tests et
l’ajout futur d’autres versions du jeu.

## Limites connues

- Pokémon Jaune n’est pas pris en charge.
- La langue et la version exactes ne sont pas encore détectées automatiquement.
- Le temps de jeu est affiché mais n’est pas modifiable.
- Les attaques et leurs PP sont affichés mais ne sont pas encore éditables.
- Les objets du sac, le Pokédex et les options du jeu ne sont pas éditables.
- La création d’un Pokémon est cohérente mais ne simule pas encore tous les
  détails d’une capture naturelle dans le jeu.
- Les sauvegardes enrichies d’un en-tête ou d’un pied propre à un émulateur ne
  sont pas reconnues.

## Feuille de route

### Version 0.1.0

- [x] ouvrir, vérifier et exporter une sauvegarde ;
- [x] modifier le dresseur, l’argent et les badges ;
- [x] consulter et modifier les statistiques de l’équipe ;
- [x] ajouter, remplacer, supprimer, dupliquer et réorganiser des Pokémon ;
- [x] gérer les 12 boîtes PC et les transferts ;
- [x] afficher les types, attaques et sprites ;
- [x] unifier l’interface rétro sur toutes les fenêtres ;
- [x] compiler automatiquement chaque Pull Request sur Windows ;
- [ ] ajouter des tests automatisés sur des sauvegardes anonymisées ;
- [ ] publier la première version Windows.

### Évolutions envisagées

- [ ] sauvegarde automatique de secours avant modification ;
- [ ] glisser-déposer d’un fichier `.sav` ;
- [ ] détection automatique du jeu, de la langue et de la région ;
- [ ] édition des attaques, PP, DV et EV ;
- [ ] édition du sac, du PC objets et du Pokédex ;
- [ ] historique d’annulation/rétablissement ;
- [ ] prise en charge de Pokémon Jaune ;
- [ ] rapport de validation avant export.

## Contribuer

Les retours, rapports de bugs et contributions sont bienvenus dans les
[Issues GitHub](https://github.com/ThomasPeccavet/PkmGen1SaveEditor/issues).

Pour contribuer au code :

1. créez une branche dédiée ;
2. conservez la logique de sauvegarde séparée de l’interface ;
3. utilisez uniquement des copies anonymisées de sauvegardes ;
4. vérifiez la compilation en mode `Release` ;
5. décrivez clairement le changement dans la Pull Request.

## Remerciements

- [pret/pokered](https://github.com/pret/pokered) pour la documentation issue du
  désassemblage des jeux ;
- [PokeAPI/sprites](https://github.com/PokeAPI/sprites) pour l’accès aux sprites
  utilisés à la demande ;
- les communautés de préservation et de rétro-ingénierie Pokémon.

## Mentions légales

Pokémon et les noms associés sont des marques de Nintendo, Game Freak et
Creatures. Ce projet amateur et non officiel n’est affilié, approuvé ou sponsorisé
par aucune de ces sociétés.

Le dépôt ne contient ni ROM, ni BIOS, ni clé de console. Les utilisateurs sont
responsables de l’export légal de leurs propres sauvegardes.

## Licence

Le code source est distribué sous [licence MIT](LICENSE.txt).

Copyright © 2026 [Thomas Peccavet](https://github.com/ThomasPeccavet).
