# Assets asset (css, js) files

When adding / modifying assets, you should use the `gulp` script to transpile and copy the assets into the wwwroot folder. 
To install gulp, run `npm install` from the root of this solution.

Use the `npm run watch` command to automatically rebuild assets when changes are detected.

Supported asset types are: SaSS, css, ts, js, and less.


### How to define assets

The convention used in this repository is to put all css / js files in the `Assets` folder of your theme or module. Then, you define a `Assets.json` file that specifies which scripts to compile to the wwwroot folder of the solution.

Please see the DigitalTheme for an example on how this works. The [Assets](https://github.com/StatCan/StatCan.OrchardCore/tree/master/src/Themes/DigitalTheme/Assets) folder contains the raw js and sass files and the [Assets.json](https://github.com/StatCan/StatCan.OrchardCore/blob/master/src/Themes/DigitalTheme/Assets.json) file defines which files should be copied / combined in the output.

