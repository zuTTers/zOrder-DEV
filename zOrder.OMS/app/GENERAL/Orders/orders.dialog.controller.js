(function () {
    'use strict'
    angular
        .module('Orders')
        .controller('OrdersDialogController', OrdersDialogController);

    /* @ngInject */
    function OrdersDialogController($scope, $state, $http, $timeout, $uibModalInstance, $filter, data) {
        var vm = this;

        vm.operationDataList = [];
        vm.productDataList = [];
        vm.orderdetailDataList = [];
        vm.customerDataList = [];

        vm.row = data.order;
        vm.edit = data.edit;

        vm.fastOrder = true;

        vm.activeTabSet = 2;
        
        if (!vm.edit) { vm.Title = 'Yeni Fiş'; vm.minDate = new Date().toDateString();}

        if (vm.edit) {
            vm.Title = 'Fiş Detay';
            vm.activeTabSet = 3;
            vm.orderdetailDataList = data.orderDetail;
            vm.row.OrderDate = $filter("jsDate")($filter("formatDate")(vm.row.OrderDate));
            //vm.row.OrderDate = new Date(vm.row.OrderDate).toDateString();
            vm.totalprice = 0;

            if (vm.orderdetailDataList.length > 0) {
                $.each(vm.orderdetailDataList, function (i, v) {
                    vm.totalprice += v.TotalPrice;
                });
            }
        }

        /*Textbox'ın sadece int değer almasını sağlar.*/
        vm.IsNumber = function (evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        //if (localStorage.getItem("uk") == undefined) {
        //    $state.go("SignIn");
        //    //location.href = "/SignIn";
        //}

        vm.query = {
            filter: '',
            limit: '10',
            order: 'Created_Date',
            page: 1,
            count: 0
        };

        vm.filter = {};

        vm.ok = function () {
                if (!vm.row.IsDelivered) { vm.row.IsDelivered = false; }
                if (!vm.row.IsPaid) { vm.row.Paid = false; }
                if (vm.fastOrder && !vm.edit) {
                    vm.orderdetailDataList = [{
                        Operation_Id: "9999",
                        Operation_Text: "Diğer",
                        Price: vm.row.Price,
                        Quantity: 1,
                        TotalPrice: vm.row.Price
                    }];
                }
                vm.row.OrderDate = new Date(vm.row.OrderDate.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));

                var input = {
                    order: vm.row,
                    orderDetail: vm.orderdetailDataList
                }
                
            if (vm.row.PhoneNumber && vm.row.OrderDate && vm.row.CustomerName) {
                $http.post('/api/Orders/Save', input)
                    .then(function (response) {
                        if (response.data.success) {
                            $filter("showInfo")($filter, response.data.message, 1000, 'info'); // JSON text denenebilir
                            $state.reload();
                            $uibModalInstance.close('ok');
                        }
                        else {
                            $filter("showInfo")($filter, response.data.message, 1000, 'info'); // JSON text denenebilir
                        }
                    });
            }
            else {
                $filter("showInfo")($filter, "Bilgiler eksik!", 1000, 'info'); // JSON text denenebilir
            }
        };

 


        vm.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        $http.get('/api/Orders/ProductList')
            .then(function (response) {
                vm.productDataList = [];
                vm.productDataList = response.data.retObject;
            });

        vm.getOperations = function (id) {
            $http.get('/api/Orders/OperationList?id=' + id)
                .then(function (response) {
                    vm.operationDataList = [];
                    vm.operationDataList = response.data.retObject;
                });
        }

        vm.getOperationPrice = function (id) {
            $http.get('/api/Orders/PriceList?id=' + id)
                .then(function (response) {
                    vm.row.Quantity = 1;
                    vm.row.Price = response.data.retObject.Price;
                });
        }

        vm.getCustomers = function (id) {
            $http.get('/api/Customer/List')
                .then(function (response) {
                    vm.customerDataList = [];
                    vm.customerDataList = response.data.retObject;
                });
        }
        vm.getCustomers();

        vm.getCustomerData = function (no) {
            $.each(vm.customerDataList, function (i, v) {
                if (v.PhoneNumber == no) {
                    vm.row.CustomerName = v.Name;
                    vm.row.Debt = v.Debt;
                }
            });
        }

        //vm.row.Price = 1;

        vm.addOrderDetail = function (opt, qty, prc) {
            vm.totalprice = 0;

            var orderdetail = {};
            vm.totalprice += qty * prc;

            orderdetail.Operation_Id = opt;
            $.each(vm.operationDataList, function (i, v) {
                if (v.Operation_Id == opt) {
                    orderdetail.Operation_Text = v.Name;
                }
            });
            orderdetail.Quantity = qty;
            orderdetail.Price = prc;
            orderdetail.TotalPrice = vm.totalprice;

            vm.orderdetailDataList.push(orderdetail);
        }

        vm.removeOrderDetail = function (index) {
            vm.totalprice -= vm.orderdetailDataList[index - 1].Price;
            vm.orderdetailDataList.splice(index - 1, 1);
        }

        if (!vm.totalprice) {
            vm.totalprice = 0;
        }

        vm.getBarcode = function () {

            $http.get('/api/Orders/GetBarcode?barcode=' + id)
                .then(function (response) {
                    vm.barcodeData = response.data.retObject;
                });
        }

    }
})();


