# PeAR
###### __**Pe**__rformance __**A**__nalyzer __**R**__eporter

## Required steps

### Configure the Unity plugin

* First, you have to add the `pear` folder to the `Assets` folder of your Unity project.
* In this folder, you can configure your plugin thanks to the `config.ini` file. It allows you to choose which performances you want to record, and how often to do so.

### Configure the nodeJS server

* To be able to launch the server, you need to run `npm install` in the backend folder. It will download the require modules.

## How to use it

1. Launch your game executable in a terminal.
..* The `-pear` parameter activates the plugin.
..* The `-scene` parameter, followed by the scene name, load the specified scene. The name has to be written without the `.unity`, but the full path is only required if you have multiple scenes with that name.

2. Exit the game to end the recording session.
..* The data are sent to the server and stored in the database.

3. You can get the data with the API (`[serverURL]/[sessions]/[sessionID]`)

4. The session infos are also available in the `sessionLogs.txt` file (its location is configurable in `config.ini`)
