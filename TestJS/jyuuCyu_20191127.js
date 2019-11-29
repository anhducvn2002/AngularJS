﻿
var jyuucyu = angular.module('myApp2', ['ui.bootstrap', 'pascalprecht.translate', 'ngCookies']);

var langCollection = [];
var locationstr = window.location.pathname + "";
var serverDir = "/" + locationstr.split('/')[1];
//alert(serverDir);
//locationstr.split('//')[0].substring(0, locationstr.split('//')[0].length - 1);
//"/app.dev";
//alert(window.location.pathname + "@@@" + serverDir);



jyuucyu.controller("facName", function ($scope, $http, $translate) {
    $scope.FacName = $http({
        method: "POST",
        url: serverDir + "/Home/getParamByID/",
        data: {
            "id": "fid"
        },
    })
         .success(function (value) {
             if (value == 2) $scope.FacName = "大田原";
             else $scope.FacName = "富士宮";
         });

    $http.get(serverDir + "/About/getLangSel")
         .success(function (data) {
             

             $scope.langMulti = data;
             $translate.use(data);
         });

});

jyuucyu.controller("loadJyucyu", function ($scope, $http, $window, $translate) {
    $scope.langMulti = "";
    $scope.selectedAll = false;
    $scope.selected = false;

    $scope.jyuuCyuNo = "";
    $scope.tokuiC = "";
    $scope.tokuiN = "得意名称";
    $scope.nounyuC = "";
    $scope.nounyuN = "納入場所";
    $scope.sehinC = "";
    $scope.sehinZ = "図番番号";
    $scope.sehinN = "製品名称";
    $scope.sehinK = "機種";
    $scope.quantity = "1";
    $scope.sehinT = "0";

    $scope.bumonCode = "999";
    $scope.bumonName = "データCONV";
    $scope.tantouC = "";
    $scope.tantouN = "担当者";
    $scope.cyumonNo1 = "";
    $scope.cyumonNo2 = "";
    $scope.seizou = "";
    $scope.bikou = "";
    $scope.Status = "";
    $scope.dateNouki = "";
    $scope.dateNounyu = "";

    /*
    $scope.tokuiC = "00001";
    $scope.tokuiN = "(株)玉吉製作所大田原工場";
    $scope.nounyuC = "00001";
    $scope.nounyuN = "大田原工場";
    $scope.sehinC = "097873";
    $scope.sehinZ = "AE64757             ";
    $scope.sehinN = "カバーサイド                                      ";
    $scope.sehinK = "図面２部有          ";
    $scope.quantity = "1348";
    $scope.sehinT = "0";
    $scope.tantouC = "182";
    $scope.tantouN = "ｸﾞｴﾝ ｱｲﾝ ﾄﾞｩｯｸ";
    $scope.dateNouki = "20191225";
    $scope.dateNounyu = "20191224";
    */

    $scope.btnRegDis = false;

    var date = new Date();

    //Add 10 days
    date.setDate(date.getDate() + 9);

    var yyyy = date.getFullYear() + '';
    var MM = ('0' + (date.getMonth() + 1)).slice(-2);
    var dd = ('0' + date.getDate()).slice(-2);

    //$scope.dateNouki = yyyy + MM + dd;

    //Add 10 days
    date.setDate(date.getDate() - 1);

    var yyyy = date.getFullYear() + '';
    var MM = ('0' + (date.getMonth() + 1)).slice(-2);
    var dd = ('0' + date.getDate()).slice(-2);
    //$scope.dateNounyu = yyyy + MM + dd;

    //$scope.dbf_hide = true;

    //$scope.hideDBF = function (){
    //    $scope.dbf_hide = !$scope.dbf_hide;
    //}

    $scope.wait_hide = true;
    $scope.processHide = true;
    $scope.nounyuHide = true;
    $scope.tokuiHide = true;
    $scope.sehinHide = true;


    //Seisan 
    $scope.kubuns = [
    { name: '0: 製品', code: '0' },
    { name: '1: 部品', code: '1' },
    ];
    $scope.kubun = $scope.kubuns[0]; //←注目！
});

