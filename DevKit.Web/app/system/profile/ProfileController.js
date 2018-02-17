﻿
angular.module('app.controllers').controller('ProfileController',
['$scope', '$state', '$stateParams', '$rootScope', 'Api', 'ngSelects',
function ($scope, $state, $stateParams, $rootScope, Api, ngSelects)
{
	$rootScope.exibirMenu = true;
	$scope.loading = false;
	
	function CheckPermissions()
	{
        Api.Permission.get({ id: $scope.permID }, function (data)
		{
			$scope.permModel = data;
			if (!$scope.permModel.visualizar) {
                toastr.error('Accesso negado!', 'Permissão');
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
        $scope.permModel = {};
        $scope.permID = 101;
        $scope.auditLogPerm = 116;

        $scope.viewModel =
            {
                tg3011: false, tg3012: false, tg3013: false, tg3014: false,
                tg4013: false, tg4023: false, tg4033: false, tg4043: false,
                tg5011: false, tg5012: false, 
                tg6011: false, tg6021: false,
                tg1011: false, tg1012: false, tg1013: false, tg1014: false, tg1015: false,
                tg1021: false, tg1022: false, tg1023: false, tg1024: false, tg1025: false,
                tg2001: false,
                tg2011: false,
            };

		CheckPermissions();

		if (id > 0)
        {
            if ($scope.loaded == undefined)
                $scope.loading = true;

            Api.Profile.get({ id: id }, function (data)
            {
                $scope.loaded = true;

                console.log(data.stPermissions);

                // associados
                if (data.stPermissions.indexOf('|3011|') >= 0) data.tg3011 = true; else data.tg3011 = false;
                if (data.stPermissions.indexOf('|3012|') >= 0) data.tg3012 = true; else data.tg3012 = false;
                if (data.stPermissions.indexOf('|3013|') >= 0) data.tg3013 = true; else data.tg3013 = false;
                if (data.stPermissions.indexOf('|3014|') >= 0) data.tg3014 = true; else data.tg3014 = false;

                // troca senha
                if (data.stPermissions.indexOf('|4013|') >= 0) data.tg4013 = true; else data.tg4013 = false;

                // segunda via
                if (data.stPermissions.indexOf('|4023|') >= 0) data.tg4023 = true; else data.tg4023 = false;

                // bloq
                if (data.stPermissions.indexOf('|4033|') >= 0) data.tg4033 = true; else data.tg4033 = false;

                // desbloq
                if (data.stPermissions.indexOf('|4043|') >= 0) data.tg4043 = true; else data.tg4043 = false;

                // credenciados
                if (data.stPermissions.indexOf('|5011|') >= 0) data.tg5011 = true; else data.tg5011 = false;
                if (data.stPermissions.indexOf('|5012|') >= 0) data.tg5012 = true; else data.tg5012 = false;                

                // relatorios
                if (data.stPermissions.indexOf('|6011|') >= 0) data.tg6011 = true; else data.tg6011 = false;
                if (data.stPermissions.indexOf('|6021|') >= 0) data.tg6021 = true; else data.tg6021 = false;

				// profiles
				if (data.stPermissions.indexOf('|1011|') >= 0) data.tg1011 = true; else data.tg1011 = false;
				if (data.stPermissions.indexOf('|1012|') >= 0) data.tg1012 = true; else data.tg1012 = false;
				if (data.stPermissions.indexOf('|1013|') >= 0) data.tg1013 = true; else data.tg1013 = false;
				if (data.stPermissions.indexOf('|1014|') >= 0) data.tg1014 = true; else data.tg1014 = false;
				if (data.stPermissions.indexOf('|1015|') >= 0) data.tg1015 = true; else data.tg1015 = false;

				// users
				if (data.stPermissions.indexOf('|1021|') >= 0) data.tg1021 = true; else data.tg1021 = false;
				if (data.stPermissions.indexOf('|1022|') >= 0) data.tg1022 = true; else data.tg1022 = false;
				if (data.stPermissions.indexOf('|1023|') >= 0) data.tg1023 = true; else data.tg1023 = false;
				if (data.stPermissions.indexOf('|1024|') >= 0) data.tg1024 = true; else data.tg1024 = false;
				if (data.stPermissions.indexOf('|1025|') >= 0) data.tg1025 = true; else data.tg1025 = false;
                				
				// password change
                if (data.stPermissions.indexOf('|2001|') >= 0) data.tg2001 = true; else data.tg2001 = false;

				// password reset
				if (data.stPermissions.indexOf('|2011|') >= 0) data.tg2011 = true; else data.tg2011 = false;
				
				$scope.viewModel = data;

				$scope.loading = false;

			},
			function (response)
			{
				if (response.status === 404) { toastr.error('Invalid ID', 'Error'); }
				$scope.list();
			});
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
            toastr.error('Accesso negado!', 'Permissão');
		else
		{
			$scope.stName_fail = invalidCheck($scope.viewModel.stName);

			if (!$scope.stName_fail) 
			{
				var perms = ''; var _mdl = $scope.viewModel;

                // associados
                if (_mdl.tg3011 == true) perms += '|3011|'; if (_mdl.tg3012 == true) perms += '|3012|'; if (_mdl.tg3013 == true) perms += '|3013|';
                if (_mdl.tg3014 == true) perms += '|3014|'; 

                // troca senha
                if (_mdl.tg4013 == true) perms += '|4013|'; 

                // segunda via
                if (_mdl.tg4023 == true) perms += '|4023|'; 

                // bloq
                if (_mdl.tg4033 == true) perms += '|4033|'; 

                // desbloq
                if (_mdl.tg4043 == true) perms += '|4043|'; 

                // credenciados
                if (_mdl.tg5011 == true) perms += '|5011|'; if (_mdl.tg5012 == true) perms += '|5012|';

                // rel / aut
                if (_mdl.tg6011 == true) perms += '|6011|'; 

                // rel / fech
                if (_mdl.tg6021 == true) perms += '|6021|'; 

				// profiles
				if (_mdl.tg1011 == true) perms += '|1011|'; if (_mdl.tg1012 == true) perms += '|1012|'; if (_mdl.tg1013 == true) perms += '|1013|';
				if (_mdl.tg1014 == true) perms += '|1014|'; 
				// users
				if (_mdl.tg1021 == true) perms += '|1021|'; if (_mdl.tg1022 == true) perms += '|1022|'; if (_mdl.tg1023 == true) perms += '|1023|';
				if (_mdl.tg1024 == true) perms += '|1024|'; if (_mdl.tg1025 == true) perms += '|1025|';
				// password change 
				if (_mdl.tg2001 == true) perms += '|2001|';
				// password reset
				if (_mdl.tg2011 == true) perms += '|2011|';

				$scope.viewModel.stPermissions = perms;
                
				if (id > 0)
                {
					Api.Profile.update({ id: id }, $scope.viewModel, function (data)
					{
						toastr.success('Perfil salvo!', 'Sucesso');
						$scope.viewModel.logs = data.logs;
					},
					function (response)
					{
						toastr.error(response.data.message, 'Error');
					});
				}
				else
				{
					Api.Profile.add($scope.viewModel, function (data)
					{
						toastr.success('Perfil adicionado!', 'Suceso');
                        $state.go('profiles'); $state.go('profiles');
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
		$state.go('profiles');
	}

	$scope.remove = function ()
	{
		if (!$scope.permModel.remover)
            toastr.error('Accesso negado!', 'Permissão');
		else
		{
            Api.Profile.remove({ id: id }, function (data)
			{
				toastr.success('Perfil removido!', 'Sucesso');
				$scope.list();
			},
			function (response)
			{
				toastr.error(response.data.message, 'Error');
			});
		}
	}

	$scope.showUser = function (mdl)
	{
		$state.go('user', { id: mdl.id });
	}	

}]);
