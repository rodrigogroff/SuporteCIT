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
                // ## System #########################
                // setup
                tg1001: false, tg1002: false, tg1003: false, tg1004: false, tg1005: false,
                // profiles
                tg1011: false, tg1012: false, tg1013: false, tg1014: false, tg1015: false,
                // users
                tg1021: false, tg1022: false, tg1023: false, tg1024: false, tg1025: false,
                // projects
                tg1031: false, tg1032: false, tg1033: false, tg1034: false, tg1035: false,
                // sprints
                tg1041: false, tg1042: false, tg1043: false, tg1044: false, tg1045: false,
                // task type
                tg1051: false, tg1052: false, tg1053: false, tg1054: false, tg1055: false,
                // task 
                tg1061: false, tg1062: false, tg1063: false, tg1064: false, tg1065: false,
                // news 
                tg1181: false, tg1182: false, tg1183: false, tg1184: false, tg1185: false,
                // surveys 
                tg1191: false, tg1192: false, tg1063: false, tg1194: false, tg1195: false,
                // clients
                tg1201: false, tg1202: false, tg1203: false, tg1204: false, tg1205: false,
                // client groups
                tg1211: false, tg1212: false, tg1213: false, tg1214: false, tg1215: false,
                // user Kanban
                tg1071: false,
                // management
                tg1081: false,
                // timesheet
                tg1091: false,
                // timesheet admin
                tg1101: false,
                // audit log (project)
                tg1111: false,
                // audit log (user)
                tg1121: false,
                // audit log (task type)
                tg1131: false,
                // audit log (setup)
                tg1141: false,
                // audit log (task)
                tg1151: false,
                // audit log (profile)
                tg1161: false,
                // user change password (98)
                tg2001: false,
                // reset password (99)
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

				// setup
				if (data.stPermissions.indexOf('|1001|') >= 0) data.tg1001 = true; else data.tg1001 = false;
				if (data.stPermissions.indexOf('|1002|') >= 0) data.tg1002 = true; else data.tg1002 = false;
				if (data.stPermissions.indexOf('|1003|') >= 0) data.tg1003 = true; else data.tg1003 = false;
				if (data.stPermissions.indexOf('|1004|') >= 0) data.tg1004 = true; else data.tg1004 = false;
				if (data.stPermissions.indexOf('|1005|') >= 0) data.tg1005 = true; else data.tg1005 = false;

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

				// projects
				if (data.stPermissions.indexOf('|1031|') >= 0) data.tg1031 = true; else data.tg1031 = false;
				if (data.stPermissions.indexOf('|1032|') >= 0) data.tg1032 = true; else data.tg1032 = false;
				if (data.stPermissions.indexOf('|1033|') >= 0) data.tg1033 = true; else data.tg1033 = false;
				if (data.stPermissions.indexOf('|1034|') >= 0) data.tg1034 = true; else data.tg1034 = false;
				if (data.stPermissions.indexOf('|1035|') >= 0) data.tg1035 = true; else data.tg1035 = false;

				// sprints
				if (data.stPermissions.indexOf('|1041|') >= 0) data.tg1041 = true; else data.tg1041 = false;
				if (data.stPermissions.indexOf('|1042|') >= 0) data.tg1042 = true; else data.tg1042 = false;
				if (data.stPermissions.indexOf('|1043|') >= 0) data.tg1043 = true; else data.tg1043 = false;
				if (data.stPermissions.indexOf('|1044|') >= 0) data.tg1044 = true; else data.tg1044 = false;
				if (data.stPermissions.indexOf('|1045|') >= 0) data.tg1045 = true; else data.tg1045 = false;

				// task type
				if (data.stPermissions.indexOf('|1051|') >= 0) data.tg1051 = true; else data.tg1051 = false;
				if (data.stPermissions.indexOf('|1052|') >= 0) data.tg1052 = true; else data.tg1052 = false;
				if (data.stPermissions.indexOf('|1053|') >= 0) data.tg1053 = true; else data.tg1053 = false;
				if (data.stPermissions.indexOf('|1054|') >= 0) data.tg1054 = true; else data.tg1054 = false;
				if (data.stPermissions.indexOf('|1055|') >= 0) data.tg1055 = true; else data.tg1055 = false;

				// task 
				if (data.stPermissions.indexOf('|1061|') >= 0) data.tg1061 = true; else data.tg1061 = false;
				if (data.stPermissions.indexOf('|1062|') >= 0) data.tg1062 = true; else data.tg1062 = false;
				if (data.stPermissions.indexOf('|1063|') >= 0) data.tg1063 = true; else data.tg1063 = false;
				if (data.stPermissions.indexOf('|1064|') >= 0) data.tg1064 = true; else data.tg1064 = false;
				if (data.stPermissions.indexOf('|1065|') >= 0) data.tg1065 = true; else data.tg1065 = false;

				// news 
				if (data.stPermissions.indexOf('|1181|') >= 0) data.tg1181 = true; else data.tg1181 = false;
				if (data.stPermissions.indexOf('|1182|') >= 0) data.tg1182 = true; else data.tg1182 = false;
				if (data.stPermissions.indexOf('|1183|') >= 0) data.tg1183 = true; else data.tg1183 = false;
				if (data.stPermissions.indexOf('|1184|') >= 0) data.tg1184 = true; else data.tg1184 = false;
				if (data.stPermissions.indexOf('|1185|') >= 0) data.tg1185 = true; else data.tg1185 = false;

				// surveys
				if (data.stPermissions.indexOf('|1191|') >= 0) data.tg1191 = true; else data.tg1191 = false;
				if (data.stPermissions.indexOf('|1192|') >= 0) data.tg1192 = true; else data.tg1192 = false;
				if (data.stPermissions.indexOf('|1193|') >= 0) data.tg1193 = true; else data.tg1193 = false;
				if (data.stPermissions.indexOf('|1194|') >= 0) data.tg1194 = true; else data.tg1194 = false;
				if (data.stPermissions.indexOf('|1195|') >= 0) data.tg1195 = true; else data.tg1195 = false;

				// clients
				if (data.stPermissions.indexOf('|1201|') >= 0) data.tg1201 = true; else data.tg1201 = false;
				if (data.stPermissions.indexOf('|1202|') >= 0) data.tg1202 = true; else data.tg1202 = false;
				if (data.stPermissions.indexOf('|1203|') >= 0) data.tg1203 = true; else data.tg1203 = false;
				if (data.stPermissions.indexOf('|1204|') >= 0) data.tg1204 = true; else data.tg1204 = false;
				if (data.stPermissions.indexOf('|1205|') >= 0) data.tg1205 = true; else data.tg1205 = false;

				// client groups
				if (data.stPermissions.indexOf('|1211|') >= 0) data.tg1211 = true; else data.tg1211 = false;
				if (data.stPermissions.indexOf('|1212|') >= 0) data.tg1212 = true; else data.tg1212 = false;
				if (data.stPermissions.indexOf('|1213|') >= 0) data.tg1213 = true; else data.tg1213 = false;
				if (data.stPermissions.indexOf('|1214|') >= 0) data.tg1214 = true; else data.tg1214 = false;
				if (data.stPermissions.indexOf('|1215|') >= 0) data.tg1215 = true; else data.tg1215 = false;

				// user Kanban  
				if (data.stPermissions.indexOf('|1071|') >= 0) data.tg1071 = true; else data.tg1071 = false;
				// management  
				if (data.stPermissions.indexOf('|1081|') >= 0) data.tg1081 = true; else data.tg1081 = false;
				// timesheet
				if (data.stPermissions.indexOf('|1091|') >= 0) data.tg1091 = true; else data.tg1091 = false;
				// timesheet admin
				if (data.stPermissions.indexOf('|1101|') >= 0) data.tg1101 = true; else data.tg1101 = false;
				// audit log (project)
				if (data.stPermissions.indexOf('|1111|') >= 0) data.tg1111 = true; else data.tg1111 = false;
				// audit log (user)
				if (data.stPermissions.indexOf('|1121|') >= 0) data.tg1121 = true; else data.tg1121 = false;
				// audit log (tasktype)
				if (data.stPermissions.indexOf('|1131|') >= 0) data.tg1131 = true; else data.tg1131 = false;
				// audit log (setup)
				if (data.stPermissions.indexOf('|1141|') >= 0) data.tg1141 = true; else data.tg1141 = false;
				// audit log (task)
				if (data.stPermissions.indexOf('|1151|') >= 0) data.tg1151 = true; else data.tg1151 = false;
				// audit log (profile)
				if (data.stPermissions.indexOf('|1161|') >= 0) data.tg1161 = true; else data.tg1161 = false;
				// audit log (sprint)
				if (data.stPermissions.indexOf('|1171|') >= 0) data.tg1171 = true; else data.tg1171 = false;
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

				// setup
				if (_mdl.tg1001 == true) perms += '|1001|'; if (_mdl.tg1002 == true) perms += '|1002|'; if (_mdl.tg1003 == true) perms += '|1003|';
				if (_mdl.tg1004 == true) perms += '|1004|'; if (_mdl.tg1005 == true) perms += '|1005|';
				// profiles
				if (_mdl.tg1011 == true) perms += '|1011|'; if (_mdl.tg1012 == true) perms += '|1012|'; if (_mdl.tg1013 == true) perms += '|1013|';
				if (_mdl.tg1014 == true) perms += '|1014|'; if (_mdl.tg1015 == true) perms += '|1015|';
				// users
				if (_mdl.tg1021 == true) perms += '|1021|'; if (_mdl.tg1022 == true) perms += '|1022|'; if (_mdl.tg1023 == true) perms += '|1023|';
				if (_mdl.tg1024 == true) perms += '|1024|'; if (_mdl.tg1025 == true) perms += '|1025|';
				// projects
				if (_mdl.tg1031 == true) perms += '|1031|'; if (_mdl.tg1032 == true) perms += '|1032|'; if (_mdl.tg1033 == true) perms += '|1033|';
				if (_mdl.tg1034 == true) perms += '|1034|'; if (_mdl.tg1035 == true) perms += '|1035|';
				// sprints
				if (_mdl.tg1041 == true) perms += '|1041|'; if (_mdl.tg1042 == true) perms += '|1042|'; if (_mdl.tg1043 == true) perms += '|1043|';
				if (_mdl.tg1044 == true) perms += '|1044|'; if (_mdl.tg1045 == true) perms += '|1045|';
				// task type
				if (_mdl.tg1051 == true) perms += '|1051|'; if (_mdl.tg1052 == true) perms += '|1052|'; if (_mdl.tg1053 == true) perms += '|1053|';
				if (_mdl.tg1054 == true) perms += '|1054|'; if (_mdl.tg1055 == true) perms += '|1055|';
				// task 
				if (_mdl.tg1061 == true) perms += '|1061|'; if (_mdl.tg1062 == true) perms += '|1062|'; if (_mdl.tg1063 == true) perms += '|1063|';
				if (_mdl.tg1064 == true) perms += '|1064|'; if (_mdl.tg1065 == true) perms += '|1065|';
				// news
				if (_mdl.tg1181 == true) perms += '|1181|'; if (_mdl.tg1182 == true) perms += '|1182|'; if (_mdl.tg1183 == true) perms += '|1183|';
				if (_mdl.tg1184 == true) perms += '|1184|'; if (_mdl.tg1185 == true) perms += '|1185|';
				// surveys
				if (_mdl.tg1191 == true) perms += '|1191|'; if (_mdl.tg1192 == true) perms += '|1192|'; if (_mdl.tg1193 == true) perms += '|1193|';
				if (_mdl.tg1194 == true) perms += '|1194|'; if (_mdl.tg1195 == true) perms += '|1195|';

				// clients
				if (_mdl.tg1201 == true) perms += '|1201|'; if (_mdl.tg1202 == true) perms += '|1202|'; if (_mdl.tg1203 == true) perms += '|1203|';
				if (_mdl.tg1204 == true) perms += '|1204|'; if (_mdl.tg1205 == true) perms += '|1205|';

				// client groups
				if (_mdl.tg1211 == true) perms += '|1211|'; if (_mdl.tg1212 == true) perms += '|1212|'; if (_mdl.tg1213 == true) perms += '|1213|';
				if (_mdl.tg1214 == true) perms += '|1214|'; if (_mdl.tg1215 == true) perms += '|1215|';

				// user Kanban
				if (_mdl.tg1071 == true) perms += '|1071|';
				// management
				if (_mdl.tg1081 == true) perms += '|1081|';
				// timesheet
				if (_mdl.tg1091 == true) perms += '|1091|';
				// admin timesheet
				if (_mdl.tg1101 == true) perms += '|1101|';
				// audit log (project)
				if (_mdl.tg1111 == true) perms += '|1111|';
				// audit log (user)
				if (_mdl.tg1121 == true) perms += '|1121|';
				// audit log (tasktype)
				if (_mdl.tg1131 == true) perms += '|1131|';
				// audit log (setup)
				if (_mdl.tg1141 == true) perms += '|1141|';
				// audit log (task)
				if (_mdl.tg1151 == true) perms += '|1151|';
				// audit log (profile)
				if (_mdl.tg1161 == true) perms += '|1161|';
				// audit log (profile)
				if (_mdl.tg1171 == true) perms += '|1171|';
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
