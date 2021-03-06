﻿
angular.module('app.controllers').controller('TimesheetController',
['$window', '$scope', '$rootScope', '$state', 'Api', 'ngSelects',
function ($window, $scope, $rootScope, $state, Api, ngSelects)
{
	$rootScope.exibirMenu = true;
	$scope.loading = false;
    	
	function CheckPermissions()
	{
        Api.Permission.get({ id: $scope.permID }, function (data)
        {
			$scope.permModel = data;

			if (!$scope.permModel.listagem) {
                toastr.error('Acesso negado!', 'Permissão');
				$state.go('home');
			}			
		},
		function (response) { });

        Api.Permission.get({ id: 110 }, function (data) {
			
			if ($scope.permModel.listagem) {
				$scope.adminTimesheet = true;
			}
		},
		function (response) { });
	}

	init();

	function init()
    {
        $scope.windowWidth = 900; var w = angular.element($window);
        $scope.$watch(function () { return $window.innerWidth; },
            function (value) { $scope.availWidth = value; if (value > 1400) $scope.windowWidth = 1630; else $scope.windowWidth = 900; },
            true); w.bind('resize', function () { $scope.$apply(); });

        $scope.permModel = {};
        $scope.viewModel = undefined;
        $scope.adminTimesheet = false;
        $scope.permID = 109;
        $scope.userId = 0;
        $scope.stMessage = '';
        
		var currentDate = new Date();

		$scope.nuYear = currentDate.getFullYear();
		$scope.nuMonth = currentDate.getMonth() + 1;

        $scope.selectMonths = ngSelects.obterConfiguracao(Api.MonthCombo, { });
        $scope.selectUsers = ngSelects.obterConfiguracao(Api.UserCombo, {});

        CheckPermissions();
	}
	
	$scope.load = function()
	{
		$scope.viewModel = undefined;
		$scope.stMessage = '';

		if ($scope.nuYear == '' ||
			$scope.nuMonth == undefined )
		{
			$scope.stMessage = 'Informe todos os campos de data';
		}
		else
		{
			$scope.loading = true;

            Api.Timesheet.listPage({
                nuYear: $scope.nuYear,
                nuMonth: $scope.nuMonth,
                fkUser: $scope.userId
            }, function (data)
			{
				if (data.fail == true)
				{
					$scope.viewModel = undefined;
					$scope.stMessage = 'No records found';
				}					
				else
					$scope.viewModel = data;
			});

			$scope.loading = false;
		}
	}

	$scope.show = function (id) {
		$state.go('task', { id: id });
	}

}]);
