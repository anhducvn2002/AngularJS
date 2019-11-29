app.controller("myCtrl", function ($scope) {
    $scope.firstName = "John";
    $scope.lastName = "Doe";
});


/*

dammio-com  ->  dammioCom

Một số giá trị giới hạn hợp lệ là:

E là tên phần tử
A là thuộc tính
C là lớp
M là chú thích (comment)
*/
app.directive("dammioCom", function () {
    return {
        //restrict: "C",
        template: "<p>Testing Directive</p>"
    };
});


app.controller('personCtrl', function ($scope) {
    $scope.firstName = "Dammio";
    $scope.lastName = "Ta";
    $scope.fullName = function () {
        return $scope.firstName + " " + $scope.lastName;
    };
});


/* Test root scope*/
app.run(function ($rootScope) {
    $rootScope.color = 'xanh';
});
app.controller('rootColor', function ($scope) {
    $scope.color = "xanh";
});

/* Get response data */
app.controller('dammioCtrl', function () {
    $.getJSON("/Index/GetList", null, function (GetList) {
        alert(GetList.status);
    });
});

app.controller('angularController', function ($scope, $http) {
    $http.get(serverDir + "/Home/GetSehins/")
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

                var isConfirmed = confirm("Are you sure to delete code "+sehin.Code+" ?");
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

            $scope.doPost123 = function () {

                /*
                var isConfirmed = confirm("Are you sure to delete this record ?");
                if (isConfirmed) {
                    alert('OK');
                }
                else {
                    alert('NO');
                }

                alert('start123');
                */
                $http({
                    method: "POST",
                    url: serverDir + "/Home/PostData/",
                    data: {
                        "listSehins": $scope.selectedSehins
                    },
                })
                .success(function (data) {

                    if ($scope.selectedSehins.length < 1)
                    {
                        alert('Empty');
                    }
                    else {
                        alert('OK');
                    }
                    
                    

                    /*
                    if (!data.success) {
                        // if not successful, bind errors to error variables
                        $scope.errorName = data.errors.name;
                        $scope.errorSuperhero = data.errors.superheroAlias;
                    }
                    else {
                        // if successful, bind success message to message
                        $scope.message = data.message;
                    }
                    */
                });
                /*
                $http.post('/HomeController/PostData/', selectedSehins)
                .success(function (selectedSehins) {
                    $scope.something = selectedSehins;
                    console.log(selectedSehins);

                }).error(function (response) {

                    alert("Fail");

                });
                */
            }


        });
});
