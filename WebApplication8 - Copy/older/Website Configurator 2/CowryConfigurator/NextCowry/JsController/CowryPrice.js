var app = angular.module('CowryUSDPrice', []);
app.controller('CowryUSDPriceCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.price = [];
    var currencyType = localStorage.getItem("CurrencyType");
    var SpId = localStorage.getItem("SpId");

    var premiumPrice = 0;
    var essentialPrice = 0;
    var teamPrice = 0;
    var standardSupport = 0;
    var enhancedSupport = 0;
    var enhanbase = 0;
    var enhanuser = 0;
    var monthlyPrice = "0";
    var yearlyPrice = "0";
    var essential = "";
    var premium = "";
    var gst = "0.9";
    var supportTotal = 0;
    var finalTotal = 0;
    var tableTotal = 0;
    var finalMonthly = 0;
    var finalYearly = 0;

    var puser = "0";
    var euser = "0";
    var tuser = "0";
    var totalUsers = "0";

    var finalPremiumUsers = "0";
    var finalEssentialUsers = "0";
    var finalTeamUsers = "0";
    var finalTotalUsers = "0";
    var finalStandardSupportPrice = "0";
    var finalEnhancedSupportPrice = "0";    
    var finalEnhancedSupportChecked;
    var finalPaymentOptionAns = "0"
    var finalTotalPriceMonthly = "0";
    var finalTotalPriceYearly = "0";

    $scope.TotalPrice = "0";

    $scope.users = [];
    $scope.users.premiumUsers = "0";
    $scope.users.essentialUsers = "0";
    $scope.users.teamUsers = "0";

    $scope.cowryUSDPriceData = function () {
       // debugger;
        $(".my_Loader").show();
        $http.get('/cowry/result/GetPriceDetailsPerCurrency?CurrencyType=' + currencyType).then(function (results) {
            //debugger;
            if (results.status == 200) {
              //  debugger;
                if (currencyType == "USD") {
                    $("#premiumprice").html("$ " + results.data[0].ProductPrice);
                    premiumPrice = parseInt(results.data[0].ProductPrice);

                    $("#essentialprice").html("$ " + results.data[1].ProductPrice);
                    essentialPrice = parseInt(results.data[1].ProductPrice);

                    $("#teamprice").html("$ " + results.data[2].ProductPrice);
                    teamPrice = parseInt(results.data[2].ProductPrice);

                    $("#standardprice").html("$ " + results.data[3].ProductPrice);

                    standardSupport = parseInt(results.data[3].ProductPrice);
                    enhanbase = parseInt(results.data[4].ProductPrice);
                    enhanuser = parseInt(results.data[5].ProductPrice);

                    $("#monthly1").html("$ --");

                    $("#yearly1").html("$ --");
                }
                if (currencyType == "GBP") {
                    $("#premiumprice").html("£ " + results.data[0].ProductPrice);
                    premiumPrice = parseInt(results.data[0].ProductPrice);

                    $("#essentialprice").html("£ " + results.data[1].ProductPrice);
                    essentialPrice = parseInt(results.data[1].ProductPrice);

                    $("#teamprice").html("£ " + results.data[2].ProductPrice);
                    teamPrice = parseInt(results.data[2].ProductPrice);

                    $("#standardprice").html("£ " + results.data[3].ProductPrice);

                    standardSupport = parseInt(results.data[3].ProductPrice);
                    enhanbase = parseInt(results.data[4].ProductPrice);
                    enhanuser = parseInt(results.data[5].ProductPrice);

                    $("#monthly1").html("£ --");

                    $("#yearly1").html("£ --");
                }
            }
            $(".my_Loader").fadeOut("slow");
        }).catch(function (fallback) {
            debugger;
            alert(fallback.statusText);
        });
    }

    $scope.multiplyUsers1 = function (userCount, inputId) {
  //      debugger       
        if (userCount == null || userCount == undefined || userCount == "") {
            $("#premiumUsers").removeAttr('disabled');
            $("#premiumUsers").val("");
            $("#essentialUsers").removeAttr('disabled');
            $("#essentialUsers").val("");
            if (currencyType == "USD")
            {
                $("#monthly1").html("$ -- ");
            }
            if (currencyType == "GBP")
            {
                $("#monthly1").html("£ -- ");
            }
            
        }

        else if (inputId == "premiumUsers") {
            $scope.users.essentialUsers = "0";
            if (currencyType == "USD") {
                //debugger;
                //checking User Counts
                puser = $scope.users.premiumUsers;
                euser = $scope.users.essentialUsers;
                tuser = $scope.users.teamUsers;
                totalUsers = parseInt(puser) + parseInt(euser) + parseInt(tuser);

                //getting total Price
                var totalPremiumPrice = parseInt(puser) * premiumPrice;
                var totalEssentialPrice = parseInt(euser) * essentialPrice;
                var totalTeamPrice = parseInt(tuser) * teamPrice;

                //setting Monthly Price
                monthlyPrice = totalPremiumPrice + totalEssentialPrice + totalTeamPrice;

                $("#monthly1").html("$ " + monthlyPrice);
            }
            if (currencyType == "GBP") {
                debugger;
                //checking User Counts
                puser = $scope.users.premiumUsers;
                euser = $scope.users.essentialUsers;
                tuser = $scope.users.teamUsers;
                totalUsers = parseInt(puser) + parseInt(euser) + parseInt(tuser);

                //getting total Price
                var totalPremiumPrice = parseInt(puser) * premiumPrice;
                var totalEssentialPrice = parseInt(euser) * essentialPrice;
                var totalTeamPrice = parseInt(tuser) * teamPrice;

                //setting Monthly Price
                monthlyPrice = totalPremiumPrice + totalEssentialPrice + totalTeamPrice;


                $("#monthly1").html("£ " + monthlyPrice);
            }           
        }

        else if (inputId == "essentialUsers") {
            $scope.users.premiumUsers = "0";          
            if (currencyType == "USD") {
                //debugger;
                //checking User Counts
                puser = $scope.users.premiumUsers;
                euser = $scope.users.essentialUsers;
                tuser = $scope.users.teamUsers;
                totalUsers = parseInt(puser) + parseInt(euser) + parseInt(tuser);

                //getting total Price
                var totalPremiumPrice = parseInt(puser) * premiumPrice;
                var totalEssentialPrice = parseInt(euser) * essentialPrice;
                var totalTeamPrice = parseInt(tuser) * teamPrice;

                //setting Monthly Price
                monthlyPrice = totalPremiumPrice + totalEssentialPrice + totalTeamPrice;

                $("#monthly1").html("$ " + monthlyPrice);
            }
            if (currencyType == "GBP") {
                
                debugger;
                //checking User Counts
                puser = $scope.users.premiumUsers;
                euser = $scope.users.essentialUsers;
                tuser = $scope.users.teamUsers;
                totalUsers = parseInt(puser) + parseInt(euser) + parseInt(tuser);

                //getting total Price
                var totalPremiumPrice = parseInt(puser) * premiumPrice;
                var totalEssentialPrice = parseInt(euser) * essentialPrice;
                var totalTeamPrice = parseInt(tuser) * teamPrice;

                //setting Monthly Price
                monthlyPrice = totalPremiumPrice + totalEssentialPrice + totalTeamPrice;

                $("#monthly1").html("£ " + monthlyPrice);
            }
        }

        else if (inputId == "teamUsers") {           
            if (currencyType == "USD") {
                
                debugger;
                //checking User Counts
                puser = $scope.users.premiumUsers;
                euser = $scope.users.essentialUsers;
                tuser = $scope.users.teamUsers;
                totalUsers = parseInt(puser) + parseInt(euser) + parseInt(tuser);

                //getting total Price
                var totalPremiumPrice = parseInt(puser) * premiumPrice;
                var totalEssentialPrice = parseInt(euser) * essentialPrice;
                var totalTeamPrice = parseInt(tuser) * teamPrice;

                //setting Monthly Price
                monthlyPrice = totalPremiumPrice + totalEssentialPrice + totalTeamPrice;

                $("#monthly1").html("$ " + monthlyPrice);
            }
            if (currencyType == "GBP") {
              
                debugger;
                //checking User Counts
                puser = $scope.users.premiumUsers;
                euser = $scope.users.essentialUsers;
                tuser = $scope.users.teamUsers;
                totalUsers = parseInt(puser) + parseInt(euser) + parseInt(tuser);

                //getting total Price
                var totalPremiumPrice = parseInt(puser) * premiumPrice;
                var totalEssentialPrice = parseInt(euser) * essentialPrice;
                var totalTeamPrice = parseInt(tuser) * teamPrice;

                //setting Monthly Price
                monthlyPrice = totalPremiumPrice + totalEssentialPrice + totalTeamPrice;

                $("#monthly1").html("£ " + monthlyPrice);
            }            
        }

        finalPremiumUsers = puser;
        finalEssentialUsers = euser;
        finalTeamUsers = tuser;
        finalTotalUsers = totalUsers;
        finalStandardSupportPrice = standardSupport;
    }

    $scope.checkvalidation_submit = function (data) {
        var isvalidate = true;
        //debugger;
        if (data == undefined) {
            isvalidate = false;
        }
        else if ((data.essentialUsers == "0" || data.essentialUsers == undefined) && (data.premiumUsers == "0" || data.premiumUsers == undefined)) {
            isvalidate = false;
        }
        if (isvalidate == true) {
          //  debugger;
            //$scope.updatePricingOption(data)
            if (currencyType == "USD") {
                var enhancedPrice = parseInt(enhanbase) + (parseInt(enhanuser) * parseInt(totalUsers));
                $("#enhancedprice").html("$ " + enhancedPrice);
                enhancedSupport = enhancedPrice;
                $("#monthly2").html("$ " + (parseInt(monthlyPrice) + parseInt(standardSupport)));
                $("#yearly2").html("$ --");
            }

            if (currencyType == "GBP") {
                var enhancedPrice = parseInt(enhanbase) + (parseInt(enhanuser) * parseInt(totalUsers));
                $("#enhancedprice").html("£ " + enhancedPrice);
                enhancedSupport = enhancedPrice;
                $("#monthly2").html("£ " + (parseInt(monthlyPrice) + parseInt(standardSupport)));
                $("#yearly2").html("£ --");
            }
                  
            finalEnhancedSupportPrice = enhancedPrice;

            $("#supportPremiumUsers").html(puser);
            $("#supportEssentialUsers").html(euser);
            $("#supportTeamUsers").html(tuser);
            $("#supportTotalUsers").html(totalUsers);        


            $("#tabPricingOpt").hide();
            $("#tabSupportOpt").show();
            $("#tabOtherOptions").hide();
            $("#tabPaymentOpt").hide();
            $("#tabPersonalInfo").hide();
            $("#tabCompanyInfo").hide();
            $("#tabMicrosoftInfo").hide();
            $("#tabMakePayment").hide();
            $("#tabFinalPage").hide();

            $("#support").addClass("active");
            $("#config").removeClass("active");
            $("#pricing").removeClass("active");
            $("#summary").removeClass("active");
            $("#payment").removeClass("active");
            $("#finish").removeClass("active");
           
        }
        else {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Please enter number of users to continue!',
                footer: '<a href>Why do I have this issue?</a>'
            })
        }

    }

    $scope.setSubTotal = function () {
        debugger;
        var enhancedAns = $("#enhance").is(':checked');
        if (enhancedAns == true) {
            var newMonthlyPrice = parseInt(standardSupport) + parseInt(enhancedSupport) + parseInt(monthlyPrice);
            if (currencyType == "USD") {
                $("#monthly2").html("$ " + newMonthlyPrice);
            }
            if (currencyType == "GBP") {
                $("#monthly2").html("£ " + newMonthlyPrice);
            }
        }    
        if (enhancedAns == false) {
            var newMonthlyPrice = parseInt(standardSupport) + parseInt(monthlyPrice);
            if (currencyType == "USD") {
                $("#monthly2").html("$ " + newMonthlyPrice);
            }
            if (currencyType == "GBP") {
                $("#monthly2").html("£ " + newMonthlyPrice);
            }
        }
    }

    $scope.updateSupportData = function () {

        //debugger;
        $("#summaryPremiumUsers").html(puser);
        $("#summaryEssentialUsers").html(euser);
        $("#summaryTeamUsers").html(tuser);
        $("#summaryTotalUsers").html(totalUsers);
     
        if (currencyType == "USD") {
            var totalPremiumPrice = "$ " + (parseInt(puser) * premiumPrice);
            var totalEssentialPrice = "$ " + (parseInt(euser) * essentialPrice);
            var totalTeamPrice = "$ " + (parseInt(tuser) * teamPrice);

        }
        if (currencyType == "GBP") {
            var totalPremiumPrice = "£ " + (parseInt(puser) * premiumPrice);
            var totalEssentialPrice = "£ " + (parseInt(euser) * essentialPrice);
            var totalTeamPrice = "£ " + (parseInt(tuser) * teamPrice);
        }

        debugger;
        if (puser == "0") {
            $('#premiumbox').addClass('tabledata');
            
        }
        if (euser == "0") {
            $('#essentialbox').addClass('tabledata');
        }
        if (tuser == "0") {
            $('#teambox').addClass('tabledata');
        }

        var enhancedAns = $("#enhance").is(':checked');
        finalEnhancedSupportChecked = enhancedAns;
        if (enhancedAns == true){
            var total = enhancedSupport + standardSupport;
            if(currencyType == "USD"){               
                supportTotal = "$ " + total;
            }
            if (currencyType == "GBP") {           
                supportTotal = "£ " + total;
            }
        }
        else {
            var total = standardSupport;
            if (currencyType == "USD") {
                supportTotal = "$ " + total;
            }
            if (currencyType == "GBP") {
                supportTotal = "£ " + total;
            }
        }

        tableTotal = (parseInt(puser) * premiumPrice) + (parseInt(euser) * essentialPrice) + (parseInt(tuser) * teamPrice) + parseInt(total);

        if (currencyType == "USD")
        {
            finalTotal = "$ " + tableTotal;
        }
        if (currencyType == "GBP") {
            finalTotal = "£ " + tableTotal;
        }

        if (currencyType == "USD") {

            $("#monthly3").html("$ " + (parseInt(tableTotal)));
            $("#yearly3").html("$ -- ");
        }

        if (currencyType == "GBP") {

            $("#monthly3").html("£ " + (tableTotal));
            $("#yearly3").html("£ -- ");
        }

        // To set Inside table
        $("#td_premiumUser").html(puser);
        $("#td_premiumPrice").html(premiumPrice);
        $("#td_premiumTotal").html(totalPremiumPrice);
        $("#td_essentialsUser").html(euser);
        $("#td_essentialsPrice").html(essentialPrice);
        $("#td_essentialsTotal").html(totalEssentialPrice);
        $("#td_teamUser").html(tuser);
        $("#td_teamPrice").html(teamPrice); 
        $("#td_teamTotal").html(totalTeamPrice);
        $("#td_supportTotal").html(supportTotal);
        $("#td_finalTotalPrice").html(finalTotal);
        $("#td_powerAppsUser").html(totalUsers);

        $scope.TotalPrice = finalTotal;

        $("#tabPricingOpt").hide();
        $("#tabSupportOpt").hide();
        $("#tabOtherOptions").show();
        $("#tabPaymentOpt").hide();
        $("#tabPersonalInfo").hide();
        $("#tabCompanyInfo").hide();
        $("#tabMicrosoftInfo").hide();
        $("#tabMakePayment").hide();
        $("#tabFinalPage").hide();
        $("#support").removeClass('active');
        $("#pricing").removeClass('active');
        $("#config").removeClass('active');
        $("#summary").addClass("active");
        $("#payment").removeClass("active");
        $("#finish").removeClass('active');
		
    }

    $scope.showPaymentOption = function () {
        //debugger;
        if (currencyType == "USD")
        {
           
            finalMonthly = "$ "+(tableTotal);
            finalYearly ="$ " +(parseInt(parseFloat(gst) * (parseInt(tableTotal) * 12)).toFixed(0));

            $("#finalMonthylPrice").html(finalMonthly);
            $("#finalYearlylPrice").html(finalYearly);
        }
        if (currencyType == "GBP") {
           
           finalMonthyl = "£ " + (tableTotal);
           finalYearly = "£ " + (parseInt(parseFloat(gst) * (parseInt(tableTotal) * 12)).toFixed(0));

           $("#finalMonthylPrice").html(finalMonthly);
            $("#finalYearlylPrice").html(finalYearly);
        }
        

        $("#tabPricingOpt").hide();
        $("#tabSupportOpt").hide();
        $("#tabOtherOptions").hide();
        $("#tabPaymentOpt").show();
        $("#tabNewCustomerSignUp").hide();
        $("#tabMakePayment").hide();
        $("#tabFinalPage").hide();
        $("#support").removeClass('active');
        $("#pricing").removeClass('active');
        $("#finish").removeClass('active');
        $("#config").removeClass('active');
        $("#summary").removeClass("active");
        $("#payment").addClass("active");
    }

    $scope.updatePaymentOptions = function () {

        //debugger;
        var radioValue = $("input[name='optradio']:checked").val();
        finalPaymentOptionAns = radioValue;

        if (radioValue == "Monthly") {
            $(".permonth").html(finalMonthly);           
            if (currencyType == "USD") {
                $(".peryear").html("$ --");
                $("#totalPrice").html(finalMonthly);                
            }
            if (currencyType == "GBP") {
                $(".peryear").html("£ --");
                $("#totalPrice").html(finalMonthly);
            }
            $("#message").html("If you have chosen monthly payments, the same payment will be taken monthly, starting a month from today.");
        }
        else if (radioValue == "Annual") {                    
            $(".peryear").html(finalYearly);
            if (currencyType == "USD") {
                $(".permonth").html("$ --");
                $("#totalPrice").html(finalYearly);
            }
            if (currencyType == "GBP") {
                $("#totalPrice").html(finalYearly);
                $(".permonth").html("£ --");
            }
            $("#message").html("If you have chosen the annual option, you will only be charged once.");
        }

        
      


        $("#tabPricingOpt").hide();
        $("#tabSupportOpt").hide();
        $("#tabOtherOptions").hide();
        $("#tabPaymentOpt").hide();
        $("#tabNewCustomerSignUp").show();
        $("#tabMakePayment").hide();
        $("#tabFinalPage").hide();
        $("#support").removeClass('active');
        $("#pricing").removeClass('active');
        $("#finish").addClass('active');
        $("#config").removeClass('active');
        $("#summary").removeClass("active");
        $("#payment").removeClass("active");

    }

    $scope.checkUserData = function (userData) {
      
        var isvalidate = true;
        var isvalidate1 = true;
        var form = $("#myForm")
        //debugger;
        if (userData == undefined) {
            isvalidate = false;
        }        
        //else if (form[0].checkValidity() === false)
        //{
        //    isvalidate = false;           
        //}

        else if (userData.FirstName == null || userData.FirstName == undefined || userData.FirstName == "")
        {           
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


        else if (userData.Email == null || userData.Email == undefined || userData.ContactNumber == "") {
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


        else if (userData.CompanyPostalCode == null || userData.CompanyPostalCode == undefined || userData.CompanyPostalCode == "") {
            isvalidate = false;
        }

        else if ($("#microsoftAccount").is(':checked') == false) {
            isvalidate = false;
        }

        else if ($("#microsoftCloudAggrement").is(':checked') == false || $("#supportContract").is(':checked') == false) {
            isvalidate = false;
        }

        else if ($("#microsoftAccount").is(':checked') == true) {
            if ($("#findDomain").is(':checked') == false && $("#domainName").val() == "" || $("#domainName").val() == undefined || $("#domainName").val() == null) {
                isvalidate = false;
                isvalidate1 = false;
                Swal.fire({
                    type: 'error',
                    title: 'Oops...',
                    text: 'Domain name cannot be blank!',
                    footer: '<a href>Why do I have this issue?</a>'
                })
            }           
        }        

        if (isvalidate == true) {
            if (userData.Email != userData.ConfirmEmail) {
                Swal.fire({
                    type: 'error',
                    title: 'Oops...',
                    text: '<b>Email</b> does not match.',                    
                })
            }
            else {
                $scope.SaveUserData(userData);
            }           
        }

        else {
            if (isvalidate1) {                
                event.preventDefault();
                event.stopPropagation();
                form.addClass('was-validated');
            }
        }      

    }

    $scope.SaveUserData = function (userData) {

        //debugger;
        $(".my_Loader").show();
        var datatoPost = {
            SpId: SpId,
            Currency: currencyType,

            //Main Data
            PremiumUsers: finalPremiumUsers,
            EssentialUsers: finalEssentialUsers,
            TeamUsers: finalTeamUsers,
            StandardSupportPrice :finalStandardSupportPrice,
            EnhancedSupportPrice: finalEnhancedSupportPrice,
            EnhancedSupportChecked: finalEnhancedSupportChecked,
         
            //PersonalInformation
            Firstname: userData.FirstName,
            Lastname: userData.LastName,
            JobTitle: userData.JobTitle,
            ContactNumber: userData.ContactNumber,
            Email: userData.Email,

            //Payment Option Information
            PaymentOptionMonthlyPrice: finalMonthly,
            PaymentOptionYearlyPrice: finalYearly,
            PaymentOptionAns:finalPaymentOptionAns,

            //Company Information
            CompanyName: userData.CompanyName,
            CompanyNumber: userData.CompanyNumber,
            CompanyAddress: userData.CompanyAddress,
            CompanyCountry: userData.CompanyCountry,
            CompanyState: userData.CompanyState,
            CompanyCity: userData.CompanyCity,
            CompanyPostalCode: userData.CompanyPostalCode,

            //Microsoft Account Information
            MicrosoftOfficeAccChecked: $("#microsoftCloudAggrement").is(':checked'),
            DomainName: $("#domainName").val() != null ? $("#domainName").val() : "",
            FindDomain: $("#findDomain").is(':checked'),


            //Authorization
            MicrosoftCloudAgreement: $("#microsoftCloudAggrement").is(':checked'),
            SupportContract: $("#supportContract").is(':checked')

        }

        $http.post('/cowry/result/UpdateUserData', datatoPost).then(function (results) {
            //debugger;
            if (results.status == 200) {

                $("#tabPricingOpt").hide();
                $("#tabSupportOpt").hide();
                $("#tabOtherOptions").hide();
                $("#tabPaymentOpt").hide();
                $("#tabNewCustomerSignUp").hide();
                $("#tabPersonalInfo").hide();
                $("#tabCompanyInfo").hide();
                $("#tabMicrosoftInfo").hide();
                $("#tabMakePayment").show();
                $("#tabFinalPage").hide();
                $("#support").removeClass('active');
                $("#finish").addClass('active');
                $("#config").removeClass('active');
                $("#pricing").removeClass('active');


            }
            $(".my_Loader").fadeOut("slow");
        }).catch(function (fallback) {
            alert(fallback.statusText);
        });


    }

    $scope.checkPaymentInfo = function (paymentInfo) {
        var isvalidate = true;

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
            $scope.savePaymentInfo(paymentInfo, $scope.userData);
        }
        else {          
                Swal.fire({
                    type: 'error',
                    title: 'Oops...',
                    text: 'Please enter all the required details to continue!',
                    footer: '<a href>Why do I have this issue?</a>'
                })        
        }
    }
    
    $scope.savePaymentInfo = function (paymentInfo, userData) {

        debugger;
        $(".my_Loader").show();
        var datatoPost = {
            SpId: SpId,
            CardType: paymentInfo.CardType,
            NameOnCard: paymentInfo.NameOnCard,
            CardNumber: paymentInfo.CardNumber,
            CardCVV: paymentInfo.CardCVV,
            CardExpiryMonth: paymentInfo.CardExpiryMonth,
            CardExpiryYear: paymentInfo.CardExpiryYear,
            Email: userData.Email,
            phone: userData.CompanyNumber,
            address: userData.CompanyAddress,
            city: userData.CompanyCity,
            zip: userData.CompanyPostalCode,
            state: userData.CompanyState,
            country: userData.CompanyCountry,
            TotalAmount: $scope.TotalPrice 
        };
        
        $http.post('/cowry/result/UpdatePaymentInfo', datatoPost).then(function (results) {
            debugger;
            $(".my_Loader").fadeOut("slow");
            if (results.status == 200) {
                $scope.emailData();

                $("#tabPricingOpt").hide();
                $("#tabSupportOpt").hide();
                $("#tabOtherOptions").hide();
                $("#tabPaymentOpt").hide();
                $("#tabNewCustomerSignUp").hide();
                $("#tabMakePayment").hide();
                $("#tabFinalPage").show();
                $("#breadcrumb").hide();               
            }
            else if(results.status == 202)
            {
                Swal.fire({
                    type: 'error',
                    title: 'Oops...',
                    text: results.data.Message,
                    footer: '<a href>Why do I have this issue?</a>'
                })
            }          
        }).catch(function (fallback) {
            $(".my_Loader").fadeOut("slow");            
        });       
    }   
 
    $scope.emailData = function () {
        debugger;
        $(".my_Loader").show();
        $http.get('/cowry/result/EmailResultToCowry?SpId=' + SpId).then(function (results) {
            debugger;
            $(".my_Loader").fadeOut("slow");
            if (results.status == 200) {
            }
        }).catch(function (fallback) {
        });
    }

    $scope.allCountries = [
{ "name": " United States" },
{ "name": " United Kingdom" },
{ "name": " Albania" }, { "name": " Algeria" }, { "name": " American Samoa" }, { "name": " AndorrA" }, { "name": " Angola" },
{ "name": " Anguilla" }, { "name": " Antarctica" }, { "name": " Antigua and Barbuda" }, { "name": " Argentina" }, { "name": " Armenia" }, { "name": " Aruba" },
{ "name": " Australia" }, { "name": " Austria" }, { "name": " Azerbaijan" }, { "name": " Bahamas" }, { "name": " Bahrain" }, { "name": " Bangladesh" }, { "name": " Barbados" },
{ "name": " Belarus" }, { "name": " Belgium" }, { "name": " Belize" }, { "name": " Benin" }, { "name": " Bermuda" }, { "name": " Bhutan" }, { "name": " Bolivia" },
{ "name": " Bosnia and Herzegovina" }, { "name": " Botswana" }, { "name": " Bouvet Island" }, { "name": " Brazil" }, { "name": " British Indian Ocean Territory" },
{ "name": " Brunei Darussalam" }, { "name": " Bulgaria" }, { "name": " Burkina Faso" }, { "name": " Burundi" }, { "name": " Cambodia" }, { "name": " Cameroon" },
{ "name": " Canada" }, { "name": " Cape Verde" }, { "name": " Cayman Islands" }, { "name": " Central African Republic" }, { "name": " Chad" }, { "name": " Chile" },
{ "name": " China" }, { "name": " Christmas Island" }, { "name": " Cocos (Keeling) Islands" }, { "name": " Colombia" }, { "name": " Comoros" }, { "name": " Congo" },
{ "name": " Congo, The Democratic Republic of the" }, { "name": " Cook Islands" }, { "name": " Costa Rica" }, { "name": " Cote D\"Ivoire" }, { "name": " Croatia" },
{ "name": " Cuba" }, { "name": " Cyprus" }, { "name": " Czech Republic" }, { "name": " Denmark" }, { "name": " Djibouti" }, { "name": " Dominica" }, { "name": " Dominican Republic" },
{ "name": " Ecuador" }, { "name": " Egypt" }, { "name": " El Salvador" }, { "name": " Equatorial Guinea" }, { "name": " Eritrea" }, { "name": " Estonia" }, { "name": " Ethiopia" },
{ "name": " Falkland Islands (Malvinas)" }, { "name": " Faroe Islands" }, { "name": " Fiji" }, { "name": " Finland" }, { "name": " France" }, { "name": " French Guiana" }, { "name": " French Polynesia" },
{ "name": " French Southern Territories" }, { "name": " Gabon" }, { "name": " Gambia" }, { "name": " Georgia" }, { "name": " Germany" }, { "name": " Ghana" }, { "name": " Gibraltar" }, { "name": " Greece" },
{ "name": " Greenland" }, { "name": " Grenada" }, { "name": " Guadeloupe" }, { "name": " Guam" }, { "name": " Guatemala" }, { "name": " Guernsey" }, { "name": " Guinea" }, { "name": " Guinea-Bissau" },
{ "name": " Guyana" }, { "name": " Haiti" }, { "name": " Heard Island and Mcdonald Islands" }, { "name": " Holy See (Vatican City State)" }, { "name": " Honduras" }, { "name": " Hong Kong" },
{ "name": " Hungary" }, { "name": " Iceland" }, { "name": " India" }, { "name": " Indonesia" }, { "name": " Iran, Islamic Republic Of" }, { "name": " Iraq" }, { "name": " Ireland" },
{ "name": " Isle of Man" }, { "name": " Israel" }, { "name": " Italy" }, { "name": " Jamaica" }, { "name": " Japan" }, { "name": " Jersey" }, { "name": " Jordan" }, { "name": " Kazakhstan" },
{ "name": " Kenya" }, { "name": " Kiribati" }, { "name": " Korea, Democratic People\"S Republic of" }, { "name": " Korea, Republic of" }, { "name": " Kuwait" },
{ "name": " Kyrgyzstan" }, { "name": " Lao People\"S Democratic Republic" }, { "name": " Latvia" }, { "name": " Lebanon" }, { "name": " Lesotho" }, { "name": " Liberia" },
{ "name": " Libyan Arab Jamahiriya" }, { "name": " Liechtenstein" }, { "name": " Lithuania" }, { "name": " Luxembourg" }, { "name": " Macao" }, { "name": " Macedonia, The Former Yugoslav Republic of" },
{ "name": " Madagascar" }, { "name": " Malawi" }, { "name": " Malaysia" }, { "name": " Maldives" }, { "name": " Mali" }, { "name": " Malta" }, { "name": " Marshall Islands" },
{ "name": " Martinique" }, { "name": " Mauritania" }, { "name": " Mauritius" }, { "name": " Mayotte" }, { "name": " Mexico" }, { "name": " Micronesia, Federated States of" },
{ "name": " Moldova, Republic of" }, { "name": " Monaco" }, { "name": " Mongolia" }, { "name": " Montenegro" }, { "name": " Montserrat" }, { "name": " Morocco" }, { "name": " Mozambique" },
{ "name": " Myanmar" }, { "name": " Namibia" }, { "name": " Nauru" }, { "name": " Nepal" }, { "name": " Netherlands" }, { "name": " Netherlands Antilles" }, { "name": " New Caledonia" },
{ "name": " New Zealand" }, { "name": " Nicaragua" }, { "name": " Niger" }, { "name": " Nigeria" }, { "name": " Niue" }, { "name": " Norfolk Island" },
{ "name": " Northern Mariana Islands" }, { "name": " Norway" }, { "name": " Oman" }, { "name": " Pakistan" }, { "name": " Palau" }, { "name": " Palestinian Territory, Occupied" },
{ "name": " Panama" }, { "name": " Papua New Guinea" }, { "name": " Paraguay" }, { "name": " Peru" }, { "name": " Philippines" }, { "name": " Pitcairn" }, { "name": " Poland" },
{ "name": " Portugal" }, { "name": " Puerto Rico" }, { "name": " Qatar" }, { "name": " Reunion" }, { "name": " Romania" }, { "name": " Russian Federation" }, { "name": " RWANDA" },
{ "name": " Saint Helena" }, { "name": " Saint Kitts and Nevis" }, { "name": " Saint Lucia" }, { "name": " Saint Pierre and Miquelon" }, { "name": " Saint Vincent and the Grenadines" },
{ "name": " Samoa" }, { "name": " San Marino" }, { "name": " Sao Tome and Principe" }, { "name": " Saudi Arabia" }, { "name": " Senegal" }, { "name": " Serbia" }, { "name": " Seychelles" },
{ "name": " Sierra Leone" }, { "name": " Singapore" }, { "name": " Slovakia" }, { "name": " Slovenia" }, { "name": " Solomon Islands" }, { "name": " Somalia" },
{ "name": " South Africa" }, { "name": " South Georgia and the South Sandwich Islands" }, { "name": " Spain" }, { "name": " Sri Lanka" }, { "name": " Sudan" },
{ "name": " Suriname" }, { "name": " Svalbard and Jan Mayen" }, { "name": " Swaziland" }, { "name": " Sweden" }, { "name": " Switzerland" }, { "name": " Syrian Arab Republic" },
{ "name": " Taiwan, Province of China" }, { "name": " Tajikistan" }, { "name": " Tanzania, United Republic of" }, { "name": " Thailand" }, { "name": " Timor-Leste" },
{ "name": " Togo" }, { "name": " Tokelau" }, { "name": " Tonga" }, { "name": " Trinidad and Tobago" }, { "name": " Tunisia" }, { "name": " Turkey" },
{ "name": " Turkmenistan" }, { "name": " Turks and Caicos Islands" }, { "name": " Tuvalu" }, { "name": " Uganda" }, { "name": " Ukraine" }, { "name": " United Arab Emirates" },
{ "name": " United States Minor Outlying Islands" }, { "name": " Uruguay" }, { "name": " Uzbekistan" }, { "name": " Vanuatu" },
{ "name": " Venezuela" }, { "name": " Viet Nam" }, { "name": " Virgin Islands, British" }, { "name": " Virgin Islands, U.S." }, { "name": " Wallis and Futuna" }, { "name": " Western Sahara" },
{ "name": " Yemen" }, { "name": " Zambia" }];

    $scope.SetValid = function (data) {
        //debugger;
        if (data == 'Amex') {
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
    }

    $scope.validateForm = function () {
       // debugger;
        var x = document.forms["myForm"]["fname"].value;
        if (x == "") {
           
            return false;
        }
    }

    
}]);

