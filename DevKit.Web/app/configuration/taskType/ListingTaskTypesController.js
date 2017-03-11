﻿angular.module('app.controllers').controller('ListingTaskTypesController',
['$scope', 'AuthService', '$state', 'ngHistoricoFiltro', 'Api',
function ($scope, AuthService, $state, ngHistoricoFiltro, Api)
{
	$scope.permModel = {};
	$scope.loading = false;
	$scope.permID = 105;

	function CheckPermissions()
	{
		Api.Permission.get({ id: $scope.permID }, function (data)
		{
			$scope.permModel = data;

			if (!$scope.permModel.listagem)
			{
				toastr.error('Access denied!', 'Permission');
				$state.go('home');
			}				
		},
		function (response) { });
	}

	init();

	function init()
	{
		CheckPermissions();

		if (ngHistoricoFiltro.filtro)
			ngHistoricoFiltro.filtro.exibeFiltro = false;
	}

	$scope.campos = { ativo: 'true' };
	$scope.itensporpagina = 15;
	
	$scope.search = function ()
	{
		$scope.load(0, $scope.itensporpagina);
		$scope.paginador.reiniciar();
	}

	$scope.load = function (skip, take)
	{
		$scope.loading = true;

		var opcoes = { ativo: 'true', skip: skip, take: take };
		var filtro = ngHistoricoFiltro.filtro.filtroGerado;
		if (filtro)
			angular.extend(opcoes, filtro);

		Api.TaskType.listPage(opcoes, function (data) {
			$scope.list = data.results;
			$scope.total = data.count;		
			$scope.loading = false;
		});
	}

	$scope.show = function (mdl)
	{
		if (!$scope.permModel.visualizar) 
			toastr.error('Access denied!', 'Permission');
		else
			$state.go('taskType', { id: mdl.id });
	}

	$scope.new = function ()
	{
		if (!$scope.permModel.novo)
			toastr.error('Access denied!', 'Permission');
		else
			$state.go('taskType-new');
	}
	
}]);