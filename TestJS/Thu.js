
var question = angular.module('myApp2', ['ui.bootstrap', 'pascalprecht.translate', 'ngCookies']);

//var CustomCtrlApp = angular.module('CustomCtrlApp', ['ui.bootstrap']);

var langCollection = [];
var locationstr = window.location.pathname + "";
var serverDir = "/" + locationstr.split('/')[1];

question.controller("", function ($location, $filter, $anchorScroll, $scope, $http, $translate) {
    $scope.testbyDuc = function(){
		alert('abcd');
	}
});

question.config(["datepickerConfig", "datepickerPopupConfig", "timepickerConfig",
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

question.config(['$translateProvider',
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

question.controller('changeLang', ['$scope', '$translate', '$http',
    function ($scope, $translate, $http) {
        $scope.switchLanguage = function (key) {

            $scope.langMulti = key;
            $translate.use(key);
            //langCollection = key;

            $http.get('/app.dev/TestJS/lang_' + key + '.json').success(function (data2) {
                langCollection = data2;
                //alert(data2.registerquestionu + data);
            });

            $http({
                method: "POST",
                url: serverDir + "/About/setLangSel/",
                data: {
                    "lang": key
                },
            })
            .success(function(){
                $scope.langMulti = key;
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


question.directive('jqdatepicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            element.datepicker({
                dateFormat: 'DD, d  MM, yy',
                onSelect: function (date) {
                    scope.date = date;
                    scope.$apply();
                }
            });
        }
    };
});
question.controller('MyController', ['$scope', function ($scope) {
    $scope.current = new Date();
    // （5）変数currentの値を監視
    $scope.$watch('current', function (new_value, old_value) {
        if (!angular.isDate(new_value)) {
            $scope.current = new Date();
        }
    });

    $scope.onopen = function ($event) {
        // （4）カレンダーを開く
        $scope.opened = true;
    };
}]);