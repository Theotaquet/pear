# PeAR
**Pe**rformance **A**nalyser and **R**eporter

## What is PeAR?

### Features

* Logs: PeAR can store simplified information about the session recordings and the errors encountered.
    It will be saved in a `sessionLogs.txt` file at the project root.
    See the second step of the "How to use it" section.

## Required steps

### Configure the Unity plugin

* First, you have to add the `pear` folder to the `Assets` folder of your Unity project.
* In this folder, you can configure your plugin thanks to the `config.json` file.
    It also allows you to choose which performances you want to record, and how often to do so.

### Configure the NodeJS server

* To be able to launch the server, you need to run `npm install` in the `server` folder.
    It will download the require modules.
* You can configure the server in the `config.json` file.
    It also allows you to choose the thresholds you want to use for the sessions' validation.
    A server reboot will be required in order to apply the changes.

## How to use it

1. Launch your NodeJS server and your database server.

2. Launch your game executable in a terminal.
    * OPTIONAL: The `-pear` parameter activates the plugin.
    * OPTIONAL: The `-scene` parameter, followed by the scene name,
        loads the specified scene if PeAR is enabled.
        The full path is only required if you have multiple scenes with that name,
            otherwise the first matching scene in the list will be loaded.
        If not specified, the default scene will be loaded and analysed.
    * OPTIONAL: The `-log` parameter enables the session logging in a .txt file.

3. Exit the game to end the recording session.
    * The data are sent to the server and stored in the database.

4. Whenever a GET request is sent to the API,
    the processings are performed and the results are added to the session object.

4. You can get the data with the API (`[serverURL]/api`).
    * GET on `/sessions`: returns all the sessions.
        * The sessions are sorted by date, descending order.
        * You can filter the result with the parameters: `game`, `build` and `scene`
    * GET on `/sessions/[sessionID]`: returns the session with the specified ID.
    * POST on `/sessions`: adds a new session (the ID is auto-generated, no need to specify it).

5. You can also use the web application (`[serverURL]`).
    * The home page is showing the sessions' list with their status.
    * If you click on a session, you can access to the session's details and some charts.