jyuucyu.controller("jyucyucontroller", function ($location, $filter, $anchorScroll, $scope, $http, $translate) {    

    
    

    

    $scope.enterkey = function (key, nextID) {
        
        //Enter -> keyCode == 13

        if (key.keyCode == 13) {

            if (nextID == 'dateNoukiID') {

                $scope.this = {
                    "Jyuucyuno": $scope.jyuuCyuNo,
                    "Tokuicode": $scope.tokuiC, "Tokuiname": $scope.tokuiN,
                    "Nounyucode": $scope.nounyuC, "Nonyuname": $scope.nounyuN,
                    "Seihincode": $scope.sehinC, "Seihinzuban": $scope.sehinZ, "Seihinname": $scope.sehinN, "Seihinkisyu": $scope.sehinK,
                    "Quantity": $scope.quantity, "Tanka": $scope.sehinT,
                    "Nouki": $scope.dateNouki, "Nounyu": $scope.dateNounyu,
                    "Bumoncode": $scope.bumonCode, "Bumonname": $scope.bumonName,
                    "Tantoucode": $scope.tantouC, "Tantouname": $scope.tantouN,
                    "Cyumonno1": $scope.cyumonNo1, "Cyumonno2": $scope.cyumonNo2,
                    "Seizou": $scope.seizou, "Bikou": $scope.bikou, "Ndate": "20191111", "Status": $scope.Status,
                };

                $http({
                    method: "POST",
                    url: serverDir +"/About/validateDATA/",
                    data: {
                        "JyuuProcess": $scope.this
                    },
                })
                .success(function (error) {
                    if (error.indexOf("NG: TANKA") > -1) {
                        $scope.sehinTStyle = {
                            "color": "red"
                        }
                    } else {
                        $scope.sehinTStyle = {
                            "color": "black"
                        }
                    }
                });
                document.querySelector("#" + nextID).focus();
            }
            else 
            document.querySelector("#" + nextID).focus();
            //a.focus();
        }
        //alert(value.charCode);
    }

    //Get Tokuisaki + Nounyu data
    $scope.listTokui = []
    $scope.searchTokui = function (string, b) {

        //ESC Case
        /*
        if (b.keyCode == 27) {
            $scope.tokuiHide = true;
            return;
        }
        */

        if (b.keyCode == 13) {
            if ($scope.tokuiC.length == 5) {
                document.querySelector("#nounyuCID").focus();
            }
            //a.focus();
        }        

        if (string.length > 0) {
            $http.get(serverDir + "/About/GetTokuiCD?param=" + string)
            .success(function (data) {
                $scope.listTokui = data;
                if ($scope.listTokui.length == 1) {

                    $scope.getTokui($scope.listTokui[0].split("  ,  ")[0]);                    
                    $scope.tokuiN = $scope.listTokui[0].split("  ,  ")[1];
                    $scope.tokuiHide = true;
                    /*
                    $scope.tokuiC = $scope.listTokui[0].split("  ,  ")[0];                    
                    $scope.searchNounyu($scope.tokuiC, null);
                    */
                }
                if ($scope.listTokui.length > 1) {
                    $scope.tokuiHide = false;
                }
            });
        }
    }
    $scope.getTokui = function (string) {
        $scope.tokuiC = string.split("  ,  ")[0];
        $scope.tokuiN = string.split("  ,  ")[1];
        $scope.tokuiHide = true;

        //NounyuCode + Name
        $scope.listNounyu = [];
        $http.get(serverDir + "/About/GetNouNyu?param=" + $scope.tokuiC)
           .success(function (data) {
               $scope.listNounyu = data;
                   
               var a = $scope.listNounyu.length;

               if ($scope.listNounyu.length == 1) {
                   $scope.nounyuStyle = {
                       "color": "black"
                   }
                   $scope.nounyuC = $scope.listNounyu[0].split("  ,  ")[0];
                   $scope.nounyuN = $scope.listNounyu[0].split("  ,  ")[1];
                   $scope.nounyuHide = true;

                   document.querySelector("#sehinCID").focus();
               }

               else if ($scope.listNounyu.length > 1) {
                   $scope.nounyuC = "";
                   $scope.nounyuN = "";
                   $scope.nounyuHide = false;
                   document.querySelector("#nounyuCID").focus();
                   return;
               }
               else {
                   alert('納入場所は存在しません。');

                   $scope.nounyuStyle = {
                       "color": "red"
                   }
                   $scope.nounyuC = "";
                   $scope.nounyuN = "納入場所は存在しません。";

                   $scope.nounyuHide = true;
                   document.querySelector("#nounyuCID").focus();
               }

           });
    }

    //Nounyu data
    $scope.searchNounyu = function (string, b) {

        //alert(string + b.keyCode);

        if (b != null && b != "") {
            if (b.keyCode == 27) {
                $scope.nounyuHide = true;
                return;
            }

            if (b.keyCode == 13) {
                var a = document.querySelector("#sehinCID");
                a.focus();
                return;
            }
        }

        if (string.length > 0) {
            $http.get(serverDir + "/About/GetNouNyu?param=" + string + " - " + $scope.tokuiC)
            .success(function (data) {
                $scope.listNounyu = data;
                if ($scope.listNounyu.length == 1) {
                    $scope.nounyuStyle = {
                        "color": "black"
                    }
                    $scope.nounyuC = $scope.listNounyu[0].split("  ,  ")[0];
                    $scope.nounyuN = $scope.listNounyu[0].split("  ,  ")[1];
                    $scope.nounyuHide = true;

                    document.querySelector("#sehinCID").focus();
                }

                else if ($scope.listNounyu.length > 1) {
                    $scope.nounyuC = "";
                    $scope.nounyuN = "";

                    document.querySelector("#nounyuCID").focus();

                    $scope.nounyuHide = false;
                    return;
                }
                else {
                    alert('納入場所は存在しません。');

                    $scope.nounyuStyle = {
                        "color": "red"
                    }
                    $scope.nounyuC = "";
                    $scope.nounyuN = "納入場所は存在しません。";

                    $scope.nounyuHide = true;
                    document.querySelector("#nounyuCID").focus();
                }
            });
        }
    }
    $scope.getNounyu = function (string) {
        $scope.nounyuStyle = {
            "color": "black"
        }
        $scope.nounyuC = string.split("  ,  ")[0];
        $scope.nounyuN = string.split("  ,  ")[1];
        $scope.nounyuHide = true;

        document.querySelector("#sehinCID").focus();
    }

    //Get sehin data
    $scope.listSehin = [];
    $scope.searchSehin = function (string) {
        if (string.length > 2) {
            $http.get(serverDir + "/About/GetSehin?param=" + string + " - 0")
            .success(function (data) {
                $scope.listSehin = data;
                if ($scope.listSehin.length == 1) {
                    $scope.sehinC = $scope.listSehin[0].split("  ,  ")[0];
                    $scope.sehinZ = $scope.listSehin[0].split("  ,  ")[1];
                    $scope.sehinN = $scope.listSehin[0].split("  ,  ")[2];
                    $scope.sehinK = $scope.listSehin[0].split("  ,  ")[3];
                    $scope.sehinT = $scope.listSehin[0].split("  ,  ")[4];
                    $scope.sehinHide = true;

                    document.querySelector("#quantityID").focus();
                }
                if ($scope.listSehin.length > 1) {
                    $scope.sehinHide = false;
                }
            });
        }
    }
    $scope.getValueSehin = function (string) {
        $scope.sehinC = string.split("  ,  ")[0];
        $scope.sehinZ = string.split("  ,  ")[1];
        $scope.sehinN = string.split("  ,  ")[2];
        $scope.sehinK = string.split("  ,  ")[3];
        $scope.sehinT = string.split("  ,  ")[4];
        $scope.sehinHide = true;

        $scope.nounyuHide = true;
        document.querySelector("#quantityID").focus();
    }


    
    $scope.changedateNouki = function (key) {
        if (key.keyCode == 13) {

            if ($scope.dateNouki.length == 4) {
                $scope.dateNouki = new Date().getFullYear() + '' + $scope.dateNouki;
            }

            $http({
                method: "POST",
                url: serverDir + "/Home/changedateNouki/",
                data: {
                    "dateNouki": $scope.dateNouki,
                    "dateNounyu":$scope.dateNounyu
                },
            })
           .success(function (result) {
               $scope.validateDATA(result);
               if (result.indexOf("NG: ") > -1) {
                   alert(result);
               }
               else {
                   $scope.dateNounyu = result;
                   document.querySelector("#dateNounyuID").focus();
               }
           });
        }
    }
    $scope.changedateNounyu = function (key) {
        if (key.keyCode == 13) {
            $http({
                method: "POST",
                url: serverDir +"/Home/changedateNounyu/",
                data: {
                    "dateNouki": $scope.dateNouki,
                    "dateNounyu": $scope.dateNounyu
                },
            })
           .success(function (result) {
               $scope.validateDATA(result);
               if (result.indexOf("NG: ") > -1) {
                   alert(result);
               }
               else {
                   document.querySelector("#bumonCodeID").focus();
               }
           });
        }
    }

    //Get Tantou data
    $scope.listTantou = [];
    $scope.searchTantou = function (string) {

        if (string.length > 0) {
            $http.get(serverDir + "/About/getTantou?param=" + string)
            .success(function (data) {
                $scope.listTantou = data;
                if ($scope.listTantou.length == 1) {
                    $scope.tantouC = $scope.listTantou[0].split("  ,  ")[0];
                    $scope.tantouN = $scope.listTantou[0].split("  ,  ")[1];
                    $scope.tantouHide = true;

                    document.querySelector("#cyumonNo1ID").focus();
                }
                if ($scope.listTantou.length > 1) {
                    $scope.tantouHide = false;
                }
            });
        }
    }
    $scope.getValueTantou = function (string) {
        $scope.tantouC = string.split("  ,  ")[0];
        $scope.tantouN = string.split("  ,  ")[1];
        $scope.tantouHide = true;
        document.querySelector("#cyumonNo1ID").focus();
    }
    

    //Get Data from DBF File
    $scope.listCyumn = [];
    $scope.listJyuProcess = [];
    $scope.JyuuProcess = { jyuuCyuNo: "" };

    $scope.filterJyuProcess = [];
    $scope.odbcs = [
               { name: '0: Microsoft Text Driver (*.txt; *.csv)', code: '0' },
               { name: '1: Microsoft.Jet.OLEDB.4.0', code: '1' },
                { name: '2: StreamReader', code: '2' },
                { name: '3: Microsoft.ACE.OLEDB.12.0', code: '3' },
    ];
    $scope.odbc = $scope.odbcs[2];

    $scope.TESTBYDUC = function () {        
        $scope.abcTest = [];


        $http({
            method: "POST",
            url: serverDir +"/Home/TESTBYDUC/",
            data: {
                "testV":$scope.odbc.code
            },
        })
         .success(function (param1) {
             $scope.abcTest = param1;
             var a = $scope.abcTest;
             
         });

        return;


        $scope.employees = [
        { empName: 'vijay', sex: 'M', salary: 10000 },
        { empName: 'Aruna', sex: 'F', salary: 25000 },
        { empName: 'Jayaram', sex: 'M', salary: 23000 },
        { empName: 'john', sex: 'M', salary: 980000 },
        { empName: 'Rashmi', sex: 'F', salary: 12233 },
        ];
        $scope.filterEmployees = $filter('filter')($scope.employees, { empName: '' });

        var a = $scope.filterEmployees.length
        $scope.orderEmployees = $filter('orderBy')($scope.employees, '-empName');
        alert("OK");
    }
    
    $scope.delJyuProcess = function () {

        $http({
            method: "POST",
            url: serverDir +"/Home/delJyuProcess/",
            data: {
                //"listcyumn": listCyumn
                "Jyuu": $scope.JyuuProcess
            },
        })
         .success(function (param1) {
             //$scope.listJyuProcess = data;
             $scope.GetJyuProcess();
             $scope.wait_hide = !$scope.wait_hide;
             $scope.processHide = false;
             $scope.ClearAllFill();
         });
    }    

    //SEARCH INPUT VALUES
    $scope.searchInput = function () {
        //if ($scope.searchValue == "") {
        //    $scope.selectedAll = false;
        //}
    }

    //SYNC DATA BUTTON FROM EDI
    $scope.syncEDIDATA = function () {
        $scope.wait_hide = !$scope.wait_hide;
        $http({
            method: "POST",
            url: serverDir +"/About/syncEDIDATA/",
            data: {
                //"listcyumn": listCyumn
                //"listJyuProcess": $scope.listJyuProcess
            },
        })
          .success(function () {
              //$scope.listJyuProcess = data;
              $scope.GetJyuProcess();
              $scope.wait_hide = !$scope.wait_hide;
              $scope.processHide = false;
          });
    }


    //BUTTON GET JYUUCYU + EDIT JYUCYU
    $scope.GetJyuProcess = function () {
        $scope.processHide = !$scope.processHide;
        $scope.selectedAll = false;
        $scope.wait_hide = !$scope.wait_hide;
        $http({
            method: "POST",
            url: serverDir +"/About/GetJyuProcess/",
            data: {
                //"listSehins": $scope.selectedSehins
            },
        })
        .success(function (data) {
            $scope.listJyuProcess = data;
            $scope.searchValue = "";

            //EDIT BUTTON
            $scope.edit = function (jyuProcess) {
                $scope.wait_hide = false;

                $scope.JyuuProcess = jyuProcess;

                $scope.jyuuCyuNo = jyuProcess.Jyuucyuno;
                $scope.tokuiC = jyuProcess.Tokuicode;
                $scope.tokuiN = jyuProcess.Tokuiname;
                $scope.nounyuC = jyuProcess.Nounyucode;
                $scope.nounyuN = jyuProcess.Nonyuname;
                $scope.sehinC = jyuProcess.Seihincode;
                $scope.sehinZ = jyuProcess.Seihinzuban;
                $scope.sehinN = jyuProcess.Seihinname;
                $scope.sehinK = jyuProcess.Seihinkisyu;
                $scope.quantity = jyuProcess.Quantity;
                $scope.sehinT = jyuProcess.Tanka;

                $scope.bumonCode = "999";
                $scope.bumonName = "データCONV";
                $scope.tantouC = "999";
                $scope.tantouN = "データCONV";

                $scope.tantouC = jyuProcess.Tantoucode;
                $scope.tantouN = $http({
                    method: "POST",
                    url: serverDir +"/About/findPIC/",
                    data: {
                        "JTANCD": jyuProcess.Tantoucode
                    },
                }).success(function (tantouN) {
                    $scope.tantouN = tantouN;
                });

                //$scope.cyumonStyle = "";
                $scope.cyumonNo1 = jyuProcess.Cyumonno1;
                $scope.cyumonNo2 = jyuProcess.Cyumonno2;
                $scope.seizou = jyuProcess.Seizou;
                $scope.bikou = jyuProcess.Bikou;
                $scope.Status = jyuProcess.Status;

                /*
                //Set Date for Nouki                        
                var yyyy = jyuProcess.Nouki.substring(0, 4);
                var MM = jyuProcess.Nouki.substring(4, 6);
                var dd = jyuProcess.Nouki.substring(6, 8);
                var nouki = new Date(yyyy + "-" + MM + "-" + dd);

                $scope.dateNouki = yyyy + "-" + MM + "-" + dd;

                //Set Date for Nounyu
                yyyy = jyuProcess.Nounyu.substring(0, 4);
                MM = jyuProcess.Nounyu.substring(4, 6);
                dd = jyuProcess.Nounyu.substring(6, 8);
                var nounyu = new Date(yyyy + "-" + MM + "-" + dd);

                $scope.dateNounyu = yyyy + "-" + MM + "-" + dd;
                */

                $scope.dateNouki = jyuProcess.Nouki;
                $scope.dateNounyu = jyuProcess.Nounyu;


                $scope.validateDATA(jyuProcess.Status);

                $location.hash('jyuReg');
                $anchorScroll();

                $scope.btnRegDis = "false";
                $scope.wait_hide = true;
            }

            //ALL SELECTED CLICK
            $scope.selectedALLJYUCYU = function () {
                $scope.selectedAll = !$scope.selectedAll;

                angular.forEach($filter('filter')($scope.listJyuProcess, $scope.searchValue), function (jyuu, key) {
                    /*
                    angular.forEach(jyuu, function (value, key) {
                        alert(value);
                    });*/

                    if ($scope.selectedAll == false) {
                        jyuu.selected = false;
                    }
                    else jyuu.selected = true;
                });

            }

            //$scope.processHide = false;
            $scope.wait_hide = true;
        });
    }

    //Register new Order BUTTON
    $scope.registerOrder = function () {        

        var result = "";
        if ($scope.tokuiC == "" || $scope.tokuiC == null) {
            result += "NG: " + langCollection.cusL + langCollection.inputError + "\n";
        }
        if ($scope.nounyuC == "" || $scope.nounyuC == null) {
            result += "NG: " + langCollection.deliveryLocL + langCollection.inputError + "\n";
        }
        if ($scope.sehinC == "" || $scope.sehinC == null) {
            result += "NG: " + langCollection.productL + langCollection.inputError + "\n";
        }
        if ($scope.bumonCode == "" || $scope.bumonCode == null) {
            result += "NG: " + langCollection.departmentL + langCollection.inputError + "\n";
        }

        if ($scope.tantouC == "" || $scope.tantouC == null) {
            result += "NG: " + langCollection.picL + langCollection.inputError + "\n";
        }

        if (result.indexOf("NG: ") > -1) {            
            return alert(result);
        }

        $scope.wait_hide = !$scope.wait_hide;
        $scope.this = {
            "Jyuucyuno": $scope.jyuuCyuNo,
            "Tokuicode": $scope.tokuiC, "Tokuiname": $scope.tokuiN,
            "Nounyucode": $scope.nounyuC, "Nonyuname": $scope.nounyuN,
            "Seihincode": $scope.sehinC, "Seihinzuban": $scope.sehinZ, "Seihinname": $scope.sehinN, "Seihinkisyu": $scope.sehinK,
            "Quantity": $scope.quantity, "Tanka": $scope.sehinT,
            "Nouki": $scope.dateNouki, "Nounyu": $scope.dateNounyu,
            "Bumoncode": $scope.bumonCode, "Bumonname": $scope.bumonName,
            "Tantoucode": $scope.tantouC, "Tantouname": $scope.tantouN,
            "Cyumonno1": $scope.cyumonNo1, "Cyumonno2": $scope.cyumonNo2,
            "Seizou": $scope.seizou, "Bikou": $scope.bikou, "Ndate": "20191111", "Status": $scope.Status,
        };

        $http({
            method: "POST",
            url: serverDir +"/About/validateDATA/",
            data: {
                "JyuuProcess": $scope.this
            },
        })
        .success(function (testValue) {
            //testValue = data;
            if (testValue.indexOf("NG: ") < 0) {
                $http({
                    method: "POST",
                    url: serverDir +"/About/registerOrder/",
                    data: {
                        "newYuucyu": $scope.this
                    },
                })
                    .success(function (data, $translate) {
                        if ($scope.this.tantouName < 1) {
                            alert('Empty');
                        }
                        else {
                            $scope.ClearAllFill();
                            alert(langCollection.registerJyuucyuu + data);
                            document.querySelector("#tokuiC").focus();
                        }
                    });
                $scope.wait_hide = !$scope.wait_hide;
            }
            else {

                if (testValue == "NG: TANKA not correct \n") {                    

                    if (confirm(langCollection.confirmTanka)) {
                        $http({
                            method: "POST",
                            url: serverDir + "/About/registerOrder/",
                            data: {
                                "newYuucyu": $scope.this
                            },
                        })
                       .success(function (data, $translate) {
                           if ($scope.this.tantouName < 1) {
                               alert('Empty');
                           }
                           else {
                               $scope.ClearAllFill();
                               alert(langCollection.registerJyuucyuu + data);
                               document.querySelector("#tokuiC").focus();
                           }
                       });
                        $scope.wait_hide = true;
                    }
                    else {
                        $scope.wait_hide = true;
                        return;
                    }
                }
                else if (testValue == "NG: CYUNO \n") {
                    alert(langCollection.customerOrderDoube);
                }
                else if (testValue == "NG: CYUNO1 \n") {
                    alert(langCollection.customerOrderL + langCollection.inputError);
                }

                else {
                    $scope.wait_hide = true;
                    alert("Pls check ERRORs \n" + testValue);
                    $scope.validateDATA(data);
                }
                
            }

            $scope.wait_hide = true;
        });


    }

    //BUTTON UPDATE
    $scope.savetoJyuEDI = function () {
        $scope.wait_hide = !$scope.wait_hide;

        $scope.this = {
            "Jyuucyuno": $scope.jyuuCyuNo,
            "Tokuicode": $scope.tokuiC, "Tokuiname": $scope.tokuiN,
            "Nounyucode": $scope.nounyuC, "Nonyuname": $scope.nounyuN,
            "Seihincode": $scope.sehinC, "Seihinzuban": $scope.sehinZ, "Seihinname": $scope.sehinN, "Seihinkisyu": $scope.sehinK,
            "Quantity": $scope.quantity, "Tanka": $scope.sehinT,
            "Nouki": $scope.dateNouki, "Nounyu": $scope.dateNounyu,
            "Bumoncode": $scope.bumonCode, "Bumonname": $scope.bumonName,
            "Tantoucode": $scope.tantouC, "Tantouname": $scope.tantouN,
            "Cyumonno1": $scope.cyumonNo1, "Cyumonno2": $scope.cyumonNo2,
            "Seizou": $scope.seizou, "Bikou": $scope.bikou, "Ndate": "20191111", "Status": $scope.Status,
        };

        if ($scope.JyuuProcess.jyuuCyuNo != "") {

            $http({
                method: "POST",
                url: serverDir +"/About/checkData/",
                data: {
                    //"this": $scope.this,
                    "JyuuProcess": $scope.this
                },
            })
            .success(function (result) {
                if (result.indexOf("NG: ") > -1) {
                    alert("Please check ERRORS as below: \n" + result);
                    $scope.wait_hide = !$scope.wait_hide;
                }
                else {
                    $http({
                        method: "POST",
                        url: serverDir +"/About/savetoJyuEDI/",
                        data: {
                            //"this": $scope.this,
                            "JyuuProcess": $scope.this
                        },
                    })
                   .success(function (data) {
                       $scope.listJyuProcess = data;
                       //$scope.ClearAllFill();
                       //
                       $scope.wait_hide = !$scope.wait_hide;

                       $("html, body").stop().animate({
                           scrollTop: $('#jyuReg').offset().top - 40
                       }, '500', 'linear');


                       $location.hash('jyuReg');
                       $anchorScroll();


                       $scope.btnRegDis = !$scope.btnRegDis;
                       $scope.ClearAllFill();
                   });
                }

            });
        }
        else {
            alert("Pls select one of Processing Order");
            $scope.wait_hide = !$scope.wait_hide;
        }

    }

    //BUTTON CANCEL
    $scope.ClearAllFill = function () {

        $scope.wait_hide = false;

        $scope.jyuuCyuNo = "";
        $scope.tokuiC = "";
        $scope.tokuiN = "得意名称";
        $scope.nounyuC = "";
        $scope.nounyuN = "納入場所";
        $scope.sehinC = "";
        $scope.sehinZ = "図番番号";
        $scope.sehinN = "製品名称";
        $scope.sehinK = "機種";
        $scope.quantity = "1";
        $scope.sehinT = "0";

        $scope.bumonCode = "999";
        $scope.bumonName = "データCONV";
        //$scope.tantouC = "";
        //$scope.tantouN = "担当者";
        $scope.cyumonNo1 = "";
        $scope.cyumonNo2 = "";        
        $scope.seizou = "";
        $scope.bikou = "";
        $scope.Status = "";
        $scope.cyumonReadOnly = "";
        $scope.dateNouki = "";
        $scope.dateNounyu = "";

        $scope.btnRegDis = true;

        var date = new Date();

        //Add 10 days
        date.setDate(date.getDate() + 9);

        var yyyy = date.getFullYear() + '';
        var MM = ('0' + (date.getMonth() + 1)).slice(-2);
        var dd = ('0' + date.getDate()).slice(-2);

        //$scope.dateNouki = yyyy + MM + dd;

        //Add 10 days
        date.setDate(date.getDate() - 1);

        var yyyy = date.getFullYear() + '';
        var MM = ('0' + (date.getMonth() + 1)).slice(-2);
        var dd = ('0' + date.getDate()).slice(-2);
        //$scope.dateNounyu = yyyy + MM + dd;

        $scope.JyuuProcess = { jyuuCyuNo: "" };
        $scope.wait_hide = true;
        $scope.btnRegDis = !$scope.btnRegDis;

        document.querySelector("#tokuiC").focus();
    }


    //BUTTON COMPLETE ALL EDI
    $scope.completeEDI = function () {
        $scope.wait_hide = !$scope.wait_hide;
        var strcond = "In ('";
        var strall = "In ('";

        angular.forEach($filter('filter')($scope.listJyuProcess, { selected: true }), function (jyu) {
            strcond = strcond + jyu.Jyuucyuno + "','";
        });

        angular.forEach($scope.listJyuProcess, function (jyu) {
            strall = strall + jyu.Jyuucyuno + "','";
        });

        if (strcond == "In ('") {
            alert("Pls select ");
            $scope.wait_hide = true;
        }
        else {
            strcond = strcond.substring(0, strcond.length - 2) + ")";
            strall = strall.substring(0, strall.length - 2) + ")";

            $http({
                method: "POST",
                url: serverDir +"/About/completeEDI/",
                data: {
                    //"listcyumn": listCyumn
                    "listselectedProcess": strcond,
                    "listJyuProcess": strall
                },
            })
              .success(function (data) {
                  $scope.listJyuProcess = data;
                  $scope.wait_hide = true;
              });
        }
    }

    //BUTTON DELETE SELECTED ORDER
    $scope.deleteSelectedOrder = function () {
        $scope.wait_hide = !$scope.wait_hide;
        var strcond = "In ('";
        var strall = "In ('";

        angular.forEach($filter('filter')($scope.listJyuProcess, { selected: true }), function (jyu) {
            strcond = strcond + jyu.Jyuucyuno + "','";
        });

        angular.forEach($scope.listJyuProcess, function (jyu) {
            strall = strall + jyu.Jyuucyuno + "','";
        });

        if (strcond == "In ('") alert("Pls select ");
        else {
            strcond = strcond.substring(0, strcond.length - 2) + ")";
            strall = strall.substring(0, strall.length - 2) + ")";
            $http({
                method: "POST",
                url: serverDir +"/Home/deleteSelectedOrder/",
                data: {
                    //"this": $scope.this,
                    "listselectedProcess": strcond,
                    "listJyuProcess": strall
                },
            })
            .success(function (result) {
                $scope.listJyuProcess = result;
                $scope.wait_hide = true;
            });
        }
        //alert(strcond);

        $scope.wait_hide = true;
        return;
    }

    //BUTTON UPDATE PROCESS
    $scope.updateOrderProcess = function () {
        $scope.GetJyuProcess();
        $scope.processHide = false;
    }

    //BUTTON CLOSE PROCESS
    $scope.closeOrderProcess = function () {

        var isConfirmed = confirm("Are you sure to delete this record ?");
        if (isConfirmed) {
            vm.users.splice(index, 1);
        } else {
            return false;
        }

        $scope.processHide = true;
    }

    //SortbyCol
    // column to sort
    $scope.column = 'CYUNO';

    // sort ordering (Ascending or Descending). Set true for desending
    $scope.reverse = false;

    // called on header click
    $scope.sortColumn = function (col) {
        $scope.column = col;
        if ($scope.reverse) {
            $scope.reverse = false;
            $scope.reverseclass = 'arrow-up';
        } else {
            $scope.reverse = true;
            $scope.reverseclass = 'arrow-down';
        }
    };

    // remove and change class
    $scope.sortClass = function (col) {
        if ($scope.column == col) {
            if ($scope.reverse) {
                return 'arrow-down';
            } else {
                return 'arrow-up';
            }
        } else {
            return '';
        }
    }

    //CHECK INPUT DATA
    $scope.validateDATA = function (error) {
        $scope.wait_hide = !$scope.wait_hide;

        if (error.indexOf("NG: ZAICD") > -1) {
            $scope.sehinCStyle = {
                "color": "red"
            }
        } else {
            $scope.sehinCStyle = {
                "color": "black"
            }
        }

        if (error.indexOf("NG: PRODUCT NAME") > -1) {
            $scope.sehinNStyle = {
                "color": "red"
            }
        } else {
            $scope.sehinNStyle = {
                "color": "black"
            }
        }
        if (error.indexOf("NG: KISYU") > -1) {
            $scope.sehinKStyle = {
                "color": "red"
            }
        } else {
            $scope.sehinKStyle = {
                "color": "black"
            }
        }
        if (error.indexOf("NG: TANKA") > -1) {
            $scope.sehinTStyle = {
                "color": "red"
            }
        } else {
            $scope.sehinTStyle = {
                "color": "black"
            }
        }

        if (error.indexOf("NOUKIDATE") > -1) {
            $scope.noukiDateStyle = {
                "color": "red"
            }
        } else {
            $scope.noukiDateStyle = {
                "color": "black"
            }
        }

        if (error.indexOf("NOUNYUDATE") > -1) {
            $scope.nounyuDateStyle = {
                "color": "red"
            }
        } else {
            $scope.nounyuDateStyle = {
                "color": "black"
            }
        }

        $scope.wait_hide = true;
    }


});


