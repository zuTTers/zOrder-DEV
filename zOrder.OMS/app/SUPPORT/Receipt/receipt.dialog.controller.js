(function () {
    'use strict'
    angular
        .module('Receipt')
        .controller('ReceiptDialogController', ReceiptDialogController);

    /* @ngInject */
    function ReceiptDialogController($scope, $state, $http, $timeout, $filter, $stateParams) {
        var vm = this;

        vm.data = [];
        //var myid = $stateParams.Order_Id;

        vm.printReceipt_1 = function () {
            setTimeout(function () { window.print(); }, 500);
        }

        vm.printReceipt_2 = function () {
            setTimeout(function () { window.print(); }, 8500);
            window.onfocus = function () {
                setTimeout(function () { window.close(); }, 9000);
            }
        }

        vm.receipt = function () {
            $http.get('/api/Receipt/Detail?id=' + $stateParams.Order_Id)
                .then(function (response) {
                    vm.data = response.data.retObject;
                    vm.row = response.data.retObject;
                    vm.printReceipt_1();
                    vm.printReceipt_2();
                });
        };

        vm.receipt();





    }
})();


