﻿angular.module('app.controllers').controller('ListingTasksController',
['$scope', 'AuthService', '$state', 'ngHistoricoFiltro', 'Api', 'ngSelects',
function ($scope, AuthService, $state, ngHistoricoFiltro, Api, ngSelects)
{
	$scope.permModel = {};
	$scope.loading = false;
	$scope.permID = 106;

	function CheckPermissions() {
		Api.Permission.get({ id: $scope.permID }, function (data) {
			$scope.permModel = data;

			if (!$scope.permModel.listagem) {
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
	
	$scope.campos = {
		ativo: 'true',
		selects: {
			priority: ngSelects.obterConfiguracao(Api.Priority, { tamanhoPagina: 15, campoNome: 'stName' }),
			project: ngSelects.obterConfiguracao(Api.Project, { tamanhoPagina: 15, campoNome: 'stName' }),
			phase: ngSelects.obterConfiguracao(Api.Phase, {
				tamanhoPagina: 15, campoNome: 'stName',

				scope: $scope,
				filtro:
					{
						campo: 'idProject',
						valor: 'campos.fkProject'
					}

			}),
		}
	};
	$scope.itensporpagina = 15;

	$scope.search = function ()
	{
		$scope.load(0, $scope.itensporpagina);
		$scope.paginador.reiniciar();
	}

	$scope.load = function (skip, take)
	{
		$scope.loading = true;

		var options = { active: 'true', skip: skip, take: take };
		var filter = ngHistoricoFiltro.filtro.filtroGerado;
		if (filter)
			angular.extend(options, filter);

		Api.Task.listPage(options, function (data) {
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
			$state.go('task', { id: mdl.id });
	}

	$scope.new = function ()
	{
		if (!$scope.permModel.novo)
			toastr.error('Access denied!', 'Permission');
		else
			$state.go('task-new');
	}

}]);