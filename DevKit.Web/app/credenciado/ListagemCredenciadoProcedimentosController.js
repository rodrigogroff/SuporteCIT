﻿
angular.module('app.controllers').controller('ListagemCredenciadoProcedimentosController',
['$scope', '$rootScope', '$state', 'Api', 'ngSelects', 
function ($scope, $rootScope, $state, Api, ngSelects )
{
	$rootScope.exibirMenu = true;
	$scope.loading = false;
    
	init();

	function init()
    {
        $scope.campos = {
            ativo: 'true',
        };

        $scope.itensporpagina = 15;
	}

	$scope.search = function ()
	{
		$scope.load(0, $scope.itensporpagina);
		$scope.paginador.reiniciar();
	}

	$scope.load = function (skip, take)
	{
		$scope.loading = true;

        var opcoes = {
            skip: skip,
            take: take,
            tuss: $scope.campos.tuss,
            nomeAssociado: $scope.campos.nomeAssociado,
            dtInicial: $scope.campos.dtInicial,
            dtFinal: $scope.campos.dtFinal,
        };

		Api.CredenciadoListagemAutorizacao.listPage(opcoes, function (data)
		{
			$scope.list = data.results;
			$scope.total = data.count;
			$scope.loading = false;
		});
	}

}]);
