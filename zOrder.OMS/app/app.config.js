﻿var angularModules = [];
var zk_modules = [];

var appversion = '1.0';

(function () {
    angularModules = [];
    zk_modules = [];

    angularModules = [
        'ui.router',      
        'ngAnimate',
        'ngSanitize',
        'ui.bootstrap',
        '720kb.datepicker',
        'SignIn'
        //'ngAnimate', 'ngSanitize', 
        //'ngMessages', 'ngMeterial',
        //'chart.js', 
        //'linkfy', 
        //'localstoragemodule', 'ui.calender', 'hljs',
        //'angularmoment', 'textangular', 
        //'uigmapgoogle-maps', 'ngfileupload', 'md.data.table',

    ];

    $.each(angularModules, function (i, v) { zk_modules.push(v); });

    //@NewModules
})();
