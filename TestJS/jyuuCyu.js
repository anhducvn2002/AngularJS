jyuucyu.controller("loadJyucyu", function ($scope) {
    $scope.jyuuCyuNo = "445159";
    $scope.tokuiCode = "00001";
    $scope.tokuiName = "(株)玉吉製作所大田原工場";
    $scope.nouNyuCode = "00001";
    $scope.noNyuName = "大田原工場";
    $scope.seiHinCode = "173";
    $scope.seiHinZuban = "3X25-18788*D        ";
    $scope.seiHinKisyu = "DT-MCS-2            ";
    $scope.quantity = "10";
    $scope.tanka = "4500";
    $scope.nouki = "191010";
    $scope.nounyu = "191009";

    $scope.bumonCode = "999";
    $scope.bumonName = "データCONV";
    $scope.tantouCode = "209";
    $scope.tantouName = "和久井清美";
    $scope.cyumonNo1 = "45679";
    $scope.cyumonNo2 = "1";
    $scope.seizou = "AAAAA";
    $scope.bikou = "BBBBB";



});


jyuucyu.controller("usercontroller", function ($scope, $http) {

    $scope.countryList = ["USA", "UK", "INDIA", "KOREA"];

    /*
    $http.get("/About/GetSehins?param="+country)
        
        .success(function (data) {
            //alert(country);
        });
    */

    $scope.complete = function (string) {


        $scope.tokuiCode = string;

        if (string.length > 0) {

            $http.get("/About/GetSehins?param=" + $scope.tokuiCode)
            .success(function (data) {
                $scope.filterCountry = data;
            });
        }

        if ($scope.filterCountry.length == 1) {
            $scope.country = $scope.filterCountry[0].split(" - ")[0];
            $scope.tokuiName = $scope.filterCountry[0].split(" - ")[1];
            $scope.hidethis = true;
        }

        if ($scope.filterCountry.length > 1) {
            $scope.hidethis = false;
        }
        /*
        var output = [];
        angular.forEach($scope.countryList, function (country) {
            if (country.toLowerCase().indexOf(string.toLowerCase()) >= 0) {

                output.push(country);
            }
        });*/
        //$scope.filterCountry = $scope.countryList;
    }
    $scope.fillTextbox = function (string) {
        $scope.country = string.split(" - ")[0];
        $scope.tokuiName = string.split(" - ")[1];
        $scope.hidethis = true;
    }

    $scope.test2 = function (string) {
        $scope.hidethis = true;
    }
});