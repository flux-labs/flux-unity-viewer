var express = require('express');
var app = express();

app.set('port', (process.env.PORT || 8091));

// Views is the default directory for all template files
app.set('view engine', 'ejs');

app.use(/(\/constructor\/)?\/Release/,
    express.static(__dirname + '/unity-build-target/webgl/Release'));
app.use(/(\/constructor\/)?\/TemplateData/,
    express.static(__dirname + '/unity-build-target/webgl/TemplateData'));
app.use(/(\/constructor\/)?\/scripts/,
    express.static(__dirname + '/client-scripts'));
app.use(/(\/constructor\/)?\/flux-data-selector/,
    express.static(__dirname + '/flux-data-selector'));

app.get('/', function(req, res) {
  res.render('viewport-constructor');
});
app.get('/:extra', function(req, res) {
  console.log(req.params.extra);
  res.render('viewport-simple-viewer');
});
app.get('/constructor/:extra', function(req, res) {
  console.log(req.params.extra);
  res.render('viewport-constructor');
});

app.listen(app.get('port'), function() {
  console.log('Node app is running on port', app.get('port'));
});

