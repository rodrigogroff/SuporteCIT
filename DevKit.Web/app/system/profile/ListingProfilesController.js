﻿angular.module('app.controllers').controller('ListingProfilesController',
['$scope', 'AuthService', '$state', 'ngHistoricoFiltro', 'Api', 'ngSelects',
function ($scope, AuthService, $state, ngHistoricoFiltro, Api, ngSelects)
{
	$scope.loading = false;
	$scope.campos = {
		selects: {
			user: ngSelects.obterConfiguracao(Api.User, { campoNome: 'stLogin' }),
		}
	};
	$scope.itensporpagina = 15;
	$scope.permModel = {};	
	$scope.permID = 101;

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

	$scope.search = function ()
	{
		$scope.load(0, $scope.itensporpagina);
		$scope.paginador.reiniciar();
	}

	$scope.load = function (skip, take)
	{
		$scope.loading = true;

		var opcoes = { skip: skip, take: take };

		var filtro = ngHistoricoFiltro.filtro.filtroGerado;

		if (filtro)
			angular.extend(opcoes, filtro);

		delete opcoes.selects;

		Api.Profile.listPage(opcoes, function (data) {
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
			$state.go('profile', { id: mdl.id });
	}

	$scope.new = function ()
	{
		if (!$scope.permModel.novo)
			toastr.error('Access denied!', 'Permission');
		else
			$state.go('profile-new');
	}
	
}]);
