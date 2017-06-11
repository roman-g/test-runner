module.exports = {
     entry: './wwwroot/src/app.js',
     output: {
         path: __dirname + '/wwwroot/distr',
         filename: 'app.bundle.js'
     },
     module: {
         loaders: [{
             test: /\.js$/,
             exclude: /node_modules/,
             loader: 'babel-loader'
         }]
     },
    devtool: "#source-map"
 };