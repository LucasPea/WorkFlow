var app = angular.module('cscUK', []);
app.filter('test', ['$sce', function ($sce) {
    return function (html) {
        return $sce.trustAsHtml(html);
    };
}]);

if (ApplicationInsights === "undefined") {
    var ApplicationInsights = {};
}

ApplicationInsights = {
    trackApplicationInsight: function (entityName, properties) {
        var appInsights = window.appInsights || function (config) { function i(config) { t[config] = function () { var i = arguments; t.queue.push(function () { t[config].apply(t, i) }) } } var t = { config: config }, u = document, e = window, o = "script", s = "AuthenticatedUserContext", h = "start", c = "stop", l = "Track", a = l + "Event", v = l + "Page", y = u.createElement(o), r, f; y.src = config.url || "https://az416426.vo.msecnd.net/scripts/a/ai.0.js"; u.getElementsByTagName(o)[0].parentNode.appendChild(y); try { t.cookie = u.cookie } catch (p) { } for (t.queue = [], t.version = "1.0", r = ["Event", "Exception", "Metric", "PageView", "Trace", "Dependency"]; r.length;) i("track" + r.pop()); return i("set" + s), i("clear" + s), i(h + a), i(c + a), i(h + v), i(c + v), i("flush"), config.disableExceptionTracking || (r = "onerror", i("_" + r), f = e[r], e[r] = function (config, i, u, e, o) { var s = f && f(config, i, u, e, o); return s !== !0 && t["_" + r](config, i, u, e, o), s }), t }(
            {
                instrumentationKey: "34501890-eecd-44da-b9a5-07f144d2ae00"
            });
        window.appInsights = appInsights;

        appInsights.trackPageView(entityName, document.location.href, properties);
    }
}

