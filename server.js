var express = require('express');
var app = express();

app.set('port', (process.env.PORT || 8091));

// Views is the default directory for all template files
app.set('view engine', 'ejs');

app.get('/view/', function(req, res) {
  res.render('viewport-simple-viewer');
});

app.use(/(\/view)?\/Release/,
    express.static(__dirname + '/unity-build-target/webgl/Release'));
app.use(/(\/view)?\/TemplateData/,
    express.static(__dirname + '/unity-build-target/webgl/TemplateData'));
app.use(/(\/view)?\/flux-data-selector/,
    express.static(__dirname + '/flux-data-selector'));
app.use(/(\/view)?\/public/,
    express.static(__dirname + '/public'));
app.use(/(\/view)?\/flux-sdk-js/,
    express.static(__dirname + '/node_modules/flux-sdk-browser/dist/'));

app.get('/', function(req, res) {
  res.render('viewport-constructor');
});

app.listen(app.get('port'), function() {
  console.log('Navigate to localhost:' + app.get('port'));
});