jyuucyu.controller("seisancontroller", function ($location, $filter, $anchorScroll, $scope, $http, $translate) {

    $http.get(serverDir + "/About/getLangSel")
            .success(function (data) {
                $scope.langMulti = data;
                $translate.use(data);
            });

    //$translate.use("vn");

    //Get Tokuisaki + Nounyu data
    $scope.createButtonHide = true;

    $scope.listTokui = []
    $scope.searchTokui = function (string) {
        if (string.length > 0) {
            $http.get(serverDir + "/About/GetTokuiCD?param=" + string)
            .success(function (data) {
                $scope.listTokui = data;
                if ($scope.listTokui.length == 1) {
                    $scope.tokuiC = $scope.listTokui[0].split("  ,  ")[0];
                    $scope.tokuiN = $scope.listTokui[0].split("  ,  ")[1];

                    $scope.tokuiHide = true;
                    $scope.createButtonHide = false;
                }
                if ($scope.listTokui.length > 1) {
                    $scope.tokuiHide = false;
                }
            });
        }
        else $scope.tokuiHide = true;
    }
    $scope.getTokui = function (string) {
        $scope.tokuiC = string.split("  ,  ")[0];
        $scope.tokuiN = string.split("  ,  ")[1];
        $scope.tokuiHide = true;
        $scope.createButtonHide = false;        
    }

    
    $scope.seiProcess = [];
    $scope.listSeiProcess = [];

    //Get sehin data
    $scope.listSehin = [];
    $scope.searchSehin = function (seiProcess) {
        $scope.seiProcess = seiProcess;
        string = seiProcess.ZAICD + " - " + $scope.kubun.code;
        if (seiProcess.ZAICD > 4) {
            $http.get(serverDir + "/About/GetSehin?param=" + string)
            .success(function (data) {
                $scope.listSehin = data;
                if ($scope.listSehin.length == 1) {
                    $scope.seiProcess.ZAICD = $scope.listSehin[0].split("  ,  ")[0];
                    $scope.seiProcess.ZUBAN = $scope.listSehin[0].split("  ,  ")[1];
                    $scope.seiProcess.NAME = $scope.listSehin[0].split("  ,  ")[2];
                    $scope.seiProcess.KISYU = $scope.listSehin[0].split("  ,  ")[3];
                    $scope.seiProcess.JTANKA = $scope.listSehin[0].split("  ,  ")[4];
                    $scope.updateSeiProcess($scope.seiProcess);
                    $scope.sehinHide = true;
                    document.querySelector("#seisu" + seiProcess.SIYNO + "ID").select().focus();
                }
                if ($scope.listSehin.length > 1) {
                    
                    $scope.sehinListStyle = {
                    //"top": "0px",
                    //"left": "0px",
                    "color":"red",
                    //"background-position":"100px 200px",
                    //"height": "250px",
                    //"width": "500px",
                    //"background-color": "red",

                    }
                    if ($scope.sehinHide) {
                        $scope.sehinHide = false;
                        document.querySelector("#seisu" + $scope.seiProcess.SIYNO + "ID").focus();
                    }
                }
            });
        }
        //document.querySelector("#searchInput").select().focus();
        //$scope.processHide = false;
    }
    //SELECT  SEIPROCESS SEHIN
    $scope.selectSehin = function (string) {
        $scope.seiProcess.ZAICD = string.split("  ,  ")[0];
        $scope.seiProcess.ZUBAN = string.split("  ,  ")[1];
        $scope.seiProcess.NAME = string.split("  ,  ")[2];
        $scope.seiProcess.KISYU = string.split("  ,  ")[3];
        $scope.seiProcess.JTANKA = string.split("  ,  ")[4];

        $scope.updateSeiProcess($scope.seiProcess);
        $scope.sehinHide = true;
        document.querySelector("#seisu" + $scope.seiProcess.SIYNO + "ID").select().focus();

    }


    //BUTTON GET SEIPROCESS
    $scope.GetSeiProcess = function () {
        $scope.selectedAll = false;
        $scope.wait_hide = !$scope.wait_hide;
        $http({
            method: "POST",
            url: serverDir +"/Home/GetSeiProcess/",
            data: {
                //"listSehins": $scope.selectedSehins
            },
        })
        .success(function (data) {
            $scope.listSeiProcess = data;                        

            //ALL SELECTED CLICK
            $scope.selectedALLSEI = function () {
                $scope.selectedAll = !$scope.selectedAll;

                angular.forEach($filter('filter')(($filter('filter')($scope.listSeiProcess, { TOKCD: $scope.tokuiC, ZAIKB: $scope.kubun.code })), $scope.searchValue), function (sei, key) {
                    /*
                    angular.forEach(jyuu, function (value, key) {
                        alert(value);
                    });*/

                    if ($scope.selectedAll == false) {
                        sei.selected = false;
                    }
                    else sei.selected = true;
                });
            }

            $scope.processHide = !$scope.processHide;
            $scope.wait_hide = true;
        });
    }
    
    
    //DEL SEIPROCESS
    $scope.delSeiProcess = function (seiProcess) {
        $scope.wait_hide = false;
        $http({
            method: "POST",
            url: serverDir +"/Home/delSeiProcess/",
            data: {
                "SeiProcess": seiProcess
            },
        })
          .success(function () {
              $scope.GetSeiProcess();
              $scope.wait_hide = true;
              $scope.processHide = true;
          });
    }

    //CREATE NEW PROCESS
    $scope.createSeiProcess = function () {
        $scope.wait_hide = false;
        $scope.this = {
             "selected": $scope.selected,
            "SIYNO": "", "SIYKB": "",
            "TOKCD": $scope.tokuiC, "TNAME": $scope.tokuiN,
            "ZAIKB": $scope.kubun.code, "ZAICD": "", "ZUBAN": "", "KISYU": "",
            "SEISU": "", "JTANKA": "",
            "NOUKI": "", "NAIJI": "",
            "HIKSU": "", "TMKBN": "",
            "SKKBN": "", "NDATE": "",
            "Status": "", 
        };
        $http({
            method: "POST",
            url: serverDir +"/Home/createSeiProcess/",
            data: {
                "SeiProcess": $scope.this
            },
        })
          .success(function (result) {
              if (result == "False")
              {
                  alert("NG: TKCD NOT FOUND");
              }

              $scope.GetSeiProcess();
              $scope.wait_hide = true;

              $scope.processHide = true;
          });
    }

    //UPDATE SEISU
    $scope.updateSeiSu = function (seiProcess, event) {
        if (event.keyCode == 13) {            
            $scope.wait_hide = false;
            if (!isNaN(seiProcess.SEISU)) {
                $scope.updateSeiProcess(seiProcess);
            }
            else {
                alert("input number");
                seiProcess.SEISU = seiProcess.SEISU.slice(0, -1);
            }

            //var a = $scope.getNextID(seiProcess.SIYNO);


            //document.querySelector("#searchInputID").focus();

            document.querySelector("#nouki" + seiProcess.SIYNO + "ID").select().focus();
            $scope.wait_hide = true;
            $scope.processHide = false;
        }
        else {
            if (isNaN(seiProcess.SEISU)) {
                alert("input number");
                seiProcess.SEISU = seiProcess.SEISU.slice(0, -1);

            }
            else {
                if (event.char == '.') {
                    alert("input number");
                    seiProcess.SEISU = seiProcess.SEISU.slice(0, -1);
                }
            }
        }
    }


    $scope.tesssst = function (seiProcess) {
        if (!$scope.sehinHide) {
            var a = document.querySelector("#searchInputID")
            a.focus();
            var b = seiProcess.ZAICD.length
            a.setSelectionRange(b, b);            
        }
    };

    //UPDATE NOUKI
    $scope.updateNouki = function (seiProcess, event) {
        if (event.keyCode == 13) {
            $scope.wait_hide = false;

            if (seiProcess.NOUKI.length == 4) {
                var date = new Date();
                seiProcess.NOUKI = date.getFullYear() + seiProcess.NOUKI + '';
            }


            $http({
                method: "POST",
                url: serverDir +"/Home/updateSeiProcess/",
                data: {
                    "SeiProcess": seiProcess
                },
            })
              .success(function (result) {
                  //go to nextID
                  if (result.indexOf("NG: ") > -1) alert(result);
                  else {
                      var nextID = $scope.getNextID(seiProcess.SIYNO);                      
                      if (nextID != "last") {
                          document.querySelector("#sehin" + nextID + "ID").select().focus();
                          $scope.wait_hide = true;
                      }
                  }
              });
            $scope.wait_hide = true;
        }
    }


    //UPDATE SEIPROCESS
    $scope.updateSeiProcess = function (seiProcess, nextID) {
        $scope.wait_hide = false;
        $http({
            method: "POST",
            url: serverDir +"/Home/updateSeiProcess/",
            data: {
                "SeiProcess": seiProcess
            },
        })
          .success(function () {
              //go to nextID
              //$scope.GetSeiProcess();
              //document.querySelector("#nouki" + seiProcess.SIYNO + "ID").select().focus();
              $scope.wait_hide = true;
              //$scope.processHide = false;

          });
    }

    //REFRESH DATA
    $scope.refreshSeiProcess = function () {
        $scope.wait_hide = false;
        $scope.GetSeiProcess();
        $scope.processHide = true;
        $scope.wait_hide = false;
    }

    //CONFRIM IMPORT TO TW
    $scope.TWSeiProcess = function () {
        $scope.wait_hide = false;

        var strcond = "In ('";
        //var strall = "In ('";

        angular.forEach($filter('filter')($scope.listSeiProcess, { selected: true }), function (sei) {
            strcond = strcond + sei.SIYNO + "','";
        });

        //angular.forEach($scope.listSeiProcess, function (sei) {
        //    strall = strall + sei.SIYNO + "','";
        //});

        if (strcond == "In ('") {
            alert(langCollection.nonSelect);
            $scope.wait_hide = true;
        }
        else {
            strcond = strcond.substring(0, strcond.length - 2) + ")";
            //strall = strall.substring(0, strall.length - 2) + ")";
            $http({
                method: "POST",
                url: serverDir +"/Home/TWSeiProcess/",
                data: {
                    "listseiSelected": strcond
                },
            })
              .success(function () {
                  $scope.GetSeiProcess();
                  $scope.wait_hide = true;
                  $scope.processHide = true;
              });
        }
    }

    $scope.getNextID = function(currentID){
        var listFilter = $filter('filter')(($filter('filter')($scope.listSeiProcess, { TOKCD: $scope.tokuiC, ZAIKB: $scope.kubun.code })), $scope.searchValue);
        {
            for (i = 0; i < listFilter.length; i++) {                
                if (listFilter[i].SIYNO == currentID) {
                    if (i < listFilter.length - 1) {
                        return listFilter[i + 1].SIYNO;
                    }
                    else return "last";
                }
            }
            return "";
        }
    }


    //Get Tantou data
    $scope.listTantou = [];
    $scope.searchTantou = function (string) {
        if (string.length > 0) {
            $http.get(serverDir + "/About/getTantou?param=" + string)
            .success(function (data) {
                $scope.listTantou = data;
                if ($scope.listTantou.length == 1) {
                    $scope.tantouC = $scope.listTantou[0].split("  ,  ")[0];
                    $scope.tantouN = $scope.listTantou[0].split("  ,  ")[1];
                    $scope.tantouHide = true;
                }
                if ($scope.listTantou.length > 1) {
                    $scope.tantouHide = false;
                }
            });
        }
    }
    $scope.getValueTantou = function (string) {
        $scope.tantouC = string.split("  ,  ")[0];
        $scope.tantouN = string.split("  ,  ")[1];
        $scope.tantouHide = true;
    }



    //Get Data from DBF File
    $scope.listCyumn = [];
    $scope.listSeiProcess = [];
    $scope.JyuuProcess = { jyuuCyuNo: "" };

    $scope.filterJyuProcess = [];

    $scope.TESTBYDUC = function () {
        
        alert( angular.isNumber(45));
    }

    $scope.kubunSelect = function (kubunV) {
        if (kubunV.code == 1) $scope.buhinCheckboxHide = true;
        else $scope.buhinCheckboxHide = false;
    }
    
     
    
    //SYNC DATA BUTTON FROM EDI
    $scope.getSeiEDI = function () {
        $scope.wait_hide = !$scope.wait_hide;

        if ($scope.kaitenCheck == null) $scope.kaitenCheck = false;

        $http({
            method: "POST",
            url: serverDir +"/Home/getSeiEDI/",
            data: {
                "kubun": $scope.kubun.code, 
                "kaitenCheck":$scope.kaitenCheck
            },
        })
          .success(function () {
              if ($scope.kaitenCheck == true) {
                  $scope.kubun = $scope.kubuns[1];
                  $scope.buhinCheckboxHide = true;
              }
              $scope.GetSeiProcess();
              $scope.wait_hide = true;
              $scope.processHide = true;
          });
    }
    
   
    //Register new Order BUTTON
    $scope.registerOrder = function () {
        $scope.wait_hide = !$scope.wait_hide;
        $scope.this = {
            "Jyuucyuno": $scope.jyuuCyuNo,
            "Tokuicode": $scope.tokuiC, "Tokuiname": $scope.tokuiN,
            "Nounyucode": $scope.nounyuC, "Nonyuname": $scope.nounyuN,
            "Seihincode": $scope.sehinC, "Seihinzuban": $scope.sehinZ, "Seihinname": $scope.sehinN, "Seihinkisyu": $scope.sehinK,
            "Quantity": $scope.quantity, "Tanka": $scope.sehinT,
            "Nouki": $scope.dateNouki, "Nounyu": $scope.dateNounyu,
            "Bumoncode": $scope.bumonCode, "Bumonname": $scope.bumonName,
            "Tantoucode": $scope.tantouC, "Tantouname": $scope.tantouN,
            "Cyumonno1": $scope.cyumonNo1, "Cyumonno2": $scope.cyumonNo2,
            "Seizou": $scope.seizou, "Bikou": $scope.bikou, "Ndate": "20191111", "Status": $scope.Status,
        };

        $http({
            method: "POST",
            url: serverDir +"/About/validateDATA/",
            data: {
                "JyuuProcess": $scope.this
            },
        })
        .success(function (data) {
            testValue = data;
            if (testValue.indexOf("NG: ") < 0) {
                $http({
                    method: "POST",
                    url: serverDir +"/About/registerOrder/",
                    data: {
                        "newYuucyu": $scope.this
                    },
                })
                    .success(function (data, $translate) {
                        if ($scope.this.tantouName < 1) {
                            alert('Empty');
                        }
                        else {
                            $scope.ClearAllFill();
                            alert(langCollection.registerJyuucyuu + data);

                            /*
                            $http({
                                method: "POST",
                                url: serverDir +"/About/getLangSel/",
                                data: {
                                    //"newYuucyu": $scope.this
                                },
                            })
                            .success(function (lang) {
                                $scope.langMulti = lang;
                                $http.get('/app.dev/TestJS/lang_' + $scope.langMulti + '.json').success(function (data2) {
                                    console.log("success!");
                                    if (data.indexOf("NG: ") > -1) {
                                        alert(data2.registerJyuucyuu + data);
                                        
                                    }
                                    else {
                                        alert(data);
                                    }
                                });
                            })*/
                        }
                    });
                $scope.wait_hide = !$scope.wait_hide;
            }
            else {
                alert("Pls check ERRORs \n" + testValue);

                $scope.validateDATA(data);
            }

            $scope.wait_hide = true;
        });


    }

    //BUTTON UPDATE
    $scope.savetoJyuEDI = function () {
        $scope.wait_hide = !$scope.wait_hide;

        $scope.this = {
            "Jyuucyuno": $scope.jyuuCyuNo,
            "Tokuicode": $scope.tokuiC, "Tokuiname": $scope.tokuiN,
            "Nounyucode": $scope.nounyuC, "Nonyuname": $scope.nounyuN,
            "Seihincode": $scope.sehinC, "Seihinzuban": $scope.sehinZ, "Seihinname": $scope.sehinN, "Seihinkisyu": $scope.sehinK,
            "Quantity": $scope.quantity, "Tanka": $scope.sehinT,
            "Nouki": $scope.dateNouki, "Nounyu": $scope.dateNounyu,
            "Bumoncode": $scope.bumonCode, "Bumonname": $scope.bumonName,
            "Tantoucode": $scope.tantouC, "Tantouname": $scope.tantouN,
            "Cyumonno1": $scope.cyumonNo1, "Cyumonno2": $scope.cyumonNo2,
            "Seizou": $scope.seizou, "Bikou": $scope.bikou, "Ndate": "20191111", "Status": $scope.Status,
        };

        if ($scope.JyuuProcess.jyuuCyuNo != "") {

            $http({
                method: "POST",
                url: serverDir +"/About/checkData/",
                data: {
                    //"this": $scope.this,
                    "JyuuProcess": $scope.this
                },
            })
            .success(function (result) {
                if (result.indexOf("NG: ") > -1) {
                    alert("Please check ERRORS as below: \n" + result);
                    $scope.wait_hide = !$scope.wait_hide;
                }
                else {
                    $http({
                        method: "POST",
                        url: serverDir +"/About/savetoJyuEDI/",
                        data: {
                            //"this": $scope.this,
                            "JyuuProcess": $scope.this
                        },
                    })
                   .success(function (data) {
                       $scope.listSeiProcess = data;
                       //$scope.ClearAllFill();
                       //
                       $scope.wait_hide = !$scope.wait_hide;

                       $("html, body").stop().animate({
                           scrollTop: $('#jyuReg').offset().top - 40
                       }, '500', 'linear');


                       $location.hash('jyuReg');
                       $anchorScroll();


                       $scope.btnRegDis = !$scope.btnRegDis;
                   });
                }

            });
        }
        else {
            alert(langCollection.nonSelect);
            $scope.wait_hide = !$scope.wait_hide;
        }

    }
      

    //BUTTON COMPLETE ALL EDI
    $scope.completeEDI = function () {
        $scope.wait_hide = !$scope.wait_hide;


        var strcond = "In ('";
        //var strall = "In ('";

        angular.forEach($filter('filter')($scope.listSeiProcess, { selected: true }), function (sei) {
            strcond = strcond + sei.SIYNO + "','";
        });

        //angular.forEach($scope.listSeiProcess, function (sei) {
        //    strall = strall + sei.SIYNO + "','";
        //});

        if (strcond == "In ('") {
            alert(langCollection.nonSelect);
            $scope.wait_hide = true;
        }
        else {
            strcond = strcond.substring(0, strcond.length - 2) + ")";
            strall = strall.substring(0, strall.length - 2) + ")";
            $http({
                method: "POST",
                url: serverDir +"/About/completeEDI/",
                data: {
                    //"listcyumn": listCyumn
                    "listSelectedSei": strcond
                },
            })
              .success(function (data) {
                  $scope.listSeiProcess = data;
                  $scope.wait_hide = true;
              });
        }
    }

    //BUTTON DELETE SELECTED SEIYO
    $scope.deleteSelectedSeiyo = function () {
        $scope.wait_hide = !$scope.wait_hide;
        var strcond = "In ('";
        var strall = "In ('";

        angular.forEach($filter('filter')($scope.listSeiProcess, { selected: true }), function (sei) {
            strcond = strcond + sei.SIYNO + "','";
        });

        angular.forEach($scope.listSeiProcess, function (sei) {
            strall = strall + sei.SIYNO + "','";
        });

        if (strcond == "In ('") alert("Pls select ");
        else {
            strcond = strcond.substring(0, strcond.length - 2) + ")";
            strall = strall.substring(0, strall.length - 2) + ")";
            $http({
                method: "POST",
                url: serverDir +"/Home/deleteSelectedSeiyo/",
                data: {
                    //"this": $scope.this,
                    "listselectedProcess": strcond,
                    "listSeiProcess": strall
                },
            })
            .success(function (result) {
                $scope.listSeiProcess = result;
                $scope.wait_hide = true;
            });
        }
        //alert(strcond);

        $scope.wait_hide = true;
        return;
    }



    //BUTTON CLOSE PROCESS
    $scope.closeOrderProcess = function () {
        $scope.processHide = true;
    }

    //SortbyCol
    // column to sort
    $scope.column = 'SIYNO';

    // sort ordering (Ascending or Descending). Set true for desending
    $scope.reverse = false;

    // called on header click
    $scope.sortColumn = function (col) {
        $scope.column = col;
        if ($scope.reverse) {
            $scope.reverse = false;
            $scope.reverseclass = 'arrow-up';
        } else {
            $scope.reverse = true;
            $scope.reverseclass = 'arrow-down';
        }
    };

    // remove and change class
    $scope.sortClass = function (col) {
        if ($scope.column == col) {
            if ($scope.reverse) {
                return 'arrow-down';
            } else {
                return 'arrow-up';
            }
        } else {
            return '';
        }
    }

    //CHECK INPUT DATA
    $scope.validateDATA = function (error) {
        $scope.wait_hide = !$scope.wait_hide;

        if (error.indexOf("NG: ZAICD") > -1) {
            $scope.sehinCStyle = {
                "color": "red"
            }
        } else {
            $scope.sehinCStyle = {
                "color": "black"
            }
        }

        if (error.indexOf("NG: PRODUCT NAME") > -1) {
            $scope.sehinNStyle = {
                "color": "red"
            }
        } else {
            $scope.sehinNStyle = {
                "color": "black"
            }
        }
        if (error.indexOf("NG: KISYU") > -1) {
            $scope.sehinKStyle = {
                "color": "red"
            }
        } else {
            $scope.sehinKStyle = {
                "color": "black"
            }
        }
        if (error.indexOf("NG: TANKA") > -1) {
            $scope.sehinTStyle = {
                "color": "red"
            }
        } else {
            $scope.sehinTStyle = {
                "color": "black"
            }
        }

        if (error.indexOf("NOUKIDATE") > -1) {
            $scope.noukiDateStyle = {
                "color": "red"
            }
        } else {
            $scope.noukiDateStyle = {
                "color": "black"
            }
        }

        if (error.indexOf("NOUNYUDATE") > -1) {
            $scope.nounyuDateStyle = {
                "color": "red"
            }
        } else {
            $scope.nounyuDateStyle = {
                "color": "black"
            }
        }
    }

});


