﻿
angular.module('app.controllers').controller('EmissoraRelExtratTransController',
['$scope', '$rootScope', 'AuthService', '$state', 'ngHistoricoFiltro', 'Api', 'ngSelects',
function ($scope, $rootScope, AuthService, $state, ngHistoricoFiltro, Api, ngSelects)
{
    $rootScope.exibirMenu = true;
    $scope.loading = false;

    $scope.campos = {
        sit: '1',
        selects: {
            empresa: ngSelects.obterConfiguracao(Api.Empresa, { tamanhoPagina: 15 }),
        }
    };

    var invalidCheck = function (element) {
        if (element == undefined)
            return true;
        else
            if (element.length == 0)
                return true;

        return false;
    }
    
    $scope.search = function ()
    {
        if ($scope.tipo == '5') {
            $scope.emp_fail = $scope.campos.idEmpresa == undefined;
            if ($scope.emp_fail == true)
                return;
        }

        $scope.list = undefined;

        $scope.dtIni_fail = invalidCheck($scope.campos.dtInicial);        
        $scope.dtFim_fail = invalidCheck($scope.campos.dtFinal);        
    
        $scope.loading = true;

        var opcoes = {
            idEmpresa: $scope.campos.idEmpresa,
            mat: $scope.campos.mat,
            dtInicial: $scope.campos.dtInicial,
            dtFinal: $scope.campos.dtFinal,
            sit: $scope.campos.sit,
        };

        Api.EmissoraRelExtratoTrans.listPage(opcoes, function (data)
        {
            if (data.fail == true) {
                $scope.list = [];
            }
            else
            {
                $scope.list = data.results;
                $scope.dtEmissao = data.dtEmissao;
                $scope.cartao = data.cartao;
                $scope.periodo = data.periodo;
                $scope.total = data.total;                
            }

            $scope.loading = false;
            
        },
        function (response) {
            $scope.loading = false;
            $scope.list = [];
        });
    }

}]);