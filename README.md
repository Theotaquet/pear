# PeAR
**Pe**rformance **A**nalyzer **R**eporter

## Required steps

### Configure the Unity plugin

* First, you have to add the `pear` folder to the `Assets` folder of your Unity project.
* In this folder, you can configure your plugin thanks to the `config.json` file. It allows you to choose which performances you want to record, and how often to do so.

### Configure the NodeJS server

* To be able to launch the server, you need to run `npm install` in the `backend` folder. It will download the require modules.
* You can configure the server in the `config.json` file. A server reboot will be required in order to apply the changes.

## How to use it

1. Launch your NodeJS server and your database server.

2. Launch your game executable in a terminal.
    * OPTIONAL: The `-pear` parameter activates the plugin.
    * OPTIONAL: The `-scene` parameter, followed by the scene name, loads the specified scene if PeAR is enabled.
    The full path is only required if you have multiple scenes with that name, otherwise the first matching scene in the list will be loaded.
    If not specified, the default scene will be loaded and analysed.

3. Exit the game to end the recording session.
    * The data are sent to the server and stored in the database.

4. You can get the data with the API (`[serverURL]/[sessions]/[sessionID]`).

5. The session infos are also available in the `sessionLogs.txt` file (its location is configurable in `config.ini`).
