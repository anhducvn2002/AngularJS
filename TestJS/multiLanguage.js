
var jyuucyu = angular.module("myApp2", ["ui.bootstrap"]);

jyuucyu.config(["datepickerConfig", "datepickerPopupConfig", "timepickerConfig",
         function (datepickerConfig, datepickerPopupConfig, timepickerConfig) {
             datepickerConfig.showWeeks = false; // �T�ԍ��i���{�ł͓���݂������j���\���ɂ���
             datepickerConfig.dayTitleFormat = "yyyy�N MMMM";
             datepickerPopupConfig.currentText = "�{��";
             datepickerPopupConfig.clearText = "����";
             datepickerPopupConfig.toggleWeeksText = "�T�ԍ�";
             datepickerPopupConfig.closeText = "����";
             timepickerConfig.showMeridian = false; // ������24���ԕ\���ɂ���i�f�t�H���g�ł�12���ԕ\���j
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


