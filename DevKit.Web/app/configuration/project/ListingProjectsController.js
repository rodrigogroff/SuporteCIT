﻿
angular.module('app.controllers').controller('ListingProjectsController',
['$scope', '$rootScope', '$state', 'Api', 'ngSelects', 
function ($scope, $rootScope, $state, Api, ngSelects)
{
	$rootScope.exibirMenu = true;
    $scope.loading = false;

	function CheckPermissions()
	{
        Api.Permission.get({ id: $scope.permID }, function (data)
		{
			$scope.permModel = data;

			if (!$scope.permModel.listagem)
			{
                toastr.error('Acesso negado!', 'Permissão');
				$state.go('home');
			}				
		},
		function (response) { });
	}

	init();

	function init()
    {
        $scope.campos = {
            selects: {
                user: ngSelects.obterConfiguracao(Api.UserCombo, {}),
            }
        };

        $scope.itensporpagina = 15;

        $scope.permModel = {};
        $scope.permID = 103;

		CheckPermissions();
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
            busca: $scope.campos.busca,
            fkUser: $scope.campos.fkUser,
        };

		Api.Project.listPage(opcoes, function (data) {
			$scope.list = data.results;
			$scope.total = data.count;		
			$scope.loading = false;
		});
	}

	$scope.show = function (mdl)
	{
		if (!$scope.permModel.visualizar) 
            toastr.error('Acesso negado!', 'Permissão');
		else
			$state.go('project', { id: mdl.id });
	}

	$scope.new = function ()
	{
		if (!$scope.permModel.novo)
            toastr.error('Acesso negado!', 'Permissão');
		else
			$state.go('project-new');
	}
	
}]);
