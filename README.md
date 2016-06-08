# Unity-Flux Viewer

## User information

The application is available at https://flux-unity.herokuapp.com/

If you have any questions as a user of the website, check the ? button for help. 
If you have any comments, please email nickm@flux.io. We also encourage creating 
github issues for bugs you find -- it's fun and it's free!

## Developer information

### Getting started

Whether you're on mac, windows, or linux, the steps should be the same:

    1. install node.js
    2. run `npm install && npm start` inside this directory

### Launching the unity project

Install Unity, then open the "unity-project" folder as a project in unity.

You can edit things inside the unity editor and they will show up in the webgl
application after you rebuild the unity files. To rebuild the unity files in the 
webgl client, select: File > Build settings... and then click "Build." Select 
the "unity-build-target" folder, replacing the folder 'webgl.' The .ejs templates 
in views/ will automatically load your new version of the unity project.

### Project notes

Flux employees: Please find the project notes here.
https://docs.google.com/document/d/1zhflOlrxRYR-xru0biE-voybzLv_zLDCi_2gUcgNGks/edit#heading=h.zg4o77vrpkc4
