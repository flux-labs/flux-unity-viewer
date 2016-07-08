# Unity-Flux Viewer

## User information

The application is available at https://flux-unity.herokuapp.com/

If you have any questions as a user of the website, check the ? button for help.
If you have any comments, please email nickm@flux.io. We also encourage creating
github issues for bugs you find -- it's fun and it's free!

## Developer information

### Getting started

Whether you're on mac, windows, or linux, the steps should be the same:

1. clone this repo
2. go inside the repo and get flux data selector with `git submodule update --init`
3. install node.js
4. run `npm install && npm start` inside this directory

### Launching the unity project

Install Unity, then open the "unity-project" folder as a project in unity.

You can edit things inside the unity editor and they will show up in the webgl
application after you rebuild the unity files. To rebuild the unity files in the
webgl client, select: File > Build settings... and then click "Build." Select
the "unity-build-target" folder, replacing the folder 'webgl.' The .ejs templates
in views/ will automatically load your new version of the unity project.

### Project structure

  flux-data-selector: this is a submodule from github that implements the flux SDK.
  node_modules: modules installed by the command `npm install`
  public: some static public assets (images, stylesheets, etc.)
  unity-build-target: when building (command+b) the unity project, build it into this folder,
    replacing the webgl folder
  unity-project: open this inside unity3d
  views: this is where all the html template files live.

### Communication between Unity and the browser

**Browser to unity**

        SendMessage(gameobject, method, argument) 

**Unity to browser**

        Application.ExternalCall(method, argument); 

### Logging into the flux sdk on localhost
1. Click + button
2. Click 'use your own data'
3. Click allow
4. Copy everything starting from #access_token=
5. Paste into localhost:8091 so it looks like: localhost:8091/#access_token=... (very long url)
6. It will redirect to flux-unity.herokuapp.com. but go back to localhost:8091
7. It works!

### Project notes

Flux employees: Please find the project notes here.
https://docs.google.com/document/d/1zhflOlrxRYR-xru0biE-voybzLv_zLDCi_2gUcgNGks/edit#heading=h.zg4o77vrpkc4
