const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const path = require('path');

module.exports = {
    entry: {
        editorjs: './Assets/Editor.js/js/index',
        styles: path.join(process.cwd(), 'Assets/Editor.js/css/index.scss'),
    },
    mode: 'development',
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                },
            },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    'css-loader?-url',
                    'sass-loader',
                ],
            },
        ],
    },
    externals: {
        bootstrap: 'bootstrap',
        jquery: 'jQuery',
    },
    output: {
        filename: '[name]/admin.js',
        path: path.resolve(__dirname, './wwwroot/Scripts/'),
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: '../Styles/editorjs/admin.css',
        }),
    ],
};
