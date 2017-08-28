﻿angular.module('app.controllers').controller('VendaController',
['$scope', '$rootScope', 'AuthService', '$state', 'ngHistoricoFiltro', 'Api', 'ngSelects', 
function ($scope, $rootScope, AuthService, $state, ngHistoricoFiltro, Api, ngSelects )
{
	$rootScope.exibirMenu = true;
    $scope.loading = false;
    
    $scope.viewModel = {};
    
    var invalidCheck = function (element) {
        if (element == undefined)
            return true;
        else
            if (element.length == 0)
                return true;

        return false;
    }

    $scope.$watch("viewModel.parcelas", function (novo, antigo)
    {
        if (novo !== antigo)
            $scope.simulacao = undefined;
    });
    
    $scope.parcelar = function ()
    {
        $scope.loading = true;
        $scope.erroSimulacao = '';

        Api.SimulaParcelada.listPage(
        {
            cartao: $scope.viewModel.data.id,
            valor: $scope.viewModel.valor,
            parcelas: $scope.viewModel.parcelas,
            p1: $scope.viewModel.p1,
            p2: $scope.viewModel.p2,
            p3: $scope.viewModel.p3,
            p4: $scope.viewModel.p4,
            p5: $scope.viewModel.p5,
            p6: $scope.viewModel.p6,
            p7: $scope.viewModel.p7,
            p8: $scope.viewModel.p8,
            p9: $scope.viewModel.p9,
            p10: $scope.viewModel.p10,
            p11: $scope.viewModel.p11,
            p12: $scope.viewModel.p12,
        },
        function (data)
        {
            $scope.simulacao = true;

            $scope.viewModel.p1 = ''; $scope.viewModel.p2 = ''; $scope.viewModel.p3 = ''; $scope.viewModel.p4 = ''; $scope.viewModel.p5 = '';
            $scope.viewModel.p6 = ''; $scope.viewModel.p7 = ''; $scope.viewModel.p8 = ''; $scope.viewModel.p9 = ''; $scope.viewModel.p10 = '';
            $scope.viewModel.p11 = ''; $scope.viewModel.p12 = '';

            $scope.viewModel.p1m = ''; $scope.viewModel.p2m = ''; $scope.viewModel.p3m = ''; $scope.viewModel.p4m = ''; $scope.viewModel.p5m = '';
            $scope.viewModel.p6m = ''; $scope.viewModel.p7m = ''; $scope.viewModel.p8m = ''; $scope.viewModel.p9m = ''; $scope.viewModel.p10m = '';
            $scope.viewModel.p11m = ''; $scope.viewModel.p12m = '';

            if ($scope.viewModel.parcelas >= 1) $scope.viewModel.p1 = data.results[0].valor;
            if ($scope.viewModel.parcelas >= 2) $scope.viewModel.p2 = data.results[1].valor;
            if ($scope.viewModel.parcelas >= 3) $scope.viewModel.p3 = data.results[2].valor;
            if ($scope.viewModel.parcelas >= 4) $scope.viewModel.p4 = data.results[3].valor;
            if ($scope.viewModel.parcelas >= 5) $scope.viewModel.p5 = data.results[4].valor;
            if ($scope.viewModel.parcelas >= 6) $scope.viewModel.p6 = data.results[5].valor;
            if ($scope.viewModel.parcelas >= 7) $scope.viewModel.p7 = data.results[6].valor;
            if ($scope.viewModel.parcelas >= 8) $scope.viewModel.p8 = data.results[7].valor;
            if ($scope.viewModel.parcelas >= 9) $scope.viewModel.p9 = data.results[8].valor;
            if ($scope.viewModel.parcelas >= 10) $scope.viewModel.p10 = data.results[9].valor;
            if ($scope.viewModel.parcelas >= 11) $scope.viewModel.p11 = data.results[10].valor;
            if ($scope.viewModel.parcelas >= 12) $scope.viewModel.p12 = data.results[11].valor;
            
            if ($scope.viewModel.parcelas >= 1) $scope.viewModel.p1m = data.results[0].valorMax;
            if ($scope.viewModel.parcelas >= 2) $scope.viewModel.p2m = data.results[1].valorMax;
            if ($scope.viewModel.parcelas >= 3) $scope.viewModel.p3m = data.results[2].valorMax;
            if ($scope.viewModel.parcelas >= 4) $scope.viewModel.p4m = data.results[3].valorMax;
            if ($scope.viewModel.parcelas >= 5) $scope.viewModel.p5m = data.results[4].valorMax;
            if ($scope.viewModel.parcelas >= 6) $scope.viewModel.p6m = data.results[5].valorMax;
            if ($scope.viewModel.parcelas >= 7) $scope.viewModel.p7m = data.results[6].valorMax;
            if ($scope.viewModel.parcelas >= 8) $scope.viewModel.p8m = data.results[7].valorMax;
            if ($scope.viewModel.parcelas >= 9) $scope.viewModel.p9m = data.results[8].valorMax;
            if ($scope.viewModel.parcelas >= 10) $scope.viewModel.p10m = data.results[9].valorMax;
            if ($scope.viewModel.parcelas >= 11) $scope.viewModel.p11m = data.results[10].valorMax;
            if ($scope.viewModel.parcelas >= 12) $scope.viewModel.p12m = data.results[11].valorMax;
                
            $scope.loading = false;
        },
        function (response)
        {
            $scope.loading = false;
            $scope.simulacao = false;
            $scope.erroSimulacao = response.data.message;
        });
    }
    
    $scope.conferir = function ()
    {
        $scope.stEmpresa_fail = invalidCheck($scope.viewModel.stEmpresa);
        $scope.stMatricula_fail = invalidCheck($scope.viewModel.stMatricula);
        $scope.stAcesso_fail = invalidCheck($scope.viewModel.stAcesso);
        $scope.stVencimento_fail = invalidCheck($scope.viewModel.stVencimento);

        if ($scope.stEmpresa_fail ||
            $scope.stMatricula_fail ||
            $scope.stAcesso_fail ||
            $scope.stVencimento_fail)
        {
            return;
        }

        $scope.loading = true;

        Api.Associado.listPage({
            empresa: $scope.viewModel.stEmpresa,
            matricula: $scope.viewModel.stMatricula,
            acesso: $scope.viewModel.stAcesso,
            vencimento: $scope.viewModel.stVencimento,
        },
        function (data)
        {
            $scope.viewModel.data = data.results[0];
            $scope.loading = false;
        },
        function (response)
        {
            $scope.viewModel.data = {};
            $scope.viewModel.data.error = "Cartão Inválido";
            $scope.loading = false;
        });
    }

}]);

