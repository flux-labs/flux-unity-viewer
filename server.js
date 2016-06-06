var express = require('express');
var app = express();

app.set('port', (process.env.PORT || 8091));

// Redirect to https.
if (process.env.IS_HEROKU) {
  app.get('*',function(req,res,next){
    if(req.headers['x-forwarded-proto']!='https')
      res.redirect('https://flux-unity.herokuapp.com'+req.url)
    else
      next() /* Continue to other routes if we're not redirecting */
  });
}

// Views is the default directory for all template files
app.set('view engine', 'ejs');

app.get('/vr/', function(req, res) {
  res.render('vr-view');
});

app.use(/(\/vr)?\/Release/,
    express.static(__dirname + '/unity-build-target/webgl/Release'));
app.use(/(\/vr)?\/TemplateData/,
    express.static(__dirname + '/unity-build-target/webgl/TemplateData'));
app.use(/(\/vr)?\/flux-data-selector/,
    express.static(__dirname + '/flux-data-selector'));
app.use(/(\/vr)?\/public/,
    express.static(__dirname + '/public'));
app.use(/(\/vr)?\/flux-sdk-js/,
    express.static(__dirname + '/node_modules/flux-sdk-browser/dist/'));

app.get('/', function(req, res) {
  res.render('desktop-view');
});

app.listen(app.get('port'), function() {
  console.log('Navigate to localhost:' + app.get('port'));
});

