﻿'use strict';

angular.module('app.controllers').controller('MenuController',
['$scope', '$rootScope', '$location', 'AuthService', 'Api', 'version','$state',
function ($scope, $rootScope, $location, AuthService, Api, version, $state)
{
	$scope.version = version;
	$scope.userTasks = 0;
	$scope.projectTasks = 0;
	$scope.userDeadlineTasks = 0;
	$scope.searchParam = '';

	init();

	function init()
	{
		AuthService.fillAuthData();

		$scope.authentication = AuthService.authentication;

        var tipo = $rootScope.tipo;

		if (!AuthService.authentication.isAuth)
            $location.path('login');    
	}

//	$scope.searchSystem = function () {
	//	$location.path('/task/tasks').search({ searchSystem: $scope.searchParam });
	//}

    $scope.logOut = function ()
    {
        AuthService.fillAuthData();

        $scope.authentication = AuthService.authentication;

        console.log($scope.authentication)

        var tipo = $scope.authentication.tipo;

        if (tipo == 2 || tipo == 1)
            AuthService.logOut();
        else
            window.location = '/login?tipo=' + tipo;        
    };

}]);