app.controller('cscUKCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.price = [];
    var currencyType = "GBP";

    var CustomerId = "";

    $scope.users = [];
    $scope.users.premiumUsers = "0";
    $scope.users.essentialUsers = "0";
    $scope.users.teamUsers = "0";

    $scope.ProductLicenses = [];
    $scope.OneTimeProducsFinancials = [];
    $scope.OneTimeProducsCommercials = [];
    $scope.OneTimeProducsData = [];
    $scope.OneTimeProducsReporting = [];
    $scope.OneTimeProducsAdditional = [];
    $scope.OneTimeHumanResources = [];
    $scope.OneTime = [];
    $scope.TestArray = [];
    $scope.SubjectType = [];
    $scope.SupportOption = [];
    $scope.SupportUser = [];
    $scope.currencyType = "GBP";
    $scope.currencySymbol = "£";
    
    function getUrlParameter(name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    };

    function getTotalDetail() {
        var licenses = 0;
        var products = new Array();
        var indice = 0;
        for (var a = 0; a < $scope.ProductLicenses.length; a++) {
            if (parseInt(document.getElementById("inputTest" + a).value) > 0) {
                products[indice] = new Array(2);
                products[indice][0] = $scope.ProductLicenses[a].ProductId;
                products[indice][1] = document.getElementById("inputTest" + a).value;
                licenses+= parseInt(document.getElementById("inputTest" + a).value);
                indice++;
            }
        }
        for (var a = 0; a < $scope.OneTime.length; a++) {
            var getQty = false;
            var Qtyy = 1;
            if ($scope.OneTime[a].MaximumSale > 0) {
                getQty = true;
                Qtyy = document.getElementById("qty" + $scope.OneTime[a].ProductId).innerHTML;
            }
            if (checkActive($scope.OneTime[a].ProductId) || (Qtyy > 0 && getQty)) {
                products[indice] = new Array(2);
                products[indice][0] = $scope.OneTime[a].ProductId;
                products[indice][1] = Qtyy;
                indice++;
            }
        }
        for (var a = 0; a < $scope.SupportOption.length; a++) {
            if (checkActive($scope.SupportOption[a].ProductId)) {
                products[indice] = new Array(2);
                products[indice][0] = $scope.SupportOption[a].ProductId;
                products[indice][1] = 1;
                indice++;
            }
        }
        for (var a = 0; a < $scope.SupportOption.length; a++) {
            if (parseInt($scope.SupportOption[a].MinimumSale) == 0 && checkActive($scope.SupportOption[a].ProductId) && licenses >= 0) {
                for (var b = 0; b < $scope.SupportUser.length; b++) {
                    if ($scope.SupportUser[b].RequiredProduct == $scope.SupportOption[a].ProductId && licenses >= 0) {
                        products[indice] = new Array(2);
                        products[indice][0] = $scope.SupportUser[b].ProductId;
                        products[indice][1] = licenses;
                    }
                }
            }
        }
        var resume = '';
        for (var a = 0; a < products.length; a++) {
            for (var j = 0; j < 2; j++) {
                if (j == 0) {
                    resume += '' + products[a][j] + '' + ':';
                } else {
                    resume += '' + products[a][j] + '';
                }
            }
            if (a < products.length-1) resume += ",";
        }
        return resume;
    }
    function getTotalDetailNames() {
        var licenses = 0;
        var products = new Array();
        var indice = 0;
        for (var a = 0; a < $scope.ProductLicenses.length; a++) {
            if (parseInt(document.getElementById("inputTest" + a).value) > 0) {
                products[indice] = new Array(4);
                products[indice][0] = $scope.ProductLicenses[a].ProductId;
                products[indice][1] = $scope.ProductLicenses[a].ProductName;
                products[indice][2] = document.getElementById("inputTest" + a).value;
                products[indice][3] = $scope.ProductLicenses[a].ProductPrice;
                licenses += parseInt(document.getElementById("inputTest" + a).value);
                indice++;
            }
        }
        for (var a = 0; a < $scope.OneTime.length; a++) {
            var getQty = false;
            var Qtyy = 1;
            if ($scope.OneTime[a].MaximumSale > 0) {
                getQty = true;
                Qtyy = document.getElementById("qty" + $scope.OneTime[a].ProductId).innerHTML;
            }
            if (checkActive($scope.OneTime[a].ProductId) || (Qtyy > 0 && getQty)) {
                products[indice] = new Array(4);
                products[indice][0] = $scope.OneTime[a].ProductId;
                products[indice][1] = $scope.OneTime[a].ProductName;
                products[indice][2] = Qtyy;
                products[indice][3] = $scope.OneTime[a].ProductPrice;
                indice++;
            }
        }
        for (var a = 0; a < $scope.SupportOption.length; a++) {
            if (checkActive($scope.SupportOption[a].ProductId)) {
                products[indice] = new Array(4);
                products[indice][0] = $scope.SupportOption[a].ProductId;
                products[indice][1] = $scope.SupportOption[a].ProductName;
                products[indice][2] = 1;
                products[indice][3] = $scope.SupportOption[a].ProductPrice;
                indice++;
            }
        }
        for (var a = 0; a < $scope.SupportOption.length; a++) {
            if (parseInt($scope.SupportOption[a].MinimumSale) == 0 && checkActive($scope.SupportOption[a].ProductId) && licenses >= 0) {
                for (var b = 0; b < $scope.SupportUser.length; b++) {
                    if ($scope.SupportUser[b].RequiredProduct == $scope.SupportOption[a].ProductId && licenses >= 0) {
                        products[indice] = new Array(4);
                        products[indice][0] = $scope.SupportUser[b].ProductId;
                        products[indice][1] = $scope.SupportUser[b].ProductName;
                        products[indice][2] = licenses;
                        products[indice][3] = $scope.SupportUser[b].ProductPrice;
                    }
                }
            }
        }
        return products;
    }
    function checkActive(n) {
        try {
            var check = document.getElementById(n).className;
            var substring = " active";
            if (check.includes(substring)) {
                return true;
            } else {
                return false;
            }
        } catch (ex1) {
            return false;
        }
    }

    function changeActive(n, p) {
        try {
            var btn = document.getElementById(n);
            if (!checkActive(n)) {
                document.getElementById("totalResume" + n).innerHTML = parseFloat(p);
                document.getElementById("qty" + n).checked = true;
                btn.className += " active";
                btn.innerHTML = "Selected";
                $('#btn_chkRes' + n).prop('disabled', false);
                $('#btn_chkAdd' + n).prop('disabled', true);
                changeOneTimePrice(p, true);
            } else {
                resetActive(n, false, p);
            }
        } catch(ex){
            //console.log(n);
            //console.log(ex);
        }
    }

    function changeOneTimePrice(n, m) {
        n = parseFloat(n);
        var totalAux = parseFloat(document.getElementById("totalOneTime").innerHTML);
        if (m) {
            totalAux += n;
        }
        else {
            totalAux -= n;
        }
        document.getElementById("totalOneTime").innerHTML = totalAux;
        document.getElementById("td_packageTotal2").innerHTML = totalAux;
        document.getElementById("td_packageOneTimeTotal").innerHTML = totalAux;
    }

    function checkResetActive(id) {
        for (var a = 0; a < $scope.OneTime.length; a++) {
            if ($scope.OneTime[a].RequiredProduct == id) {
                if ($scope.OneTime[a].MaximumSale > 0) {
                    resetAdditional($scope.OneTime[a].ProductId, $scope.OneTime[a].MinimumSale, $scope.OneTime[a].MaximumSale, $scope.OneTime[a].ProductId);
                } else {
                    resetActive($scope.OneTime[a].ProductId, $scope.OneTime[a].Required, $scope.OneTime[a].ProductPrice);
                }
            }
        }
    }

    function getRequired(id) {
        var name = null;
        for (var a = 0; a < $scope.OneTime.length; a++) {
            if ($scope.OneTime[a].ProductId == id) {
                name = $scope.OneTime[a].ProductName;
            }
        }
        return name;
    }

    function httpGet(theUrl) {
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.open("GET", theUrl, false); // false for synchronous request
        xmlHttp.send(null);
        return xmlHttp.responseText;
    }

    function resetActive(n, m, p) {
        var btn = document.getElementById(n);
        if (checkActive(n)) {
            document.getElementById("totalResume" + n).innerHTML = 0;
            document.getElementById("qty" + n).checked = false;
            btn.className = btn.className.replace(" active", "");
            btn.innerHTML = "Select";
            $('#btn_chkRes' + n).prop('disabled', true);
            $('#btn_chkAdd' + n).prop('disabled', false);
            changeOneTimePrice(p, false);
            if (m) {
                checkResetActive(n);
            }
        }        
    }

    function updateTotalMonth() {
        var baseSupport = 0;//standard support
        var support = 0; //selected support
        var totalUsers = 0;
        var total = 0;

        for (var a = 0; a < $scope.ProductLicenses.length; a++) {
            totalUsers += parseFloat(document.getElementById("inputTest" + a).value);  // total users
            total += (parseFloat(document.getElementById("inputTest" + a).value) * parseFloat($scope.ProductLicenses[a].ProductPrice)); // total price of users
        }
        for (var a = 0; a < $scope.SupportOption.length; a++) {
            if (parseInt($scope.SupportOption[a].MinimumSale) > 0) {
                var n = $scope.SupportOption[a].ProductId;
                var btn = document.getElementById(n);
                if (checkActive(n)) baseSupport += parseFloat($scope.SupportOption[a].ProductPrice);
                if (totalUsers > 0) {
                    if (!checkActive(n)) {
                        btn.className += " active";
                        baseSupport += parseFloat($scope.SupportOption[a].ProductPrice); // standard support
                    }
                    btn.innerHTML = "Selected";
                    document.getElementById('lb_standardSuppRes').removeAttribute("hidden");
                } else {
                    document.getElementById('lb_standardSuppRes').setAttribute("hidden", true);
                }
            } else {
                if (checkActive($scope.SupportOption[a].ProductId)) {
                    for (var b = 0; b < $scope.SupportUser.length; b++) {
                        if ($scope.SupportUser[b].RequiredProduct == $scope.SupportOption[a].ProductId) {
                            support = (parseFloat($scope.SupportUser[b].ProductPrice) * totalUsers) + parseFloat($scope.SupportOption[a].ProductPrice); //support (with total users)
                        }
                    }
                }
            }
        }
        total = total + support + baseSupport; // total per month price totalResume
        if (isInt(total)) {
            document.getElementById('totalMonth').innerHTML = total;
            document.getElementById('td_packageTotal').innerHTML = total;
            document.getElementById('td_serviceTotal').innerHTML = total;
        } else {
            document.getElementById('totalMonth').innerHTML = total.toFixed(2);
            document.getElementById('td_packageTotal').innerHTML = total.toFixed(2);
            document.getElementById('td_serviceTotal').innerHTML = total.toFixed(2);
        }
        
    }

    $(document).ready(function () {
        $(window).keydown(function (event) {
            if (event.keyCode == 13) {
                event.preventDefault();
                return false;
            }
        });
    });

    $scope.optionPayment = function (n) {
        if (n == 'credit') {
            $scope.nextPrevNav(1, 0);
        } else {
            if (n == 'invoice') {
                $scope.OptionGenerateInvoice();
            } else {
                if (n == 'debit') {

                }
            }
        }
    };

    $scope.supportType = function (id, name, price, minimum) {
        if (minimum > 0) {
            $scope.chk_btnStandard(id, name, id, price);
        } else{
            $scope.AddUserTotal(id, name, price);
        }
    };

    function GenerateReportInvoice(id) {
        $(".my_Loader").show();
        var products =  getTotalDetailNames();
        var totalOneTime = 0;
        var totalMonth = 0;
        var paymentOption = "";
        if (checkActive('btn_standardPay')) {
            paymentOption = "Standard";
            totalOneTime += parseFloat(document.getElementById('td_packageOneTimeTotal').innerHTML);
            totalMonth += parseFloat(document.getElementById('td_serviceTotal').innerHTML);
        }
        if (checkActive('btn_AnnuityPay')) {
            paymentOption = "Annuity (10% surcharge)";
            totalOneTime += parseFloat(document.getElementById('txt_standarTermsPayMonth').innerHTML);
            totalMonth += parseFloat(document.getElementById('txt_standarTermsPayMonth').innerHTML);
        }
        if (checkActive('btn_PayUpFront')) {
            paymentOption = "Up Front (10% saving)";
            totalOneTime += parseFloat(document.getElementById('txt_standarTermsPayOneTime').innerHTML);
        }
        var curr = "";
        if (getUrlParameter("currency") === "USD") {
            curr = "$";
        }
        else {
            curr = "£";
        }
        var lastName = document.getElementById('lastName').value;
        var companyName = document.getElementById('companyName').value;
        var companyAddress = document.getElementById('address').value;
        var companyPostal = document.getElementById('postalcode').value;
        var Country = document.getElementById('SelectedCountry').innerHTML;
        var cpyState = document.getElementById('stateTest').value;
        var cpycity = document.getElementById('cityTest').value;
        var datatoPost = new Array();

        datatoPost[0] = paymentOption;
        datatoPost[1] = totalOneTime;
        datatoPost[2] = totalMonth;
        localStorage.clear();
        localStorage.setItem('products', products);
        localStorage.setItem('datatoReport', datatoPost);
        localStorage.setItem('curr', curr);
        localStorage.setItem('invoiceId', id);
        localStorage.setItem('lastName', lastName);
        localStorage.setItem('companyName', companyName);
        localStorage.setItem('companyAddress', companyAddress);

        localStorage.setItem('companyPostal', companyPostal);
        localStorage.setItem('country', Country);
        localStorage.setItem('companyState', cpyState);
        localStorage.setItem('city', cpycity);
        window.open("/InvoiceReport/InvoiceReport","_blank");
    }
    $scope.OptionGenerateInvoice = function () {
        var totalOneTime = 0;
        var totalMonth = 0;
        var paymentOption = "";
        if (checkActive('btn_standardPay')) {
            paymentOption = "Standard";
            totalOneTime += parseFloat(document.getElementById('td_packageOneTimeTotal').innerHTML);
            totalOneTime += parseFloat(document.getElementById('td_serviceTotal').innerHTML);
            totalMonth += parseFloat(document.getElementById('td_serviceTotal').innerHTML);
        }
        if (checkActive('btn_AnnuityPay')) {
            paymentOption = "Annuity";
            totalOneTime += parseFloat(document.getElementById('txt_standarTermsPayMonth').innerHTML);
            totalMonth += parseFloat(document.getElementById('txt_standarTermsPayMonth').innerHTML);
        }
        if (checkActive('btn_PayUpFront')) {
            paymentOption = "UpFront";
            totalOneTime += parseFloat(document.getElementById('txt_standarTermsPayOneTime').innerHTML);
        }
        var resume = getTotalDetail();
        var curr = "";
        if (getUrlParameter("currency") === "USD") {
            curr = getUrlParameter("currency");
        }
        else {
            curr = "GBP";
        }
        $(".my_Loader").show();
        var datatoPost = {
            CustomerId: CustomerId,
            TotalPayableAmount: totalOneTime,
            PaymentCardType: "", 
            NameOnCreditCard: document.getElementById('firstname').value,
            CreditCardNumber: "",
            CreditCardCVV: "",
            CreditCardExpiryMonth: "",
            CreditCardExpiryYear: "",
            currencyType: curr,
            Detail: resume,
            PaymentOption: paymentOption
        };
        $http.post('/Cowry/Common/GenerateInvoice', datatoPost).then(function (results) {
            $(".my_Loader").fadeOut("slow");
            if (results.status == 200) {
                Swal.fire({
                    type: 'success',
                    title: 'Invoice',
                    text: 'Thank you for contacting Cowry Solutions! Click OK to print the invoice.'
                }).then(function () {
                    GenerateReportInvoice(results.data);
                    location.reload();
                });
            }
            else {
                var msg = 'Something went wrong! Try again later. Error Code: ' + results.status;
                Swal.fire({
                    type: 'error',
                    title: 'Oops...',
                    text: msg
                });
            }
        }).catch(function (fallback) {
            $(".my_Loader").fadeOut("slow");
        });
    }

    $scope.paymentTerm = function (n) {
        document.getElementById('btn_standardPay').className = document.getElementById('btn_standardPay').className.replace(" active", "");
        document.getElementById('btn_standardPay').innerHTML = "Select";
        document.getElementById('btn_AnnuityPay').className = document.getElementById('btn_AnnuityPay').className.replace(" active", "");
        document.getElementById('btn_AnnuityPay').innerHTML = "Select";
        document.getElementById('btn_PayUpFront').className = document.getElementById('btn_PayUpFront').className.replace(" active", "");
        document.getElementById('btn_PayUpFront').innerHTML = "Select";

        var btn = document.getElementById(n);
        btn.className += " active";
        btn.innerHTML = "Selected";
    };

    $scope.AddUserTotal = function (ProductId,Name,Price) {
        var btn = document.getElementById(ProductId);
        if (checkActive(ProductId)) {
            btn.className = btn.className.replace(" active", "");
            ga('send', 'event', Name, 'Item deselected', 'Price: ' + Price);
            btn.innerHTML = "Select";
            $('#btn_chkRes' + ProductId).prop('disabled', true);
            $('#btn_chkAdd' + ProductId).prop('disabled', false);
        } else {
            var properties = {
                Currency: $scope.getSymbol(),
                ProductID: ProductId,
                Price: Price,
                Amount: 1
            };
            ApplicationInsights.trackApplicationInsight(Name, properties);
            ga('send', 'event', Name, 'Item selected', 'Price: ' + price);
            btn.className += " active";
            btn.innerHTML = "Selected";
            $('#btn_chkRes' + ProductId).prop('disabled', false);
            $('#btn_chkAdd' + ProductId).prop('disabled', true);
        }
        updateTotalMonth();
    };

    $scope.getPowerApps = function () {
        var totalUsers = 0;
        try {
            for (var a = 0; a < $scope.ProductLicenses.length; a++) {
                totalUsers += parseInt(document.getElementById("inputTest" + a).value);  // total users
            }
            document.getElementById('showPower').innerHTML = totalUsers;
        } catch (ex2) {
            //console.log(ex2);
        }
    };

    $scope.getPriceSup = function (id) {
        var price = parseFloat(document.getElementById("Support" + id).innerHTML);
        document.getElementById("resumeSupPrice" + id).innerHTML = price;
    };

    $scope.chkQty = function (id) {
        var qty = 0;
        if (checkActive(id)) {
            qty = 1;
        }
        if (qty == 0) {
            document.getElementById("txt_" + id).style.fontStyle = "italic";
            document.getElementById("txt_" + id).style.color = "#808080";
        } else {
            document.getElementById("txt_" + id).style.fontStyle = "normal";
            document.getElementById("txt_" + id).style.color = "#365DA7";
        }
        document.getElementById('qty' + id).innerHTML = qty;
        document.getElementById("qty" + id).checked = qty;
    };

    $scope.getQty = function (id) {
        var total = 0;
        for (var a = 0; a < $scope.ProductLicenses.length; a++) {
            if ($scope.ProductLicenses[a].ProductId === id) {
                total = parseInt(document.getElementById("inputTest" + a).value);
            }
        }
        try {
            if (total == 0) {
                document.getElementById("txt_" + id).style.fontStyle = "italic";
                document.getElementById("txt_" + id).style.color = "#808080";
            } else {
                document.getElementById("txt_" + id).style.fontStyle = "normal";
                document.getElementById("txt_" + id).style.color = "#365DA7";
            }
            document.getElementById("qty" + id).innerHTML = total;
            document.getElementById("showFull" + id).innerHTML = total;
        } catch (err2) {
            //console.log(err2);
        }
    };

    $scope.getQtyAdd = function (id) {
        var total = 0;
        var price = 0;
        var max = false;
        for (var a = 0; a < $scope.OneTime.length; a++) {
            if ($scope.OneTime[a].ProductId === id && $scope.OneTime[a].MaximumSale>0) {
                total = parseInt(document.getElementById("inputTestAddComp" + $scope.OneTime[a].ProductId).value);
                price = $scope.OneTime[a].ProductPrice;
                max = true;
            }
        }
        if (total == 0) {
            document.getElementById("txt_" + id).style.fontStyle = "italic";
            document.getElementById("txt_" + id).style.color = "#808080";
        } else {
            document.getElementById("txt_" + id).style.fontStyle = "normal";
            document.getElementById("txt_" + id).style.color = "#365DA7";
        }
        document.getElementById("qty" + id).innerHTML = total;
        $scope.getTotal(price, id, 1,max);
    };

    $scope.getTotal = function (price, id, p,max) {
        var total = 0;
        pri = 0;
        var qty;
        try {
            var element = document.getElementById("qty" + id);

            qty = element.innerHTML;
            if (qty == "") {
                qty = document.getElementById("qty" + id).checked;
            }

            if (p === 1) {
                pri = parseFloat(price);
            } else {
                pri = parseFloat(document.getElementById("resumeSupPrice" + parseInt(id)).innerHTML);
            }
            total = qty * pri;
            if (max) {
                if (!isInt(total)) {
                    document.getElementById("totalResume" + id).innerHTML = total.toFixed(2);
                } else {
                    document.getElementById("totalResume" + id).innerHTML = total;
                }
            }

        } catch (ex) {
            //console.log(ex);
        }        
    };

    function isInt(n) {
        return n % 1 === 0;
    }

    $scope.changeFloat = function (price) {
        var value = parseFloat(price);
        return value;
    };

    $scope.getSymbol = function () {
        if (getUrlParameter("currency") === "USD") {
            $scope.currencySymbol = "$";
        }
        else {
            $scope.currencySymbol = "£";
        }
        var value = $scope.currencySymbol;
        return value;
    };

    $scope.changeQty = function (index, price, type, MinimumSale, MaximumSale, Name, ProductId) { // Adds users
        var minimum = parseFloat(MinimumSale);
        var maximum = parseFloat(MaximumSale);
        var total = document.getElementById('inputTest' + index).value;
        var prices = parseFloat(price);
        
        if (type === '+') {
            total++;
            updateTotalMonth();
        }
        else {
            total--;
            updateTotalMonth();
        }
        var properties = {
            Currency: $scope.getSymbol(),
            ProductID: ProductId,
            Price: price,
            Amount: total
        };
        ApplicationInsights.trackApplicationInsight(Name, properties);
        ga('send', 'event', Name, 'Amount: ' + total, 'Price: ' + price);
        $('#btn_plus' + index).prop('disabled', (total >= maximum));
        $('#btn_minus' + index).prop('disabled', (total <= minimum));
        $('#btn_plusRes' + index).prop('disabled', (total >= maximum));
        $('#btn_plusRess' + index).prop('disabled', (total >= maximum));
        $('#btn_minusRes' + index).prop('disabled', (total <= minimum));
        $('#btn_minusRess' + index).prop('disabled', (total <= minimum));
        document.getElementById('inputTest' + index).value = total;
    };

    function resetAdditional(index, MinimumSale, MaximumSale, id) {
        var total = parseFloat(document.getElementById('totalResume' + id).innerHTML); 
        var minimum = parseFloat(MinimumSale);
        var maximum = parseFloat(MaximumSale);
        changeOneTimePrice(total, false);
        total = 0;
        $('#btn_plusAddComp' + id).prop('disabled', (total >= maximum));
        $('#btn_minusAddComp' + id).prop('disabled', (total <= minimum));
        $('#btn_plusResAddComp' + id).prop('disabled', (total >= maximum));
        $('#btn_minusResAddComp' + id).prop('disabled', (total <= minimum));
        document.getElementById('inputTestAddComp' + id).value = 0;
    };

    $scope.chk_btnStandard = function (n, Name, ProductId,price) {
        var totalUsers = 0;
        var btn = document.getElementById(n);
        for (var a = 0; a < $scope.ProductLicenses.length; a++) {
            totalUsers += parseInt(document.getElementById("inputTest" + a).value);
        }
        if (totalUsers == 0) {
            if (!checkActive(n)) {
                var properties = {
                    Currency: $scope.getSymbol(),
                    ProductID: ProductId,
                    Price: price,
                    Amount: 1
                };
                ApplicationInsights.trackApplicationInsight(Name, properties);
                ga('send', 'event', Name, 'Item selected', 'Price: ' + price);
                console.log(Name);
                console.log(properties);
                btn.className += " active";
                btn.innerHTML = "Selected";
                document.getElementById("qty" + n).checked = true;
            } else {
                ga('send', 'event', Name, 'Item deselected', 'Price: ' + price);
                btn.className = btn.className.replace(" active", "");
                btn.innerHTML = "Select"; // change btn of standard support
                document.getElementById("qty" + n).checked = false;
            }
        }
    };

    $scope.nextPrev = function (n, m) {
        if (m == 0) {
            var navLength = $scope.TestArray.length + 1;
            var x = document.getElementsByClassName("tab");
            x[currentTab].style.display = "none";
            currentTab = currentTab + n;
            if (currentTab == 0 || (currentTab == 1 && currentConf == 1)) navFix(0);
            if (currentTab == 2 && currentConf == 0) navFix(1);
            if (currentTab == navLength && currentConf == 2) navFix(1);
            if (currentTab == (navLength+1) && currentConf == 1) navFix(2);
            if (currentTab == (navLength + 1) && currentConf == 3) navFix(2);
            if (currentTab == (navLength + 2) && currentConf == 2) navFix(3);
            if (currentTab >= x.length) {
                currentTab = 0;
                fixStepIndicator(currentTab);
            }
        } else {
            var x = document.getElementsByClassName("tab");
            x[currentTab].style.display = "none";
            currentTab = n;
            fixStepIndicator(currentTab)
        }
        showTab(currentTab, (navLength+7));
    };

    $scope.CheckTotalPay = function () {
        var month = parseFloat(document.getElementById("td_serviceTotal").innerHTML);
        var annuity = parseFloat(document.getElementById("td_packageOneTimeTotal").innerHTML);
        if (month == 0 && annuity == 0) {
            Swal.fire({
                type: 'error',
                text: 'You must select an option to continue.'
            });
        } else {
            $scope.nextPrevNav(1, 0);
        }
    }

    $scope.nextPrevNav= function (n, m) {//3-1
        var navLength = $scope.TestArray.length + 2;
        var x = document.getElementsByClassName("tab");
        var xy = document.getElementsByClassName("stepN");
        x[currentTab].style.display = "none";
        if (currentTab != 0 || m != 0) {
            if (m == 0) {
                if (currentConf < xy.length - 1) {
                    currentConf = currentConf + n;
                }
            } else {
                currentConf = n;//3
            }
            if (currentConf == 0) {
                currentTab = 1;
            } else {
                if (currentConf == 2) {
                    currentTab = navLength;
                } else {
                    if (currentConf == 3) {
                        if (m != 1 && m != 2) {
                            setPaymentOption();
                            if (currentTab < (navLength+1)) {
                                currentTab = (navLength+1);
                            } else {
                                currentTab += n;
                                if (currentTab < (navLength+1)) {
                                    currentConf = 2;
                                }
                            }
                        } else {
                            currentTab = (navLength+1);
                        }
                    } else {
                        currentTab = 2;
                    }
                }
            }
        } else {
            currentTab = 1;
        }
        if (currentTab >= x.length) {
            currentTab = 0;
            fixStepIndicator(currentTab);
        }
        navFix(currentConf);
        showTab(currentTab,(navLength+6));
    };

    $scope.changeQtyAdd = function (index, price, type, MinimumSale, MaximumSale, id, RequiredProduct, Required,Name) {
        var bool = false;
        var reqBool = false;
        if (RequiredProduct != null) {
            reqBool = true;
            if (checkActive(RequiredProduct)) {
                bool = true;
            }
        } else {
            bool = true;
        }
        if (bool) {
            var minimum = parseFloat(MinimumSale);
            var maximum = parseFloat(MaximumSale);
            var total = document.getElementById('inputTestAddComp' + id).value;
            var prices = parseFloat(price);
            if (type === '+') {
                total++;
                var properties = {
                    Currency: $scope.getSymbol(),
                    ProductID: id,
                    Price: price,
                    Amount:total
                };
                ApplicationInsights.trackApplicationInsight(Name, properties);
                console.log(Name);
                console.log(properties);
                changeOneTimePrice(prices, true);
            }
            else {
                total--;
                changeOneTimePrice(prices, false);
            }
            ga('send', 'event', Name, 'Amount: ' + total, 'Price: ' + price);
            $('#btn_plusAddComp' + id).prop('disabled', (total >= maximum));
            $('#btn_minusAddComp' + id).prop('disabled', (total <= minimum));
            $('#btn_plusResAddComp' + id).prop('disabled', (total >= maximum));
            $('#btn_minusResAddComp' + id).prop('disabled', (total <= minimum));
            document.getElementById('inputTestAddComp' + id).value = total;
        }
        if (reqBool && !bool) {
            var reqName = getRequired(RequiredProduct);
            var name = getRequired(id);
            Swal.fire({
                type: 'warning',
                text: reqName + ' is a prerequisite for the ' + name + ' package.'
            });
        }
    };

    $scope.AddSupportUser = function (n, m, z) { // Adds base price to the total support, depending on total users
        var totalUsers = 0;

        for (var a = 0; a < $scope.ProductLicenses.length; a++) {
            totalUsers += parseInt(document.getElementById("inputTest" + a).value);
        }
        var priceN = parseFloat(n);
        var priceM = parseFloat(m);
        var total = priceN + (priceM * totalUsers);
        document.getElementById("Support" + z).innerHTML = total;
        updateTotalMonth();
    };

    $scope.supportAdd = function (id) { // Adds support price to the Month amount
        var price = parseFloat(document.getElementById("Support" + id).innerHTML);
        var totalAux = parseFloat(document.getElementById("totalMonth").innerHTML);
        if (checkActive("Support" + id)) {
            totalAux -= price;
        } else {
            totalAux += price;
        }

        document.getElementById("totalMonth").innerHTML = totalAux;
    };

    $scope.packagesAdd = function (id, price, RequiredProduct, Required, Name) {
        var bool = false;
        var reqBool = false;
        if (RequiredProduct != null) {
            reqBool = true;
            if (checkActive(RequiredProduct)) {
                bool = true;
            }
        } else {
            bool = true;
        }
        if (Required && checkActive(id)) {
            checkResetActive(id);
        }
        if (bool) {
            changeActive(id, price);
        }
        if (bool && checkActive(id)) {
            var properties = {
                Currency: $scope.getSymbol(),
                ProductID: id,
                Price: price,
                Amount: 1
            };
            ApplicationInsights.trackApplicationInsight(Name, properties);
            ga('send', 'event', Name, 'Item selected', 'Price: ' + price);
        }
        if (reqBool && !bool) {
            document.getElementById("totalResume" + id).innerHTML = 0;
            var reqName = getRequired(RequiredProduct);
            document.getElementById("qty" + id).checked = false;
            var name = getRequired(id);
            Swal.fire({
                type: 'warning',
                text: reqName + ' is a prerequisite for the ' + name + ' package.'
            });
        }
    };

    

    $scope.cowryGBP = function () {
        $(".my_Loader").show();
        var t0 = performance.now();
        if (getUrlParameter("currency") !== null) {
            $scope.currencyType = getUrlParameter("currency");
        }
        else {
            $scope.currencyType = "GBP";
        }

        $http.get('/Cowry/Common/GetCRMPrice?CurrencyType=' + $scope.currencyType).then(function (results) {
            if (results.status === 200 && results.data !== null) {
                for (ii = 0; ii < results.data.length; ii++) {

                    if (parseInt(results.data[ii].ProductType) === 847200000) {// Microsoft License 
                        $scope.ProductLicenses.push(results.data[ii]);
                    }else if (parseInt(results.data[ii].ProductType) === 847200002) {// One Time Product License, Financials 
                        $scope.OneTime.push(results.data[ii]);
                    } else if (parseInt(results.data[ii].ProductType) === 847200001 && results.data[ii].Subject === "Support Option") {// Support Option
                        $scope.SupportOption.push(results.data[ii]);
                    } else if (parseInt(results.data[ii].ProductType) === 847200001 && results.data[ii].Subject === "Support User") {// Support User
                        $scope.SupportUser.push(results.data[ii]);
                    }
                    //if (parseInt(results.data[ii].ProductType) === 847200002 && results.data[ii].Subject === "Human Resources") {// Support User
                      //  $scope.OneTimeHumanResources.push(results.data[ii]);
                    //}
                }
                var arrayIndex = 0;
                var subjectArray = 0;
                $scope.OneTime.sort(function (a, b) { return a.OrderPosition - b.OrderPosition });
                for (var type = 0; type < $scope.OneTime.length; type++) {
                    var exist = true;
                    var more = false;
                    for (var subj = 0; subj < $scope.SubjectType.length; subj++) {
                        if ($scope.OneTime[type].Subject == $scope.SubjectType[subj][0]) {
                            $scope.SubjectType[subj][1]++;
                            for (var amt = 0; amt < $scope.TestArray.length; amt++) {
                                if ($scope.TestArray[amt][0] == $scope.OneTime[type].Subject) {
                                    if ($scope.TestArray[amt][1] < 3) {
                                        $scope.TestArray[amt][1]++;
                                        $scope.TestArray[amt][3] = parseInt($scope.OneTime[type].ProductId);
                                        more = false;
                                    } else {
                                        more = true;
                                    }
                                }
                            }
                            if (more) {
                                $scope.TestArray[arrayIndex] = new Array(5);
                                $scope.TestArray[arrayIndex][0] = $scope.OneTime[type].Subject;
                                $scope.TestArray[arrayIndex][1] = 1;
                                $scope.TestArray[arrayIndex][2] = parseInt($scope.OneTime[type].ProductId);
                                $scope.TestArray[arrayIndex][3] = parseInt($scope.OneTime[type].ProductId);
                                $scope.TestArray[arrayIndex][4] = true;
                                arrayIndex++;
                            }
                            exist = false;
                        }
                    }
                    if (exist) {
                        $scope.SubjectType[subjectArray] = new Array(2);
                        $scope.SubjectType[subjectArray][0] = $scope.OneTime[type].Subject;
                        $scope.SubjectType[subjectArray][1] = 1;

                        $scope.TestArray[arrayIndex] = new Array(5);
                        $scope.TestArray[arrayIndex][0] = $scope.OneTime[type].Subject;
                        $scope.TestArray[arrayIndex][1] = 1;
                        $scope.TestArray[arrayIndex][2] = parseInt($scope.OneTime[type].ProductId);
                        $scope.TestArray[arrayIndex][3] = parseInt($scope.OneTime[type].ProductId);
                        $scope.TestArray[arrayIndex][4] = false;
                        arrayIndex++;
                        subjectArray++;
                    }
                }
            }
            $(".my_Loader").fadeOut("slow");
        }).catch(function (fallback) {
            alert(fallback.statusText);
        });
    };

    $scope.createLead = function (userData) {
        var isvalidate = true;
        var isvalidate1 = true;
        var selectIndex = 0;
        var text = 'In order to proceed you need to complete all required information!';
        var form = $("#myForm");
        if (userData != undefined) {
            try {
                var companyCountry = document.getElementById('countryId');
                var Country = companyCountry.options[companyCountry.selectedIndex].innerHTML;
                var CountryValue = companyCountry.options[companyCountry.selectedIndex].value;

                userData.CompanyCountry = Country;
                userData.CompanyState = document.getElementById('stateTest').value;
                userData.CompanyCity = document.getElementById('cityTest').value;

                document.getElementById('SelectedCountry').innerHTML = Country;
                document.getElementById('SelectedCountryValue').innerHTML = CountryValue;
                selectIndex = companyCountry.selectedIndex;
                if (companyCountry.length > 3) selectIndex--;

            } catch (ex) {
                //console.log(ex);
                isvalidate = false;
            }
        }
        if (userData == undefined) {
            isvalidate = false;
        }
        else if (userData.FirstName == null || userData.FirstName == undefined || userData.FirstName == "") {
            isvalidate = false;
        }
        else if (userData.LastName == null || userData.LastName == undefined || userData.LastName == "") {
            isvalidate = false;
        }
        else if (userData.JobTitle == null || userData.JobTitle == undefined || userData.JobTitle == "") {
            isvalidate = false;
        }
        else if (userData.ContactNumber == null || userData.ContactNumber == undefined || userData.ContactNumber == "") {
            isvalidate = false;
        }
        else if (userData.Email == null || userData.Email == undefined || userData.Email == "") {
            isvalidate = false;
        }
        else if (userData.ConfirmEmail == null || userData.ConfirmEmail == undefined || userData.ConfirmEmail == "") {
            isvalidate = false;
        }
        else if (userData.CompanyName == null || userData.CompanyName == undefined || userData.CompanyName == "") {
            isvalidate = false;
        }
        else if (userData.CompanyNumber == null || userData.CompanyNumber == undefined || userData.CompanyNumber == "") {
            isvalidate = false;
        }
        else if (userData.CompanyAddress == null || userData.CompanyAddress == undefined || userData.CompanyAddress == "") {
            isvalidate = false;
        }
        else if (userData.CompanyCountry == null || userData.CompanyCountry == undefined || userData.CompanyCountry == "") {
            isvalidate = false;
        }
        else if (userData.CompanyState == null || userData.CompanyState == undefined || userData.CompanyState == "") {
            isvalidate = false;
        }
        else if (userData.CompanyCity == null || userData.CompanyCity == undefined || userData.CompanyCity == "") {
            isvalidate = false;
        }
        else if (document.getElementById('city').value.replace(/ /g, '').toUpperCase() != userData.CompanyCity.replace(/ /g, '').toUpperCase()) {
            text = 'The postal code do not match with the city.';
            var ct = document.getElementById('city');
            ct.style.borderColor = "Red";
            isvalidate = false;
        }
        else if (userData.CompanyPostalCode == null || userData.CompanyPostalCode == undefined || userData.CompanyPostalCode == "") {
            isvalidate = false;
        }
        else if (($("#microsoftAccount").is(':checked') && !$("#findDomain").is(':checked'))) {
            if (userData.DomainName == null || userData.DomainName == undefined || userData.DomainName == "") {
                isvalidate = false;
            }
        }
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (isvalidate) {
            if (!regex.test(userData.Email)) {
                isvalidate = false;
                text = 'Invalid Email.';
            }
            if (userData.Email != userData.ConfirmEmail && isvalidate) {
                text = 'Email does not match.';
                isvalidate = false;
            }
            var regex = /((^\(?(?:(?:0(?:0|11)\)?[\s-]?\(?|\+)44\)?[\s-]?\(?(?:0\)?[\s-]?\(?)?|0)(?:\d{2}\)?[\s-]?\d{4}[\s-]?\d{4}|\d{3}\)?[\s-]?\d{3}[\s-]?\d{3,4}|\d{4}\)?[\s-]?(?:\d{5}|\d{3}[\s-]?\d{3})|\d{5}\)?[\s-]?\d{4,5}|8(?:00[\s-]?11[\s-]?11|45[\s-]?46[\s-]?4\d))(?:(?:[\s-]?(?:x|ext\.?\s?|\#)\d+)?)$)|(\(?[2-9][0-8][0-9]\)?[-. ]?[0-9]{3}[-. ]?[0-9]{4}))/;
            if (!regex.test(userData.ContactNumber)) {
                isvalidate = false;
                text = 'Invalid Contact Number.';
            }
        }
        
        if (($("#microsoftCloudAggrement").is(':checked') == false || $("#supportContract").is(':checked') == false)&&isvalidate) {
            isvalidate = false;
            var btn = document.getElementById("mcaContractVal");
            var btn2 = document.getElementById("supportContractVal");
            if (!$("#microsoftCloudAggrement").is(':checked')) {
                btn.removeAttribute("hidden");
            } else {
                btn.style.visibility = "hidden";
            }
            if (!$("#supportContract").is(':checked')) {
                btn2.removeAttribute("hidden");
            } else {
                btn2.style.visibility = "hidden";
            }
            text = 'In order to proceed you need to agree to the Microsoft Cloud Agreement and Our Support Contract';
        }
        if (isvalidate) {
            $scope.saveUserData(userData);
            $scope.nextPrevNav(1, 0);
        }
        else {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: text
            }).then(function () {
                document.getElementById('countryId').selectedIndex = selectIndex+1;
            });
            if (isvalidate1) {
                event.preventDefault();
                event.stopPropagation();
                form.addClass('was-validated');
            }
        }
    }

    $scope.saveUserData = function (userData) {

        $(".my_Loader").show();
        var totEssen = $("#totalEssential").html();
        var totTeams = $("#totalTeam").html();
        var totUsers = $("#summaryPremiumUsers").html();
        
        var datatoPost = {
            //PersonalInformation
            CustomerFirstname: userData.FirstName,
            CustomerLastname: userData.LastName,
            CustomerJobTitle: userData.JobTitle,
            CustomerContactNumber: userData.ContactNumber,
            CustomerEmailAddress: userData.Email,

            //Company Information
            CustomerCompanyName: userData.CompanyName,
            CustomerCompanyContact: userData.CompanyNumber,
            CustomerCompanyAddress: userData.CompanyAddress,
            CustomerCompanyCountry: userData.CompanyCountry,
            CustomerCompanyState: userData.CompanyState,
            CustomerCompanyCity: userData.CompanyCity,
            CustomerCompanyPostalCode: userData.CompanyPostalCode,

            //Microsoft Account Information
            Has_MicrosoftOffice365Account: $("#microsoftCloudAggrement").is(':checked'),
            DomainName: $("#domainName").val() !== null ? $("#domainName").val() : "",
            Find_DomainName: $("#findDomain").is(':checked'),

            //Authorization
            Accept_MicrosoftCloudAgreement: $("#microsoftCloudAggrement").is(':checked'),
            Accept_SupportContract: $("#supportContract").is(':checked')
        };

        $http.post('/Cowry/Common/AddUserData', datatoPost).then(function (results) {
            if (results.status === 200) {
                //Nothing;
                CustomerId = results.data;
                localStorage.setItem("CustomerId", results.data);
                var first = $("#signupmonthly").html();
                var second = $("#signuponeTime").html();

                var finalAmount = parseFloat(first) + parseFloat(second);
                $(".totalAmount").html(finalAmount);
            }
            $(".my_Loader").fadeOut("slow");
        }).catch(function (fallback) {
            alert(fallback.statusText);
        });
    };

    $scope.checkPaymentInfo = function (paymentInfo, userData) {
        var isvalidate = true;

        paymentInfo.finalAmount
        if (paymentInfo == undefined) {
            isvalidate = false;
        }
        else if (paymentInfo.CardType == null || paymentInfo.CardType == undefined || paymentInfo.CardType == "") {
            isvalidate = false;
        }
        else if (paymentInfo.NameOnCard == null || paymentInfo.NameOnCard == undefined || paymentInfo.NameOnCard == "") {
            isvalidate = false;
        }
        else if (paymentInfo.CardNumber == null || paymentInfo.CardNumber == undefined || paymentInfo.CardNumber == "") {
            isvalidate = false;
        }
        else if (paymentInfo.CardCVV == null || paymentInfo.CardCVV == undefined || paymentInfo.CardCVV == "") {
            isvalidate = false;
        }
        else if (paymentInfo.CardExpiryMonth == null || paymentInfo.CardExpiryMonth == undefined || paymentInfo.CardExpiryMonth == "") {
            isvalidate = false;
        }
        else if (paymentInfo.CardExpiryYear == null || paymentInfo.CardExpiryYear == undefined || paymentInfo.CardExpiryYear == "") {
            isvalidate = false;
        }
        if (isvalidate == true) {
            $scope.savePaymentInfo(paymentInfo, userData);
        }
        else {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Please enter all the required details to continue!',
                footer: '<a href>Why do I have this issue?</a>'
            })
        }
    };

    $scope.savePaymentInfo = function (paymentInfo, userData) {
        var totalOneTime = 0;
        var totalMonth = 0;
        var paymentOption = "";
        if (checkActive('btn_standardPay')) {
            paymentOption = "Standard";
            totalOneTime += parseFloat(document.getElementById('td_packageOneTimeTotal').innerHTML);
            totalOneTime += parseFloat(document.getElementById('td_serviceTotal').innerHTML);
            totalMonth += parseFloat(document.getElementById('td_serviceTotal').innerHTML);
        }
        if (checkActive('btn_AnnuityPay')) {
            paymentOption = "Annuity";
            totalOneTime += parseFloat(document.getElementById('txt_standarTermsPayMonth').innerHTML);
            totalMonth += parseFloat(document.getElementById('txt_standarTermsPayMonth').innerHTML);
        }
        if (checkActive('btn_PayUpFront')) {
            paymentOption = "UpFront";
            totalOneTime += parseFloat(document.getElementById('txt_standarTermsPayOneTime').innerHTML);
        }
        var resume = getTotalDetail();
        var curr = "";
        if (getUrlParameter("currency") === "USD") {
            curr = getUrlParameter("currency");
        }
        else {
            curr = "GBP";
        }
        $(".my_Loader").show();
        var datatoPost = {
            CustomerId: CustomerId,
            TotalPayableAmount: totalOneTime,
            PaymentCardType: paymentInfo.CardType,
            NameOnCreditCard: paymentInfo.NameOnCard,
            CreditCardNumber: paymentInfo.CardNumber,
            CreditCardCVV: paymentInfo.CardCVV,
            CreditCardExpiryMonth: paymentInfo.CardExpiryMonth,
            CreditCardExpiryYear: paymentInfo.CardExpiryYear,
            currencyType: curr,
            Detail: resume,
            PaymentOption: paymentOption
        };
        ga('send', 'event', 'Credit Payment', 'Payment Option: ' + paymentOption, 'Total: ' + TotalPayableAmount);
        $http.post('/Cowry/Common/UpdatePaymentInfo', datatoPost).then(function (results) {
            $(".my_Loader").fadeOut("slow");
            if (results.status == 200) {
                crayon(userData);
                $scope.nextPrev(1, 0);
            }
            else if (results.status == 202) {
                Swal.fire({
                    type: 'error',
                    title: 'Oops...',
                    text: 'Invalid Card Number!'
                })
            }
        }).catch(function (fallback) {
            $(".my_Loader").fadeOut("slow");
        });
    };
    function crayon(userData) {
        var products = new Array();
        var indice = 0;
        for (var a = 0; a < $scope.ProductLicenses.length; a++) {
            if (parseInt(document.getElementById("inputTest" + a).value) > 0) {
                products[indice] = new Array(2);
                products[indice][0] = $scope.ProductLicenses[a].ProductId;
                products[indice][1] = document.getElementById("inputTest" + a).value;
                indice++;
            }
        }
        var findDomain = document.getElementById('findDomain').value;
        var Domain = document.getElementById('domainName').value;
        var hasDomain = document.getElementById('microsoftAccount').checked;
        if (indice > 0 && (findDomain=="") /*&& (!hasDomain)*/) {
            var cpyCountry = document.getElementById('SelectedCountry');

            var cpyState = document.getElementById('stateTest');
            var cpycity = document.getElementById('cityTest');

            var essentialUsers = 0;
            var teamUsers = 0;
            for (var i = 0; i < indice; i++) {
                if (products[i][0] == 2) {
                    essentialUsers = products[i][1];
                }
                if (products[i][0] == 3) {
                    teamUsers = products[i][1];
                }
            }
            if (!hasDomain) {
                userData.DomainName = "";
            }
            var datatoPost = {
                //PersonalInformation
                CustomerFirstname: userData.FirstName,
                CustomerLastname: userData.LastName,
                CustomerJobTitle: userData.JobTitle,
                CustomerContactNumber: userData.ContactNumber,
                CustomerEmailAddress: userData.Email,

                //Company Information
                CustomerCompanyName: userData.CompanyName,
                CustomerCompanyContact: userData.CompanyNumber,
                CustomerCompanyAddress: userData.CompanyAddress,
                CustomerCompanyCountry: cpyCountry.innerHTML,
                CustomerCompanyState: cpyState.value,
                CustomerCompanyCity: cpycity.value,
                CustomerCompanyPostalCode: userData.CompanyPostalCode,

                //Microsoft Account Information
                DomainName: userData.DomainName,
                Has_MicrosoftOffice365Account: $("#microsoftCloudAggrement").is(':checked'),

                //Users Licenses
                TotalEssentialUsers: essentialUsers,
                TotalTeamUsers: teamUsers
            };
            $http.post('/Cowry/Common/GenerateCrayon', datatoPost); 
        }

    }
    
    $scope.SetValid = function (data) {
        if (data === 'Amex') {
            $('#cc_cardname').removeAttr("disabled");
            $('#cc_cardcvv').removeAttr("disabled");
            $('#cc_cardnumber').removeAttr("disabled");
            $("#cc_cardcvv").attr({ "maxlength": 4 });
            $("#cc_cardnumber").attr({ "maxlength": 15 });
        }
        else {
            $('#cc_cardname').removeAttr("disabled");
            $('#cc_cardcvv').removeAttr("disabled");
            $('#cc_cardnumber').removeAttr("disabled");
            $("#cc_cardcvv").attr({ "maxlength": 3 });
            $("#cc_cardnumber").attr({ "maxlength": 16 });
        }
    };
}]);