var app = angular.module('cscContactUK', []);
app.controller('cscContactUKCtrl', ['$scope', '$http', function ($scope, $http) {

    $scope.users = [];
    $scope.users.premiumUsers = "0";
    $scope.users.essentialUsers = "0";
    $scope.users.teamUsers = "0";

    $scope.checkContactData = function (myData) {
        debugger;
        var isvalidate = true;
        var isvalidate1 = true;
        var form = $("#myForm1");
        debugger;
        if (myData === undefined) {
            isvalidate = false;
        }

        else if (myData.FirstName === null || myData.FirstName === undefined || myData.FirstName === "") {
            isvalidate = false;
        }

        else if (myData.LastName === null || myData.LastName === undefined || myData.LastName === "") {
            isvalidate = false;
        }


        else if (myData.JobTitle === null || myData.JobTitle === undefined || myData.JobTitle === "") {
            isvalidate = false;
        }


        else if (myData.ContactNumber === null || myData.ContactNumber === undefined || myData.ContactNumber === "") {
            isvalidate = false;
        }


        else if (myData.Email === null || myData.Email === undefined || myData.ContactNumber === "") {
            isvalidate = false;
        }


        else if (myData.ConfirmEmail === null || myData.ConfirmEmail === undefined || myData.ConfirmEmail === "") {
            isvalidate = false;
        }


        else if (myData.CompanyName === null || myData.CompanyName === undefined || myData.CompanyName === "") {
            isvalidate = false;
        }


        else if (myData.CompanyNumber === null || myData.CompanyNumber === undefined || myData.CompanyNumber === "") {
            isvalidate = false;
        }


        else if (myData.CompanyAddress === null || myData.CompanyAddress === undefined || myData.CompanyAddress === "") {
            isvalidate = false;
        }


        else if (myData.CompanyCountry === null || myData.CompanyCountry === undefined || myData.CompanyCountry === "") {
            isvalidate = false;
        }


        else if (myData.CompanyState === null || myData.CompanyState === undefined || myData.CompanyState === "") {
            isvalidate = false;
        }


        else if (myData.CompanyCity === null || myData.CompanyCity === undefined || myData.CompanyCity === "") {
            isvalidate = false;
        }


        else if (myData.CompanyPostalCode === null || myData.CompanyPostalCode === undefined || myData.CompanyPostalCode === "") {
            isvalidate = false;
        }


        if (isvalidate === true) {
            if (myData.Email != myData.ConfirmEmail) {
                Swal.fire({
                    type: 'error',
                    title: 'Oops...',
                    text: '<b>Email</b> does not match.',
                })
            }
            else {
                $scope.SavemyContactData(myData);
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

    $scope.SavemyContactData = function (userData) {
        debugger;
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


        };

        $http.post('/Cowry/Common/AddContactData', datatoPost).then(function (results) {
            debugger;
            if (results.status === 200) {
                Swal.fire({
                    type: 'success',
                    text: 'Thank you for contacting us, we aim to respond within 2 hours of the inquiry.'
                }).then(function () {
                    window.location = 'https://cowrysolutions.com';
                });
            }
            $(".my_Loader").fadeOut("slow");
        }).catch(function (fallback) {
            alert(fallback.statusText);
        });


    };

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

}]);