jyuucyu.config(["datepickerConfig", "datepickerPopupConfig", "timepickerConfig",
         function (datepickerConfig, datepickerPopupConfig, timepickerConfig) {
             datepickerConfig.showWeeks = false; // 週番号（日本では馴染みが薄い）を非表示にする
             datepickerConfig.dayTitleFormat = "yyyy年 MMMM";
             datepickerPopupConfig.currentText = "本日";
             datepickerPopupConfig.clearText = "消去";
             datepickerPopupConfig.toggleWeeksText = "週番号";
             datepickerPopupConfig.closeText = "閉じる";
             timepickerConfig.showMeridian = false; // 時刻を24時間表示にする（デフォルトでは12時間表示）
             datepickerPopupConfig.language = 'jp';
         }])

jyuucyu.config(['$translateProvider',
  function ($translateProvider, $window) {
      //var language = (window.navigator.userLanguage || window.navigator.language).toLowerCase();
      //console.log(language);

      $translateProvider.registerAvailableLanguageKeys(['jp', 'en', 'vn'], {
          'jp': 'jp',
          'en': 'en',
          'vn': 'vn'
      });

      $translateProvider.useStaticFilesLoader({
          prefix: '/app.dev/TestJS/lang_',
          suffix: '.json'
      });     

      $translateProvider.preferredLanguage("jp");
      //$translateProvider.preferredLanguage('vn');
      // $translateProvider.use('de');
      //$translateProvider.useCookieStorage();
      //$translateProvider.fallbackLanguage("en");
  }
]);

