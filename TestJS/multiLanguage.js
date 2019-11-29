
var jyuucyu = angular.module("myApp2", ["ui.bootstrap"]);

jyuucyu.config(["datepickerConfig", "datepickerPopupConfig", "timepickerConfig",
         function (datepickerConfig, datepickerPopupConfig, timepickerConfig) {
             datepickerConfig.showWeeks = false; // 週番号（日本では馴染みが薄い）を非表示にする
             datepickerConfig.dayTitleFormat = "yyyy年 MMMM";
             datepickerPopupConfig.currentText = "本日";
             datepickerPopupConfig.clearText = "消去";
             datepickerPopupConfig.toggleWeeksText = "週番号";
             datepickerPopupConfig.closeText = "閉じる";
             timepickerConfig.showMeridian = false; // 時刻を24時間表示にする（デフォルトでは12時間表示）
         }])

angular.module('myApp', ['pascalprecht.translate', 'ngCookies']);


angular.module('myApp')
    .config(['$translateProvider',
  function ($translateProvider) {
      var language = (window.navigator.userLanguage || window.navigator.language).toLowerCase();
      console.log(language);
      $translateProvider.registerAvailableLanguageKeys(['jp', 'en', 'vn'], {
          'jp': 'jp',
          'en': 'en',
          'vn': 'vn'
      });

      $translateProvider.useStaticFilesLoader({
          prefix: '/app.dev/TestJS/lang_',
          suffix: '.json'
      });


      $translateProvider.preferredLanguage('jp');
      //$translateProvider.preferredLanguage('vn');
      // $translateProvider.use('de');
      //$translateProvider.useCookieStorage();
      //$translateProvider.fallbackLanguage("en");
  }
    ]);
angular.module('myApp').controller('changeLang', ['$scope', '$translate',
  function ($scope, $translate) {
      $scope.switchLanguage = function (key) {
          $translate.use(key);
      };
  }
]);


