# SAP Custom Pet Manager

This is library mod for creating and managing custom pets in Super Auto Pets.
Check the [example implementation](https://github.com/Zeprus/sap_custom_pet_example).

## Installation
1. Download the latest BepInEx Build from [here](https://builds.bepis.io/projects/bepinex_be).
2. Follow the installation instructions for Il2Cpp Unity [here](https://docs.bepinex.dev/master/articles/user_guide/installation/unity_il2cpp.html).
3. Download the latest release.
4. Move it to "Super Auto Pets\BepInEx\plugins\"

## Development
If you want to continue working on this project make sure to check the [project file](https://github.com/Zeprus/sap_custom_pet_manager/blob/master/custom_pet_manager.csproj) and set the GameDir property to the root directory of your Super Auto Pets installation.

Build the project with 'dotnet publish' for automatic deployment.

If you are running into unresolved references during build you most likely did not configure the GameDir correctly or forgot to run BepInEx once.

## TODO:
- Implement help with abilities
- Implement custom packs
- Implement overwriting or deleting pets
- Implement help with Assets
