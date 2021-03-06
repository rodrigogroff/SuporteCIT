﻿
angular.module('app.controllers').controller('TaskTypeController',
['$scope', '$state', '$stateParams', '$rootScope', 'Api', 'ngSelects',
function ($scope, $state, $stateParams, $rootScope, Api, ngSelects)
{
	$rootScope.exibirMenu = true;

	$scope.selectAccType = ngSelects.obterConfiguracao(Api.AccType, {});
	$scope.selectProject = ngSelects.obterConfiguracao(Api.ProjectCombo, {});
	$scope.selectTaskCategory = ngSelects.obterConfiguracao(Api.TaskCategoryCombo, { scope: $scope, filtro: { campo: 'fkTaskType', valor: 'viewModel.id' } });
	$scope.selectTaskFlow = ngSelects.obterConfiguracao(Api.TaskFlowCombo, { scope: $scope, filtro: { campo: 'fkTaskCategory', valor: 'newAcc.fkTaskCategory' } });

	$scope.loading = false;

	$scope.viewModel = {};
	$scope.permModel = {};	
	$scope.permID = 105;
	$scope.auditLogPerm = 113;

	function CheckPermissions()
	{
        Api.Permission.get({ id: $scope.permID }, function (data)
		{
			$scope.permModel = data;
			if (!$scope.permModel.visualizar)
			{
                toastr.error('Acesso negado!', 'Permissão');
				$state.go('home');
			}
		},
		function (response) { });

        Api.Permission.get({ id: $scope.auditLogPerm }, function (data)
        {
			$scope.auditLogView = $scope.permModel.visualizar;
		},
		function (response) { });
	}
	
	var id = ($stateParams.id) ? parseInt($stateParams.id) : 0;

	init();

	function init()
	{
		CheckPermissions();

		if (id > 0)
        {
            if ($scope.loaded == undefined)
                $scope.loading = true;

            Api.TaskType.get({ id: id }, function (data)
			{
				$scope.viewModel = data;
                $scope.loading = false;
                $scope.loaded = true;
			},
			function (response)
			{
				if (response.status === 404) { toastr.error('Invalid ID', 'Error'); }
				$scope.list();
			});
		}
		else
		{
			$scope.viewModel = { };
		}
	}

	var invalidCheck = function (element) {
		if (element == undefined)
			return true;
		else
			if (element.length == 0)
				return true;

		return false;
	}
	
	$scope.save = function ()
	{
		if (!$scope.permModel.novo && !$scope.permModel.edicao)
            toastr.error('Acesso negado!', 'Permissão');
		else
		{
			$scope.stName_fail = invalidCheck($scope.viewModel.stName);
			$scope.fkProject_fail = $scope.viewModel.fkProject == undefined;
	
			if (!$scope.stName_fail && 
				!$scope.fkProject_fail )
            {
				if (id > 0)
				{
					$scope.viewModel.updateCommand = "entity";

					Api.TaskType.update({ id: id }, $scope.viewModel, function (data)
					{
                        toastr.success('Tipo de tarefa salvo!', 'Sucesso');
						init();
					},
					function (response)
					{
						toastr.error(response.data.message, 'Error');
					});
				}
				else
				{
					Api.TaskType.add($scope.viewModel, function (data)
					{
                        toastr.success('Tipo de tarefa adicionado!', 'Sucesso');
                        $state.go('taskTypes');
					},
					function (response)
					{
						toastr.error(response.data.message, 'Error');
					});
				}
			}
		}
	};

	$scope.list = function () {
		$state.go('taskTypes');
	}

	$scope.remove = function ()
	{
		if (!$scope.permModel.remover)
            toastr.error('Acesso negado!', 'Permissão');
		else
		{
            Api.TaskType.remove({ id: id }, function (data)
			{
                toastr.success('Tipo de tarefa removido!', 'Sucesso');
				$scope.list();
			},
			function (response) {
				toastr.error(response.data.message, 'Permission');
			});
		}
	}

	// ---------------------------------
	// categories
	// ---------------------------------

	$scope.addCategorie = false;

	$scope.removeCategorie = function (index, lista)
	{
		if (!$scope.permModel.novo && !$scope.permModel.edicao)
            toastr.error('Acesso negado!', 'Permissão');
		else
        {
			$scope.viewModel.updateCommand = "removeCategorie";
			$scope.viewModel.anexedEntity = lista[index];

			Api.TaskType.update({ id: id }, $scope.viewModel, function (data)
			{
                toastr.success('Categoria removida', 'Sucesso');
				init();
			});
		}
	}

	$scope.addNewCategorie = function ()
	{
		if (!$scope.permModel.novo && !$scope.permModel.edicao)
            toastr.error('Acesso negado!', 'Permissão');
		else
			$scope.addCategorie = !$scope.addCategorie;
	}

	$scope.editCategorie = function (mdl) {
		$scope.addCategorie = true;
		$scope.newCategorie = mdl;
	}

	$scope.cancelCategorie = function () {
		$scope.addCategorie = false;
		$scope.newCategorie = {};
	}

	$scope.newCategorie = { };

	$scope.saveNewCategorie = function ()
	{
		$scope.stCategorieName_fail = invalidCheck($scope.newCategorie.stName);

		if (!$scope.stCategorieName_fail)
        {
			$scope.viewModel.updateCommand = "newCategorie";
			$scope.viewModel.anexedEntity = $scope.newCategorie;

			Api.TaskType.update({ id: id }, $scope.viewModel, function (data)
			{
				$scope.newCategorie = {};
				$scope.addCategorie = false;
                toastr.success('Categoria salva', 'Sucesso');
				init();
			},
            function (response)
			{
				toastr.error(response.data.message, 'Error');
			});
		}
	}

	// ---------------------------------
	// flows
	// ---------------------------------

	$scope.$watch('newFlow.fkTaskCategory', function (newState, oldState)
	{
		if (newState !== oldState)
			$scope.loadFlows();
	});

	$scope.flows = [];

	$scope.loadFlows = function ()
	{
		var opcoes =
            {
				fkTaskCategory: $scope.newFlow.fkTaskCategory
			};

		$scope.flows = [];

		Api.TaskFlow.listPage(opcoes, function (data)
		{
			$scope.flows = data.results;
		});
	}

	$scope.addFlow = false;

	$scope.editFlow = function (mdl)
	{
		$scope.addFlow = true;
		$scope.newFlow = mdl;
	}

	$scope.removeFlow = function (index, lista)
	{
		if (!$scope.permModel.novo && !$scope.permModel.edicao)
            toastr.error('Acesso negado!', 'Permissão');
		else
        {
            $scope.viewModel.updateCommand = "removeFlow";
			$scope.viewModel.anexedEntity = lista[index];

			Api.TaskType.update({ id: id }, $scope.viewModel, function (data)
			{
                toastr.success('Fluxo removido', 'Sucesso');
				init();
				$scope.loadFlows();
			});
		}
	}

	$scope.addNewFlow = function ()
	{
		if (!$scope.permModel.novo && !$scope.permModel.edicao)
            toastr.error('Acesso negado!', 'Permissão');
		else
			$scope.addFlow = !$scope.addFlow;
	}

	$scope.cancelFlow = function () {
		$scope.addFlow = false;
		$scope.newFlow = {};
	}

	$scope.newFlow = { };

	$scope.saveNewFlow = function ()
	{
		$scope.stFlowName_fail = invalidCheck ($scope.newFlow.stName);
		$scope.stFlowOrder_fail = invalidCheck ($scope.newFlow.nuOrder);

		if (!$scope.stFlowName_fail && !$scope.stFlowOrder_fail)
		{
			var tmp = $scope.newFlow.fkTaskCategory;

			$scope.viewModel.updateCommand = "newFlow";
			$scope.viewModel.anexedEntity = $scope.newFlow;

			Api.TaskType.update({ id: id }, $scope.viewModel, function (data)
			{
                toastr.success('Fluxo salvo', 'Sucesso');

				$scope.newFlow = {};
				$scope.newFlow.fkTaskCategory = tmp;

				init();
				$scope.loadFlows();

				$scope.addFlow = false;

			}, function (response) {
				toastr.error(response.data.message, 'Error');
			});
		}
	}

	// ---------------------------------
	// accs
	// ---------------------------------

	$scope.$watch('newAcc.fkTaskCategory', function (newState, oldState) {
		if (newState !== oldState)
			$scope.loadAccs();
	});

	$scope.accs = [];

	$scope.loadAccs = function ()
	{
		$scope.addAcc = false;

		var opcoes =
            {
				fkTaskCategory: $scope.newAcc.fkTaskCategory
			};

		$scope.accs = [];

		Api.TaskTypeAccumulator.listPage(opcoes, function (data) {
			$scope.accs = data.results;
		});
	}

	$scope.addAcc = false;

	$scope.editAcc = function (mdl)
	{
		if ($scope.addAcc == false)
		{
			$scope.addAcc = true;
			$scope.newAcc = mdl;
		}		
	}

	$scope.cancelAcc = function () {
		$scope.addAcc = false;
		$scope.newAcc = {};
	}

	$scope.removeAcc = function (index, lista)
	{
		if (!$scope.permModel.novo && !$scope.permModel.edicao)
            toastr.error('Acesso negado!', 'Permissão');
        else
        {
			$scope.viewModel.updateCommand = "removeAcc";
			$scope.viewModel.anexedEntity = lista[index];

			Api.TaskType.update({ id: id }, $scope.viewModel, function (data) {
                toastr.success('Acumulador removido', 'Sucesso');
			    init();
				$scope.loadAccs();
			});
		}
	}

	$scope.addNewAcc = function () {
		if (!$scope.permModel.novo && !$scope.permModel.edicao)
            toastr.error('Acesso negado!', 'Permissão');
		else
			$scope.addAcc = !$scope.addAcc;
	}

	$scope.newAcc = {};

	$scope.saveNewAcc = function ()
	{
		$scope.stAccName_fail = invalidCheck($scope.newAcc.stName);
		$scope.selectAccType_fail = $scope.newAcc.fkTaskAccType == undefined;
		$scope.selectAccFlow_fail = $scope.newAcc.fkTaskFlow == undefined;

		if (!$scope.stAccName_fail &&
			!$scope.selectAccType_fail &&
			!$scope.selectAccFlow_fail)
		{
			var tmp = $scope.newAcc.fkTaskCategory;

			$scope.viewModel.updateCommand = "newAcc";
			$scope.viewModel.anexedEntity = $scope.newAcc;

			Api.TaskType.update({ id: id }, $scope.viewModel, function (data)
			{
                toastr.success('Acumulador salvo', 'Sucesso');

				$scope.newAcc = {};
				$scope.newAcc.fkTaskCategory = tmp;

                init();

				$scope.loadAccs();
				$scope.addAcc = false;

			},
			function (response) {
				toastr.error(response.data.message, 'Error');
			});
		}
	}

	// ---------------------------------
	// check points
	// ---------------------------------
		
	$scope.$watch('newCC.fkCategory', function (newState, oldState) {
		if (newState !== oldState)
			$scope.loadCCs();
	});

	$scope.checkpoints = [];

	$scope.loadCCs = function ()
	{
		$scope.addCC = false;

		var opcoes =
            {
				fkCategory: $scope.newCC.fkCategory
			};

		$scope.checkpoints = [];

		Api.TaskCheckPoint.listPage(opcoes, function (data) {
			$scope.checkpoints = data.results;
		});
	}

	$scope.addCC = false;

	$scope.editCC = function (mdl)
	{
		if ($scope.addCC == false) {
			$scope.addCC = true;
			$scope.newCC = mdl;
		}
	}

	$scope.cancelCC = function () {
		$scope.addCC = false;
		$scope.newCC = {};
	}

	$scope.removeCC = function (index, lista)
	{
		if (!$scope.permModel.novo && !$scope.permModel.edicao)
            toastr.error('Acesso negado!', 'Permissão');
        else
        {
			$scope.viewModel.updateCommand = "removeCC";
			$scope.viewModel.anexedEntity = lista[index];

			Api.TaskType.update({ id: id }, $scope.viewModel, function (data) {
                toastr.success('Check point removido', 'Sucesso');
				$scope.loadCCs();
			});
		}
	}

	$scope.addNewCC = function () {
		if (!$scope.permModel.novo && !$scope.permModel.edicao)
            toastr.error('Acesso negado!', 'Permissão');
		else
			$scope.addCC = !$scope.addCC;
	}

	$scope.newCC = {};

	$scope.saveNewCC = function () {
		$scope.stCCName_fail = invalidCheck($scope.newCC.stName);

		if (!$scope.stAccName_fail)
		{
			var tmp = $scope.newCC.fkCategory;

			$scope.viewModel.updateCommand = "newCC";
			$scope.viewModel.anexedEntity = $scope.newCC;

			Api.TaskType.update({ id: id }, $scope.viewModel, function (data)
			{
                toastr.success('Check point salvo', 'Sucesso');
				$scope.newCC = {};
				$scope.newCC.fkCategory = tmp;
				init();
				$scope.loadCCs();
				$scope.addCC = false;
			},
			function (response) {
				toastr.error(response.data.message, 'Error');
			});
		}
	}

}]);
