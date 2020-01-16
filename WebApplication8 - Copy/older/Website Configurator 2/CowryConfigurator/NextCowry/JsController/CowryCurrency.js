var app = angular.module('CowryCurrency', []);
app.controller('CowryCurrencyPriceCtrl', ['$scope', '$http', function ($scope, $http) {

    $scope.currencySelect = "USD";

    $scope.ShowCurrencySelect = function (CurrencyType) {
        $(".my_Loader").show();
        //debugger;
        $http.post('/cowry/result/AddCurrencyDetails?CurrencyType=' + CurrencyType).then(function (results) {
          //  debugger;
            if (results.status == 200) {
                if (results.data != null) {
                    localStorage.setItem("CurrencyType", CurrencyType);
                    localStorage.setItem("SpId", results.data);
                    window.location.href="/CowryMain/CowryMain"
                }
            }
            $(".my_Loader").fadeOut("slow");
        }).catch(function (fallback) {
            alert(fallback.statusText);
        });
    }
    
}]);