jyuucyu.controller('changeLang', ['$scope', '$translate','$http',
  function ($scope, $translate, $http) {
      $scope.switchLanguage = function (key) {
          $translate.use(key);
          $scope.langMulti = key;

          //langCollection = key;

          $http.get('/app.dev/TestJS/lang_' + key + '.json').success(function (data2) {
              langCollection = data2;
              //alert(data2.registerJyuucyuu + data);
          });

          $http({
              method: "POST",
              url: serverDir + "/About/setLangSel/",
              data: {
                  "lang": key
              },
          })
            .success({
                
            });
      };

      $http.get(serverDir + "/About/getLangSel")
         .success(function (data) {
             $scope.switchLanguage(data);
             $scope.langMulti = data;
             $translate.use(data);
         });

      
  }
]);
/*
jyuucyu.run(["$rootScope", function ($scope) {
    $scope.date = new Date();  // デフォルトで現在時刻を表示
    $scope.alert = null;
    $scope.done = function () {
        $scope.alert = { type: 'success', msg: "" + $scope.date + " = " + $scope.date.getTime() };
    }
}]);
*/

//no need
jyuucyu.controller('angularController', function ($scope, $http) {
    $http.get(serverDir + "/About/GetSehins/")
        .success(function (data) {
            $scope.listSehin = data;
            //console.warn(GetSehins.status);
            //alert(GetSehins.status);
            $scope.selectedSehins = [];

            $scope.clickedItem = function (sehin) {
                console.log("Selected: " + sehin.Code);
                //$scope.doneAfterClick = todo.done;
                //$scope.todoText = todo.text;

                var idx = $scope.selectedSehins.indexOf(sehin);
                if (idx > -1) {
                    $scope.selectedSehins.splice(idx, 1);
                }

                    // is newly selected
                else {
                    //var obj = selectedItem(itemId);
                    $scope.selectedSehins.push(sehin);
                    idx = $scope.listSehin.indexOf(sehin);
                    if (idx > -1) {
                        $scope.listSehin.splice(idx, 1);
                    }

                };
            }

            $scope.del = function (sehin) {

                var isConfirmed = confirm("Are you sure to delete code " + sehin.Code + " ?");
                if (isConfirmed) {
                    var idx = $scope.selectedSehins.indexOf(sehin);
                    if (idx > -1) {
                        $scope.selectedSehins.splice(idx, 1);
                        $scope.listSehin.push(sehin);
                        $scope.listSehin.sort();
                    }
                }
                else {
                    alert('NO');
                }
            }

            $scope.doubleClickedItem = function (sehin) {
                console.log("Selected: " + sehin.Code);

                //$scope.doneAfterClick = todo.done;
                //$scope.todoText = todo.text;

                var idx = $scope.selectedSehins.indexOf(sehin);
                if (idx > -1) {
                    $scope.selectedSehins.splice(idx, 1);
                }

                    // is newly selected
                else {
                    //var obj = selectedItem(itemId);
                    $scope.selectedSehins.push(sehin);
                    idx = $scope.listSehin.indexOf(sehin);
                    if (idx > -1) {
                        $scope.listSehin.splice(idx, 1);
                    }

                };

            }

            

        });

    $scope.doPost123 = function () {
        $http({
            method: "POST",
            url: serverDir +"/About/PostData/",
            data: {
                "listSehins": $scope.selectedSehins
            },
        })
        .success(function (data) {

            if ($scope.selectedSehins.length < 1) {
                alert('Empty');
            }
            else {
                alert('OK');
            }
        });

    }

});