(function () {
    'use strict'
    angular
        .module('Home')
        .controller('HomeController', HomeController);

    /* @ngInject */
    function HomeController($scope, $state, $http, $filter, $timeout, $uibModal, $log) {

        var vm = this;
        vm.Title = 'Tüm Siparişler';

        vm.query = {
            filter: '',
            limit: '25',
            order: 'Created_Date',
            page: 1,
            count: 0
        };

        vm.filter = {};
        vm.data = [];

        vm.sort = function (keyname) {
            vm.sortKey = keyname;
            vm.reverse = !vm.reverse;
        };

        vm.sendTweet = function () {
            var input = { 'data' : vm.tweet }

            $http.post('/api/Home/UpdateStatus', input)
                .then(function (response) {
                    vm.data = response.data.retObject;
                    $filter("showInfo")($filter, 'Tweet Atıldı!', 1000, 'info');
                });


        }
    }
})();


